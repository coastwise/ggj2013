using UnityEngine;
using System.Collections;

public class RBCScript : MonoBehaviour {
	
	private RBCState state;
	
	private float mInfectionTime;
	private float mExplosionDuration;
	private bool mShouldExplode;
	private bool mIsInfected;
	private bool mIsThreatDetected;
	private bool mIsKilled;
	
	private float mHealth;
	
	private int mAntibodies;
	
	// Use this for initialization
	void Start () {
		mIsInfected = false;
		mInfectionTime = 100f;
		mExplosionDuration = 3f;
		mShouldExplode = false;
		mHealth = 1000;
		
		state = new RBCStateIdle(this);
		
		RBCAIScript ai = (RBCAIScript)gameObject.AddComponent("RBCAIScript");
		gameObject.AddComponent("Rotation");
	}
	
	// Update is called once per frame
	void Update () {
		state.Execute();
	}
	
	public bool IsKilled ()
	{
		return mIsKilled;	
	}
	
	public void SetState (RBCState state)
	{
		this.state = state;
	}
	
	public void KillInit ()
	{
		StartCoroutine("KillDeleteCoroutine");	
	}
	
	private IEnumerator KillDeleteCoroutine ()
	{
		yield return new WaitForSeconds (2.0f);
		Destroy(gameObject);	
	}
	
	public void Kill ()
	{
		//no-op
		//increment score?
	}
	
	public bool IsInfected ()
	{
		return mIsInfected;	
	}
	
	public void InfectedInit ()
	{
		StartCoroutine("InfectionTimer");
	}
	
	private IEnumerator InfectionTimer ()
	{
		GameObject go = (GameObject)Instantiate(Resources.Load("DetectionSphere"));
		
		go.transform.parent = transform;
		go.transform.localPosition = new Vector3(0, 0, 0);
		yield return new WaitForSeconds(mInfectionTime);
		mShouldExplode = true;
	}
	
	public void Infected ()
	{
		// Seek to player
	}
	
	public void SetInfected ()
	{
		mIsInfected = true;
	}
	
	
	public bool CheckShouldExplode ()
	{
		return mShouldExplode;
	}
	
	public void ExplodeInit ()
	{
		StartCoroutine("ExplosionDeleteCoroutine");
		StartCoroutine("ExplosionSpawnVirus");
		// Add a sphere collider
		
	}
	
	private IEnumerator ExplosionDeleteCoroutine ()
	{
		yield return new WaitForSeconds(mExplosionDuration);
		Destroy(gameObject);
	}
	
	private IEnumerator ExplosionSpawnVirus ()
	{
		yield return new WaitForSeconds(0.2f);
		// Spawn virus
	}
	
	public void Explode ()
	{
		// increase sphere collider size over time
	}
	
	
	public bool IsThreatDetected ()
	{
		return mIsThreatDetected;	
	}
	
	
	public void SetThreatDetected (bool val)
	{
		mIsThreatDetected = val;	
	}
	
	public void FleeInit ()
	{
			
	}
	
	public void Flee ()
	{
		// find nearest threat
		
		// run away from nearest threat
	}
	
	public void IdleInit ()
	{
		// no-op
	}
	
	public void Idle ()
	{
		// no-op	
	}
	
	public void ApplyDamage (float damage)
	{
		mHealth -= damage;
		mIsKilled = mHealth <= 0;	
	}
	
	public void IncrementAB ()
	{
		mAntibodies++;
	}
	
	public int AntibodyCount ()
	{
		return mAntibodies;	
	}
}
