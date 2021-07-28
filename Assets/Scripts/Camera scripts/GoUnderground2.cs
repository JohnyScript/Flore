using UnityEngine;
using System.Collections;

public class GoUnderground2 : MonoBehaviour {

	public GameObject cam;

	public Vector2 firstPressPos;
	public Vector2 secondPressPos;
	public Vector2 currentSwipe;
	

	public bool isUnderground = false;
	public bool isOverground = true;

	private Vector3 posCamaraSwipeUnder;

	Vector3 cameraPosition;

	//lerp

	private bool moveDown = false;
	private bool moveUp = false;
	private float interpolator = 0f;
	public float speed = 1f;

	private Vector3 posCamaraSwipeUp;
	private Vector3 posCamaraSwipeDown;
	private Vector3 currentPosition;
	
	// Use this for initialization
	void Start ()
	{


	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			//save began touch 2d point
			firstPressPos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		}
		if (Input.GetMouseButtonUp (0)) {
			//save ended touch 2d point
			secondPressPos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			
			//create vector from the two points
			currentSwipe = new Vector2 (secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
			
			//normalize the 2d vector
			currentSwipe.Normalize ();
			
			//swipe upwards
			if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f && isOverground) {
				//Debug.Log("up swipe");
				AdvanceOnTerrain();
				isOverground=false;
				isUnderground=true;
			}
			//swipe down
			if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f && isUnderground) {
				//Debug.Log("down swipe");
				RetreatOnTerrain();
				isOverground=true;
				isUnderground=false;
			}

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
		float goAheadOnTerrain = -6.2f;
		
		posCamaraSwipeDown = new Vector3 (transform.position.x,
		                                  goAheadOnTerrain,
		                                  transform.position.z);
		currentPosition = cam.transform.position;
		
		if (posCamaraSwipeDown.y != -6f) {
			//transform.position = posCamaraSwipeDown;
			moveUp = true;
			interpolator = 0f;
		}
	}
	
	void RetreatOnTerrain ()
	{
		float goBackOnTerrain = 4.3f;
		
		posCamaraSwipeUp = new Vector3 (transform.position.x,
		                                goBackOnTerrain,
		                                transform.position.z);
		currentPosition = cam.transform.position;
		
		if (posCamaraSwipeUp.y != 4f) {
			//this.transform.position = posCamaraSwipeUp;
			moveDown = true;
			interpolator = 0f;
		}
	}

}
