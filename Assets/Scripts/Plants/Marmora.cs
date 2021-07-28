using UnityEngine;
using System.Collections;

public class Marmora : PlantClass
{
	//public variables
	public GameObject poison;

	//private variables
	GameObject waterButton;
	Waterbutton waterButtonScript;
	private GameObject scytheButton;
	Scythe scytheScript;
	private GameObject shovelButton;
	Shovel shovelScript;

	GameObject addParticlesNearby;

	void Start ()
	{
		InvokeRepeating ("DecreaseWaterBar", 0, 1);
		InvokeRepeating ("IncreaseLevelBar", 0, 1);

		soundSource = GetComponent<AudioSource> ();
		waterButton = GameObject.Find ("Water_Button");
		waterButtonScript = waterButton.GetComponent<Waterbutton> ();
		scytheButton = GameObject.Find ("ScytheCrafted");
		scytheScript = scytheButton.GetComponent<Scythe> ();
		shovelButton = GameObject.Find ("Shovel");
		shovelScript = shovelButton.GetComponent<Shovel> ();
				
		/*thisIndex = int.Parse (transform.parent.name);
		spotBefore = GameObject.Find ((thisIndex - 1).ToString ());
		spotAfter = GameObject.Find ((thisIndex + 1).ToString ());*/
		
		divideWaterValue = waterBarTransform.localScale.x / waterLevelTimer;
		divideLevelValue = 1 / plantLevelTimer;
		
		waterBarOriginalValue = new Vector3 (waterBarTransform.localScale.x, waterBarTransform.localScale.y, waterBarTransform.localScale.z);
		levelBarOriginalValue = new Vector3 (levelBarTransform.localScale.x, levelBarTransform.localScale.y, levelBarTransform.localScale.z);
		levelBarScaleToLevel = new Vector3 (1, 1, 1);

		levelNumber = levelNumberObj.GetComponent<TextMesh> ();
	}

	void Update ()
	{	
		//CheckPlantsNearby (spotBefore, spotAfter);
		
		/*if (!shovelScript.hasShovel) {
			SetPlantToNormal ();
			thisIndex = int.Parse (transform.parent.name);
			spotBefore = GameObject.Find ((thisIndex - 1).ToString ());
			spotAfter = GameObject.Find ((thisIndex + 1).ToString ());
		}*/
	}

	void OnMouseUpAsButton ()
	{
		if (waterButtonScript.hasWater) {
			InvokeRepeating ("GrowMarmora", 0, 1);
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

	private void GrowMarmora ()
	{		
		switch (numberOfStages) {
		case NumberOfStages.Stage1:
			if (levelBarTransform.localScale == levelBarScaleToLevel) {
				plantAnimator.SetTrigger ("changeAnimation");
				plantRend.material.mainTexture = stage2Texture;
				numberOfStages = NumberOfStages.Stage2;
				levelBarTransform.localScale = levelBarOriginalValue;
				levelNumber.text = "2";
				PlaySound (growingSound);
				wasWatered = false;
				//Instantiate poison on Marmora
				Vector3 posParticles1 = new Vector3 (0, 0.23f, 0.64f); 
				addParticlesNearby = (GameObject)Instantiate (poison, posParticles1, Quaternion.AngleAxis (0, Vector3.up));
				addParticlesNearby.transform.parent = transform;
				addParticlesNearby.transform.localPosition = posParticles1;

				//Instantiate poison cloud on Marmora's left side
				/*Transform spotBeforeTrans = spotBefore.GetComponent<Transform> ();
				Vector3 posParticles1 = new Vector3 (0, spotBeforeTrans.localPosition.y, 3.5f); 
				addParticlesNearby = (GameObject)Instantiate (poison, posParticles1, Quaternion.AngleAxis (0, Vector3.up));
				addParticlesNearby.transform.parent = transform;
				addParticlesNearby.transform.localPosition = posParticles1;
				//Instantiate poison cloud on Marmora's right side
				Transform spotAfterTrans = spotAfter.GetComponent<Transform> ();
				Vector3 posParticles2 = new Vector3 (0, spotAfterTrans.localPosition.y, -2.5f); 
				addParticlesNearby = (GameObject)Instantiate (poison, posParticles2, Quaternion.AngleAxis (0, Vector3.up));
				addParticlesNearby.transform.parent = transform;
				addParticlesNearby.transform.localPosition = posParticles2;
				CancelInvoke ("GrowMarmora");*/
			}
			break;
		case NumberOfStages.Stage2:
			if (levelBarTransform.localScale == levelBarScaleToLevel) {
				plantAnimator.SetTrigger ("changeAnimation");
				plantRend.material.mainTexture = stage3Texture;
				numberOfStages = NumberOfStages.Stage3;
				levelNumber.text = "3";
				PlaySound (growingSound);
				CancelInvoke ("GrowMarmora");
			}
			break;
		}
	}

}
