using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Collider))]
public class ArteryGenerator : MonoBehaviour {
	
	private static int arteryCount = 0;
	
	private static Object[] prefabs;
	
	public List<Transform> branchRoots;
	
	public iTweenPath path;
	
	void Awake () {
		if (prefabs == null) {
			prefabs = Resources.LoadAll("ArteryPrefabs", typeof(GameObject));
		}
	}
	
	void Start () {
		name = "Artery " + arteryCount;
		arteryCount = arteryCount + 1;
		
		// fix for stupid iTweenPath not using local coordinates
		// the idea of moving the whole path never crossed anyone's mind?? jeez
		for (int i = 0; i < path.nodes.Count; i++) {
			path.nodes[i] = gameObject.transform.TransformPoint(path.nodes[i]);
		}
	}
	
	public void GenerateBranches () {
		Debug.Log(name + " generating branches...");
		foreach (Transform branchRoot in branchRoots) {
			Object randomPrefab = prefabs[Random.Range(0, prefabs.Length)];
			GameObject branch = Instantiate(randomPrefab, branchRoot.position, branchRoot.rotation) as GameObject;
		}	
	}
	
}
