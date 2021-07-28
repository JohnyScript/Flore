using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TilesGenerator : MonoBehaviour
{
	private float posTileZ = -0.1f;
	
	public int numberOfTiles;
	public float tileHeight;
	public float beginningOfTiles; 
	public float endOfTiles; 


	private float distance;
	private float tileWidth;
	protected int tilesCreated;
	private float posTileX;

	//to instantiate quad prefab
	private GameObject instanciatedQuad;
	public GameObject spotPrefab;
	public float spotYplacementMin;
	public float spotYplacementMax;

	//to change material and color
	public Material altMat;
	public Material altMat2;
	private Renderer rend;

	//to give each spot a number, starting from -1 because the first index of an array is 0
	public int spotIndex = -1;

	void Start ()
	{ 



		posTileX = beginningOfTiles;
		if (beginningOfTiles <= 0 && endOfTiles >= 0) {
			GenerateTilesSpecial ();
		} else {
			GenerateTiles ();
		}
	}

	//In order to Spot.cs know how many tiles were created
	public int NumberTilesCreated ()
	{
		return tilesCreated;
	}

	private void GenerateTiles ()
	{
		distance = System.Math.Abs (beginningOfTiles - endOfTiles);
		tileWidth = distance / numberOfTiles;
		bool alternateMat = false;

		for (; posTileX < endOfTiles; posTileX += tileWidth) { 
			instanciatedQuad = (GameObject)Instantiate (spotPrefab);
			instanciatedQuad.transform.parent = transform;
			instanciatedQuad.transform.localScale = new Vector3 (tileWidth, tileHeight, 1);
			instanciatedQuad.transform.localPosition = new Vector3 (posTileX + tileWidth / 2, Random.Range (spotYplacementMin, spotYplacementMax), posTileZ);
			//change object name according to index
			instanciatedQuad.name = spotIndex.ToString ();
			//increase index
			spotIndex++;
			tilesCreated++;

			//alternate on material
			if (alternateMat) {
				rend = instanciatedQuad.GetComponent<Renderer> ();
				rend.sharedMaterial = altMat;
				alternateMat = false;
				
			} else if (!alternateMat) {
				rend = instanciatedQuad.GetComponent<Renderer> ();
				rend.sharedMaterial = altMat2;
				alternateMat = true;
			}
		}
		NumberTilesCreated ();
		/*Debug.Log ("Tile width = " + tileWidth);
		Debug.Log ("Total width = " + distance);*/
	}

	private void GenerateTilesSpecial ()
	{
		distance = System.Math.Abs (beginningOfTiles - endOfTiles);
		tileWidth = distance / numberOfTiles;
		bool alternateMat = false;
		
		for (; posTileX < endOfTiles - tileWidth/2; posTileX += tileWidth) { 
			instanciatedQuad = (GameObject)Instantiate (spotPrefab);
			instanciatedQuad.transform.parent = transform;
			instanciatedQuad.transform.localScale = new Vector3 (tileWidth, tileHeight, 1);
			instanciatedQuad.transform.localPosition = new Vector3 (posTileX + tileWidth / 2, Random.Range (spotYplacementMin, spotYplacementMax), posTileZ);
			//change object name according to index
			instanciatedQuad.name = spotIndex.ToString ();
			//increase index
			spotIndex++;
			tilesCreated++;

			//Debug.Log ("Tile " + tilesCreated + " = " + spotIndex);
			
			//alternate on material
			if (alternateMat) {
				rend = instanciatedQuad.GetComponent<Renderer> ();
				rend.sharedMaterial = altMat;
				alternateMat = false;

			} else if (!alternateMat) {
				rend = instanciatedQuad.GetComponent<Renderer> ();
				rend.sharedMaterial = altMat2;
				alternateMat = true;
			}
		}
		NumberTilesCreated ();
		/*Debug.Log ("Tile width = " + tileWidth);
		Debug.Log ("Distance = " + distance);*/
	}

}
