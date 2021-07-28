using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public GameObject loadingPanel;

	Canvas info;
	bool isLoadingSavedGame = false;

	void Start ()
	{
		DontDestroyOnLoad (gameObject);
	}

	public void StartNewGame ()
	{
		StartCoroutine (Loading ());
	}
	
	public void StartLoadedGame ()
	{
		StartCoroutine (Loading ());
		isLoadingSavedGame = true;
	}

	IEnumerator Loading ()
	{
		loadingPanel.SetActive (true);
		/*Animation loadingAnim = loadingPanel.GetComponentInChildren<Animation> ();
		loadingAnim.Play ();*/
		yield return new WaitForSeconds (1);
		AsyncOperation myAsync = Application.LoadLevelAsync ("New_Flore");

		while (!myAsync.isDone) {
			yield return null;
		}

		if (myAsync.isDone && isLoadingSavedGame) {
			//Debug.Log ("finished loading. Now calling SceneWasLoaded");
			StartCoroutine (SceneWasLoaded ());
		} else if (myAsync.isDone && !isLoadingSavedGame) {
			Destroy (gameObject);
		}
	}

	IEnumerator SceneWasLoaded ()
	{
		//Debug.Log ("SceneWasLoaded was called.");
		Button pressLoad = GameObject.Find ("Load").GetComponent<Button> ();
		pressLoad.onClick.Invoke ();
		isLoadingSavedGame = false;

		yield return new WaitForSeconds (1);
		Destroy (gameObject);
	}
}
