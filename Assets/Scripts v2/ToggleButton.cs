using UnityEngine;
using System.Collections;

public class ToggleButton : MonoBehaviour
{
	public GameObject florepedia;
	public GameObject crafting;

	public static bool craftIsOn;
	static bool florepediaIsOn;

	void Start ()
	{		
	}

	public void ToggleCraft ()
	{
		craftIsOn = !craftIsOn;
		crafting.SetActive (craftIsOn);
		if (craftIsOn) {
			florepedia.SetActive (false);
			florepediaIsOn = false;
		}
	}

	public void ToggleFlorepedia ()
	{
		florepediaIsOn = !florepediaIsOn;
		florepedia.SetActive (florepediaIsOn);
		if (florepediaIsOn) {
			crafting.SetActive (false);
			craftIsOn = false;
		}
	}

}
