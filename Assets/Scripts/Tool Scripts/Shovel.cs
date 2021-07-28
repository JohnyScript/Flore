using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Shovel : MonoBehaviour
{

	public bool hasShovel = false;
	public bool isActive = false;
	public bool clickedPlant = false;
	public GameObject plantToMove;
	public GameObject plantToMoveOldSpot;

	public Image buttonImage;
	
	public Sprite normalSprite;
	public Sprite pressedSprite;

	void Update ()
	{
		
		if (isActive) {
			buttonImage.sprite = pressedSprite;
		} else {
			buttonImage.sprite = normalSprite;
		}
		
	}

	public void ShovelWasClicked ()
	{
		if (!isActive) {
			hasShovel = true;
			isActive = true;
		} else if (isActive) {
			hasShovel = false;
			isActive = false;

		}
	}

	public void MovePlant (GameObject plantReceived, GameObject spotReceived)
	{

		clickedPlant = true;
		plantToMove = plantReceived;
		plantToMoveOldSpot = spotReceived;

	}

}
