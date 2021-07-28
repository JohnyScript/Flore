using UnityEngine;
using System.Collections;

public class PlantClass: MonoBehaviour
{
	public GameObject gameHolder;
	public Texture stage2Texture;
	public Texture stage3Texture;
	public SkinnedMeshRenderer plantRend;
	
	public Animator plantAnimator;
	//float plantOnAir;
	
	public bool wasWatered;
	
	protected AudioSource soundSource;
	public AudioClip growingSound;
	public AudioClip plantingSound;
	
	public enum NumberOfStages
	{
		Stage1,
		Stage2,
		Stage3
	}
	public NumberOfStages numberOfStages; 
	
	//Bars part
	public Transform waterBarTransform;
	public Transform levelBarTransform;
	public GameObject levelNumberObj;
	protected TextMesh levelNumber;
	
	protected Vector3 waterBarOriginalValue;
	protected Vector3 levelBarOriginalValue;
	protected Vector3 levelBarScaleToLevel;
	
	public float waterLevelTimer;
	public float plantLevelTimer;
	
	protected float divideWaterValue;
	protected float divideLevelValue;
	//end of Bars part
	
	protected int thisIndex;
	public Transform leftContainer;
	public Transform rightContainer;

	protected GameObject warningObj;
	public GameObject plantWarning;
	public float warningHeight;
	public int timeLimit;
	int timeHasPassed = 0;
	public GameObject[] plantsNearby;
	public GameObject[] incompatiblePlants;
	public GameObject[] compatiblePlants;
	protected bool foundIncompatible1;
	protected bool foundIncompatible2;

	//private variables
	bool value;
	Color color;
	Transform leftPlant;
	Transform rightPlant;

	public PlantClass ()
	{
		numberOfStages = NumberOfStages.Stage1;
		plantsNearby = new GameObject[2];
	}

	void Awake ()
	{
		color = plantRend.material.color;
		gameHolder = GameObject.Find ("GameHolder");

	}

	public void InitNearbySpots ()
	{
		//string[] plantName = transform.name.Split ("(" [0]);
		//Debug.Log ("plant name = " + plantName [0]);
		thisIndex = int.Parse (transform.parent.parent.name);
		//Debug.Log ("this index = " + thisIndex);

	}

	public void KillPlant ()
	{
		Destroy (gameObject);
		Spot spot = transform.parent.GetComponent<Spot> ();
		spot.isPlanted = false;
	}

	public Transform[] GetPlantsNearby ()
	{
		leftContainer = gameHolder.transform.Find ((thisIndex - 1).ToString ());
		rightContainer = gameHolder.transform.Find ((thisIndex + 1).ToString ());

		if (leftContainer != null) {
			if (leftContainer.childCount != 0 && leftContainer.GetChild (0).tag == "PlantingSpots") {
				Transform leftSpot = leftContainer.GetChild (0);
				if (leftSpot.childCount != 0 && leftSpot.GetChild (0).tag == "Plant") {
					leftPlant = leftSpot.GetChild (0);
					Debug.Log ("Found a plant on the left!");
				} else {
					leftPlant = null;
					Debug.Log ("There is nothing planted on the left Container");
				}
			}
		} else if (leftContainer == null) {
			Debug.Log ("There's no left container");
			leftPlant = null;
		}
		if (rightContainer != null) {
			if (rightContainer.childCount != 0 && rightContainer.GetChild (0).tag == "PlantingSpots") {
				Transform rightSpot = rightContainer.GetChild (0);
				if (rightSpot.childCount != 0 && rightSpot.GetChild (0).tag == "Plant") {
					rightPlant = rightSpot.GetChild (0);
					Debug.Log ("Found a plant on the right!");
				} else {
					rightPlant = null;
					Debug.Log ("There is nothing planted on the right Container");
				}
			}
		} else if (rightContainer == null) {
			Debug.Log ("There's no right container");
			rightPlant = null;
		}
		Transform [] sidePlants = new Transform[2];
		sidePlants [0] = leftPlant;
		sidePlants [1] = rightPlant;
		return sidePlants;
	}

	/*public void CheckPlantsNearby (GameObject spotBefore, GameObject spotAfter)
	{
		if (spotBefore.transform.childCount != 0 && !spotBefore.transform.GetChild (0).name.Contains ("Seed") && !foundIncompatible1 && spotAfter.transform.childCount == 0) {
			plantsNearby [0] = spotBefore.transform.GetChild (0).gameObject;
			string[] plantName = plantsNearby [0].name.Split (" " [0]);
			plantsNearby [0].name = plantName [0];
			CheckCompatibility ();
			foundIncompatible1 = true;
		}
		if (spotBefore.transform.childCount == 0) {
			plantsNearby [0] = null;
			foundIncompatible1 = false;
		}
		if (spotAfter.transform.childCount == 0) {
			plantsNearby [1] = null;
			foundIncompatible2 = false;
		}
		if (spotAfter.transform.childCount != 0 && !spotAfter.transform.GetChild (0).name.Contains ("Seed") && !foundIncompatible2 && spotBefore.transform.childCount == 0) {
			plantsNearby [1] = spotAfter.transform.GetChild (0).gameObject;
			string[] plantName = plantsNearby [1].name.Split (" " [0]);
			plantsNearby [1].name = plantName [0];
			CheckCompatibility ();
			foundIncompatible2 = true;
		}
		if (spotBefore.transform.childCount != 0 && !spotBefore.transform.GetChild (0).name.Contains ("Seed") && !foundIncompatible1 && spotAfter.transform.childCount != 0 && !spotAfter.transform.GetChild (0).name.Contains ("Seed") && !foundIncompatible2) {
			DisplayWarning ();
			InvokeRepeating ("IncrementTime", 0, 1);
		}
	}*/
	
	void CheckCompatibility ()
	{
		for (int i = 0; i < plantsNearby.Length; i++) {
			if (plantsNearby [i] != null) {
				if (plantsNearby [i].name == incompatiblePlants [0].name) {
					DisplayWarning ();
					InvokeRepeating ("IncrementTime", 0, 1);
				}
			}
		}
	}

	public void DisplayWarning ()
	{
		Debug.Log ("Warning");
		Vector3 warningPos = new Vector3 (0, warningHeight, 0.50f);
		warningObj = (GameObject)Instantiate (plantWarning, warningPos, Quaternion.AngleAxis (0, Vector3.up));
		warningObj.transform.parent = transform;
		warningObj.transform.localPosition = warningPos;
	}
	
	public void IncrementTime ()
	{
		Debug.Log ("Time has passed = " + timeHasPassed);
		if (IsPlantStillBeingAfected ()) {
			if (timeHasPassed < timeLimit) {
				timeHasPassed++;
			} else if (timeHasPassed >= timeLimit) {
				Destroy (warningObj);
				KillPlant ();
				timeHasPassed = 0;
				CancelInvoke ("IncrementTime");
			}
		}
		if (!IsPlantStillBeingAfected ()) {
			Destroy (warningObj);
			CancelInvoke ("IncrementTime");
		}
	}

	bool IsPlantStillBeingAfected ()
	{
		for (int i = 0; i < plantsNearby.Length; i++) {
			if (plantsNearby [i] != null) {
				if (plantsNearby [i].name == incompatiblePlants [0].name) {
					value = true;
				}
				if (plantsNearby [i].name == compatiblePlants [0].name) {
					value = true;
				}
			}
		}
		
		if (plantsNearby [0] == null && plantsNearby [1] == null) {
			value = false;
			timeHasPassed = 0;
			Destroy (warningObj);
		}

		return value;
	}

	void IncrementTimeDouble ()
	{
		if (timeHasPassed < timeLimit / 2) {
			timeHasPassed++;
		} else if (timeHasPassed >= timeLimit / 2) {
			Destroy (warningObj);
			KillPlant ();
			timeHasPassed = 0;
			CancelInvoke ("IncrementTimeDouble");
		}
	}
	
	void DecreaseWaterBar ()
	{
		waterBarTransform.localScale = new Vector3 (Mathf.Clamp (waterBarTransform.localScale.x - divideWaterValue, -0.5f, waterLevelTimer), waterBarTransform.localScale.y, waterBarTransform.localScale.z);
		if (waterBarTransform.localScale.x < 0)
			KillPlant ();
	}
	
	void IncreaseLevelBar ()
	{
		levelBarTransform.localScale = new Vector3 (levelBarTransform.localScale.x, Mathf.Clamp (levelBarTransform.localScale.y + divideLevelValue, 0, 1), levelBarTransform.localScale.z);
	}

	public void PlaySound (AudioClip soundToPlay)
	{
		soundSource.clip = soundToPlay;
		soundSource.Play ();
	}

	public void GetPlantUp ()
	{

		transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y + 1.5f, transform.localPosition.z);
		color.a = 0.5f;
		color.r = 1f;
		color.g = 1f;
		color.b = 1f;
		plantRend.material.SetColor ("_Color", color);

	}

	public void SetPlantToNormal ()
	{

		color.a = 1f;
		color.r = 1f;
		color.g = 1f;
		color.b = 1f;
		plantRend.material.SetColor ("_Color", color);
		thisIndex = int.Parse (transform.parent.parent.name);
		Debug.Log ("this index = " + thisIndex);
	}

}
