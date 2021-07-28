using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragJoao : MonoBehaviour, IBeginDragHandler, IDragHandler
{
	
	//public
	public float perspectiveZoomSpeed = .5f;
	public float orthoZoomSpeed = .2f; //.5f é muito rápido

	public Transform objectToMove;
	public Transform limitLeft;
	public Transform limitRight;
	public Transform limitTop;
	public Transform limitBottom;

	//private
	Vector3 initialPos;
	Vector3 newPos;
	new Camera camera;
	private Vector3 ResetCamera;
	private Vector3 Origin;
	private Vector3 Diference; 

	void Start ()
	{
		camera = objectToMove.GetComponent<Camera> ();
		Origin = objectToMove.transform.position;
	}

	Vector3 MousePos ()
	{
		return Camera.main.ScreenToWorldPoint (Input.mousePosition);
	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		Origin = MousePos ();
	}
	
	public void OnDrag (PointerEventData eventData)
	{
		Diference = MousePos () - objectToMove.position;
		objectToMove.position = CameraClamp();
	}

	void Update ()
	{
		if (Input.touchCount == 2) {
			Touch touchZero = Input.GetTouch (0);
			Touch touchOne = Input.GetTouch (1);
			
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
			
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
			
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
			
			if (camera.orthographic) {
				camera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
				camera.orthographicSize = Mathf.Clamp (camera.orthographicSize, 5, 10);
				objectToMove.position = CameraClamp();
				//camera.orthographicSize = Mathf.Max (camera.orthographicSize, .1f);
			} /*else {
				camera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
				camera.fieldOfView = Mathf.Clamp (camera.fieldOfView, .1f, 179.9f);

			}*/
		}
		#region Scroll Wheel Zoom
		// Zoom Out
		if (Input.GetAxis("Mouse ScrollWheel") < 0) 
		{
			camera.orthographicSize++;
			camera.orthographicSize = Mathf.Clamp (camera.orthographicSize, 5, 10);
			objectToMove.position = CameraClamp();
		}
		//Zoom In
		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			camera.orthographicSize--;
			camera.orthographicSize = Mathf.Clamp (camera.orthographicSize, 5, 10);
		}
		#endregion

	}

	#region Camera Clamp Values
	Vector3 CameraClamp(){
		
		Vector3 clampValues = new Vector3 (Mathf.Clamp (Origin.x - Diference.x, 
		                                                limitLeft.position.x + (Camera.main.orthographicSize * 1.8f), 
		                                                limitRight.position.x - (Camera.main.orthographicSize * 1.8f)),
		                                   Mathf.Clamp (Origin.y - Diference.y, 
		             limitBottom.position.y + (Camera.main.orthographicSize), 
		             limitTop.position.y - (Camera.main.orthographicSize)), 
		                                   objectToMove.position.z);
		
		return clampValues;
	}
	#endregion

}
