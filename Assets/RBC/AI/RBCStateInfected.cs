using UnityEngine;
using System.Collections;

public class RBCStateInfected : RBCState {

	public RBCStateInfected (RBCScript rbc)
		: base (rbc) {
		rbc.InfectedInit();
	}
	
	public override void Execute ()
	{
		rbc.Infected();
	}
}
