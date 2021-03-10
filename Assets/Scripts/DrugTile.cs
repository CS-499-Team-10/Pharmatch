using UnityEngine;
using UnityEngine.UI;
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

	public bool CheckMatch(DrugTile other) {
		if (drugMatches.Contains(other.nameLabelTMP.text) && nameLabelTMP.text != other.nameLabelTMP.text) {
			return true;
		}
		else return false;
	}

	public void Select() {
		GetComponent<Image>().color = new Color(170F/255F, 170F/255F, 100F/255F);
	}

	public void UnSelect() {
		GetComponent<Image>().color = new Color(186F/255F, 195F/255F, 121F/255F);
	}
}
