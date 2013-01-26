using UnityEngine;
using System.Collections;

public class RBCStateIdle : RBCState {

	public RBCStateIdle (RBCScript rbc)
		: base (rbc) {}
	
	public override void Execute ()
	{
		rbc.Idle();
	}
}
