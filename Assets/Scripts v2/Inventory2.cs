using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory2 : MonoBehaviour
{
	public int visualCapacity;
	public GameObject toolBar;
	public RectTransform invMask;
	public RectTransform holder;
	public List<GameObject> items;
	public List<int> quantities;
	//List<Items> items2;

	//private
	RectTransform thisRect; 
	Vector2 invSpace;
	Vector2 startHolderSize;
	bool changeAppearance;
	public int timesToExpand = 0;

	void Start ()
	{
		thisRect = GetComponent<RectTransform> ();
		invSpace = new Vector2 (80, 0);
		startHolderSize = new Vector2 (holder.sizeDelta.x, holder.sizeDelta.y);
	}
	
	public void AddItem (GameObject itemToAdd)
	{
		if (items.Exists (p => p.name == itemToAdd.name)) {
			GameObject itemFound = items.Find (p => p.name == itemToAdd.name);
			ObjectCounter counter = itemFound.GetComponent<ObjectCounter> ();
			counter.IncreaseQuantity ();
			int index = items.FindIndex (p => p.name == itemToAdd.name);
			quantities [index] = counter.quantia;
			
		} else {
			if (ToggleButton.craftIsOn && items.Count >= 9) {
				thisRect.sizeDelta += invSpace;
			}
			if (items.Count >= 6) {
				timesToExpand++;
			}
			GameObject instantiateItem = (GameObject)Instantiate (itemToAdd);
			instantiateItem.transform.SetParent (transform);
			instantiateItem.transform.localScale = transform.localScale;
			instantiateItem.transform.localPosition = new Vector3 (0, 0, 0);
			instantiateItem.name = itemToAdd.name;
			items.Add (instantiateItem);
			quantities.Add (1);
		}
		//Debug.Log (items.Count);
	}

	public void RemoveItem (GameObject itemToRemove)
	{
		GameObject itemFound = items.Find (p => p.name == itemToRemove.name);
		ObjectCounter counter = itemFound.GetComponent<ObjectCounter> ();
		int index = items.FindIndex (p => p.name == itemToRemove.name);
		if (counter.quantia > 1) {
			counter.DecreaseQuantity ();
			quantities [index] = counter.quantia;
			
		} else if (counter.quantia == 1) {
			Destroy (itemFound);
			items.RemoveAt (index);
			quantities.RemoveAt (index);
			if (ToggleButton.craftIsOn && items.Count >= 9) {
				thisRect.sizeDelta -= invSpace;
			}
			timesToExpand--;
		}
	}

	public void ChangeInventoryAppearance ()
	{
		if (ToggleButton.craftIsOn) {
			toolBar.SetActive (false);
			Vector2 holderSize = new Vector2 (0, 0);
			holder.sizeDelta = holderSize;
			invMask.sizeDelta = holderSize;
		} else if (!ToggleButton.craftIsOn) {
			toolBar.SetActive (true);
			holder.sizeDelta = startHolderSize;
			invMask.sizeDelta = startHolderSize;
			//ExpandInventory ();
		}
	}

	/*public void ExpandInventory ()
	{
		Vector2 currentSize;
		if (items.Count < 7) {
			currentSize = ToggleButton.inventoryWidth; 
			thisRect.sizeDelta = currentSize;
			timesToExpand = 0;
			//Debug.Log ("reset size");

		} else if (items.Count >= visualCapacity && timesToExpand != 0) {
			currentSize = new Vector2 (invSpace.x * timesToExpand, thisRect.sizeDelta.y);
			thisRect.sizeDelta = currentSize;
		}
	}*/

}

/*public class Items
{
	public string name { get; set; }
	public int quantity { get; set; }
}*/
