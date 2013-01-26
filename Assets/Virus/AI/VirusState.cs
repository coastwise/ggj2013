using UnityEngine;
using System.Collections;

public abstract class VirusState {
	
	protected VirusScript virus;
	
	public VirusState (VirusScript virus)
	{
		this.virus = virus;	
	}
	
	public abstract void Execute ();
}
