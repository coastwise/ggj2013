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
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.tag != "Player") return;
		
		Debug.Log(name + " generating branches...");
		foreach (Transform branchRoot in branchRoots) {
			Object randomPrefab = prefabs[Random.Range(0, prefabs.Length)];
			GameObject branch = Instantiate(randomPrefab, branchRoot.position, branchRoot.rotation) as GameObject;
		}	
	}
	
}
