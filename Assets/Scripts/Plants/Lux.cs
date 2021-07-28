using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Lux : PlantClass
{
	//public
	public GameObject luxLight;
	public GameObject luxStrongLight;

	public GameObject seedDroped;

	//private
	GameObject waterButton;
	Waterbutton waterScript;

	private GameObject scytheButton;
	Scythe scytheScript;

	private GameObject shovelButton;
	Shovel shovelScript;
	
	private GameObject pressedButton;
	//private GameObject lastPressed;

	GameObject addParticlesToPlant;

	Vector3 lootSpawn;

	void Start ()
	{
		InvokeRepeating ("DecreaseWaterBar", 0, 1);
		InvokeRepeating ("IncreaseLevelBar", 0, 1);

		soundSource = GetComponent<AudioSource> ();

		InitNearbySpots ();

		//thisIndex = int.Parse ();
		
		waterScript = GameObject.Find ("Water_Button").GetComponent<Waterbutton> ();
		scytheScript = GameObject.Find ("ScytheCrafted").GetComponent<Scythe> ();
		shovelScript = GameObject.Find ("Shovel").GetComponent<Shovel> ();

		
		divideWaterValue = waterBarTransform.localScale.x / waterLevelTimer;
		divideLevelValue = 1 / plantLevelTimer;
		
		waterBarOriginalValue = new Vector3 (waterBarTransform.localScale.x, waterBarTransform.localScale.y, waterBarTransform.localScale.z);
		levelBarOriginalValue = new Vector3 (levelBarTransform.localScale.x, levelBarTransform.localScale.y, levelBarTransform.localScale.z);
		levelBarScaleToLevel = new Vector3 (1, 1, 1);

		lootSpawn = new Vector3 (-0.1f, 0.3f, -0.4f);

		levelNumber = levelNumberObj.GetComponent<TextMesh> ();
	}

	void Update ()
	{		
		GetPlantsNearby ();

		if (EventSystem.current.currentSelectedGameObject == null) {
			pressedButton = null;
		} else if (EventSystem.current.currentSelectedGameObject != null) {
			pressedButton = EventSystem.current.currentSelectedGameObject;
			if (pressedButton != null) {
				//lastPressed = pressedButton;
			}
		}

		if (!shovelScript.hasShovel) {
			SetPlantToNormal ();
		}
	}	
	
	void OnMouseUpAsButton ()
	{
		if (waterScript.hasWater) {
			InvokeRepeating ("GrowLux", 0, 1);
			wasWatered = true;
			waterBarTransform.localScale = waterBarOriginalValue;
			waterScript.hasWater = false;
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

	private void GrowLux ()
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
				Vector3 posParticles = new Vector3 (0, 1.63f, 0.92f);
				addParticlesToPlant = (GameObject)Instantiate (luxLight, posParticles, Quaternion.AngleAxis (0, Vector3.up));
				addParticlesToPlant.transform.parent = transform;
				addParticlesToPlant.transform.localPosition = posParticles;
				instatiatedObject = Instantiate (seedDroped);
				instatiatedObject.transform.parent = transform;
				instatiatedObject.transform.localPosition = lootSpawn;
				CancelInvoke ("GrowLux");
			}
			break;
		case NumberOfStages.Stage2:
			if (levelBarTransform.localScale == levelBarScaleToLevel) {
				Destroy (addParticlesToPlant);
				plantAnimator.SetTrigger ("changeAnimation");
				plantRend.material.mainTexture = stage3Texture;
				numberOfStages = NumberOfStages.Stage3;
				levelBarTransform.localScale = levelBarOriginalValue;
				levelNumber.text = "3";
				PlaySound (growingSound);
				Vector3 posParticles = new Vector3 (0, 2.5f, 0.75f);
				addParticlesToPlant = (GameObject)Instantiate (luxStrongLight, posParticles, Quaternion.AngleAxis (0, Vector3.up));
				addParticlesToPlant.transform.parent = transform;
				addParticlesToPlant.transform.localPosition = posParticles;
				CancelInvoke ("GrowLux");
			}
			break;
		}
	}






}
