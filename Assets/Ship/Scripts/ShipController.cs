using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {
	
	public float maxHorizontalAngle = 40;
	public float maxVerticalAngle = 40;
	public float horizontalSpeed = 0.1f;
	public float verticalSpeed = 0.1f;

	void Update () {
		
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		
		transform.rotation = Quaternion.AngleAxis(h*maxHorizontalAngle, Vector3.up) * Quaternion.AngleAxis(v*maxVerticalAngle, Vector3.right);
		
		transform.parent.Translate(h*horizontalSpeed, -v*verticalSpeed, 0);
	}
}
