using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Seasons : MonoBehaviour
{
	//public
	public GameObject gameHolder;
	public SpriteRenderer background;
	public Sprite[] seasonBackgrounds;
	public enum GameSeasons
	{
		Summer,
		Autumn,
		Spring,
		Winter
	}
	;
	public static GameSeasons gameSeasons;



	void Start ()
	{
		Debug.Log (gameSeasons);
	}
	

	public void ChangeSeason (string seasonToChange)
	{
		if (seasonToChange == "Summer") {
			gameSeasons = GameSeasons.Summer;
			background.sprite = seasonBackgrounds [2];
			CheckIslandsForChanges ();
		}
		if (seasonToChange == "Autumn") {
			gameSeasons = GameSeasons.Autumn;
			background.sprite = seasonBackgrounds [3];
			CheckIslandsForChanges ();
		}
		if (seasonToChange == "Spring") {
			gameSeasons = GameSeasons.Spring;
			background.sprite = seasonBackgrounds [1];
			CheckIslandsForChanges ();
		}
		if (seasonToChange == "Winter") {
			gameSeasons = GameSeasons.Winter;
			background.sprite = seasonBackgrounds [0];
			CheckIslandsForChanges ();
		}
	}

	void CheckIslandsForChanges ()
	{
		GameObject[] islands;
		islands = GameObject.FindGameObjectsWithTag ("PlantingSpots");
		for (int i = 0; i < islands.Length; i++) {
			Spot spot = islands [i].GetComponent<Spot> ();
			spot.ApplySeasonChanges ();
			Debug.Log (gameSeasons);
		}

	}

}
