using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Plants : MonoBehaviour, IPointerClickHandler
{

	//public
	public GameObject[] drops;
	//bars part
	public Transform waterBarObj;
	public Transform levelBarObj;
	public TextMesh level;
	public float waterTime;
	public float levelTime;
	public GameObject innerWaterBucket;
	public GameObject innerScythe;
	//end bars
	public float stage2X;
	public float stage3X;

	public enum NumberOfStages
	{
		Stage1,
		Stage2,
		Stage3
	}
	public NumberOfStages numberOfStages; 
	public enum PlantType
	{
		normal,
		mud,
		water,
		ice,
		poison
	}
	public PlantType plantType;

	public float currentWaterValue;
	public float currentLevelValue;

	//private
	Animator plantAnimator;
	Vector3 lootSpawn;
	bool hasIncompatibility = false;

	float divideWaterValue;
	float divideLevelValue;
	Vector3 waterBarOriginalValue;
	Vector3 levelBarOriginalValue;
	Vector3 levelBarLevelUp;

	Animation waterAnim;
	Animation scytheAnim;

	//access to scripts
	Scythe scytheScript;
	Waterbutton waterScript;
	Spot spotScript;

	//Shovel Stuff
	
	Color plantTransparancy;
	Shovel shovelScript;
	GameObject parentObject;
	SpriteRenderer spriteRend;

	public void Awake ()
	{
		waterBarOriginalValue = waterBarObj.localScale;
		Debug.Log ("water original = " + waterBarOriginalValue.x);
		levelBarOriginalValue = levelBarObj.localScale;
		levelBarLevelUp = new Vector3 (1, 1, 1);
		plantAnimator = GetComponent<Animator> ();
		lootSpawn = new Vector3 (0, 0, 0);
		scytheScript = GameObject.Find ("Scythe").GetComponent<Scythe> ();
		waterScript = GameObject.Find ("Water").GetComponent<Waterbutton> ();
		shovelScript = GameObject.Find("Shovel").GetComponent<Shovel>();
		spriteRend = GetComponent<SpriteRenderer>();
		divideLevelValue = 1 / levelTime;
		divideWaterValue = waterBarObj.localScale.x / waterTime;
		InvokeRepeating ("IncreaseLevelBar", 0, 1);
		InvokeRepeating ("DecreaseWaterBar", 0, 1);
		waterAnim = innerWaterBucket.GetComponent<Animation> ();
		scytheAnim = innerScythe.GetComponent<Animation> ();
	}

	void Start ()
	{
		spotScript = GetComponentInParent<Spot> ();
	}

	void Update ()
	{
		if (spotScript.type.ToString () != plantType.ToString () && !hasIncompatibility) {
			hasIncompatibility = true;
			StartCoroutine ("LetPlantDieNaturally");
		} /*else if (hasIncompatibility && spotScript.type.ToString() == plantType.ToString()) {

		}*/
		if(!shovelScript.hasShovel){
			SetPlantToNormal();
		}
	}

	public void OnPointerClickActions ()
	{
		if (waterScript.hasWater) {
			InvokeRepeating ("GrowPlant", 0, 1);
			waterBarObj.localScale = waterBarOriginalValue;
			StartCoroutine ("WaterBucketAnimation");
		}
		if (scytheScript.hasScythe) {
			StartCoroutine ("ScytheAnimation");
		}
		if (shovelScript.hasShovel){
			UsedShovel();
		}
	}

	IEnumerator WaterBucketAnimation ()
	{
		innerWaterBucket.SetActive (true);
		waterAnim.Play ();
		yield return new WaitForSeconds (waterAnim.clip.length);
		innerWaterBucket.SetActive (false);
	}

	IEnumerator ScytheAnimation ()
	{
		innerScythe.SetActive (true);
		scytheAnim.Play ();
		yield return new WaitForSeconds (0.50f);
		innerScythe.SetActive (true);
		scytheScript.hasScythe = false;
		scytheScript.isActive = false;
		KillPlant ();
	}

	public void UsedShovel (){

		transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y + 1.5f, transform.localPosition.z);
		plantTransparancy.a = 0.5f;
		plantTransparancy.r = 1f;
		plantTransparancy.g = 1f;
		plantTransparancy.b = 1f;
		spriteRend.color = plantTransparancy;
		shovelScript.MovePlant(gameObject, transform.parent.gameObject);

	}

	public void SetPlantToNormal ()
	{
		plantTransparancy.a = 1f;
		plantTransparancy.r = 1f;
		plantTransparancy.g = 1f;
		plantTransparancy.b = 1f;
		spriteRend.color = plantTransparancy;
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		OnPointerClickActions ();
	}

	//virtual faz com que possa fazer override desta função nas child classes
	//na child class é só declarar esta função desta forma:
	//public override void GrowPlant() {
	// }

	public virtual void GrowPlant ()
	{
		switch (numberOfStages) {
		case NumberOfStages.Stage1: 
			if (levelBarObj.localScale == levelBarLevelUp) {
				plantAnimator.SetTrigger ("changeStage");
				levelBarObj.localScale = levelBarOriginalValue;
				transform.localPosition = new Vector3 (stage2X, transform.localPosition.y, transform.localPosition.z);
				level.text = "2";
				numberOfStages = NumberOfStages.Stage2;
				EnergyPoints.IncreaseTotalPoints (3);
				DropItems (drops [0]);
				CancelInvoke ("GrowPlant");
			}
			break;

		case NumberOfStages.Stage2:
			if (levelBarObj.localScale == levelBarLevelUp) {
				plantAnimator.SetTrigger ("changeStage");
				levelBarObj.localScale = levelBarOriginalValue;
				transform.localPosition = new Vector3 (stage3X, transform.localPosition.y, transform.localPosition.z);
				level.text = "3";
				numberOfStages = NumberOfStages.Stage3;
				EnergyPoints.IncreaseTotalPoints (3);
				DropItems (drops [1]);
				CancelInvoke ("GrowPlant");
			}
			break;

		}

	}

	public void DropItems (GameObject toDrop)
	{
		GameObject instantiatedObject = Instantiate (toDrop);
		instantiatedObject.transform.SetParent (transform);
		instantiatedObject.transform.localPosition = lootSpawn;
	}

	IEnumerator LetPlantDieNaturally ()
	{
		yield return new WaitForSeconds (5);
		KillPlant ();
	}


	public void KillPlant ()
	{
		Destroy (gameObject);
		Spot spot = transform.parent.GetComponent<Spot> ();
		spot.isPlanted = false;
		spot.islandCol.size = new Vector2 (spot.islandCol.size.x, 1.9f);
		spot.islandCol.offset = new Vector2 (spot.islandCol.offset.x, 0.95f);
		//spot.StartCoroutine ("CoDeteriorateIsland");
	}

	void IncreaseLevelBar ()
	{
		currentLevelValue = levelBarObj.localScale.y + divideLevelValue;
		levelBarObj.localScale = new Vector3 (levelBarObj.localScale.x, 
		                                     Mathf.Clamp (currentLevelValue, 0, 1), 
		                                     levelBarObj.localScale.z);
	}

	void DecreaseWaterBar ()
	{
		currentWaterValue = waterBarObj.localScale.x - divideWaterValue;
		waterBarObj.localScale = new Vector3 (Mathf.Clamp (currentWaterValue, -0.5f, waterTime),
		                                     waterBarObj.localScale.y,
		                                     waterBarObj.localScale.z);
		if (waterBarObj.localScale.x < 0) {
			KillPlant ();
		}
	}


}
