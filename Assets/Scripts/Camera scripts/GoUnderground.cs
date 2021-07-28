using UnityEngine;
using System.Collections;

public class GoUnderground : MonoBehaviour {

	public float yClampPos;
	public float yClampNeg;
	public float speed = 0.01f;
	
	public GameObject cam;

	
	private float lastPosition;
	
	void  Update ()
	{
		cam.transform.position = new Vector3 (cam.transform.position.x, 
		                                         Mathf.Clamp (cam.transform.position.y, yClampNeg, yClampPos), 
		                                         cam.transform.position.z);

		
		if (Input.GetMouseButtonDown (0)) {
			lastPosition = Input.mousePosition.y;
		}
		
		if (Input.GetMouseButton (0)) {
			float delta = Input.mousePosition.y - lastPosition;
			
			cam.transform.Translate (0, -delta * speed/2, 0);

			lastPosition = Input.mousePosition.y;
		}
	}
}
