using UnityEngine;
using System.Collections;

public class PlantsController: MonoBehaviour
{
	public bool hasSeed;
	public bool hasWater;

	//Access to TileGenerator
	private TilesGenerator accessTileGen;
	private GameObject tileGameobject;

	//Add the instantiated plants to an array for global control
	public GameObject[] instantiatedPlants;

	private float timer;
	public static int floreTimer;

	public int plantSpotIndex;
	public int plantWasBorn;
	

	void Awake ()
	{
		timer = Time.time;
	}

	void Start ()
	{
		tileGameobject = GameObject.Find ("1_terra");
		accessTileGen = (TilesGenerator)tileGameobject.GetComponent (typeof(TilesGenerator));

		instantiatedPlants = new GameObject[accessTileGen.NumberTilesCreated ()];
		//plantBirthday = new int[accessTileGen.NumberTilesCreated ()];
	}


	public void UpdatePlantArray (GameObject newPlant, int plantSpot)
	{
		//Debug.Log ("timePlantWasBorn = " + timePlantWasBorn);
		instantiatedPlants [plantSpot] = newPlant;
		//plantBirthday [plantSpot] = timePlantWasBorn;
		plantSpotIndex = plantSpot;
		//plantWasBorn = timePlantWasBorn;
	}


	void Update ()
	{
		timer += Time.deltaTime;
		floreTimer = Mathf.FloorToInt (timer);
		//Debug.Log ("Flore = " + floreTimer);
	}

	public void SeedWasClicked ()
	{
		hasSeed = true;

	}
	void waterwasclicked ()
	{
		
		
		hasWater = true;
		
	}


}
