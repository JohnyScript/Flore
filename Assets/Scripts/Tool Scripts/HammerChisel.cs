using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HammerChisel : MonoBehaviour {

	public bool hammerAndChiselAreReady = false;
	public bool isActive = false;

	public Image buttonImage;

	public Sprite normalSprite;
	public Sprite pressedSprite;
	
	void Update(){

		if(isActive){
			buttonImage.sprite = pressedSprite;
		}else{
			buttonImage.sprite = normalSprite;
		}

	}

	public void HammerAndChiselWereClicked(){
		if(!isActive){
			hammerAndChiselAreReady = true;
			isActive = true;
		}else if (isActive){
			hammerAndChiselAreReady=false;
			isActive = false;
			
		}
	}
}
