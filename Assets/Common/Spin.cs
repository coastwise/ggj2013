using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {

	public Vector3 axis = Vector3.up;
	public float degreesPerSecond = 20;
	
	void Update () {
		float angle = degreesPerSecond * Time.deltaTime;
		transform.Rotate(axis, angle);
	}
	
}
