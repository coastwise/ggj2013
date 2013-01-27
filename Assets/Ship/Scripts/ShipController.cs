using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipController : MonoBehaviour {
	
	public float maxHorizontalAngle = 80;
	public float maxVerticalAngle = 70;
	public float maxRollAngle = 40;
	public float horizontalSpeed = 0.1f;
	public float verticalSpeed = 0.1f;
	public float strafeSpeed = 0.1f;
	
	protected float h = 0;
	protected float v = 0;
	
	protected float r = 0;
	
	public float smoothing = 0.93f;
	
	private Dictionary<System.Type, State> states;
	private State currentState;
	
	void Awake () {
		states = new Dictionary<System.Type, State> ();
		states.Add(typeof(FlyingState), new FlyingState(this));
		states.Add(typeof(BarrelRollLeft), new BarrelRollLeft(this));
		states.Add(typeof(BarrelRollRight), new BarrelRollRight(this));
		
		currentState = states[typeof(FlyingState)];
		currentState.OnEnter();
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
			
			
			
			if (Input.GetAxis ("Right Trigger") != 0) {
				TryShot ();
			}
			
			
			ship.h = ship.h*ship.smoothing + Input.GetAxis("Horizontal Right")*(1-ship.smoothing);
			ship.v = ship.v*ship.smoothing - Input.GetAxis("Vertical Right")*(1-ship.smoothing);
			
			ship.transform.localRotation = Quaternion.AngleAxis(ship.h*ship.maxHorizontalAngle, Vector3.up) * Quaternion.AngleAxis(ship.v*ship.maxVerticalAngle, Vector3.right);
			
			ship.transform.parent.parent.Translate(ship.h*ship.horizontalSpeed, -ship.v*ship.verticalSpeed, 0);
			
			Transform pivot = ship.transform.parent;
			
			ship.r = ship.r*ship.smoothing + Input.GetAxis("Horizontal")*(1-ship.smoothing);
			
			
			
			pivot.localRotation = Quaternion.AngleAxis(ship.r*ship.maxRollAngle, -Vector3.forward);
			
			pivot.parent.Translate(ship.r*ship.strafeSpeed, 0, 0);
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
				
				GameObject bullet2 = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Bullet"));
				bullet2.transform.position = ship.transform.position;
				
				bullet2.transform.rotation = ship.transform.rotation;
				bullet2.transform.parent = ship.transform.parent.parent;
				
				bullet.transform.Translate(new Vector3(-0.2f, 0.15f, 0.2f));
				bullet2.transform.Translate(new Vector3(0.2f, 0.15f, 0.2f));
				
				bullet.transform.RotateAroundLocal(Vector3.right, -5f / 180 * Mathf.PI);
				bullet2.transform.RotateAroundLocal(Vector3.right, -5f / 180 * Mathf.PI);
				
				AudioSource.PlayClipAtPoint((AudioClip)Resources.Load ("Shoot1"), new Vector3(0, 0, 0) , 1f);
			}
		}
		
		public IEnumerator ShotDelayCoroutine ()
		{
			canShoot = false;
			yield return new WaitForSeconds (0.15f);
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
			ship.transform.parent.parent.Translate(0.1f,0,0);
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
			ship.transform.parent.parent.Translate(-0.1f,0,0);
		}
		IEnumerator BarrelRoll() {
			Debug.Log("do a barrel roll!! (left)");
			yield return new WaitForSeconds(1);
			ship.EnterState(typeof(FlyingState));
		}
	}
	
}
