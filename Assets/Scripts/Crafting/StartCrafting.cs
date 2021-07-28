using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class StartCrafting : MonoBehaviour
{

	public GameObject lastPressed = null;
	public GameObject buttonPressed;
	public GameObject slot1;
	public GameObject slot2;
	public GameObject slot3;
	public GameObject[] resultedItems;
	public Vector2 itemPivot;

	public bool luxWasCrafted = false;

	GameObject itemFromCraftInv1;
	GameObject itemFromCraftInv2;
	
	//access crafting inventory
	CraftInventory craftScript;

	//access normal inventory
	public GameObject inventory;
	InventoryScript inventoryScript;

	//private variables
	bool slotIsFull;
	bool slot2IsFull;
	bool slot3IsFull;
	bool hasResult;
	GameObject slot1InstantiatedButton;
	GameObject slot2InstantiatedButton;
	GameObject instantiatedButton;
	GameObject resultItem;
	
	void Start ()
	{
		craftScript = gameObject.GetComponent<CraftInventory> ();
		inventoryScript = inventory.GetComponent<InventoryScript> ();
		InvokeRepeating ("VerifyButtonPressed", 1, 0.3f);
	}

	void Update ()
	{
		if (slotIsFull && slot2IsFull && !hasResult)
			CheckCompatibility ();
	}

	void VerifyButtonPressed ()
	{
		if (EventSystem.current.currentSelectedGameObject != null) {
			buttonPressed = EventSystem.current.currentSelectedGameObject;
			if (buttonPressed != null) {
				lastPressed = buttonPressed;
				if (craftScript.inventoryCraft.Find (p => p.name == lastPressed.name) && !slotIsFull && lastPressed.tag == "craftingItems") {
					itemFromCraftInv1 = lastPressed;
					InstantiateButtonOnSlot (slot1);
					slot1InstantiatedButton = instantiatedButton;
					slotIsFull = true;

				} else if (craftScript.inventoryCraft.Find (p => p.name == lastPressed.name) && !slot2IsFull && lastPressed.tag == "craftingItems") {
					itemFromCraftInv2 = lastPressed;
					InstantiateButtonOnSlot (slot2);
					slot2InstantiatedButton = instantiatedButton;
					slot2IsFull = true;
				} 
			}
		}
	}

	void InstantiateButtonOnSlot (GameObject slotToInstantiate)
	{
		instantiatedButton = (GameObject)Instantiate (lastPressed, slotToInstantiate.transform.position, slotToInstantiate.transform.rotation);
		instantiatedButton.transform.SetParent (slotToInstantiate.transform);
		instantiatedButton.transform.localScale = slotToInstantiate.transform.localScale;
		if (instantiatedButton.transform.childCount != 0) {
			Transform child = instantiatedButton.transform.GetChild (0);
			child.gameObject.SetActive (false);
		}
		lastPressed = null;
		buttonPressed = null;
		EventSystem.current.SetSelectedGameObject (null);
	}

	public Vector3 GetInstantiatedObjectsPosition ()
	{
		Vector3 min = new Vector3 (99999, 99999, 99999);
		Vector3[] objectTrans = new Vector3[2];
		objectTrans [0] = itemFromCraftInv1.transform.localPosition;
		objectTrans [1] = itemFromCraftInv2.transform.localPosition;
		ObjectCounter item1Counter = itemFromCraftInv1.GetComponent<ObjectCounter> ();
		ObjectCounter item2Counter = itemFromCraftInv2.GetComponent<ObjectCounter> ();
		if (item1Counter.quantia != 1) 
			min = objectTrans [1];
		else if (item2Counter.quantia != 1)
			min = objectTrans [0];
		else if (item1Counter.quantia == 1 && item2Counter.quantia == 1) {
			for (int i = 0; i < objectTrans.Length; i++) {
				if (objectTrans [i].x < min.x) {
					min = objectTrans [i];
				}
				//Debug.Log (objectTrans [i]);
			}
		}
		//Debug.Log ("min = " + min);
		return min;
	}

	public void DestroyButtonOnSlot1 ()
	{
		Destroy (slot1InstantiatedButton);
		Destroy (resultItem);
		slotIsFull = false;
		hasResult = false;
		lastPressed = null;
		buttonPressed = null;
	}

	public void DestroyButtonOnSlot2 ()
	{
		Destroy (slot2InstantiatedButton);
		Destroy (resultItem);
		slot2IsFull = false;
		hasResult = false;
		lastPressed = null;
		buttonPressed = null;
	}
	
	public void DestroyVisibleObject1FromCraftInventory ()
	{
		Destroy (itemFromCraftInv1);
		
	}
	
	public void DestroyVisibleObject2FromCraftInventory ()
	{
		Destroy (itemFromCraftInv2);
		
	}

	public void GetResultedItem ()
	{
		if (slot3IsFull) {
			inventoryScript.RemoveObjectFromInventoryByCrafting (itemFromCraftInv1, itemFromCraftInv2);
			craftScript.RemoveObjectFromInventoryByCrafting (itemFromCraftInv1, itemFromCraftInv2);

			resultItem.transform.localScale = new Vector3 (0.01f, 0.01f, 0.01f);
			craftScript.AddObjectToInventory (resultItem);
			inventoryScript.AddObjectToInventory (resultItem);
			Destroy (instantiatedButton);
			Destroy (slot1InstantiatedButton);
			Destroy (slot2InstantiatedButton);
			slotIsFull = false;
			slot2IsFull = false;
			hasResult = false;
			resultItem = null;
			inventoryScript.DestroyInventory ();
			craftScript.DestroyInventory ();
		}
	}

	void InstantiateResult (GameObject resultedItem)
	{
		instantiatedButton = (GameObject)Instantiate (resultedItem, slot3.transform.position, slot3.transform.rotation);
		instantiatedButton.name = instantiatedButton.name.Replace ("(Clone)", "");
		RectTransform objectTransform = instantiatedButton.GetComponent<RectTransform> ();
		objectTransform.pivot = itemPivot;
		instantiatedButton.transform.SetParent (slot3.transform);
		instantiatedButton.transform.localScale = slot3.transform.localScale;
		resultItem = resultedItem;
		hasResult = true;
	}

	void CheckCompatibility ()
	{
		if (slot1InstantiatedButton.name.Contains ("Wood") && slot2InstantiatedButton.name.Contains ("Pickaxe")) {
			InstantiateResult (resultedItems [0]);
			slot3IsFull = true;

		}
		if (slot1InstantiatedButton.name.Contains ("Wood") && slot2InstantiatedButton.name.Contains ("Scythe")) {
			InstantiateResult (resultedItems [1]);
			slot3IsFull = true;
		}
		if (slot1InstantiatedButton.name.Contains ("Wood") && slot2InstantiatedButton.name.Contains ("HammerChisel")) {
			InstantiateResult (resultedItems [2]);
			slot3IsFull = true;
		}
		if (slot1InstantiatedButton.name.Contains ("tyraSeed") && slot2InstantiatedButton.name.Contains ("DustBlue")) {
			InstantiateResult (resultedItems [3]);
			slot3IsFull = true;
			luxWasCrafted = true;
		}
		if (slot1InstantiatedButton.name.Contains ("DustPurple") && slot2InstantiatedButton.name.Contains ("DustRed")) {
			InstantiateResult (resultedItems [4]);
			slot3IsFull = true;
		}
	}

}
