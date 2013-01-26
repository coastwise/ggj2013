using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour {
	
	public float degreesPerSecond;
	
	public float maximumRotation;
	public float minimumRotation;
	
	public bool randomizeRotation = true;
	
	// Use this for initialization
	void Start () {
	
		if (randomizeRotation)
		{
			maximumRotation = 45;
			minimumRotation = -45;
			degreesPerSecond = Random.Range(minimumRotation, maximumRotation);
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(degreesPerSecond * Time.deltaTime, degreesPerSecond * Time.deltaTime, degreesPerSecond * Time.deltaTime));
	}
}
