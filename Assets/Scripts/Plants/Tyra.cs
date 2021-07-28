using UnityEngine;
using System.Collections;

public class Tyra : PlantClass
{
	public GameObject seedDroped;
	public GameObject needSymbol;
	//public GameObject objectDropped;

	//access waterScript
	GameObject waterButton;
	Waterbutton waterButtonScript;
	//access Scythe script
	private GameObject scytheButton;
	Scythe scytheScript;
	private GameObject shovelButton;
	Shovel shovelScript;
	Lux luxScript;
	bool alreadyCalled;
	Vector3 lootSpawn;
	bool placeNeed;

	void Start ()
	{
		InvokeRepeating ("DecreaseWaterBar", 0, 1);
		InvokeRepeating ("IncreaseLevelBar", 0, 1);
		//InvokeRepeating ("LuxIsInStage3", 0, 1);
		
		soundSource = GetComponent<AudioSource> ();
		/*thisIndex = int.Parse (transform.parent.name);
		spotBefore = GameObject.Find ((thisIndex - 1).ToString ());
		spotAfter = GameObject.Find ((thisIndex + 1).ToString ());*/
		
		waterButton = GameObject.Find ("Water_Button");
		waterButtonScript = waterButton.GetComponent<Waterbutton> ();
		scytheButton = GameObject.Find ("ScytheCrafted");
		scytheScript = scytheButton.GetComponent<Scythe> ();
		shovelButton = GameObject.Find ("Shovel");
		shovelScript = shovelButton.GetComponent<Shovel> ();
		
		divideWaterValue = waterBarTransform.localScale.x / waterLevelTimer;
		divideLevelValue = 1 / plantLevelTimer;

		lootSpawn = new Vector3 (-0.1f, 0.3f, -0.4f);
		
		waterBarOriginalValue = new Vector3 (waterBarTransform.localScale.x, waterBarTransform.localScale.y, waterBarTransform.localScale.z);
		levelBarOriginalValue = new Vector3 (levelBarTransform.localScale.x, levelBarTransform.localScale.y, levelBarTransform.localScale.z);
		levelBarScaleToLevel = new Vector3 (1, 1, 1);
		
		levelNumber = levelNumberObj.GetComponent<TextMesh> ();
	}
	
	void Update ()
	{		
		/*CheckPlantsNearby (spotBefore, spotAfter);
				
		if (!shovelScript.hasShovel) {
			SetPlantToNormal ();
			thisIndex = int.Parse (transform.parent.name);
			spotBefore = GameObject.Find ((thisIndex - 1).ToString ());
			spotAfter = GameObject.Find ((thisIndex + 1).ToString ());
		}*/
		//Debug.Log ("calling = " + alreadyCalled);

	}	
	
	void OnMouseUpAsButton ()
	{
		if (waterButtonScript.hasWater) {
			InvokeRepeating ("GrowTyra", 0, 1);
			wasWatered = true;
			waterBarTransform.localScale = waterBarOriginalValue;
			waterButtonScript.hasWater = false;
		}
		if (scytheScript.hasScythe) {
			KillPlant ();
			scytheScript.hasScythe = false;
			scytheScript.isActive = false;
		}
		if (shovelScript.hasShovel && !shovelScript.clickedPlant) {
			//shovelScript.MovePlant (gameObject);
			GetPlantUp ();
		}
	}
	
	public void DisplayNeed ()
	{		
		Vector3 warningPos = new Vector3 (0, warningHeight, 0.50f);
		warningObj = (GameObject)Instantiate (needSymbol, warningPos, Quaternion.AngleAxis (0, Vector3.up));
		warningObj.transform.parent = transform;
		warningObj.transform.localPosition = warningPos;
	}

	void LuxIsInStage3 ()
	{
		if (LuxIsAround () != null) {
			luxScript = LuxIsAround ().GetComponent<Lux> ();
			if (luxScript.numberOfStages == NumberOfStages.Stage3 && !alreadyCalled) {
				DisplayWarning ();
				IncrementTime ();
				//InvokeRepeating ("IncrementTime", 0, 1);
				StartCoroutine ("WaitToCallLuxInStage3");
			}
		}
	}

	IEnumerator WaitToCallLuxInStage3 ()
	{
		yield return new WaitForSeconds (timeLimit);
		CancelInvoke ("LuxIsInStage3");
		alreadyCalled = true;
	}

	GameObject LuxIsAround ()
	{
		GameObject Lux = null;
		for (int i = 0; i < plantsNearby.Length; i++) {
			if (plantsNearby [i] != null && plantsNearby [i].name == "Lux") {
				Lux = plantsNearby [i];
			}
		}
		/*if (Lux != null && !luxWasCalled) {
			InvokeRepeating ("LuxIsInStage3", 0, 1);
			luxWasCalled = true;
		}*/
		return Lux;
	}

	private void GrowTyra ()
	{		
		switch (numberOfStages) {
		case NumberOfStages.Stage1:
			if (levelBarTransform.localScale == levelBarScaleToLevel) {
				GameObject instatiatedObject;
				plantAnimator.SetTrigger ("changeAnimation");
				plantRend.material.mainTexture = stage2Texture;
				numberOfStages = NumberOfStages.Stage2;
				levelBarTransform.localScale = levelBarOriginalValue;
				levelNumber.text = "2";
				PlaySound (growingSound);
				wasWatered = false;
				instatiatedObject = Instantiate (seedDroped);
				instatiatedObject.transform.parent = transform;
				instatiatedObject.transform.localPosition = lootSpawn;
				CancelInvoke ("GrowTyra");
			}
			break;
		case NumberOfStages.Stage2:
			LuxIsAround ();
			if (levelBarTransform.localScale == levelBarScaleToLevel && LuxIsAround () == null && !placeNeed) {
				DisplayNeed ();
				placeNeed = true;
			}
			if (plantsNearby [0] != null || plantsNearby [1] != null) {
				if (levelBarTransform.localScale == levelBarScaleToLevel && LuxIsAround () != null) {
					luxScript = LuxIsAround ().GetComponent<Lux> ();
					if (luxScript.numberOfStages == NumberOfStages.Stage2) {
						GameObject instatiatedObject;
						placeNeed = false;
						Destroy (warningObj);
						plantAnimator.SetTrigger ("changeAnimation");
						plantRend.material.mainTexture = stage3Texture;
						numberOfStages = NumberOfStages.Stage3;
						levelNumber.text = "3";
						PlaySound (growingSound);
						instatiatedObject = Instantiate (seedDroped);
						instatiatedObject.transform.parent = transform;
						instatiatedObject.transform.localPosition = lootSpawn;
						CancelInvoke ("GrowTyra");
					}
				}
			}
			break;
		}
	}



}
