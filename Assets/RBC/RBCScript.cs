using UnityEngine;
using System.Collections;

public class RBCScript : MonoBehaviour {
	
	private RBCState state;
	private ShipController ship;
	private float mInfectionTime;
	private float mExplosionDuration;
	private bool mShouldExplode;
	private bool mIsInfected;
	private bool mIsThreatDetected;
	private bool mIsKilled;
	
	private float mHealth;
	private float moveVel = 0.1f;
	private int mAntibodies;
	
	// Use this for initialization
	void Start () {
		ship = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ShipController>();
		mIsInfected = false;
		mInfectionTime = 20f;
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
		if (ship != null && Vector3.Distance(transform.position, ship.transform.position) > 2f)
		{
			Vector3 rbcPos = transform.position;
			
			Vector3 targetPos = ship.transform.position;
			
			Vector3 dir = targetPos - rbcPos;
			dir.Normalize();
			
			transform.Translate(dir * moveVel * Time.deltaTime, Space.World);
		} else if (ship == null)
		{
			ship = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ShipController>();			
		}
	
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
		StartCoroutine("ExplosionSpawnVirus");
		StartCoroutine("ExplosionDeleteCoroutine");
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
		gameObject.tag = "Untagged";
		GameObject go1 = (GameObject)Instantiate(Resources.Load ("virus" + (int)Random.Range(1, 4)), transform.position, transform.rotation);
		VirusScript v1 = go1.GetComponent<VirusScript>();
		v1.SetState(new VirusStateBirth(v1, Vector3.right));
		v1.transform.parent = transform.parent;
		
		GameObject go2 = (GameObject)Instantiate(Resources.Load ("virus" + (int)Random.Range(1, 4)), transform.position, transform.rotation);
		VirusScript v2 = go2.GetComponent<VirusScript>();
		v2.SetState(new VirusStateBirth(v2, -Vector3.right));
		v2.transform.parent = transform.parent;
			
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
