using UnityEngine;
using System.Collections;

public class InventoryObjects {

	private int quantity;

	private GameObject inventoryObject;

	public int GetQuantity(){

		return quantity;

	}

	public void SetQuantity(int quantity){

		this.quantity = quantity;

	}

	public GameObject GetInventoryObject(){
		
		return inventoryObject;
		
	}
	
	public void SetInventoryObject(GameObject inventoryObject){
		
		this.inventoryObject = inventoryObject;
		
	}

	public InventoryObjects (int quantity, GameObject gameobject){



	}

}
