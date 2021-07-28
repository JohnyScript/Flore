using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftInventory : MonoBehaviour
{

	//public variables
	public int horizontalQuantity;
	public int verticalQuantity;
	public float itemOffset;
	public Vector2 itemPivot;
	public GameObject instantiatedObject;
	
	//private variables

	private float lastXPosition = 0;
	private float lastYPosition = 0;
	RectTransform objectTransform;
	private float verticalOffset;
	private int numberLines = 1;

	//StarCrafting Script Declarations
	StartCrafting startCraftingScript;

	//quando instanciar passar o object que representa
	public List<GameObject> inventoryCraft = new List<GameObject> ();
	public List<int> inventoryQuantities = new List<int> ();

	//object counters
	GameObject object1real;
	ObjectCounter object1Counter;
	GameObject object2real;
	ObjectCounter object2Counter;


	void Start ()
	{
		startCraftingScript = gameObject.GetComponent<StartCrafting> ();
		//InvokeRepeating ("CheckQuantities", 0, 1);
	}

	/*private void CheckQuantities ()
	{ if(inventoryCraft.Count > 0){
			for (int e = 0; e < inventoryQuantities.Count; e++) {
				ObjectCounter rememberQuantity = inventoryCraft [e].GetComponent<ObjectCounter> ();
				inventoryQuantities [e] = rememberQuantity.quantia;
			}
		}
	}*/

	public void DestroyInventory ()
	{
		for (int i = 0; i < transform.childCount; i++) {
			GameObject child = transform.GetChild (i).gameObject;
			Destroy (child);
		}
		GenerateInventory ();

	}

	public void GenerateInventory ()
	{
		
		bool firstitem = true;
		
		for (int i = 0; i < inventoryCraft.Count; i++) {

			instantiatedObject = (GameObject)Instantiate (inventoryCraft [i]);

			//ObjectCounter objectCounter = instantiatedObject.GetComponent<ObjectCounter> ();
			//objectCounter.quantia = inventoryQuantities [i];
			objectTransform = instantiatedObject.GetComponent<RectTransform> ();

			objectTransform.pivot = itemPivot;
			
			instantiatedObject.transform.SetParent (transform);
			
			if (firstitem) {
				instantiatedObject.transform.localPosition = new Vector3 (0, 0, 0);
				
				firstitem = false;

			} else if (!firstitem) {
				instantiatedObject.transform.localPosition = new Vector3 (lastXPosition, lastYPosition, 0);
			}

			instantiatedObject.name = instantiatedObject.name.Replace ("(Clone)", "");
			instantiatedObject.tag = "craftingItems";
			lastXPosition = instantiatedObject.transform.localPosition.x + objectTransform.sizeDelta.x + itemOffset;
			if (inventoryCraft.Count % 7 == 0) {
				lastYPosition = -objectTransform.sizeDelta.y * numberLines;
				lastXPosition = 0;
				numberLines++;
			}
			
		}

		
	}
		

	public void AddObjectToInventory (GameObject objectFOS)
	{
		bool firstItem =false;

		if(inventoryCraft.Count == 0){

			firstItem = true;

		}

		inventoryCraft.Add (objectFOS);
		//ObjectCounter getQuantity = objectFOS.GetComponent<ObjectCounter> ();
		//inventoryQuantities.Add (getQuantity.quantia);
		
		instantiatedObject = (GameObject)Instantiate (objectFOS);
		objectTransform = instantiatedObject.GetComponent<RectTransform> ();
		objectTransform.pivot = itemPivot;
		instantiatedObject.transform.SetParent (transform);
		if (firstItem) {
			instantiatedObject.transform.localPosition = new Vector3 (0, 0, 0);
			
			firstItem = false;
			
		} else if (!firstItem) {
			instantiatedObject.transform.localPosition = new Vector3 (lastXPosition, lastYPosition, 0);
		}
		instantiatedObject.name = instantiatedObject.name.Replace ("(Clone)", "");
		instantiatedObject.tag = "craftingItems";
		lastXPosition = instantiatedObject.transform.localPosition.x + objectTransform.sizeDelta.x + itemOffset;
		if (inventoryCraft.Count % 7 == 0) {
			lastYPosition = -objectTransform.sizeDelta.y * numberLines;
			lastXPosition = 0;
			numberLines++;
		}
	}

	public void RemoveObjectFromInventoryByCrafting (GameObject object1, GameObject object2)
	{
		object1Counter = object1.GetComponent<ObjectCounter> ();
		object2Counter = object2.GetComponent<ObjectCounter> ();

		if (inventoryCraft.Contains (object1)) {
			//Debug.Log ("got it");
		}

		if (object1Counter.quantia <= 1) {
			inventoryCraft.Remove (inventoryCraft.Find (p => p.name == object1.name));
			startCraftingScript.DestroyVisibleObject1FromCraftInventory ();
			
		} else {

			
			object1Counter.DecreaseQuantity ();
			
		}
		if (object2Counter.quantia <= 1) {
			inventoryCraft.Remove (inventoryCraft.Find (p => p.name == object2.name));
			startCraftingScript.DestroyVisibleObject2FromCraftInventory ();
			
		} else {
			
			object2Counter.DecreaseQuantity ();
			
		}
		
		//GetRemovedObjectPosition (obj1Pos, obj2Pos); 
	}









}
