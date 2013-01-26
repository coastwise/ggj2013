using UnityEngine;
using System.Collections;

public abstract class RBCState {
	
	protected RBCScript rbc;
	
	public RBCState (RBCScript rbc)
	{
		this.rbc = rbc;
	}
	
	public abstract void Execute ();
}
