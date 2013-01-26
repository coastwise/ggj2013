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
	
	public ArteryGenerator nextArtery;
	
	public void Start () {
		pathLength = 1;
		nextArtery.GenerateBranches();
	}

	void OnTriggerEnter (Collider other) {
		
		ArteryGenerator artery = other.gameObject.GetComponent<ArteryGenerator>();
		
		if (artery == null) {
			Debug.Log("TODO: put the veintrain colliders and stuff in layers... its still hitting other stuff!");
			return;
		} else if (artery != nextArtery) { // we only want to do this once... sometimes it triggers more often
			// we don't want to null this out by accident...
			// so can't assign straight from GetComponent
			nextArtery = artery;
			nextArtery.GenerateBranches();
		}
	}
	
	void Update () {
		animation["Pulse"].speed = bpm / 60;
		
		if (pathProgress >= 1) {
			Debug.Log("Vein Train moving to path: " + nextArtery.name);
			
			// we've finally reached the end of the iTweenPath, so we can move to the next one
			path = nextArtery.path.nodes.ToArray();
			pathLength = Vector3.Distance(path[0], path[path.Length-1]); // estimate (no curve)
			pathProgress -= 1f; // if we went over by a bit, we don't want to skip backwards
			nextArtery = null; // null this out just in case
		}
		
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
