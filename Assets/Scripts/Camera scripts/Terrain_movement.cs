using UnityEngine;
using System.Collections;

public class Terrain_movement : MonoBehaviour {
	
	public float clampPos = 5f;
	public float clampNeg = -5f;
	public float speed = 0.01f;

	public GameObject terrain1;
	public GameObject terrain2;
	public GameObject terrain3;
	public GameObject sky;

	private float lastPosition;
	
	void  Update ()
	{
		terrain1.transform.position = new Vector3 (Mathf.Clamp (terrain1.transform.position.x, clampNeg/2, clampPos/2), 
		                                           terrain1.transform.position.y, 
		                                           terrain1.transform.position.z);
		terrain2.transform.position = new Vector3 (Mathf.Clamp (terrain2.transform.position.x, clampNeg/3, clampPos/3), 
		                                           terrain2.transform.position.y, 
		                                           terrain2.transform.position.z);
		terrain3.transform.position = new Vector3 (Mathf.Clamp (terrain3.transform.position.x, clampNeg/4, clampPos/4), 
		                                           terrain3.transform.position.y, 
		                                           terrain3.transform.position.z);
		sky.transform.position = new Vector3 (Mathf.Clamp (sky.transform.position.x, clampNeg/150, clampPos/150), 
		                                           sky.transform.position.y, 
		                                           sky.transform.position.z);

		if (Input.GetMouseButtonDown (0)) {
			lastPosition = Input.mousePosition.x;
		}
		
		if (Input.GetMouseButton (0)) {
			float delta = Input.mousePosition.x - lastPosition;

			terrain1.transform.Translate (delta * speed/2, 0, 0);
			terrain2.transform.Translate (delta * speed/3, 0, 0);
			terrain3.transform.Translate (delta * speed/4, 0, 0);
			sky.transform.Translate (delta * speed/150, 0, 0);
			lastPosition = Input.mousePosition.x;
		}
	}
}
