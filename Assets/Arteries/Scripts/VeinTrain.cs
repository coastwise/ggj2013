using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
public class VeinTrain : MonoBehaviour {
	
	public float deltaZ = 0;
	public float pathProgress = 0;
	
	public Vector3[] path;

	void OnTriggerEnter (Collider other) {
		
		ArteryGenerator artery = other.gameObject.GetComponent<ArteryGenerator>();
		
		if (artery == null) {
			Debug.Log("TODO: put the veintrain colliders and stuff in layers... its still hitting other stuff!");
			return;
		}
		
		/*
		iTween.MoveTo(gameObject, 
		              iTween.Hash("path", artery.path.nodes.ToArray(),
		                          "orienttopath", true));
		*/
		
		path = artery.path.nodes.ToArray();
	}
	
	void Update () {
		if (path == null || path.Length < 2) return;
		
		pathProgress += deltaZ/500;
		
		Vector3 pointOnPath = iTween.PointOnPath(path, pathProgress);
		Vector3 lookTarget = iTween.PointOnPath(path, pathProgress + 0.01f);
		iTween.MoveUpdate(gameObject, 
		                  iTween.Hash("position", pointOnPath,
		                              "looktarget", lookTarget,
		                              "time", 0));
	}
	
}
