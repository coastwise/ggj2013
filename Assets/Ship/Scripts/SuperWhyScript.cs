using UnityEngine;
using System.Collections;

public class SuperWhyScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy(gameObject, 5f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAroundLocal(Vector3.forward, 30f * Time.deltaTime);
	}
	
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Virus")
		{
			VirusScript v = other.GetComponent<VirusScript>();
			
			if (!v.IsAttached())
			{
				Destroy(v.gameObject);	
			}
		} else if (other.tag == "RBC")
		{
			RBCScript rbc = other.GetComponent<RBCScript>();
			
			if (rbc.IsInfected())
			{
				Destroy(rbc.gameObject);	
			}
		}
	}
}
