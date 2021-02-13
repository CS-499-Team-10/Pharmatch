using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrugTile : MonoBehaviour {
	// [SerializeField] private GameObject cardBack;
	[SerializeField] public TextMesh nameLabel;

	[SerializeField] public List<string> drugMatches;
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
		Debug.Log("clicked " + nameLabel);
		controller.CardRevealed(this);
	}

	public void Unreveal() {
		// cardBack.SetActive(true);
	}
}
