using UnityEngine;
using System.Collections;

public class SwipeMovement2 : MonoBehaviour
{
	
	public Transform terra3;
	private Vector3 initialPos;
	private float margin = 1f;
	
	private float scrolling; 
	
	public bool isOnTerrain1 = true;
	public bool isUnderground = false;
	
	//lerping in z axis
	private Vector3 posCamaraSwipeUp;
	private Vector3 posCamaraSwipeDown;
	private Vector3 currentPosition;

	Vector3 posCamaraSwipeUnder;

	private bool moveDown = false;
	private bool moveUp = false;
	private float interpolator = 0f;
	public float speed = 1f;

	//private Vector3 posCamaraSwipeUnder;

	//finger swipe
	//private float lastMousePos;
	
	void Start ()
	{
		initialPos = transform.position;
		
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			//lastMousePos = Input.mousePosition.y;
		}

		if (Input.GetMouseButtonUp (0) && !isUnderground && !moveUp && !moveDown) {
			//float delta = Input.mousePosition.y - lastMousePos;
			AdvanceOnTerrain ();
			//lastMousePos = Input.mousePosition.y;
		}

		if (Input.GetMouseButton (0) && !isUnderground && !moveDown && !moveUp) {
			//float delta = Input.mousePosition.y - lastMousePos;
			RetreatOnTerrain ();
			//lastMousePos = Input.mousePosition.y;
		}

		Debug.Log ("where am I = " + isOnTerrain1);
		
		scrolling = Input.GetAxis ("Mouse ScrollWheel");
		
		float posOverground = transform.position.z;
		float posUnderground = transform.position.y;
		
		if (posOverground == initialPos.z) {
			//isOnTerrain1 = true;
		}
		
		if (posUnderground == initialPos.y) {
			isUnderground = false;
		}
		
		if (scrolling > 0f && !isUnderground && !moveUp && !moveDown) {
			AdvanceOnTerrain ();
		}
		
		if (scrolling < 0f && !isUnderground && !moveDown && !moveUp) {
			RetreatOnTerrain ();
		}
		
		if (Input.GetMouseButtonDown (1) && isOnTerrain1) {
			isOnTerrain1 = false;
			GoUnderground ();
		}

		if (Input.GetMouseButtonDown (1) && !isOnTerrain1 && isUnderground) {
			isOnTerrain1 = true;
			GoOverground ();

		}
		
		if (moveDown)
			MoveDownAnim ();
		
		if (moveUp)
			MoveUpAnim ();
	}
	
	void MoveDownAnim ()
	{
		interpolator += Time.deltaTime * speed;	
		transform.position = Vector3.Lerp (currentPosition, posCamaraSwipeUp, interpolator);
		if (interpolator >= 1f) {
			moveDown = false;
		}
	}
	
	void MoveUpAnim ()
	{
		interpolator += Time.deltaTime * speed;
		transform.position = Vector3.Lerp (currentPosition, posCamaraSwipeDown, interpolator);
		if (interpolator >= 1f)
			moveUp = false;
	}
	
	void AdvanceOnTerrain ()
	{
		float goAheadOnTerrain = transform.position.z + margin;
		
		posCamaraSwipeDown = new Vector3 (transform.position.x,
		                                  transform.position.y + margin,
		                                  goAheadOnTerrain);
		currentPosition = transform.position;
		
		if (posCamaraSwipeDown.z != terra3.position.z) {
			//transform.position = posCamaraSwipeDown;
			isOnTerrain1 = false;
			moveUp = true;
			interpolator = 0f;
		}
	}
	
	void RetreatOnTerrain ()
	{
		float goBackOnTerrain = transform.position.z - margin;
		
		posCamaraSwipeUp = new Vector3 (transform.position.x,
		                                transform.position.y - margin,
		                                goBackOnTerrain);
		currentPosition = transform.position;
		
		if (posCamaraSwipeUp.z != initialPos.z - margin) {
			//this.transform.position = posCamaraSwipeUp;
			moveDown = true;
			interpolator = 0f;
		}
	}
	
	void GoUnderground ()
	{
		Debug.Log ("Went Underground");
		float goUnderground = -6.2f;
		
		posCamaraSwipeUnder = new Vector3 (transform.position.x,
		                                           goUnderground,
		                                           -7f);
		isUnderground = true;
		
		if (transform.position != posCamaraSwipeUnder) {
			transform.position = posCamaraSwipeUnder;
			//Debug.Log(posCamaraSwipeUnder);
		}
	}

	void GoOverground ()
	{
		
		Debug.Log ("Went Overground");
		//float goOverground = 5f;

		isUnderground = false;

		if (transform.position == posCamaraSwipeUnder) {
			transform.position = initialPos;
		}
	}
}
