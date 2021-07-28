using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
	public Florepedia florepedia;
	public Recipes recipe;
	public Inventory2 inventory;
	public GameObject slot1;
	public GameObject slot2;
	public GameObject slotResult;
	public GameObject[] resultedItems;
	public List<ItemCombinations> combinations;

	#region Joao's stuff
	Sprite normalSprite;
	public Sprite goodCombinationSprite;
	public Sprite badCombinationSprite;

	Image slotResultImageComponent;

	#endregion

	bool gotItems = false;
	List<ItemCombinations> insertedItems;

	void Start ()
	{
		#region Joao's stuff

		slotResultImageComponent = slotResult.GetComponent<Image> ();
		normalSprite = slotResultImageComponent.sprite;

		#endregion

		insertedItems = null;

		combinations = new List<ItemCombinations> {
			new ItemCombinations() { item1 = "leafBig", item2 = "wood", result = resultedItems[0] },
			new ItemCombinations() { item1 = "leafSmall", item2 = "leafBig", result = resultedItems[1] },
			new ItemCombinations() { item1 = "water", item2 = "grassNormal", result = resultedItems[2] },
			new ItemCombinations() { item1 = "mud", item2 = "rock", result = resultedItems[3] },
			new ItemCombinations() { item1 = "leafSmall", item2 = "pearlPink", result = resultedItems[4] },
			new ItemCombinations() { item1 = "kelp", item2 = "pearlPink", result = resultedItems[5] },
			new ItemCombinations() { item1 = "petalOrange", item2 = "leafSmall", result = resultedItems[6] }, 
			new ItemCombinations() { item1 = "lightYellow", item2 = "leafSmall", result = resultedItems[7] }
		};

	}

	void Update ()
	{
		if (slot1.transform.childCount == 1 && slot2.transform.childCount == 1 && !gotItems) {
			CheckItems ();
		}
		if (slot1.transform.childCount == 0 || slot2.transform.childCount == 0) {
			gotItems = false;
			slotResultImageComponent.sprite = normalSprite;
		}
	}


	void CheckItems ()
	{
		insertedItems = new List<ItemCombinations> ();
		insertedItems.Add (new ItemCombinations {
			item1 = slot1.transform.GetChild (0).name,
			item2 = slot2.transform.GetChild (0).name,
			result = null
		});


		for (int i = 0; i < combinations.Count; i++) {

			//está assim por causa do ocd do João <3
			if ((combinations [i].item1 == insertedItems [0].item1 && combinations [i].item2 == insertedItems [0].item2) ||
				(combinations [i].item1 == insertedItems [0].item2 && combinations [i].item2 == insertedItems [0].item1)) {
				InstantiateFunction (resultedItems [i], slotResult);
				Debug.Log ("Nice combination");
				slotResultImageComponent.sprite = goodCombinationSprite;
				break;
			} else {
				Debug.Log ("Bad combination");
				slotResultImageComponent.sprite = badCombinationSprite;
			}
		}

		gotItems = true;
	}
	
	void InstantiateFunction (GameObject toInstantiate, GameObject whereToInstantiate)
	{
		GameObject instantiate = Instantiate (toInstantiate);
		instantiate.transform.SetParent (whereToInstantiate.transform);
		instantiate.transform.localScale = new Vector3 (1, 1, 1);
		instantiate.name = instantiate.name.Replace ("(Clone)", "");

		//destroy components we don't need
		/*Button btnComp = instantiate.GetComponent<Button> ();
		Destroy (btnComp);
		ObjectCounter counter = instantiate.GetComponent<ObjectCounter> ();
		Destroy (counter);
		SeedButtons seedB = instantiate.GetComponent<SeedButtons> ();
		Destroy (seedB);
		GameObject child = instantiate.transform.GetChild (0).gameObject;
		Destroy (child);*/
	}

	public void InstantiateItemCopies ()
	{
		if (gameObject.activeInHierarchy) {
			if (slot1.transform.childCount == 0) {
				InstantiateFunction (EventSystem.current.currentSelectedGameObject, slot1);

			} else if (slot2.transform.childCount == 0) {
				InstantiateFunction (EventSystem.current.currentSelectedGameObject, slot2);
			}
		}
	}

	public void DeleteCopy (GameObject deleteOnSlot)
	{
		if (deleteOnSlot.transform.childCount == 1) {
			Destroy (deleteOnSlot.transform.GetChild (0).gameObject);
			if (slotResult.transform.childCount == 1) {
				Destroy (slotResult.transform.GetChild (0).gameObject);
			}
		} else {
			Debug.Log ("There is nothing to delete");
		}
	}

	public void GetResultedItem ()
	{
		if (slotResult.transform.childCount != 0) {
			recipe.UpdateRecipeBook (slot1.transform.GetChild (0).gameObject, 
		                            slot2.transform.GetChild (0).gameObject,
		                            slotResult.transform.GetChild (0).gameObject);

			florepedia.UpdateFlorepedia (slotResult.transform.GetChild (0).name);

			EnergyPoints.IncreaseTotalPoints (5);

			GameObject childResult = slotResult.transform.GetChild (0).gameObject;
			GameObject child1 = slot1.transform.GetChild (0).gameObject;
			GameObject child2 = slot2.transform.GetChild (0).gameObject;

			inventory.AddItem (childResult);
			inventory.RemoveItem (child1);
			inventory.RemoveItem (child2);

			Destroy (childResult);
			Destroy (child1);
			Destroy (child2);
		}
	}

}

public class ItemCombinations
{
	public string item1 { get; set; }
	public string item2 { get; set; }
	public GameObject result { get; set; }
}


