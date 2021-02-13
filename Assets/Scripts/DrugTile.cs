using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrugTile : MonoBehaviour {
	// [SerializeField] private GameObject cardBack;

	[SerializeField] public HashSet<string> drugMatches;
	[SerializeField] private SceneController controller;

	private int _id;
	public int id {
		get {return _id;}
	}

	public void SetCard(int id, Sprite image) {
		_id = id;
		GetComponent<SpriteRenderer>().sprite = image;
	}

	public void OnMouseDown() {
		// if (cardBack.activeSelf && controller.canReveal) {
		// // if (cardBack.activeSelf){
		// 	cardBack.SetActive(false);
		// 	// controller.CardRevealed(this);
		// }
	}

	public void Unreveal() {
		// cardBack.SetActive(true);
	}
}
