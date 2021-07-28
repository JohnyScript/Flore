using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DustButtons : MonoBehaviour 

{
	public Button button;
	
	InventoryScript inventoryScript;
	
	void Start ()
	{
		button = GetComponent<Button> ();
		inventoryScript = GetComponentInParent<InventoryScript>();
	}
	
	public string ButtonName ()
	{
		string buttonName = this.name;
		return buttonName;
	}
	
	public void SpendDust(){

		inventoryScript.RemoveObjectFromInventoryByUsingAllOnInventory(gameObject);
		
	}
}
