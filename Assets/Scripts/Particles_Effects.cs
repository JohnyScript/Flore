using UnityEngine;
using System.Collections;

public class Particles_Effects : MonoBehaviour
{

	public ParticleSystem fog;


	void Awake ()
	{
		StartCoroutine (EmitParticles ());
	}
	
	// Update is called once per frame
	void Update ()
	{
	}


	IEnumerator EmitParticles ()
	{
		fog.enableEmission = true;
		yield return new WaitForSeconds (3);
		StartCoroutine (StopEmission ());
	}

	IEnumerator StopEmission ()
	{
		fog.enableEmission = false;
		yield return new WaitForSeconds (Random.Range (20, 30));
		StartCoroutine (EmitParticles ());
	}
}
