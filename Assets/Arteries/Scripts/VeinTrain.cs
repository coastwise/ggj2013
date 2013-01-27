using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
public class VeinTrain : MonoBehaviour {
	
	private static int trainCount = 0;
	
	public float bpm = 45;
	public float flow = 10;
	
	public float deltaZ = 0;
	public float pathProgress = 0;
	
	public Vector3[] path;
	public float pathLength;
	
	public ArteryGenerator nextArtery;
	
	public VeinTrain nextTrain;
	private static Object[] trainPrefabs;
	
	public void Awake () {
		if (trainPrefabs == null) {
			trainPrefabs = Resources.LoadAll("TrainPrefabs", typeof(GameObject));
		}
	}
	
	public void Start () {
		name = "Vein Train " + trainCount;
		trainCount = trainCount + 1;
		
		pathLength = Vector3.Distance(path[0], path[path.Length-1]); // estimate (no curve)
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
			// we've finally reached the end of the iTweenPath, so we can move to the next one
			TransitionToNextArtery();
			pathProgress -= 1f; // if we went over by a bit, we don't want to skip backwards
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
	
	public void TransitionToNextArtery () {
		Debug.Log("Vein Train moving to path: " + nextArtery.name);
		path = nextArtery.path.nodes.ToArray();
		pathLength = Vector3.Distance(path[0], path[path.Length-1]); // estimate (no curve)
		//nextArtery = null; // null this out just in case
	}
	
	public void GenerateNextTrain () {
		if (nextTrain != null) {
			Debug.Log("ERROR: " + name + " asked to re-generate next train");
			return;
		} else {
			Debug.Log(name + " generating next train");
		}
		
		// figure out where to spawn the next train
		float nextTrainProgress = pathProgress + 0.25f;
		Vector3[] nextTrainPath = path;
		
		if (nextTrainProgress >= 1f) {
			nextTrainProgress -= 1f;
			nextTrainPath = nextArtery.path.nodes.ToArray();
		}
		
		Vector3 pointOnPath = iTween.PointOnPath(nextTrainPath, nextTrainProgress);
		Vector3 lookTarget = iTween.PointOnPath(nextTrainPath, nextTrainProgress + 0.01f);
		
		// pick the prefab
		Object randomPrefab = trainPrefabs[Random.Range(0, trainPrefabs.Length)];
		
		// spawn and let itween place it just so...
		GameObject go = Instantiate(randomPrefab) as GameObject;
		VeinTrain train = go.GetComponent<VeinTrain>();
		iTween.MoveUpdate(go, 
		                  iTween.Hash("position", pointOnPath,
		                              "looktarget", lookTarget,
		                              "time", 0));
		train.path = nextTrainPath;
		train.pathProgress = nextTrainProgress;
		train.nextArtery = nextArtery;
		
		nextTrain = train;
	}
	
}
