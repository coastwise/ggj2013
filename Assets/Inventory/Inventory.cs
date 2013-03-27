using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory {
	
	private List<InventoryItem> inventory = new List<InventoryItem>();
	
	// Use this for initialization
	public Inventory () {
		if (inventory.Count < 1)
			Debug.Log("Inventory Empty");
		
	}
	
	public void Add (InventoryItem item)
	{
		inventory.Add(item);	
	}
	
	public void Remove (InventoryItem item)
	{
		inventory.Remove(item);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
