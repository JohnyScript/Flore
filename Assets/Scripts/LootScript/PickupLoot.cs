using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PickupLoot : MonoBehaviour, IPointerClickHandler
{
	//inventory item, the one that has rect Transform
	public GameObject itemDropped;

	Inventory2 inventory;

	//GameObject gameController;
	//LootController2 lootControllerScript;


	void Start ()
	{
	
		//gameController = GameObject.Find("GameController");
		//lootControllerScript = gameController.GetComponent<LootController2>();
		inventory = GameObject.Find ("Inventory").GetComponent<Inventory2> ();
	}


	public void OnPointerClick (PointerEventData eventData)
	{
		inventory.AddItem (itemDropped);
		Destroy (gameObject);

	}

}
