using UnityEngine;
using System.Collections;
using Behave.Runtime;
using Tree = Behave.Runtime.Tree;

public class VirusAIScript : MonoBehaviour, IAgent {
	
	private VirusScript virus;
	private Tree tree;

	IEnumerator Start ()
	{
		virus = (VirusScript)GetComponent<VirusScript>();
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
	

	//IDLE
	public BehaveResult TickSetStateIdleAction (Tree sender)
	{
		virus.SetState(new VirusStateIdle(virus));

		return BehaveResult.Success;
	}

	public BehaveResult TickIdleAction (Tree sender)
	{
		if (virus.DetectsFreeTargets())
		{
			return BehaveResult.Success;
		}
		
		return BehaveResult.Running;
	}


	//ACTION PRECHECK
	public BehaveResult TickSetClosestAction (Tree sender)
	{
		virus.SetClosestTarget();

		return BehaveResult.Success;
	}

	public BehaveResult TickFreeTargetsAction (Tree sender)
	{
		if (virus.DetectsFreeTargets())
		{
			return BehaveResult.Success;
		}

		return BehaveResult.Failure;
	}



	//SEEK
	public BehaveResult TickIsGrabOutOfRangeAction (Tree sender)
	{
		if (virus.IsGrabOutOfRange())
		{
			return BehaveResult.Success;
		}
		return BehaveResult.Failure;
	}

	public BehaveResult TickSetStateSeekAction (Tree sender)
	{
		virus.SetState(new VirusStateSeek(virus));

		return BehaveResult.Success;
	}

	public BehaveResult TickSeekAction (Tree sender)
	{
		if (!virus.IsGrabOutOfRange())
		{
			return BehaveResult.Failure;
		}
		
		if (virus.IsAttachedTargetInfected())
		{
			return BehaveResult.Success;	
		}

		return BehaveResult.Running;
	}

	//DAMAGE
	public BehaveResult TickIsAttachedAction (Tree sender)
	{
		if (virus.IsAttached())
		{
			return BehaveResult.Success;	
		}
		
		return BehaveResult.Failure;
	}
	
	public BehaveResult TickSetStateDamageAction (Tree sender)
	{
		virus.SetState(new VirusStateDamage(virus));

		return BehaveResult.Success;
	}

	public BehaveResult TickDamageOverTimeAction (Tree sender)
	{
		if (virus.IsAttachedTargetDead())
		{
			return BehaveResult.Failure;
		}

		return BehaveResult.Running;
	}
}
