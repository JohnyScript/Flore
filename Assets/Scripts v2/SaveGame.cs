using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

public class SaveGame : MonoBehaviour
{
	GameData data;
	public Recipes recipeScript;

	//com 15min de utilização o telefone perde 6% de bateria


	public void GlobalSave ()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.dataPath + "/Saves/gameData.flore");

		data = new GameData ();

		SaveIslands (GameObject.FindGameObjectsWithTag ("PlantingSpots"));
		SavePlants (GameObject.FindGameObjectsWithTag ("Plant"));
		SaveItems ();
		SaveRecipes ();

		data.d_season = Seasons.gameSeasons.ToString ();
		
		bf.Serialize (file, data);
		file.Close ();
	}

	public void GlobalLoad ()
	{
		if (File.Exists (Application.dataPath + "/Saves/gameData.flore")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.dataPath + "/Saves/gameData.flore", FileMode.Open);
			data = (GameData)bf.Deserialize (file);
			
			Seasons season = GetComponent<Seasons> ();
			Seasons.gameSeasons = (Seasons.GameSeasons)System.Enum.Parse (typeof(Seasons.GameSeasons), data.d_season);
			season.ChangeSeason (data.d_season);


			LoadIslands ();
			StartCoroutine ("LoadPlants", 1);
			LoadItems ();
			LoadRecipes ();

			file.Close ();
		}
	}

	void SaveIslands (GameObject[] islandsArray)
	{
		data.d_islandName = new string[islandsArray.Length];
		data.d_islandParentName = new string[islandsArray.Length];
		data.d_islandType = new string[islandsArray.Length];
		data.d_islandCooldown = new int[islandsArray.Length];
		data.d_islandIsPlanted = new bool[islandsArray.Length];
		for (int i = 0; i < islandsArray.Length; i++) {
			data.d_islandName [i] = islandsArray [i].name;
			data.d_islandParentName [i] = islandsArray [i].name.Replace ("island ", "");
			Spot info = islandsArray [i].GetComponent<Spot> ();
			data.d_islandType [i] = info.type.ToString ();
			data.d_islandIsPlanted [i] = info.isPlanted;
			SpotInteractions spotInt = islandsArray [i].transform.GetChild (0).GetComponent<SpotInteractions> ();
			data.d_islandCooldown [i] = spotInt.cooldown;
			//Debug.Log ("index " + i + " = island " + data.d_islandName [i]);
			//Debug.Log ("parent = " + data.d_parentName [i]);
		}
	}

	void LoadIslands ()
	{
		GameObject islandPrefab = Resources.Load ("R_Prefabs/islandPrefab") as GameObject;
		GameObject [] previousIslands = GameObject.FindGameObjectsWithTag ("PlantingSpots");
		for (int i = 0; i < previousIslands.Length; i++) {
			Destroy (previousIslands [i]);
		}
		for (int i = 0; i < data.d_islandName.Length; i++) {
			GameObject newIsland = Instantiate (islandPrefab) as GameObject;
			GameObject parent = GameObject.Find (data.d_islandParentName [i]);
			InstantiateSpot parentScript = parent.GetComponent<InstantiateSpot> ();
			parentScript.occupied = true;

			newIsland.transform.SetParent (parent.transform);
			newIsland.transform.localPosition = islandPrefab.transform.position;
			newIsland.transform.localScale = islandPrefab.transform.lossyScale;
			newIsland.name = data.d_islandName [i];
			Spot info = newIsland.GetComponent<Spot> ();
			info.type = (Spot.spotType)System.Enum.Parse (typeof(Spot.spotType), data.d_islandType [i]);
			info.isPlanted = data.d_islandIsPlanted [i];
			SpotInteractions spotInt = newIsland.transform.GetChild (0).GetComponent<SpotInteractions> ();
			spotInt.cooldown = data.d_islandCooldown [i];
			if (spotInt.cooldown > 0) {
				spotInt.InvokeRepeating ("ApplyCooldown", 0, 1);
			}
		}
	}

	void SavePlants (GameObject[] plants)
	{
		data.d_plantName = new string[plants.Length];
		data.d_plantStage = new string[plants.Length];
		data.d_plantAnimInfo = new string[plants.Length];
		data.d_plantWaterValue = new float[plants.Length];
		data.d_plantLevelValue = new float[plants.Length];
		//Debug.Log ("plants length = " + plants.Length);
		for (int i = 0; i < plants.Length; i++) {
			data.d_plantName [i] = plants [i].name;
			//Debug.Log ("plant name = " + data.d_plantName [i]);
			Plants info = plants [i].GetComponent<Plants> ();
			data.d_plantStage [i] = info.numberOfStages.ToString ();

			string onlyName = plants [i].name.Substring (0, plants [i].name.Length - 2);
			string onlyNumber = data.d_plantStage [i].Substring (data.d_plantStage [i].Length - 1);
			data.d_plantAnimInfo [i] = onlyName + "_" + onlyNumber + "_Idle";
			data.d_plantWaterValue [i] = info.currentWaterValue;
			data.d_plantLevelValue [i] = info.currentLevelValue;
			//Debug.Log (data.d_plantAnimInfo [i]);

		}
	}

	IEnumerator LoadPlants ()
	{
		GameObject[] plantsLoaded = Resources.LoadAll ("R_Plants", typeof(GameObject)).Cast<GameObject> ().ToArray ();
		Dictionary<string, GameObject> plantsDic = new Dictionary<string, GameObject> ();
		for (int i = 0; i < plantsLoaded.Length; i++) {
			plantsDic.Add (plantsLoaded [i].name, plantsLoaded [i]);
		}

		yield return new WaitForSeconds (0.1f);
		int e = 0;
		for (int i = 0; i < data.d_islandIsPlanted.Length; i++) {
			if (data.d_islandIsPlanted [i] == true) {
				string name = data.d_plantName [e];
				int indx = name.IndexOf (" ");
				name = name.Substring (0, indx);

				string parentName = data.d_plantName [e].Substring (data.d_plantName [e].Length - 2);
				GameObject instantiatePlant = Instantiate (plantsDic [name]) as GameObject;
				GameObject parent = null;
				if (parentName.Contains (" ")) {
					parent = GameObject.Find ("island" + parentName);
				} else {
					parent = GameObject.Find ("island " + parentName);
				}
				instantiatePlant.transform.SetParent (parent.transform);
				instantiatePlant.transform.localPosition = plantsDic [name].transform.localPosition;
				instantiatePlant.name = data.d_plantName [e];
				Plants info = instantiatePlant.GetComponent<Plants> ();
				info.numberOfStages = (Plants.NumberOfStages)System.Enum.Parse (typeof(Plants.NumberOfStages), data.d_plantStage [e]);
				TextMesh text = instantiatePlant.GetComponentInChildren<TextMesh> ();
				text.text = data.d_plantStage [e].Substring (5);
				Animator plantAnim = instantiatePlant.GetComponent<Animator> ();
				plantAnim.CrossFade (data.d_plantAnimInfo [e], 0f);
				info.waterBarObj.localScale = new Vector3 (data.d_plantWaterValue [e],
				                                          info.waterBarObj.localScale.y,
				                                          info.waterBarObj.localScale.z);
				info.levelBarObj.localScale = new Vector3 (info.levelBarObj.localScale.x,
				                                          data.d_plantLevelValue [e],
				                                          info.levelBarObj.localScale.z);
				e++;
			}
		}
	}

	void SaveItems ()
	{
		Inventory2 inventoryScript = GameObject.Find ("Inventory").GetComponent<Inventory2> ();
		data.d_itemNames = new string[inventoryScript.items.Count];
		data.d_itemQuantities = new int[inventoryScript.quantities.Count];

		for (int i = 0; i < inventoryScript.items.Count; i++) {
			data.d_itemNames [i] = inventoryScript.items [i].name;
			data.d_itemQuantities [i] = inventoryScript.quantities [i];
		}
	}

	void LoadItems ()
	{
		Transform inventoryTrans = GameObject.Find ("Inventory").transform;
		Inventory2 inventoryScript = GameObject.Find ("Inventory").GetComponent<Inventory2> ();
		GameObject[] itemsLoaded = Resources.LoadAll ("R_Items", typeof(GameObject)).Cast<GameObject> ().ToArray ();
		Dictionary<string, GameObject> itemsDic = new Dictionary<string, GameObject> ();

		if (inventoryTrans.childCount != 0) {
			for (int i = 0; i < inventoryTrans.childCount; i++) {
				Destroy (inventoryTrans.GetChild (i).gameObject);
			}
		}

		for (int i = 0; i < itemsLoaded.Length; i++) {
			itemsDic.Add (itemsLoaded [i].name, itemsLoaded [i]);
		}

		inventoryScript.items = new List<GameObject> ();
		inventoryScript.quantities = new List<int> ();

		for (int i = 0; i < data.d_itemNames.Length; i++) {
			Debug.Log ("Key = " + data.d_itemNames [i]);
			GameObject instantiateItem = Instantiate (itemsDic [data.d_itemNames [i]]) as GameObject;
			instantiateItem.transform.SetParent (inventoryTrans);
			instantiateItem.transform.localScale = new Vector3 (1, 1, 1);
			instantiateItem.transform.localPosition = new Vector3 (0, 0, 0);
			instantiateItem.name = instantiateItem.name.Replace ("(Clone)", "");
			ObjectCounter counter = instantiateItem.GetComponent<ObjectCounter> ();
			counter.quantia = data.d_itemQuantities [i];


			inventoryScript.items.Add (instantiateItem);
			inventoryScript.quantities.Add (counter.quantia);
		}
	}

	void SaveRecipes ()
	{
		if (recipeScript.discoveredRecipes != null) {
			data.d_recipeItem1 = new string[recipeScript.discoveredRecipes.Count];
			data.d_recipeItem2 = new string[recipeScript.discoveredRecipes.Count];
			data.d_recipeResult = new string[recipeScript.discoveredRecipes.Count];
			for (int i = 0; i < recipeScript.discoveredRecipes.Count; i++) {

				data.d_recipeItem1 [i] = recipeScript.discoveredRecipes.ElementAt (i).Value.item1;
				data.d_recipeItem2 [i] = recipeScript.discoveredRecipes.ElementAt (i).Value.item2;
				data.d_recipeResult [i] = recipeScript.discoveredRecipes.Keys.ElementAt (i);

				Debug.Log (data.d_recipeItem1 [i]);
				Debug.Log (data.d_recipeItem2 [i]);
				Debug.Log (data.d_recipeResult [i]);
			}
		}
	}

	void LoadRecipes ()
	{
		if (data.d_recipeItem1 != null) {

			for (int i = 0; i < data.d_recipeItem1.Length; i++) {
				recipeScript.UpdateRecipeBook (LoadObj (data.d_recipeItem1 [i]), 
			                              LoadObj (data.d_recipeItem2 [i]),
			                              LoadObj (data.d_recipeResult [i]));
			}
		}
	}

	GameObject LoadObj (string itemName)
	{
		
		GameObject obj = Resources.Load ("R_Items/" + itemName) as GameObject;
		return obj;
	}


	[System.Serializable]
	class GameData
	{
		//season and island info
		public string d_season;
		public string[] d_islandName;
		public string[] d_islandParentName;
		public string[] d_islandType;
		public int[] d_islandCooldown;
		public bool[] d_islandIsPlanted;
		//plants info
		public string[] d_plantName;
		public string[] d_plantStage;
		public string[] d_plantAnimInfo;
		public float[] d_plantWaterValue;
		public float[] d_plantLevelValue;
		//items info
		public string[] d_itemNames;
		public int[] d_itemQuantities;
		//recipes info
		public string[] d_recipeItem1;
		public string[] d_recipeItem2;
		public string[] d_recipeResult;
	}
}
