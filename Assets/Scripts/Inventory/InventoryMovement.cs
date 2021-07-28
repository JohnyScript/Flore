using UnityEngine;
using System.Collections;


public class InventoryMovement : MonoBehaviour {

	//public variables

	public float clampPos = 5f;
	public float clampNeg = -5f;
	public float speed = 0.01f;
	public float lastObjectx;
	public float previousObjectX;
	public GameObject camParalax;
	public GameObject camSwipeUpDown;
	public GameObject inventory;

	//private variables
	private Vector3 lastPosition;
	private bool isCamMovementOff;
	public RectTransform inventoryRect;

	InventoryScript inventoryScript;


	// Use this for initialization
	void Start () {
		inventoryRect = gameObject.GetComponent<RectTransform>();
		inventoryScript = GetComponent<InventoryScript>();
	}
	
	// Update is called once per frame
	void Update () {



		if(isCamMovementOff){
			if(inventoryRect.sizeDelta.x >= 840){
				//lastObjectx = -inventoryScript.instantiatedObject.transform.position.x;
				inventory.transform.position = new Vector3 (Mathf.Clamp (inventory.transform.position.x, lastObjectx, clampPos), 
			                                            inventory.transform.position.y, 
			                                            inventory.transform.position.z);
			
				if (Input.GetMouseButtonDown (0)) {
					lastPosition = Input.mousePosition;
				}
			
				if (Input.GetMouseButton (0)) {
					Vector3 delta = Input.mousePosition - lastPosition;

					inventory.transform.Translate (delta.x * speed/2, 0, 0);
					lastPosition = Input.mousePosition;
				}
			}
	
		}
	}

	public void CamCantMove(){

		camParalax.SetActive(false);
		camSwipeUpDown.SetActive(false);
		isCamMovementOff = true;



	}

	public void CamCanMove(){
		
		camParalax.SetActive(true);
		camSwipeUpDown.SetActive(true);
		isCamMovementOff = false;
		
		
	}

	public void DinamicClamping(){

		previousObjectX = previousObjectX-(inventoryScript.objectTransform.sizeDelta.x+inventoryScript.offset);
		lastObjectx = Mathf.Round( previousObjectX/100);


	}

}
