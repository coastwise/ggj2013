using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
	
	public List<InventoryItem> inventory = new List<InventoryItem>();
	
	// Use this for initialization
	void Start () {
		if (inventory.Count < 1)
			Debug.Log("Inventory Empty");
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
