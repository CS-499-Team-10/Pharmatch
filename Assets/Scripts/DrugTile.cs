using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DrugTile : MonoBehaviour {
	public TMP_Text nameLabelTMP;
	[SerializeField] public List<string> drugMatches;
	[SerializeField] public SceneController controller;

	public enum Direction
	{
		Up,
		Down,
		Left,
		Right
	}

	public void Tapped() {
		// Debug.Log("clicked " + nameLabelTMP);
		// controller.CardRevealed(this);
		Slide(Direction.Up);
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

	public void Slide(Direction dir) {
		GridPosition myPos = transform.parent.gameObject.GetComponent<GridPosition>();
		GridPosition newPos = null;
		switch (dir)
		{
		case Direction.Up:
			newPos = myPos.up;
			break;
		case Direction.Down:
			newPos = myPos.down;
			break;
		case Direction.Left:
			newPos = myPos.left;
			break;
		case Direction.Right:
			newPos = myPos.right;
			break;
		}
		if(newPos) {
			RectTransform newCell = newPos.GetComponent<RectTransform>();
			transform.parent = newCell;
			GetComponent<RectTransform>().offsetMin = new Vector2(-0, 0);
			GetComponent<RectTransform>().offsetMax = new Vector2(-0, 0);
		}
	}
}
