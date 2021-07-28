using UnityEngine;
using System.Collections;

public class Scythe : MonoBehaviour
{

	public bool hasScythe = false;
	public bool isActive = false;

	public void ScytheWasClicked ()
	{

		if (!isActive) {
			hasScythe = true;
			isActive = true;
		} else if (isActive) {
			hasScythe = false;
			isActive = false;
			
		}

	}

}
