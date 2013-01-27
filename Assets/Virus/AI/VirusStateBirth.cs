using UnityEngine;
using System.Collections;

public class VirusStateBirth : VirusState {
	
	private	Vector3 dir;
	private bool endBirth = false;
	
	public VirusStateBirth (VirusScript virus, Vector3 direction)
		: base (virus) {
		ShipController ship = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ShipController>();
		
		Vector3 dirToV = virus.transform.position - ship.transform.position;
		dirToV.Normalize();
		
		dir = dirToV + direction;
		virus.StartCoroutine(EndBirthCoroutine());
	}
	
	public override void Execute ()
	{
		if (!endBirth) {
			virus.transform.Translate(dir * Time.deltaTime * 1f);	
		}
	}
	
	private IEnumerator EndBirthCoroutine ()
	{
		yield return new WaitForSeconds (1.0f);
		endBirth = true;
		VirusAIScript ai = (VirusAIScript)virus.gameObject.AddComponent("VirusAIScript");
		virus.SetState(new VirusStateIdle(virus));
	}
}
