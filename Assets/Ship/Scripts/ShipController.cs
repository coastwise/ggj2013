using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipController : MonoBehaviour {
	
	public float maxHorizontalAngle = 40;
	public float maxVerticalAngle = 40;
	public float horizontalSpeed = 0.1f;
	public float verticalSpeed = 0.1f;
	
	protected float h = 0;
	protected float v = 0;
	
	public float smoothing = 0.93f;
	
	private Dictionary<System.Type, State> states;
	private State currentState;
	
	private VeinTrain parentTrain;
	
	void Awake () {
		states = new Dictionary<System.Type, State> ();
		states.Add(typeof(FlyingState), new FlyingState(this));
		states.Add(typeof(BarrelRollLeft), new BarrelRollLeft(this));
		states.Add(typeof(BarrelRollRight), new BarrelRollRight(this));
		states.Add(typeof(TrainTransition), new TrainTransition(this));
		
		currentState = states[typeof(FlyingState)];
		currentState.OnEnter();
	}
	
	void Start () {
		VeinTrain[] trains = GameObject.FindObjectsOfType(typeof(VeinTrain)) as VeinTrain[];
		foreach (VeinTrain train in trains) {
			// only one train should be tagged "Player" at a time... find it!
			if (train.tag == "Player") {
				parentTrain = train;
				break;
			}
		}
		
		// initialize the next artery and next train
		Debug.Log(name + " calling " + parentTrain.nextArtery.name + "'s GenerateBranches();");
		parentTrain.nextArtery.GenerateBranches();
		parentTrain.TransitionToNextArtery();
		parentTrain.GenerateNextTrain();
	}
	
	public void EnterState (System.Type newStateType) {
		currentState.OnExit();
		currentState = states[newStateType];
		currentState.OnEnter();
	}

	void Update () {
		currentState.Update();
		
	}
	
	protected abstract class State {
		protected ShipController ship;
		public State (ShipController controller) {
			ship = controller;
		}
		virtual public void Update () {}
		virtual public void OnEnter () {}
		virtual public void OnExit () {}
	}
	
	protected class FlyingState : State {
		private bool canShoot = true;
		private bool canSuper = true;
		
		public FlyingState (ShipController c) : base (c) {}
		override public void Update () {
			if (Input.GetButton("Right Bumper")) {
				Debug.Log("Right Bumper");
				ship.EnterState(typeof(BarrelRollRight));
			}
			if (Input.GetButton("Left Bumper")) {
				Debug.Log("Left Bumper");
				ship.EnterState(typeof(BarrelRollLeft));
			}
			
			if (Input.GetButton ("Shoot") || Input.GetAxisRaw("Right Trigger") > 0.5) {
				TryShot ();
			}
			
			if (Input.GetButtonDown ("Fire1")) {
				TrySuper ();	
			}
			
			ship.h = ship.h*ship.smoothing + Input.GetAxis("Horizontal")*(1-ship.smoothing);
			ship.v = ship.v*ship.smoothing + Input.GetAxis("Vertical")*(1-ship.smoothing);
			
			ship.transform.localRotation = Quaternion.AngleAxis(ship.h*ship.maxHorizontalAngle, Vector3.up) * Quaternion.AngleAxis(ship.v*ship.maxVerticalAngle, Vector3.right);
			
			ship.transform.parent.Translate(ship.h*ship.horizontalSpeed, -ship.v*ship.verticalSpeed, 0);
			
			// this is (moderately) expensive, prolly shouldn't do it every frame!!! :/
			VirusScript[] viruses = ship.parentTrain.GetComponentsInChildren<VirusScript>();
			
			if (viruses.Length == 0) {
				Debug.Log("All viruses killed!!!!");
				ship.EnterState(typeof(TrainTransition));
			}
			
		}
		
		private void TryShot ()
		{
			if (canShoot)
			{
				ship.StartCoroutine(ShotDelayCoroutine());
				
				GameObject bullet = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Bullet"));
				bullet.transform.position = ship.transform.position;
				
				bullet.transform.rotation = ship.transform.rotation;
				bullet.transform.parent = ship.transform.parent.parent;
				
				bullet.transform.Translate(new Vector3(0f, 0.15f, 0.2f));
				
				bullet.transform.RotateAroundLocal(Vector3.right, -5f / 180 * Mathf.PI);
				
				AudioSource.PlayClipAtPoint((AudioClip)Resources.Load ("Shoot1"), new Vector3(0, 0, 0) , 1f);
			}
		}
		
		private void TrySuper ()
		{
			if (canSuper)
			{
				ship.StartCoroutine(SuperShotDelayCoroutine());
				
				GameObject sw = (GameObject)MonoBehaviour.Instantiate(Resources.Load("SuperWhy"), ship.transform.position + ship.transform.forward * 4.8f, ship.transform.rotation);
				
				sw.transform.parent = ship.transform;
			}
		}
		
		public IEnumerator ShotDelayCoroutine ()
		{
			canShoot = false;
			yield return new WaitForSeconds (0.15f);
			canShoot = true;
		}
		
		public IEnumerator SuperShotDelayCoroutine ()
		{
			canSuper = false;
			yield return new WaitForSeconds (60f);
			canSuper = true;
		}
	}
	
	protected class BarrelRollRight : State {
		public BarrelRollRight (ShipController c) : base (c) {}
		override public void OnEnter () {
			ship.StartCoroutine(BarrelRoll());
			iTween.RotateBy(ship.gameObject, 
			                iTween.Hash("amount", Vector3.back,
			                            "easetype", iTween.EaseType.easeOutQuint,
			                            "timeme", 1));
			iTween.MoveBy(ship.transform.parent.gameObject, 
			              iTween.Hash("amount", Vector3.right*4,
			                          "easetype", iTween.EaseType.easeInOutQuad,
			                          "time", 1));
		}
		IEnumerator BarrelRoll() {
			Debug.Log("do a barrel roll!! (right)");
			yield return new WaitForSeconds(1);
			ship.EnterState(typeof(FlyingState));
		}
	}
	
	protected class BarrelRollLeft : State {
		public BarrelRollLeft (ShipController c) : base (c) {}
		override public void OnEnter () {
			ship.StartCoroutine(BarrelRoll());
			iTween.RotateBy(ship.gameObject, 
			                iTween.Hash("amount", Vector3.forward,
			                            "easetype", iTween.EaseType.easeOutQuint,
			                            "timeme", 1));
			iTween.MoveBy(ship.transform.parent.gameObject, 
			              iTween.Hash("amount", Vector3.left*4,
			                          "easetype", iTween.EaseType.easeInOutQuad,
			                          "time", 1));
		}
		IEnumerator BarrelRoll() {
			Debug.Log("do a barrel roll!! (left)");
			yield return new WaitForSeconds(1);
			ship.EnterState(typeof(FlyingState));
		}
	}
	
	protected class TrainTransition : State {
		public TrainTransition (ShipController c) : base (c) {}
		
		private VeinTrain trainToDelete;
		
		override public void OnEnter () {
			trainToDelete = ship.parentTrain;
			ship.parentTrain = ship.parentTrain.nextTrain;
			ship.parentTrain.GenerateNextTrain();
			ship.transform.parent.parent = null;
		}
		
		override public void Update () {
			Vector3 dist = ship.transform.parent.position - ship.parentTrain.transform.position;
			if (dist.magnitude < 0.5) {
				iTween.Stop(ship.transform.parent.gameObject);
				ship.EnterState(typeof(FlyingState));
			} else {
				iTween.MoveUpdate(ship.transform.parent.gameObject,
				                  iTween.Hash("position", ship.parentTrain.transform,
				                              "looktarget", ship.parentTrain.transform,
				                              "time", 2));
			}
		}
		
		override public void OnExit () {
			ship.transform.parent.position = ship.parentTrain.transform.position;
			ship.transform.parent.rotation = ship.parentTrain.transform.rotation;
			ship.transform.parent.parent = ship.parentTrain.transform;
			ship.transform.parent.parent.tag = "Player";
			
			Debug.Log("destroying " + trainToDelete.name);
			GameObject.Destroy(trainToDelete.gameObject);
			
		}
	}
	
}
