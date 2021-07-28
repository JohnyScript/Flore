using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObjectCounter : MonoBehaviour
{
	//Public Variables
	public Text textoQuantia;
	public int quantia = 1;
	public static GameObject itemBeingDragged;

	//Private Variables
	Vector3 startPosition;
	Button button;
	Crafting crafting;


	void Start ()
	{
		textoQuantia = GetComponentInChildren<Text> ();
		if (quantia > 1) {
			textoQuantia.text = ("x" + quantia);
		}/*else {

			textoQuantia.text = "";

     		}*/
		button = GetComponent<Button> ();

		InvokeRepeating ("LookOutForCrafting", 0, 1);
	}

	public void IncreaseQuantity ()
	{
		quantia ++;
		if (quantia > 1) {
			textoQuantia.text = ("x" + quantia);
		}
	}

	public void DecreaseQuantity ()
	{

		quantia --;
		if (quantia == 1) {
			textoQuantia.text = ("");
		} else {
			textoQuantia.text = ("x" + quantia);
		}

	}

	void LookOutForCrafting ()
	{
		//Debug.Log (crafting);
		if (ToggleButton.craftIsOn) {
			crafting = GameObject.Find ("Crafting").GetComponent<Crafting> ();
			button.onClick.AddListener (crafting.InstantiateItemCopies);
			CancelInvoke ("LookOutForCrafting");
		}
	}
	

	/*public void OnBeginDrag (PointerEventData eventData)
	{
		itemBeingDragged = gameObject;
		startPosition = transform.position;
	}

	public void OnDrag (PointerEventData eventData)
	{

		transform.position = GetComponentInParent<Canvas> ().worldCamera.ScreenToWorldPoint (eventData.position);

	}


	public void OnEndDrag (PointerEventData eventData)
	{
		itemBeingDragged = null;
		transform.position = startPosition;
	}*/
}
