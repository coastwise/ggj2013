using UnityEngine;
using System.Collections;

public class RBCDetectionSphere : MonoBehaviour {
	
	private RBCScript rbc;
	// Use this for initialization
	void Start () {
		
		rbc = transform.parent.GetComponent<RBCScript>();
		float v = Random.Range(1, 5);
		Vector3 scale = new Vector3(v,v,v);
		transform.localScale = scale;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 scale = transform.localScale;
		
		scale += new Vector3(0.5f,0.5f,0.5f) * Time.deltaTime;
		
		transform.localScale = scale;
	}
	
	public RBCScript GetRBC ()
	{
		return rbc;
	}
}
