using UnityEngine;
using System.Collections;

public class WBCStateIdle : WBCState {
	
	public WBCStateIdle (WBCScript wbc)
		: base (wbc) {
		wbc.IdleInit();
	}
	
	public override void Execute ()
	{
		wbc.Idle();
	}
}
