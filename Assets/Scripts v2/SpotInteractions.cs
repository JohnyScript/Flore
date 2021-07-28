using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SpotInteractions : MonoBehaviour, IPointerClickHandler
{
	public int cooldownTime;
	[Tooltip ("Este int controla o tempo que temos de esperar ate a caixa de scavange desaparecer.")]
	public int
		timeToTurnOff;
	public GameObject[] acquirableItems;

	Spot thisSpot;
	Inventory2 inventory;
	public int cooldown;
	TextMesh text;
	MeshRenderer meshRend;

	Color scavangeColor;
	Color cannotScavangeColor;

	void Start ()
	{
		thisSpot = GetComponentInParent<Spot> ();
		inventory = GameObject.Find ("Inventory").GetComponent<Inventory2> ();
		text = GetComponentInChildren<TextMesh> ();
		meshRend = GetComponent<MeshRenderer> ();
	}

	void Update ()
	{

		if (gameObject.activeSelf) {
			StartCoroutine (TurnMyselfOff (timeToTurnOff));
		}

	}


	public void OnPointerClick (PointerEventData eventData)
	{
		if (cooldown == 0) {
			text.text = "Scavange";
			meshRend.material.color = scavangeColor;
			ChooseItem ();
			cooldown = cooldownTime;
			InvokeRepeating ("ApplyCooldown", 0, 1);
			gameObject.SetActive (false);
		} 
	}
	
	public void ApplyCooldown ()
	{
		cooldown--;
		scavangeColor = new Color (0.110f, 0.592f, 0.827f, 1);
		cannotScavangeColor = new Color (0.5f, 0.5f, 0.5f, 1);
		meshRend = GetComponent<MeshRenderer> ();
		if (text == null)
			text = transform.GetChild (0).GetComponent<TextMesh> ();
		//Debug.Log ("cooldown = " + cooldown);
		if (cooldown == 0) {
			text.text = "Scavange";
			meshRend.material.color = scavangeColor;
			CancelInvoke ();
		} else {
			text.text = "Can't scavange now";
			meshRend.material.color = cannotScavangeColor;
		}
		
	}

	void ChooseItem ()
	{
		if (thisSpot.type == Spot.spotType.normal) {
			int[] itemIndexes = new int[5];
			itemIndexes [0] = 0;
			itemIndexes [1] = 1;
			itemIndexes [2] = 2;
			itemIndexes [3] = 3;
			itemIndexes [4] = 4;
			inventory.AddItem (acquirableItems [ItemIndexGot (itemIndexes)]);
		} else if (thisSpot.type == Spot.spotType.mud) {
			int[] itemIndexes = new int[3];
			itemIndexes [0] = 5;
			itemIndexes [1] = 3;
			itemIndexes [2] = 4;
			inventory.AddItem (acquirableItems [ItemIndexGot (itemIndexes)]);
		} else if (thisSpot.type == Spot.spotType.water) {
			int[] itemIndexes = new int[5];
			itemIndexes [0] = 6;
			itemIndexes [1] = 7;
			itemIndexes [2] = 3;
			itemIndexes [3] = 11;
			itemIndexes [4] = 12;
			inventory.AddItem (acquirableItems [ItemIndexGot (itemIndexes)]);
		} else if (thisSpot.type == Spot.spotType.ice) {
			int[] itemIndexes = new int[1];
			itemIndexes [0] = 8;
			inventory.AddItem (acquirableItems [ItemIndexGot (itemIndexes)]);
		} else if (thisSpot.type == Spot.spotType.poison) {
			int[] itemIndexes = new int[2];
			itemIndexes [0] = 9;
			itemIndexes [1] = 10;
			inventory.AddItem (acquirableItems [ItemIndexGot (itemIndexes)]);
		}
	}

	int ItemIndexGot (int[] indexes)
	{
		int indexToReturn = 0;
		int count = indexes.Length;
		indexToReturn = indexes [Random.Range (0, count)];
		return indexToReturn;
	}

	IEnumerator TurnMyselfOff (int timeToWait)
	{

		yield return new WaitForSeconds (timeToWait);
		gameObject.SetActive (false);

	}
}
