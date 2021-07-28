using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryScript : MonoBehaviour
{

	//public variables
	public float offset;
	public GameObject instantiatedObject;

	//private variables

	GameObject object1ToDestroy;
	GameObject object2ToDestroy;
	GameObject objectToDestroyFromUsing;

	private float lastPosition = 0;
	private float panelWidth = 0;
	RectTransform panelTransform;
	public RectTransform objectTransform;
	Vector2 itemPivot;

	public List<GameObject> inventory = new List<GameObject> ();
	
	//access inventory
	public GameObject craftInventory;
	CraftInventory craftScript;

	//object counters
	ObjectCounter object1Counter;
	ObjectCounter object2Counter;
	ObjectCounter objectToDestroyFromUsingCounter;

	//inventory movement
	InventoryMovement inventoryMovementScript;


	void Start ()
	{
		itemPivot = new Vector2 (0, 0.5f);
		craftScript = craftInventory.GetComponent<CraftInventory> ();
		panelTransform = GetComponent<RectTransform> ();
		inventoryMovementScript = GetComponent<InventoryMovement> ();

		GenerateInventory ();
		
		craftScript.inventoryQuantities.Capacity = craftScript.inventoryCraft.Count;

		for (int i = 0; i < inventory.Count; i++) {
			craftScript.inventoryCraft.Add (inventory [i]);
			craftScript.inventoryQuantities.Add (1);
		}
		
		craftScript.GenerateInventory ();
		
	}

	public void DestroyInventory ()
	{

		panelWidth = 0;
		panelTransform.sizeDelta = new Vector2 (panelWidth, panelTransform.sizeDelta.y);
		for (int i = 0; i < transform.childCount; i++) {
			GameObject child = transform.GetChild (i).gameObject;
			Destroy (child);
		}
		lastPosition = 0;
		if(inventory.Count > 0){
			//Debug.Log ("shit");
			GenerateInventory ();
		}

		
	}
	
	
	public void WiggleIcons (Animation objectToWiggle)
	{
		objectToWiggle.Play ("wiggle");
	}
	
	public void MakeSound (AudioSource objectSoundSource)
	{
		objectSoundSource.Play ();
	}

	private void GenerateInventory ()
	{

		bool firstitem = true;
		

		for (int i = 0; i < inventory.Count; i++) {

			instantiatedObject = (GameObject)Instantiate (inventory [i]);

			objectTransform = instantiatedObject.GetComponent<RectTransform> ();

			instantiatedObject.transform.SetParent (transform);

			panelWidth = (objectTransform.sizeDelta.x + offset) * inventory.Count;
			panelTransform.sizeDelta = new Vector2 (panelWidth, panelTransform.sizeDelta.y);

			if (firstitem) {
				instantiatedObject.transform.localPosition = new Vector3 (0, 0, -0.1f);
			
				firstitem = false;
			} else if (!firstitem) {
				instantiatedObject.transform.localPosition = new Vector3 (lastPosition + objectTransform.sizeDelta.x + offset, 0, -0.1f);
			}

			instantiatedObject.name = instantiatedObject.name.Replace ("(Clone)", "");
			
			lastPosition = instantiatedObject.transform.localPosition.x;


		}

	}

	public void AddObjectToInventory (GameObject objectFOS)
	{
		bool firstitem = false;

		if(inventory.Count == 0){

			firstitem = true;

		}

		inventory.Add (objectFOS);

		instantiatedObject = (GameObject)Instantiate (objectFOS);
		objectTransform = instantiatedObject.GetComponent<RectTransform> ();
		objectTransform.pivot = itemPivot;
		instantiatedObject.transform.SetParent (transform);
		panelWidth = (objectTransform.sizeDelta.x + offset) * inventory.Count;
		panelTransform.sizeDelta = new Vector2 (panelWidth, panelTransform.sizeDelta.y);
		if (firstitem) {
			instantiatedObject.transform.localPosition = new Vector3 (0, 0, -0.1f);
			
			firstitem = false;
		} else if (!firstitem) {
			instantiatedObject.transform.localPosition = new Vector3 (lastPosition + objectTransform.sizeDelta.x + offset, 0, -0.1f);
		}
		instantiatedObject.name = instantiatedObject.name.Replace ("(Clone)", "");
		lastPosition = instantiatedObject.transform.localPosition.x;

		if (inventoryMovementScript.inventoryRect.sizeDelta.x >= 980) {
			inventoryMovementScript.DinamicClamping ();
		}
	}

	public void RemoveObjectFromInventoryByCrafting (GameObject object1, GameObject object2)
	{

		object1ToDestroy = GameObject.Find (object1.name);
		object2ToDestroy = GameObject.Find (object2.name);

		object1Counter = object1ToDestroy.GetComponent<ObjectCounter> ();
		object2Counter = object2ToDestroy.GetComponent<ObjectCounter> ();
		
		/*if (inventory.Contains (object1)) {
			Debug.Log ("got it");
		}*/
		
		if (object1Counter.quantia <= 1) {

			inventory.Remove (inventory.Find (p => p.name == object1.name));
			Destroy (object1ToDestroy);
			//Destroy(startCraftingScript.itemInSlot1);
			
		} else {
			
			
			object1Counter.DecreaseQuantity ();
			
		}
		if (object2Counter.quantia <= 1) {
		
			inventory.Remove (inventory.Find (p => p.name == object2.name));
			Destroy (object2ToDestroy);
			//Destroy(startCraftingScript.itemInSlot2);
			
		} else {
			
			object2Counter.DecreaseQuantity ();
			
		}
	}

	public void RemoveObjectFromInventoryByUsingAllOnInventory (GameObject objectToDestroy){

		objectToDestroyFromUsing = GameObject.Find(objectToDestroy.name);

		objectToDestroyFromUsingCounter = objectToDestroyFromUsing.GetComponent<ObjectCounter>();

		if(objectToDestroyFromUsingCounter.quantia<=1){

			inventory.Remove (inventory.Find (p => p.name == objectToDestroy.name));
			Destroy(objectToDestroyFromUsing);

			craftScript.inventoryCraft.Clear();
			for (int i = 0; i < inventory.Count; i++) {
				craftScript.inventoryCraft.Add (inventory [i]);
				craftScript.inventoryQuantities.Add (1);
			}
			craftScript.DestroyInventory();
			DestroyInventory();

		} else {
			
			
			objectToDestroyFromUsingCounter.DecreaseQuantity ();
			
		}
	}













}
