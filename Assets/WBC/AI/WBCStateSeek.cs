using UnityEngine;
using System.Collections;

public class WBCStateSeek : WBCState {
	
	public WBCStateSeek (WBCScript wbc)
		: base (wbc) {
		wbc.SeekInit();
	}
	
	public override void Execute ()
	{
		wbc.Seek();
	}
}
