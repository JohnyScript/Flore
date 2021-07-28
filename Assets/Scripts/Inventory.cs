using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour
{

	//public GameObject luxSeed;
	//public GameObject water;

	//To access Spot2 script
	//public Spot2.plantsEnum plants;

	//see if inventory is open
	private bool invIsOpen;
	private Animation invAnim;


	void Start ()
	{
		invAnim = GetComponent<Animation> ();

	}
	

	void Update ()
	{
	
	}

	public void OpenCloseInventory ()
	{
		if (!invIsOpen) {
			invAnim.Play ("inventory_open");
			invIsOpen = true;
		} else if (invIsOpen) {
			invAnim.Play ("inventory_close");
			invIsOpen = false;
		}


	}
}
