using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {
	
	public float maxHorizontalAngle = 40;
	public float maxVerticalAngle = 40;
	public float horizontalSpeed = 0.1f;
	public float verticalSpeed = 0.1f;
	
	private float h = 0;
	private float v = 0;
	
	public float smoothing = 0.93f;

	void Update () {
		
		h = h*smoothing + Input.GetAxis("Horizontal")*(1-smoothing);
		v = v*smoothing + Input.GetAxis("Vertical")*(1-smoothing);
		
		transform.rotation = Quaternion.AngleAxis(h*maxHorizontalAngle, Vector3.up) * Quaternion.AngleAxis(v*maxVerticalAngle, Vector3.right);
		
		transform.parent.Translate(h*horizontalSpeed, -v*verticalSpeed, 0);
	}
}
