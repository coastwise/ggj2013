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
	private int mAntibodies = 0;
	private Vector3 randomIdlePoint = new Vector3(0, 0, 0);
	void Start () {
		Init();
	}
	
	public void Init () {
		state = new WBCStateIdle(this);
		
		WBCAIScript ai = (WBCAIScript)gameObject.AddComponent("WBCAIScript");
		gameObject.AddComponent("Rotation");
	}

	public void SetState (WBCState state)
	{
		this.state = state;	
	}
	
	// Update is called once per frame
	void Update () {
		state.Execute();
	}
	 
	
	public void IdleInit ()
	{
		StartCoroutine ("IdleRandomPositionCoroutine");	
	}
	
	private IEnumerator IdleRandomPositionCoroutine ()
	{
		randomIdlePoint = transform.position;
		
		randomIdlePoint.x += Random.Range(-3, 3);
		randomIdlePoint.y += Random.Range(-3, 3);
		randomIdlePoint.z += Random.Range(-3, 3);
		
		yield return new WaitForSeconds(Random.Range(3.0f, 4.0f));
		
		StartCoroutine ("IdleRandomPositionCoroutine");
	}
	
	public void Idle ()
	{
		Vector3 dir = randomIdlePoint - transform.position;
		dir.Normalize();
		
		dir *= moveVel *0.2f;
		
		transform.Translate(dir * Time.deltaTime);
	}
	
	public void SetClosestTarget ()
	{	
		RBCScript closestRBC = null;
		
		foreach (RBCScript rbc in RBCList)
		{
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
		return RBCList.Count > 0;
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


	public void SeekInit ()
	{
		
	}
	
	public void Seek ()
	{
		if (mIsGrabOutOfRange && target != null)
		{
			Vector3 wbcPos = transform.position;
			
			Vector3 targetPos = target.transform.position;
			
			Vector3 dir = targetPos - wbcPos;
			dir.Normalize();
			
			
			transform.Translate(dir * moveVel * Time.deltaTime, Space.World);
		}
	}
	
	public void DamageOverTime ()
	{
		target.ApplyDamage(100 * Time.deltaTime);
		
		
		if (IsAttachedTargetDead())
		{
			Detach();	
		}
		
		RBCList.Remove(target);
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
		} else if (other.tag == "RBCDetectionSphere")
		{
			RBCDetectionSphere rbcDetSph = other.GetComponent<RBCDetectionSphere>();
			RBCScript rbc = rbcDetSph.GetRBC();
			RBCList.Add(rbc);
		}
	}
	
	public bool IsAttached ()
	{
		return mAttached;	
	}
	
	public void Detach ()
	{
		mAttached = false;
		mIsGrabOutOfRange = true;
		transform.parent = target.transform.parent;
	}
	
	public int GetAntibodyCount() {
		return mAntibodies;	
	}
}
