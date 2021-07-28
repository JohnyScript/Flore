using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnergyPoints : MonoBehaviour
{
	public GameObject systemBox;
	public Text systemText;
	public GameObject crafting;
	public GameObject store;
	public Text pointsText;
	public static int playerPoints = 0;

	public static bool wasCalled = false;
	public static bool purchased;
	
	void Start ()
	{
		playerPoints = int.Parse (pointsText.text.Replace (" Points", ""));
		Debug.Log (playerPoints);
	}

	public static void IncreaseTotalPoints (int numberToIncrease)
	{
		playerPoints += numberToIncrease;
		Debug.Log ("Player points = " + playerPoints);
		wasCalled = true;
	}

	void Update ()
	{
		if (wasCalled) {
			pointsText.text = playerPoints.ToString () + " Points";
			wasCalled = false;
		}
	}

	public void BuyNewIsland (Text text)
	{
		int itemPrice = int.Parse (text.text.Replace (" POINTS", ""));
		if (playerPoints >= itemPrice) {
			playerPoints -= itemPrice;
			Debug.Log ("Player points = " + playerPoints);
			ToggleButton.craftIsOn = false;
			crafting.SetActive (false);
			store.SetActive (false);
			purchased = true;
			wasCalled = true;
		} else {
			systemBox.SetActive (true);
			systemText.text = "You don't have enough points to purchase this item";
		}
	}

	public static bool NewIslandExists ()
	{
		return purchased;
	}
}
