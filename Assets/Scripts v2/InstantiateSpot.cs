using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InstantiateSpot : MonoBehaviour, IPointerClickHandler
{
	public GameObject spotPrefab;
	public bool occupied;
	public AnimationClip appear;
	public bool instantiateIslandAtStart;

	//Animator thisAnim;
	GameObject instantiateSpot;

	void Start ()
	{
		InstantiateIsland (instantiateIslandAtStart);

	}

	public void OnPointerClick (PointerEventData eventData)
	{
		if (!occupied && EnergyPoints.NewIslandExists ()) {
			InstantiateIsland (true);
			EnergyPoints.purchased = false;
		}
	}


	IEnumerator IsleAnimation ()
	{
		//thisAnim.Play ("Appear");
		yield return new WaitForSeconds (appear.length);
		instantiateSpot.transform.localScale = new Vector3 (1, 0.5f, 1);

	}

	public void InstantiateIsland (bool doIt)
	{
		if (doIt) {
			instantiateSpot = (GameObject)Instantiate (spotPrefab);
			instantiateSpot.transform.SetParent (transform);
			instantiateSpot.transform.localPosition = spotPrefab.transform.position;
			instantiateSpot.name = instantiateSpot.name.Replace ("Prefab(Clone)", " " + transform.name);
			occupied = true;
			//thisAnim = transform.GetComponentInChildren<Animator> ();
			//StartCoroutine ("IsleAnimation");
			instantiateSpot.transform.localScale = spotPrefab.transform.lossyScale;
		}
	}
}
