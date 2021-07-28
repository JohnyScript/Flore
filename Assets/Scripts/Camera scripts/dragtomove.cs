using UnityEngine;
using System.Collections;

public class dragtomove : MonoBehaviour
{

	public GameObject cam;

	public bool wantMoveOnTerrains = false;

	Vector2 firstPressPos;
	Vector2 secondPressPos;
	Vector2 currentSwipe;
	
	public bool isUnderground = false;
	public bool isOverground = true;
	public bool isOnTerrain1 = true;
	public Transform terra3;
	private Vector3 initialPos;
	private float margin = 1f;

	private Vector3 posCamaraSwipeUp;
	private Vector3 posCamaraSwipeDown;
	private Vector3 currentPosition;
	private bool moveDown = false;
	private bool moveUp = false;
	private float interpolator = 0f;
	public float speed = 1f;
	
	private Vector3 posCamaraSwipeUnder;


	// Use this for initialization
	void Start ()
	{
		initialPos = cam.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(wantMoveOnTerrains){
			if (Input.GetMouseButtonDown (0) && isOverground) {
				//save began touch 2d point
				firstPressPos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			}
			if (Input.GetMouseButtonUp (0) && isOverground) {
				//save ended touch 2d point
				secondPressPos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
				
				//create vector from the two points
				currentSwipe = new Vector2 (secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
				
				//normalize the 2d vector
				currentSwipe.Normalize ();
				
				//swipe upwards
				if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
					//Debug.Log("up swipe");
					RetreatOnTerrain ();
				}
				//swipe down
				if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
					//Debug.Log("down swipe");
					AdvanceOnTerrain ();
				}
				//swipe left
				/*if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
				{
					Debug.Log("left swipe");
				}
				//swipe right
				if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
				{
					Debug.Log("right swipe");
				}*/
				}
		}
		
		if (Input.GetMouseButtonUp (1) && !isUnderground) {
			
			GoUnderground ();
			
		} else if (Input.GetMouseButtonUp (1) && isUnderground) {
			
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
		cam.transform.position = Vector3.Lerp (currentPosition, posCamaraSwipeUp, interpolator);
		if (interpolator >= 1f) {
			moveDown = false;
		}
	}
	
	void MoveUpAnim ()
	{
		interpolator += Time.deltaTime * speed;
		cam.transform.position = Vector3.Lerp (currentPosition, posCamaraSwipeDown, interpolator);
		if (interpolator >= 1f)
			moveUp = false;
	}
	
	void AdvanceOnTerrain ()
	{
		float goAheadOnTerrain = cam.transform.position.z + margin;
		
		posCamaraSwipeDown = new Vector3 (cam.transform.position.x,
		                                  cam.transform.position.y + margin,
		                                  goAheadOnTerrain);
		currentPosition = cam.transform.position;
		
		if (posCamaraSwipeDown.z != terra3.position.z) {
			//transform.position = posCamaraSwipeDown;
			isOnTerrain1 = false;
			moveUp = true;
			interpolator = 0f;
		}
	}
	
	void RetreatOnTerrain ()
	{
		float goBackOnTerrain = cam.transform.position.z - margin;
		
		posCamaraSwipeUp = new Vector3 (cam.transform.position.x,
		                                cam.transform.position.y - margin,
		                                goBackOnTerrain);
		currentPosition = cam.transform.position;
		
		if (posCamaraSwipeUp.z != initialPos.z - margin) {
			//this.transform.position = posCamaraSwipeUp;
			moveDown = true;
			interpolator = 0f;
		}
	}

	void GoUnderground ()
	{
		//Debug.Log ("Went Underground");
		float goUnderground = -6.2f;
		
		posCamaraSwipeUnder = new Vector3 (transform.position.x,
		                                           goUnderground,
		                                           -7f);
		isUnderground = true;
		isOverground = false;
		
		if (cam.transform.position != posCamaraSwipeUnder) {
			cam.transform.position = posCamaraSwipeUnder;
			//Debug.Log(posCamaraSwipeUnder);
		}
	}

	void GoOverground ()
	{
		
		//Debug.Log ("Went Overground");
		//float goOverground = 5f;
		
		isUnderground = false;
		isOverground = true;
		
		if (cam.transform.position == posCamaraSwipeUnder) {
			cam.transform.position = initialPos;
		}
	}


}
