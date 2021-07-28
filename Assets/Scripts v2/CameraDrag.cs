using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CameraDrag : MonoBehaviour//, IDragHandler, IBeginDragHandler, IEndDragHandler
{

	//public
	public float perspectiveZoomSpeed = .5f;
	public float orthoZoomSpeed = .2f; //.5f é muito rápido
	public Transform objectToMove;

	//private
	Vector3 initialPos;
	Vector3 newPos;
	new Camera camera;

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
				//camera.orthographicSize = Mathf.Max (camera.orthographicSize, .1f);
			} else {
				camera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
				camera.fieldOfView = Mathf.Clamp (camera.fieldOfView, .1f, 179.9f);
			}
		}

	}


	void Start ()
	{
		camera = GetComponent<Camera> ();
	}





}
