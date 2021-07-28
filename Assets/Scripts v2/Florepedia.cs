using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class Florepedia : MonoBehaviour
{
	public Text titleLeft;
	public Image imageLeft;
	public Text titleRight;
	public Image imageRight;
	public List<Sprite> pages;

	Sprite empty;
	SortedList<string, Sprite> gatheredPages;
	int bookIndex = 0;


	public void UpdateFlorepedia (string plantName)
	{
		if (gatheredPages == null) {
			gatheredPages = new SortedList<string, Sprite> (new ReverseComparer ());
		}

		string newName = plantName.Replace ("Seed", "");
		Sprite newSprite = pages.Find (p => p.name == newName + "TexPage");
		

		if (!gatheredPages.TryGetValue (newName, out empty)) {

			if (titleLeft.text == "") {
				titleLeft.text = newName;
				imageLeft.sprite = newSprite;
				gatheredPages.Add (newName, newSprite);


			} else if (titleRight.text == "") {
				titleRight.text = newName;
				imageRight.sprite = newSprite;
				gatheredPages.Add (newName, newSprite);


			} else if (titleLeft.text != "" && titleRight.text != "") {
				gatheredPages.Add (newName, newSprite);
			}
		}

		for (int i = 0; i < gatheredPages.Count; i++) {
			Debug.Log ("Page " + i + " = " + gatheredPages.Keys [i]);
		}

		//Debug.Log ("plant name = " + newName + " sprite = " + newSprite);
	}

	public void ChangePageForward ()
	{
		//fazer um int com a posiçao em que estou no Florepedia
		//vou sempre mostrar as páginas que serão iguais à posição x2

		if (gatheredPages != null && gatheredPages.Keys.ElementAtOrDefault (bookIndex + 2) != null) {
			titleLeft.text = gatheredPages.Keys [bookIndex + 2];
			imageLeft.sprite = gatheredPages.Values [bookIndex + 2];
			titleRight.text = "";
			imageRight.sprite = null;
			bookIndex += 2;
			if (gatheredPages.Keys.ElementAtOrDefault (bookIndex + 1) != null) {
				titleRight.text = gatheredPages.Keys [bookIndex + 1];
				imageRight.sprite = gatheredPages.Values [bookIndex + 1];
			}
		}
	}

	public void ChangePageBackwards ()
	{		
		if (gatheredPages != null && gatheredPages.Keys.ElementAtOrDefault (bookIndex - 2) != null) {
			titleLeft.text = gatheredPages.Keys [bookIndex - 2];
			imageLeft.sprite = gatheredPages.Values [bookIndex - 2];
			titleRight.text = "";
			imageRight.sprite = null;
			bookIndex -= 2;
			if (gatheredPages.Keys.ElementAtOrDefault (bookIndex + 1) != null) {
				titleRight.text = gatheredPages.Keys [bookIndex + 1];
				imageRight.sprite = gatheredPages.Values [bookIndex + 1];
			}
		}
	}

	//criei esta class porque o SortedList adiciona os items em formato ascendente, os primeiros items ficam com os maiores indexes
	class ReverseComparer : IComparer<string>
	{
		public int Compare (string x, string y)
		{
			return -x.CompareTo (y);
		}
	}


}
