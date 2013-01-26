using UnityEngine;
using System.Collections;

public class VirusStateDamage : VirusState {

	public VirusStateDamage (VirusScript virus)
		: base (virus) {
		virus.DamageOverTime();
	}
	
	public override void Execute ()
	{
	}
}
