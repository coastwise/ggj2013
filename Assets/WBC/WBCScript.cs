using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WBCScript : MonoBehaviour {
	
	private WBCState state;
	public List<RBCScript> RBCList = new List<RBCScript>();
	private RBCScript target = null;
	private float grabRange = 1f;
	private float moveVel = 2f;
	private bool mIsGrabOutOfRange = true;
	private bool mAttached = false;
	
	void Start () {
		Init();
	}
	
	public void Init () {
		state = new WBCStateIdle(this);
		
		WBCAIScript ai = (WBCAIScript)gameObject.AddComponent("WBCAIScript");
	}

	public void SetState (WBCState state)
	{
		this.state = state;	
	}
	
	// Update is called once per frame
	void Update () {
		state.Execute();
	}
	 	
	public void SetClosestTarget ()
	{
		// from the list of free targets, compute the closest target and set it as the desired target
		List<RBCScript> rbclist = new List<RBCScript>();
		
		GameObject[] gos = GameObject.FindGameObjectsWithTag("RBC");
		
		RBCScript closestRBC = null;
		
		foreach (GameObject go in gos)
		{
			RBCScript rbc = go.GetComponent<RBCScript>();
			if (rbc.IsInfected())
			{
				if (closestRBC == null) {
					closestRBC = rbc;
					continue;
				}
				if (Vector3.Distance(closestRBC.transform.position, transform.position) > Vector3.Distance(rbc.transform.position, transform.position))
				{
					closestRBC = rbc;
				}
			}
		}
		
		if (closestRBC != null)
		{
			target = closestRBC;
		} else {
			Debug.Log ("Uh oh, something bad happenszed! Report to cyber po-po");	
		}
	}
	
	public bool DetectsFreeTargets ()
	{
		//return RBCList.Count > 0;
		return true;
	}
	
	public bool IsGrabOutOfRange ()
	{	
		return mIsGrabOutOfRange;
	}
	
	public bool IsAttachedTargetDead ()
	{
		return false;	
	}


	public void SeekInit ()
	{
		
	}
	
	public void Seek ()
	{
		if (mIsGrabOutOfRange)
		{
			Vector3 wbcPos = transform.position;
			
			Vector3 targetPos = target.transform.position;
			
			Vector3 dir = targetPos - wbcPos;
			dir.Normalize();
			
			transform.Translate(dir * moveVel * Time.deltaTime);
		}
	}
	
	public void DamageOverTime ()
	{
		target.ApplyDamage(200 * Time.deltaTime);	
	}
	
	public void OnTriggerEnter (Collider other)
	{
		
		if (other.tag == "RBC")
		{
			Debug.Log("Houston we are on the moon");
			RBCScript rbc = other.GetComponent<RBCScript>();
			
			if (target.Equals(rbc))
			{
				mIsGrabOutOfRange = false;
				
				transform.parent = other.transform;
				mAttached = true;
			}
		}
	}
}
