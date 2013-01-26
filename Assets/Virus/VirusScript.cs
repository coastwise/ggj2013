using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VirusScript : MonoBehaviour {
	
	private VirusState state;
	public List<RBCScript> RBCList = new List<RBCScript>();
	private RBCScript target = null;
	private float grabRange = 1f;
	private float moveVel = 2f;
	private bool mIsGrabOutOfRange = true;
	private bool mAttached = false;
	private int mAntibodies = 0;
	
	void Start () {
		Init();
	}
	
	public void Init () {
		state = new VirusStateIdle(this);
		
		VirusAIScript ai = (VirusAIScript)gameObject.AddComponent("VirusAIScript");
	}

	public void SetState (VirusState state)
	{
		this.state = state;	
	}
	
	// Update is called once per frame
	void Update () {
		state.Execute();
	}
	 	
	public void SetClosestTarget ()
	{	
		GameObject[] gos = GameObject.FindGameObjectsWithTag("RBC");
		RBCList.Clear();
		foreach (GameObject go in gos)
		{
			RBCScript rbc = go.GetComponent<RBCScript>();
			if (rbc.IsKilled() || rbc.IsInfected())
				continue;
		
			RBCList.Add(rbc);	
		}
		
		RBCScript closestRBC = null;
		
		foreach (RBCScript rbc in RBCList)
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
		
		if (closestRBC != null)
		{
			target = closestRBC;
		} else {
			Debug.Log ("Uh oh, something bad happenszed! Report to cyber po-po");	
		}
	}
	
	public bool DetectsFreeTargets ()
	{
		GameObject[] gos = GameObject.FindGameObjectsWithTag("RBC");
		
		return gos.Length > 0;
	}
	
	public bool IsGrabOutOfRange ()
	{	
		return mIsGrabOutOfRange;
	}
	
	public bool IsAttachedTargetDead ()
	{
		if (target == null)
			return true;
		
		return target.IsKilled();	
	}
	
	public bool IsAttachedTargetInfected ()
	{
		if (target == null)
			return true;
		
		return target.IsInfected();	
	}


	public void SeekInit ()
	{
		
	}
	
	public void Seek ()
	{
		if (mIsGrabOutOfRange && target != null)
		{
			if (target.IsInfected())
			{
				return;	
			}
			Vector3 wbcPos = transform.position;
			
			Vector3 targetPos = target.transform.position;
			
			Vector3 dir = targetPos - wbcPos;
			dir.Normalize();
			
			transform.Translate(dir * moveVel * Time.deltaTime);
		}
	}
	
	public void DamageOverTime ()
	{
		target.SetInfected();
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
	
	public bool IsAttached ()
	{
		return mAttached;	
	}

	public int GetAntibodyCount() {
		return mAntibodies;	
	}
}
