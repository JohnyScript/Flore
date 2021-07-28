using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Recipes : MonoBehaviour
{
	public GameObject recipePrefab;
	public GameObject systemMessage;
	public Image imageToChange;
	public Text message;

	//private
	public Dictionary <string, ItemCombinations> discoveredRecipes;
	Dictionary <string, string> seedInfo;
	int recipeNumber = 0;
	GameObject recipePre;
	ItemCombinations value;

	RectTransform thisRect;
	Vector2 recipeSpace;

	void Start ()
	{
	}
	
	public void UpdateRecipeBook (GameObject firstItem, GameObject secondItem, GameObject resultItem)
	{
		if (discoveredRecipes == null) {
			discoveredRecipes = new Dictionary<string, ItemCombinations> ();
		}
		
		if (!discoveredRecipes.TryGetValue (resultItem.name, out value)) {
			DisplaySystemMessage (resultItem);
			recipePre = Instantiate (recipePrefab);
			recipePre.transform.SetParent (transform);
			recipePre.transform.localScale = new Vector3 (1, 1, 1);
			recipePre.name = recipePrefab.name.Replace ("Prefab", " " + recipeNumber.ToString ());

			recipePre.transform.SetAsFirstSibling ();
			
			InstantiateRecipeItems (firstItem);
			InstantiateRecipeItems (secondItem);
			InstantiateRecipeItems (resultItem);
			
			
			discoveredRecipes.Add (resultItem.name, new ItemCombinations {
				item1 = firstItem.name,
				item2 = secondItem.name,
				result = resultItem
			});

			if (discoveredRecipes.Count >= 3) {
				thisRect = GetComponent<RectTransform> ();
				recipeSpace = new Vector2 (0, 110);
				thisRect.sizeDelta += recipeSpace;
			}
			
			recipeNumber++;
		}


	}

	void DisplaySystemMessage (GameObject seedDiscovered)
	{
		if (seedInfo == null) {
			seedInfo = new Dictionary<string, string> () {
			{ "Aliquam" , "An orange and green seed that feels squishy on touch. It seems to belong in a grassy environment." },
			{ "Icos" , "A greenish seed that feels wet on touch. It seems to belong in a watery environment." },
			{ "Viride", "A green seed that appears to be filled with small twigs. By its looks, it could belong in a grassy environment." },
			{ "Mambus", "A brown seed that feels quite hard on touch, your fingers feel sticky by touching it. By its looks, it could belong in a muddy environment." },
			{ "Purleto", "A purple seed that appears to be involted in dark green leaves. You wonder where it should be planted."},
			{ "Mollia", "A pink seed with small petals coming out of it. You can't know for sure where it belongs."},
			{ "Tyra", "A yellow seed created from one of Aliquam's components. It seems to belong in a grassy environment."},
			{ "Lumina", "A [drawing needed] seed. It was made with a ball of light, you wonder where it belongs."}
			};
		}
		Sprite tex = seedDiscovered.GetComponent<Image> ().sprite;
		systemMessage.SetActive (true);
		imageToChange.sprite = tex;
		string plantName = seedDiscovered.name.Replace ("Seed", "");
		Debug.Log ("plant Name = " + plantName);
		message.text = plantName + "\n" + seedInfo [plantName];

	}

	void InstantiateRecipeItems (GameObject toInstantiate)
	{
		GameObject instantiate = Instantiate (toInstantiate);
		instantiate.transform.SetParent (recipePre.transform);
		instantiate.transform.localScale = new Vector3 (1, 1, 1);
		instantiate.transform.localPosition = new Vector3 (0, 0, 0);

		ObjectCounter counter = instantiate.GetComponent<ObjectCounter> ();
		Destroy (counter);

		Button btn = instantiate.GetComponent<Button> ();
		Destroy (btn);

		GameObject child = instantiate.transform.GetChild (0).gameObject;
		Destroy (child);
	}




}
