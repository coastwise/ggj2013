using UnityEngine;
using System.Collections;

public class RBCStateKill : RBCState {

	public RBCStateKill (RBCScript rbc)
		: base (rbc) {
		rbc.KillInit();
	}
	
	public override void Execute ()
	{
		rbc.Kill();
	}
}
