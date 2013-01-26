using UnityEngine;
using System.Collections;
using Behave.Runtime;
using Tree = Behave.Runtime.Tree;

public class RBCAIScript : MonoBehaviour, IAgent {

	private RBCScript rbc;
	private Tree tree;

	IEnumerator Start ()
	{
		rbc = (RBCScript)GetComponent<RBCScript>();
		tree = BLLib.InstantiateTree(BLLib.TreeType.Trees_RBC, this);

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
	
	public BehaveResult TickIsKilledAction (Tree sender)
	{
		if (rbc.IsKilled())
		{
			return BehaveResult.Success;
		}
		
		return BehaveResult.Failure;
	}
	
	public BehaveResult TickSetStateKillAction (Tree sender)
	{
		rbc.SetState(new RBCStateKill(rbc));
		
		return BehaveResult.Success;
	}
	
	public BehaveResult TickKillAction (Tree sender)
	{
		return BehaveResult.Running;
	}
	
	public BehaveResult TickIsInfectedAction (Tree sender)
	{
		if (rbc.IsInfected())
		{
			return BehaveResult.Success;	
		}
		
		return BehaveResult.Failure;
	}
	
	public BehaveResult TickSetStateInfectedAction (Tree sender)
	{
		rbc.SetState (new RBCStateInfected(rbc));
		
		return BehaveResult.Success;
	}
	
	public BehaveResult TickInfectedAction (Tree sender)
	{
		if (rbc.CheckShouldExplode())
		{
			return BehaveResult.Success;	
		}
		
		if (rbc.IsKilled())
		{
			return BehaveResult.Failure;
		}
		
		return BehaveResult.Running;
	}
	
	public BehaveResult TickSetStateExplodeAction  (Tree sender)
	{
		rbc.SetState(new RBCStateExplode(rbc));
		return BehaveResult.Success;
	}
	
	public BehaveResult TickExplodeAction (Tree sender)
	{
		return BehaveResult.Running;
	}
}
