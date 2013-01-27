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
			
			if (Input.GetButtonDown ("Jump")) {
				TryShot ();
			}
			
			ship.h = ship.h*ship.smoothing + Input.GetAxis("Horizontal")*(1-ship.smoothing);
			ship.v = ship.v*ship.smoothing + Input.GetAxis("Vertical")*(1-ship.smoothing);
			
			ship.transform.localRotation = Quaternion.AngleAxis(ship.h*ship.maxHorizontalAngle, Vector3.up) * Quaternion.AngleAxis(ship.v*ship.maxVerticalAngle, Vector3.right);
			
			ship.transform.parent.Translate(ship.h*ship.horizontalSpeed, -ship.v*ship.verticalSpeed, 0);
			
			// this is (moderately) expensive, prolly shouldn't do it every frame!!! :/
			VirusScript[] viruses = ship.parentTrain.GetComponentsInChildren<VirusScript>();
			Debug.Log("Virus count: " + viruses.Length);
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
			}
		}
		
		public IEnumerator ShotDelayCoroutine ()
		{
			canShoot = false;
			yield return new WaitForSeconds (0.5f);
			canShoot = true;
		}
	}
	
	protected class BarrelRollRight : State {
		public BarrelRollRight (ShipController c) : base (c) {}
		override public void OnEnter () {
			ship.StartCoroutine(BarrelRoll());
			iTween.RotateBy(ship.gameObject, Vector3.back, 1f);
		}
		override public void Update () {
			ship.transform.parent.Translate(0.1f,0,0);
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
			iTween.RotateBy(ship.gameObject, Vector3.forward, 1);
		}
		override public void Update () {
			ship.transform.parent.Translate(-0.1f,0,0);
		}
		IEnumerator BarrelRoll() {
			Debug.Log("do a barrel roll!! (left)");
			yield return new WaitForSeconds(1);
			ship.EnterState(typeof(FlyingState));
		}
	}
	
	protected class TrainTransition : State {
		public TrainTransition (ShipController c) : base (c) {}
		override public void OnEnter () {
			Debug.Log("Ship controller train transition; parent: " + ship.transform.parent.name + ", grandparent: " + ship.transform.parent.parent.name);
			Debug.Log("ship controller next train pregen: " + ship.parentTrain.nextTrain.name);
			ship.parentTrain.nextTrain.GenerateNextTrain();
			Debug.Log("ship controller next train postgen: " + ship.parentTrain.nextTrain.name);
			ship.transform.parent.position = ship.parentTrain.nextTrain.transform.position;
			ship.transform.parent.rotation = ship.parentTrain.nextTrain.transform.rotation;
			ship.transform.parent.parent = ship.parentTrain.nextTrain.transform;
			ship.parentTrain = ship.parentTrain.nextTrain;
			//ship.transform.parent.localRotation = Quaternion.identity;
			ship.EnterState(typeof(FlyingState));
		}
	}
	
}
