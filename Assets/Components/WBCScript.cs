using UnityEngine;
using System.Collections;

public class WBCScript : MonoBehaviour {
	
	private WBCState state;
	
	void Start () {
		Init();
	}
	
	public void Init () {
		state = new WBCStateIdle(this);
		
		WBCAIScript ai = (WBCAIScript)gameObject.AddComponent("WBCAIScript");
		
		
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
