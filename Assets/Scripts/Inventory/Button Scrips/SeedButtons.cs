using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SeedButtons : MonoBehaviour
{

	public Button button;
	public bool hasSeed;


	void Start ()
	{
		if (button != null) {
			button = GetComponent<Button> ();
		
			//button.onClick.AddListener (ButtonName);
			button.onClick.AddListener (ButtonWasClicked);
		}
	}

	public string ButtonName ()
	{
		string buttonName = this.name;
		return buttonName;
	}

	public void ButtonWasClicked ()
	{
		hasSeed = true;
	}
}