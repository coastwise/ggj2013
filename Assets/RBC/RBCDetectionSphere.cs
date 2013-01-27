using UnityEngine;
using System.Collections;

public class RBCDetectionSphere : MonoBehaviour {
	
	private RBCScript rbc;
	// Use this for initialization
	void Start () {
		
		rbc = transform.parent.GetComponent<RBCScript>();
		float v = 2 * rbc.AntibodyCount() + 3;
		Vector3 scale = new Vector3(v,v,v);
		transform.localScale = scale;
	}
	
	// Update is called once per frame
	void Update () {
		float v = 2 * rbc.AntibodyCount() + 3;
		Vector3 scale = new Vector3(v,v,v);
		transform.localScale = scale;
	}
	
	public RBCScript GetRBC ()
	{
		return rbc;
	}
}
