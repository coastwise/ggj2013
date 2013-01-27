using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VirusScript : MonoBehaviour {
	
	private VirusState state;
	public List<RBCScript> RBCList = new List<RBCScript>();
	private RBCScript target = null;
	private float grabRange = 1f;
	private float moveVel = 1f;
	private bool mIsGrabOutOfRange = true;
	private bool mAttached = false;
	private int mAntibodies = 0;
	
	void Start () {
		Init();
	}
	
	public void Init () {
		state = new VirusStateBirth(this, new Vector3(0,0,0));
		
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
		GameObject train = transform.parent.gameObject;
		RBCScript[] rbcs = train.GetComponentsInChildren<RBCScript>();
		
		RBCList.Clear();
		foreach (RBCScript rbc in rbcs)
		{
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
			
			transform.Translate(dir * moveVel * Time.deltaTime, Space.World);
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
			
			if (target == rbc)
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
	
	public void IncrementAB ()
	{
		mAntibodies++;
		if (mAntibodies > 2)
		{
			Destroy(gameObject);	
		}
	}

	public int GetAntibodyCount() {
		return mAntibodies;	
	}
}
