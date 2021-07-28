using UnityEngine;
using System.Collections;

public class Inventory_Animation : MonoBehaviour
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
			invAnim.Play ("inventory_open 1");
			invIsOpen = true;
		} else if (invIsOpen) {
			invAnim.Play ("inventory_close 1");
			invIsOpen = false;
		}


	}

	public void WiggleIcons (Animation objectToWiggle)
	{
		objectToWiggle.Play ("wiggle");
	}

	public void MakeSound (AudioSource objectSoundSource)
	{
		objectSoundSource.Play ();
	}
}
