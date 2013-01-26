using UnityEngine;
using System.Collections;

public class RBCStateFlee : RBCState {

	public RBCStateFlee (RBCScript rbc)
		: base (rbc) {}
	
	public override void Execute ()
	{
		rbc.Flee();
	}
}
