using UnityEngine;
using System.Collections;

public class DisplayWarningMessage : MonoBehaviour {

	public GameObject warningText;
	bool flag = false;

	void OnMouseUpAsButton() {
		DisplayText();
	}

	public void DisplayText() {
		if(!flag) {
			//Debug.Log("is active");
			warningText.SetActive(true);
			flag = true;
		}
		else {
			warningText.SetActive(false);
			flag = false;
			//Debug.Log("is inactive");
		}
	}
}
