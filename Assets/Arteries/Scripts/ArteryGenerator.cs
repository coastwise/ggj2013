using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Collider))]
public class ArteryGenerator : MonoBehaviour {
	
	private static int arteryCount = 0;
	
	private static Object[] prefabs;
	
	private bool doneGeneratingBranches = false;
	
	public List<Transform> branchRoots;
	
	public iTweenPath path;
	
	public ArteryGenerator prevArtery;
	
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
		if (doneGeneratingBranches) {
			Debug.Log("someone attempted to generate " + name + "'s branches twice... ");
			return;
		}
		
		Debug.Log(name + " generating branches...");
		foreach (Transform branchRoot in branchRoots) {
			Object randomPrefab = prefabs[Random.Range(0, prefabs.Length)];
			GameObject branch = Instantiate(randomPrefab, branchRoot.position, branchRoot.rotation) as GameObject;
			ArteryGenerator artery = branch.GetComponentInChildren<ArteryGenerator>();
			artery.prevArtery = this;
		}
		
		if (prevArtery != null) {
			// wait a few secs then destroy old artery
			Destroy(prevArtery.transform.parent.gameObject, 10);
		}
		
		doneGeneratingBranches = true;
	}
	
}
