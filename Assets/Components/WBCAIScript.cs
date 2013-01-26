using UnityEngine;
using System.Collections;
using Behave.Runtime;
using Tree = Behave.Runtime.Tree;

public class WBCAIScript : MonoBehaviour, IAgent {
	
	private WBCScript wbc;
	private Tree tree;

	IEnumerator Start ()
	{
		wbc = (WBCScript)GetComponent<WBCScript>();
		tree = BLLib.InstantiateTree(BLLib.TreeType.Trees_WBC, this);

		while (Application.isPlaying && tree != null)
		{
			yield return new WaitForSeconds(0.0f);
			AIUpdate();	
		}
	}

	void AIUpdate()
	{
		tree.Tick();
	}
	
	public void Reset (Tree sender)
	{
		
	}
	
	public int SelectTopPriority (Tree sender, params int[] IDs)
	{
		return 1;	
	}
	
	public BehaveResult Tick (Tree sender, bool init)
	{
		return BehaveResult.Failure;	
	}
	
	
}
