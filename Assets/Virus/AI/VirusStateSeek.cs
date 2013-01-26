using UnityEngine;
using System.Collections;

public class VirusStateSeek : VirusState {

	public VirusStateSeek (VirusScript virus)
		: base (virus) {
		virus.SeekInit();
	}
	
	public override void Execute ()
	{
		virus.Seek();
	}
}
