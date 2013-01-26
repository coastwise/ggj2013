using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
public class VeinTrain : MonoBehaviour {
	
	public float bpm = 45;
	public float flow = 10;
	
	public float deltaZ = 0;
	public float pathProgress = 0;
	
	public Vector3[] path;
	public float pathLength;

	void OnTriggerEnter (Collider other) {
		
		ArteryGenerator artery = other.gameObject.GetComponent<ArteryGenerator>();
		
		if (artery == null) {
			Debug.Log("TODO: put the veintrain colliders and stuff in layers... its still hitting other stuff!");
			return;
		}
		
		path = artery.path.nodes.ToArray();
		pathLength = Vector3.Distance(path[0], path[path.Length-1]); // estimate (no curve)
	}
	
	void Update () {
		animation["Pulse"].speed = bpm / 60;
		
		if (path == null || path.Length < 2) return;
		
		pathProgress += deltaZ * Time.deltaTime * flow / pathLength;
		
		Vector3 pointOnPath = iTween.PointOnPath(path, pathProgress);
		Vector3 lookTarget = iTween.PointOnPath(path, pathProgress + 0.01f);
		iTween.MoveUpdate(gameObject, 
		                  iTween.Hash("position", pointOnPath,
		                              "looktarget", lookTarget,
		                              "time", 0));
	}
	
}
