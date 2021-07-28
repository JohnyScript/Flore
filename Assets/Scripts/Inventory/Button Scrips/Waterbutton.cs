using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Waterbutton : MonoBehaviour
{

	public bool hasWater;
	public GameObject childEffect;
	Animator effectAnim;

	void Start ()
	{
		effectAnim = childEffect.GetComponent<Animator> ();
	}

	public void WaterWasClicked ()
	{
		hasWater = !hasWater;
		if (hasWater) {
			childEffect.SetActive (true);
		} else if (!hasWater) {
			effectAnim.Stop ();
			childEffect.SetActive (false);
		}
	}

}