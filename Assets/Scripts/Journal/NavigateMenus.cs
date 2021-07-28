using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class NavigateMenus : MonoBehaviour, IDragHandler
{
	public GameObject objectToMakeActive;
	public GameObject objectToHide;
	private AudioSource soundSource;

	void Start ()
	{
	
		soundSource = GetComponent<AudioSource> ();
	}

	public void OnMouseUpAsButton ()
	{
		PlaySound ();
		objectToHide.SetActive (false);
		objectToMakeActive.SetActive (true);
	}

	void PlaySound ()
	{
		soundSource.Play ();
	}

	public void OnDrag (PointerEventData eventData)
	{

	}

}
