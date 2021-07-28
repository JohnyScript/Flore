using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Spot : MonoBehaviour, IPointerClickHandler
{
	//public variables
	public GameObject innerWaterBucket;
	public bool isPlanted;
	public Sprite[] islandTextures;
	public float centerPlantX = 0.25f;
	public float centerPlantY = 0.3f;
	public float seedYplacement;
	public enum spotType
	{
		normal,
		poison,
		mud,
		water,
		ice
	}
	public spotType type;
	public GameObject interactionBox;
	public BoxCollider2D islandCol;

	//private variables
	int waterCount = 0;
	int spotNumber;
	int spotRow;
	public static GameObject pressedButton;
	GameObject[] islandsNearby;
	SpriteRenderer thisSprite;
	enum spotPosition
	{
		none,
		left,
		right, 
		mid
	}
	GameObject previousButton;
	GameObject systemFlore;
	Animation waterAnim;

	//access to scripts
	Seed accessSeed;
	SeedButtons seedButtonsScript;
	Inventory2 inventory;
	Shovel shovelScript;
	Waterbutton waterScript;

	Spot oldSpotOfReceivedPlant;

	void Awake ()
	{
		type = spotType.normal;
	}

	void Start ()
	{
		systemFlore = GameObject.Find ("FloreSystem");
		accessSeed = GameObject.Find ("GameController").GetComponent<Seed> ();
		inventory = GameObject.Find ("Inventory").GetComponent<Inventory2> ();
		shovelScript = GameObject.Find ("Shovel").GetComponent<Shovel> ();
		waterScript = GameObject.Find ("Water").GetComponent<Waterbutton> ();
		waterAnim = innerWaterBucket.GetComponent<Animation> ();

		spotNumber = int.Parse (transform.parent.name);
		islandsNearby = new GameObject[2];
		thisSprite = GetComponent<SpriteRenderer> ();

		InvokeRepeating ("CheckIslandsNearby", 0, 0.2f);

		if (isPlanted) {
			islandCol.size = new Vector2 (islandCol.size.x, 1.4f);
			islandCol.offset = new Vector2 (islandCol.offset.x, 0.70f);
		}

	}

	public void OnPointerClick (PointerEventData eventData)
	{
		if (seedButtonsScript != null) {
			if (seedButtonsScript.hasSeed && !isPlanted) {
				StartCoroutine (MakeSeedAnimation (accessSeed.SeedType (), accessSeed.flowerType));
			} else if (seedButtonsScript.hasSeed && isPlanted) {
				GameObject systemBox = systemFlore.transform.GetChild (0).gameObject;
				systemBox.SetActive (true);
				Text systemText = systemBox.GetComponentInChildren<Text> ();
				systemText.text = "This island already has a plant";
				seedButtonsScript.hasSeed = false;
			}
		} else if (previousButton != null && previousButton.name == "poison") {
			type = spotType.poison;
			ApplySeasonChanges ();
			previousButton.name.Replace ("_d", "");
			inventory.RemoveItem (previousButton);
			previousButton = null;
		} else if (waterScript.hasWater) {
			Debug.Log ("Added Water " + waterCount + " time");
			waterCount++;
			StartCoroutine ("WaterBucketAnimation");
			if (waterCount < 2) {
				type = spotType.mud;
				ApplySeasonChanges ();
			} else if (waterCount >= 2) {
				type = spotType.water;
				ApplySeasonChanges ();
			}
			//waterScript.hasWater = false;
		} else if (!isPlanted) {
			ShowInteractionBox ();

		}
		if (shovelScript.clickedPlant) {

			shovelScript.plantToMove.transform.SetParent (transform);
			shovelScript.plantToMove.transform.localPosition = new Vector3 (0,1.5f,0);
			shovelScript.plantToMove.name = shovelScript.plantToMove.name.Substring(0,shovelScript.plantToMove.name.Length-1);
			shovelScript.plantToMove.name = shovelScript.plantToMove.name + spotNumber.ToString();
			oldSpotOfReceivedPlant = shovelScript.plantToMoveOldSpot.GetComponent<Spot>();
			oldSpotOfReceivedPlant.isPlanted = false;
			oldSpotOfReceivedPlant.islandCol.enabled = true;
			shovelScript.hasShovel = false;
			shovelScript.isActive = false;
			shovelScript.clickedPlant = false;
			shovelScript.plantToMove = null;
			shovelScript.plantToMoveOldSpot = null;
			oldSpotOfReceivedPlant = null;
			isPlanted = true;
			islandCol.enabled = false;
			
		}
	}

	IEnumerator WaterBucketAnimation ()
	{
		innerWaterBucket.SetActive (true);
		waterAnim.Play ();
		yield return new WaitForSeconds (waterAnim.clip.length);
		innerWaterBucket.SetActive (false);
	}

	void ShowInteractionBox ()
	{
		if (interactionBox.activeSelf == true) {
			interactionBox.SetActive (false);
		} else if (interactionBox.activeSelf == false) {
			interactionBox.SetActive (true);
		}
	}

	void CheckIslandsNearby ()
	{
		int indexBefore = spotNumber - 1;
		int indexAfter = spotNumber + 1;
		bool leftExists = false;
		bool rightExists = false;

		if (GameObject.Find (indexBefore.ToString ()) != null) {
			islandsNearby [0] = GameObject.Find (indexBefore.ToString ());
			thisSprite.sprite = IslandSpriteToUse (spotPosition.none);
			leftExists = true;
		}
		if (GameObject.Find (indexAfter.ToString ()) != null) {
			islandsNearby [1] = GameObject.Find (indexAfter.ToString ());
			thisSprite.sprite = IslandSpriteToUse (spotPosition.none);
			rightExists = true;
		}


		//Debug.Log ("left = " + islandsNearby [0] + ". " + "right = " + islandsNearby [1]);

		if (leftExists && islandsNearby [0].transform.childCount != 0) {
			thisSprite.sprite = IslandSpriteToUse (spotPosition.left);
			if (rightExists && islandsNearby [1].transform.childCount != 0) {
				thisSprite.sprite = IslandSpriteToUse (spotPosition.mid);
			}

		}
		if (rightExists && islandsNearby [1].transform.childCount != 0) {
			thisSprite.sprite = IslandSpriteToUse (spotPosition.right);
			if (leftExists && islandsNearby [0].transform.childCount != 0) {
				thisSprite.sprite = IslandSpriteToUse (spotPosition.mid);
			}
		}
	}

	#region Island Texture
	Sprite IslandSpriteToUse (spotPosition whichPos)
	{
		Sprite spriteToUse = null;
		switch (type) {
		case spotType.normal:
			if (whichPos == spotPosition.none)
				spriteToUse = islandTextures [0];
			if (whichPos == spotPosition.left)
				spriteToUse = islandTextures [1];
			if (whichPos == spotPosition.mid)
				spriteToUse = islandTextures [2];
			if (whichPos == spotPosition.right)
				spriteToUse = islandTextures [3];
			break;
		case spotType.mud:
			if (whichPos == spotPosition.none)
				spriteToUse = islandTextures [4];
			if (whichPos == spotPosition.left)
				spriteToUse = islandTextures [5];
			if (whichPos == spotPosition.mid)
				spriteToUse = islandTextures [6];
			if (whichPos == spotPosition.right)
				spriteToUse = islandTextures [7];
			break;
		case spotType.water:
			if (whichPos == spotPosition.none)
				spriteToUse = islandTextures [8];
			if (whichPos == spotPosition.left)
				spriteToUse = islandTextures [9];
			if (whichPos == spotPosition.mid)
				spriteToUse = islandTextures [10];
			if (whichPos == spotPosition.right)
				spriteToUse = islandTextures [11];
			break;
		case spotType.ice:
			if (whichPos == spotPosition.none)
				spriteToUse = islandTextures [12];
			if (whichPos == spotPosition.left)
				spriteToUse = islandTextures [13];
			if (whichPos == spotPosition.mid)
				spriteToUse = islandTextures [14];
			if (whichPos == spotPosition.right)
				spriteToUse = islandTextures [15];
			break;
		case spotType.poison:
			if (whichPos == spotPosition.none)
				spriteToUse = islandTextures [16];
			if (whichPos == spotPosition.left)
				spriteToUse = islandTextures [17];
			if (whichPos == spotPosition.mid)
				spriteToUse = islandTextures [18];
			if (whichPos == spotPosition.right)
				spriteToUse = islandTextures [19];
			break;
		}
		return spriteToUse;
	}
	#endregion

	public void ApplySeasonChanges ()
	{
		if (Seasons.gameSeasons == Seasons.GameSeasons.Winter) {
			if (type == spotType.water) {
				type = spotType.ice;
				CheckIslandsNearby ();
			}
		}
		if (Seasons.gameSeasons == Seasons.GameSeasons.Spring) {
			if (type == spotType.ice) {
				type = spotType.water;
				CheckIslandsNearby ();
			}
		}
		if (Seasons.gameSeasons == Seasons.GameSeasons.Summer) {
			/*if (type == spotType.water) {
				type = spotType.mud;
				CheckIslandsNearby ();
			}*/
		}
		if (Seasons.gameSeasons == Seasons.GameSeasons.Autumn) {
			//Nothing to change, yet
		}
	}

	void Update ()
	{
		#region Debugging
		if (Input.GetKeyDown (KeyCode.P))
			Debug.Break ();
		#endregion
		if (EventSystem.current.currentSelectedGameObject == null) {
			pressedButton = null;
		} else if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.tag == "seedButtons") {
			pressedButton = EventSystem.current.currentSelectedGameObject;
			seedButtonsScript = pressedButton.GetComponent<SeedButtons> ();
			previousButton = pressedButton;
		}
		if (transform.childCount == 1 && !islandCol.enabled) {
			islandCol.enabled = true;
		}
	}

	IEnumerator MakeSeedAnimation (GameObject seed, string flowerType)
	{
		Debug.Log (type.ToString ());
		if (flowerType == type.ToString ()) {
			isPlanted = true;
			accessSeed.SetSeedFalling (true);
			GameObject instantiatedSeed;
			Vector3 seedPos = new Vector3 (transform.localPosition.x + centerPlantX,
		                               seedYplacement,
		                               transform.localPosition.z - 0.1f);
			instantiatedSeed = (GameObject)Instantiate (seed, seedPos, Quaternion.AngleAxis (-90f, Vector3.up));
			instantiatedSeed.transform.SetParent (transform);
		
			Animation seedAnimation = seed.GetComponent<Animation> ();
		
			//PlaySound(plantingSound);
		
			yield return new WaitForSeconds (seedAnimation.clip.length);
		
			Destroy (instantiatedSeed);
		
		
			yield return new WaitForSeconds (0.2f);
			InstantiatePlant (accessSeed.PlantPrefab ());
			accessSeed.SetSeedFalling (false);
			inventory.RemoveItem (accessSeed.lastPressed);
		} else {
			GameObject systemBox = systemFlore.transform.GetChild (0).gameObject;
			systemBox.SetActive (true);
			Text systemText = systemBox.GetComponentInChildren<Text> ();
			systemText.text = "This seed can't be planted on this type of island";
			seedButtonsScript.hasSeed = false;
			yield return null;
		}
	}
	
	public void InstantiatePlant (GameObject plantPrefab)
	{
		islandCol.enabled = false;
		string giveName = accessSeed.plantName;
		
		Vector3 plantPos = new Vector3 (centerPlantX,
		                        plantPrefab.transform.localPosition.y,
		                        0);
		
		GameObject instantiatedPlant = (GameObject)Instantiate (plantPrefab);
		instantiatedPlant.transform.parent = transform;
		instantiatedPlant.transform.localPosition = plantPos;
		//instantiatedPlant.transform.localRotation = Quaternion.AngleAxis (-90f, Vector3.up);
		instantiatedPlant.name = giveName + " " + transform.parent.name;
	}

	//Coroutine is called by Plants.cs
	public IEnumerator CoDeteriorateIsland ()
	{
		Debug.Log ("Island will get destroyed in 10 secs");
		yield return new WaitForSeconds (10);
		Destroy (gameObject);
	}

}
