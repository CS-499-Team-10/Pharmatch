using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DrugTile : MonoBehaviour {
	public TMP_Text nameLabelTMP;
	[SerializeField] public List<string> drugMatches;
	[SerializeField] public SceneController controller;

	public void Tapped() {
		Debug.Log("clicked " + nameLabelTMP);
		controller.CardRevealed(this);
	}
}
