using UnityEngine;
using System.Collections;

public abstract class WBCState {
	
	protected WBCScript wbc;
	
	// Use this for initialization
	public WBCState (WBCScript wbc) {
		this.wbc = wbc;
	}
	
	public abstract void Execute ();
}
