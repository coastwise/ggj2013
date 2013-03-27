using UnityEngine;
using System.Collections;

public class InventoryItem {

	private Quantity _quantity;
	public Quantity Quantity
	{
		get { return _quantity;}
		set { _quantity = value;}
	}
	
	public InventoryItem (int quantity)
	{
		_quantity = new Quantity(quantity);	
		Quantity.Increase();
	}	
}

public class Quantity {

	int quantity;
	
	public Quantity (int quantity)
	{
		this.quantity = quantity;		
	}
	
	public void Increase ()
	{
		quantity++;
	}
	
	public void Increase (int amount)
	{
		quantity += amount;	
	}
	
	public void Decrease ()
	{
		quantity--;	
	}
	
	public void Decrease (int amount)
	{
		quantity -= amount;	
	}
}