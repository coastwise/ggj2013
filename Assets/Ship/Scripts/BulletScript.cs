using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
	
	private bool collided = false;
	private float timer = 0;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		
		if (!collided) {
			if (timer > 3f)
			{
				Destroy(gameObject);	
			}
			
			transform.Translate(Vector3.forward * 10f * Time.deltaTime);
		}
	}
	
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "RBC")
		{
			RBCScript rbc = other.GetComponent<RBCScript>();
			
			collided = true;
			transform.parent = rbc.transform;
			rbc.IncrementAB();
			
		}
	}
}
