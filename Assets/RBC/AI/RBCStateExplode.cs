using UnityEngine;
using System.Collections;

public class RBCStateExplode : RBCState {

	public RBCStateExplode (RBCScript rbc)
		: base (rbc) {
		rbc.ExplodeInit();
	}
	
	public override void Execute ()
	{
		rbc.Explode();
	}
}
