using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ShowQuests : MonoBehaviour {

	public GameObject questsScreen;
	public GameObject questsButton;

	Animation anim;


	// Use this for initialization
	void Start () {

		anim = questsButton.GetComponent<Animation>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowQuestScreen (){

		StartCoroutine(PlayAnimationEnter());

	}

	public void CloseQuestScreen(){

		StartCoroutine(PlayAnimationClose());

	}

	IEnumerator PlayAnimationEnter(){

		anim.Play("OpenQuests");
		yield return new WaitForSeconds(anim.clip.length);
		questsScreen.SetActive(true);
		questsButton.SetActive(false);
		anim.Stop();

	} 

	IEnumerator PlayAnimationClose(){

		questsScreen.SetActive(false);
		questsButton.SetActive(true);
		anim.Play("CloseQuests");
		yield return new WaitForSeconds(anim.clip.length);
		anim.Stop();
		
	} 

}
