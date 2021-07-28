using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Spot2 : MonoBehaviour
{
	public GameObject destructionParticles;
	public float seedYplacement;
	public float spotPlacement = 0.3f;
	public float spotHeight = 0.5f;
	public float centerPlantX = 0.25f;
	public float centerPlantY;
	public int spotIndex;
	public bool luxWasPlanted = false;
	
	private Vector3 plantPos;
	public bool isPlanted;
	bool previewMode = true;

	//Access to PlantsController, Seed and SeedButtons scripts
	private PlantsController access;
	private GameObject plantsController;
	private Seed accessSeed;
	private SeedButtons seedButtonsScript;

	//Access to Shovel and graphics
	public GameObject shovel;
	private Shovel shovelScript;


	private AudioSource soundSource;
	public AudioClip plantingSound;
	
	private GameObject pressedButton;
	GameObject instantiatedPlant;

	Color color;
	
	void Start ()
	{
		plantsController = GameObject.Find ("GameController"); 
		access = (PlantsController)plantsController.GetComponent (typeof(PlantsController));
		accessSeed = (Seed)plantsController.GetComponent (typeof(Seed));

		shovel = GameObject.Find ("Shovel");
		shovelScript = shovel.GetComponent<Shovel> ();
		
		color = GetComponent<Renderer> ().material.color;
		color.a = 0;
		GetComponent<Renderer> ().material.SetColor ("_Color", color);

		soundSource = GetComponent<AudioSource> ();
		

		//spot index will be equal to the spot's name
		spotIndex = int.Parse (this.name);
	}
	
	void Update ()
	{
		if (transform.childCount == 0)
			isPlanted = false;
		else if (transform.childCount == 1) 
			isPlanted = true;
		if (EventSystem.current.currentSelectedGameObject == null) {
			pressedButton = null;
		} else if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.tag == "seedButtons") {
			pressedButton = EventSystem.current.currentSelectedGameObject;
			seedButtonsScript = pressedButton.GetComponent<SeedButtons> ();
		}
		if (shovelScript.hasShovel && previewMode) {
			color.a = 0.5f;
			GetComponent<Renderer> ().material.SetColor ("_Color", color);
		} else if (previewMode) {
			color.a = 0;
			GetComponent<Renderer> ().material.SetColor ("_Color", color);
		}
		if (isPlanted) {
			for (int i = 0; i < gameObject.transform.childCount; i++) {
				if (gameObject.transform.GetChild (i).name.Contains ("Lux") && gameObject.transform.GetChild (i).tag == "Plant") {
					luxWasPlanted = true;
					
				}
			}
			
		}
	
	}
	
	
	void OnMouseUpAsButton ()
	{

		if (shovelScript.hasShovel && previewMode) {

			PlaySound (plantingSound);
			//rend.material.mainTexture = originalTexture;
			shovelScript.hasShovel = false;
			shovelScript.isActive = false;
			//shovelScript.spotWasDug = true;
			previewMode = false;
			color.a = 1f;
			GetComponent<Renderer> ().material.SetColor ("_Color", color);

		} else if (seedButtonsScript != null) {
			if (seedButtonsScript.hasSeed && !isPlanted && !previewMode) {
				StartCoroutine (MakeSeedAnimation (accessSeed.SeedType ()));
				isPlanted = true;
			} else if (access.hasWater && isPlanted) {
				access.hasWater = false;
			} 
		}

		if (shovelScript.clickedPlant && !previewMode) {

			shovelScript.plantToMove.transform.position = new Vector3 (transform.position.x + centerPlantX, transform.position.y + centerPlantY, transform.position.z - 0.1f);
			shovelScript.plantToMove.transform.SetParent (transform);
			shovelScript.hasShovel = false;
			shovelScript.isActive = false;
			shovelScript.clickedPlant = false;
			shovelScript.plantToMove = null;
			PlaySound (plantingSound);

		}

	}
	
	IEnumerator MakeSeedAnimation (GameObject seed)
	{
		accessSeed.SetSeedFalling (true);
		GameObject instantiatedSeed;
		Vector3 seedPos = new Vector3 (transform.position.x + centerPlantX,
		                               seedYplacement,
		                               transform.position.z - 0.1f);
		instantiatedSeed = (GameObject)Instantiate (seed, seedPos, Quaternion.AngleAxis (-90f, Vector3.up));
		instantiatedSeed.transform.parent = transform;
		
		Animation seedAnimation = seed.GetComponent<Animation> ();

		//PlaySound(plantingSound);

		yield return new WaitForSeconds (seedAnimation.clip.length);

		Destroy (instantiatedSeed);


		yield return new WaitForSeconds (0.2f);
		InstantiatePlant (accessSeed.PlantPrefab ());
		accessSeed.SetSeedFalling (false);
		//seedButtonsScript.SpendASeed();
	}
	
	public void InstantiatePlant (GameObject plantPrefab)
	{
		string giveName = accessSeed.plantName;
		
		plantPos = new Vector3 (transform.position.x + centerPlantX,
		                        transform.position.y + centerPlantY,
		                        transform.position.z - 0.1f);
		
		instantiatedPlant = (GameObject)Instantiate (plantPrefab, plantPos, Quaternion.AngleAxis (-90f, Vector3.up));
		instantiatedPlant.transform.parent = transform;
		instantiatedPlant.name = giveName + spotIndex.ToString ();
		access.UpdatePlantArray (instantiatedPlant, spotIndex);
	}

	void PlaySound (AudioClip soundToPlay)
	{

		soundSource.clip = soundToPlay;
		soundSource.Play ();

	}

}
