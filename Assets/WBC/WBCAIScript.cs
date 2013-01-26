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
			yield return new WaitForSeconds(0.5f);
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
	

	//IDLE
	public BehaveResult TickSetStateIdleAction (Tree sender)
	{
		wbc.SetState(new WBCStateIdle(wbc));

		return BehaveResult.Success;
	}

	public BehaveResult TickIdleAction (Tree sender)
	{

		return BehaveResult.Failure;
	}


	//ACTION PRECHECK
	public BehaveResult TickSetClosestAction (Tree sender)
	{
		wbc.SetClosestTarget();

		return BehaveResult.Success;
	}

	public BehaveResult TickFreeTargetsAction (Tree sender)
	{
		if (wbc.DetectsFreeTargets())
		{
			return BehaveResult.Success;
		}

		return BehaveResult.Failure;
	}



	//SEEK
	public BehaveResult TickIsGrabOutOfRangeAction (Tree sender)
	{
		if (wbc.IsGrabOutOfRange())
		{
			return BehaveResult.Success;
		}
		return BehaveResult.Failure;
	}

	public BehaveResult TickSetStateSeekAction (Tree sender)
	{
		wbc.SetState(new WBCStateSeek(wbc));

		return BehaveResult.Success;
	}

	public BehaveResult TickSeekAction (Tree sender)
	{
		if (!wbc.IsGrabOutOfRange())
		{
			return BehaveResult.Failure;
		}

		return BehaveResult.Running;
	}

	//DAMAGE
	public BehaveResult TickSetStateDamageAction (Tree sender)
	{
		wbc.SetState(new WBCStateDamage(wbc));

		return BehaveResult.Success;
	}

	public BehaveResult TickDamageOverTimeAction (Tree sender)
	{
		if (wbc.IsAttachedTargetDead())
		{
			return BehaveResult.Failure;
		}

		return BehaveResult.Running;
	}
}
