using UnityEngine;
using System.Collections;

public abstract class WBCState {
	
	protected WBCScript ai;
	
	// Use this for initialization
	public WBCState (WBCScript ai) {
		this.ai = ai;
	}
	
	public abstract void Execute ();
}
