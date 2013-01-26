using UnityEngine;
using System.Collections;

public class WBCStateDamage : WBCState {
	
	public WBCStateDamage (WBCScript wbc)
		: base (wbc) {
	}
	
	public override void Execute ()
	{
		wbc.DamageOverTime();
	}
}
