using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LootController2 : MonoBehaviour
{
	//public variables
	public GameObject pickaxe;

	//access inventory
	private GameObject inventory;
	private InventoryScript inventoryScript;
	
	//access crafting inventory
	public GameObject craftingInventory;
	CraftInventory craftScript;

	public GameObject[] loot;

	//Dust CraftCounters
	ObjectCounter DustCraftCounter;
	ObjectCounter dust1CraftCounter;
	ObjectCounter dust2CraftCounter;
	ObjectCounter dust3CraftCounter;

	//Tool CraftCounters
	ObjectCounter toolCraftCounter;
	ObjectCounter pickaxeCraftCounter;
	ObjectCounter hammerCraftCounter;
	ObjectCounter scytheCraftCounter;
	ObjectCounter rakeCraftCounter;

	//Seed CraftCounters
	ObjectCounter seedCraftCounter;
	ObjectCounter tyraSeedCraftCounter;
	ObjectCounter luxSeedCraftCounter;
	ObjectCounter marmoraSeedCraftCounter;


	void Start ()
	{
		inventory = GameObject.Find ("inventory_grid");
		inventoryScript = inventory.GetComponent<InventoryScript> ();
		
		craftScript = craftingInventory.GetComponent<CraftInventory> ();
	}

	public void GetSeeds (GameObject seedsDropped)
	{

		if (!inventoryScript.inventory.Contains (seedsDropped)) {
			
			inventoryScript.AddObjectToInventory (seedsDropped);
			craftScript.AddObjectToInventory (seedsDropped);
			
			seedsDropped = inventoryScript.instantiatedObject;
			GameObject seedCraft = craftScript.instantiatedObject;
			seedCraftCounter = seedCraft.GetComponent<ObjectCounter> ();
			if (seedCraft.name == loot [6].name) {
				tyraSeedCraftCounter = seedCraft.GetComponent<ObjectCounter> ();
			} /*else if (seedCraft.name == loot [7].name) {
				dust2CraftCounter = seedCraft.GetComponent<ObjectCounter> ();
			} else if (seedCraft.name == loot[8].name){
				dust3CraftCounter = seedCraft.GetComponent<ObjectCounter>();
			}*/
			
			
		} else if (inventoryScript.inventory.Contains (seedsDropped)) {
			
			if (inventoryScript.instantiatedObject.name == seedsDropped.name) {
				ObjectCounter seedCounter = inventoryScript.instantiatedObject.GetComponent<ObjectCounter> ();
				seedCounter.IncreaseQuantity ();
				seedCraftCounter.IncreaseQuantity ();
			} else {
				ObjectCounter seedCounter = GameObject.Find (seedsDropped.name).GetComponent<ObjectCounter> ();
				seedCounter.IncreaseQuantity ();
				if (seedsDropped.name == loot [6].name) {
					tyraSeedCraftCounter.IncreaseQuantity ();
				} /*else if (seedsDropped.name == loot [7].name) {
					dust2CraftCounter.SetItemQuantity ();
				}else if (seedsDropped.name == loot [8].name) {
					dust3CraftCounter.SetItemQuantity ();
				}*/
			}
		}

	}
}
