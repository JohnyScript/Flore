using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Seed : MonoBehaviour
{
	//seed
	public GameObject[] seeds;
	//plants
	public GameObject[] plants;
	public string plantName;
	
	private GameObject pressedButton;
	public GameObject lastPressed;
	public bool seedFalling = false;
	public string flowerType;



	void Update ()
	{
		if (EventSystem.current.currentSelectedGameObject == null) {
			pressedButton = null;
		} else if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.tag == "seedButtons" && !seedFalling) {
			pressedButton = EventSystem.current.currentSelectedGameObject;
			if (pressedButton != null) 
				lastPressed = pressedButton;
		}
		
	}
	
	public void SetSeedFalling (bool state)
	{
		seedFalling = state;
	}
	
	#region Seed
	public GameObject SeedType ()
	{
		GameObject seedToBeUsed = null;
		
		//for the new buttons to work, all seed buttons must follow this criteria: tag = seedButtons, name = [PlantName]Seed
		if (lastPressed.name == "VirideSeed") {
			seedToBeUsed = seeds [0];
			flowerType = "normal";
		} else if (lastPressed.name == "AliquamSeed") {
			seedToBeUsed = seeds [1];
			flowerType = "normal";
		} else if (lastPressed.name == "IcosSeed") {
			seedToBeUsed = seeds [2];
			flowerType = "water";
		} else if (lastPressed.name == "MambusSeed") {
			seedToBeUsed = seeds [3];
			flowerType = "mud";
		} else if (lastPressed.name == "PurletoSeed") {
			seedToBeUsed = seeds [4];
			flowerType = "mud";
		} else if (lastPressed.name == "MolliaSeed") {
			seedToBeUsed = seeds [5];
			flowerType = "water";
		} else if (lastPressed.name == "TyraSeed") {
			seedToBeUsed = seeds [6];
			flowerType = "normal";
		} else if (lastPressed.name == "LuminaSeed") {
			seedToBeUsed = seeds [7];
			flowerType = "normal";
		}
		
		return seedToBeUsed;
	}
	#endregion
	
	#region Plant
	public GameObject PlantPrefab ()
	{
		GameObject plantToBeUsed = null;
		
		if (SeedType () == seeds [0]) {
			plantToBeUsed = plants [0];
			plantName = "Viride";
		}
		if (SeedType () == seeds [1]) {
			plantToBeUsed = plants [1];
			plantName = "Aliquam";
		} 
		if (SeedType () == seeds [2]) {
			plantToBeUsed = plants [2];
			plantName = "Icos";
		}
		if (SeedType () == seeds [3]) {
			plantToBeUsed = plants [3];
			plantName = "Mambus";
		}
		if (SeedType () == seeds [4]) {
			plantToBeUsed = plants [4];
			plantName = "Purleto";
		}
		if (SeedType () == seeds [5]) {
			plantToBeUsed = plants [5];
			plantName = "Mollia";
		}
		if (SeedType () == seeds [6]) {
			plantToBeUsed = plants [6];
			plantName = "Tyra";
		}
		if (SeedType () == seeds [7]) {
			plantToBeUsed = plants [7];
			plantName = "Lumina";
		}
		return plantToBeUsed;
	}
	
	#endregion
}
