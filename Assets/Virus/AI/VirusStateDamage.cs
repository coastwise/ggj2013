using UnityEngine;
using System.Collections;

public class VirusStateDamage : VirusState {

	public VirusStateDamage (VirusScript virus)
		: base (virus) {
		
	}
	
	public override void Execute ()
	{
		virus.DamageOverTime();
	}
}
