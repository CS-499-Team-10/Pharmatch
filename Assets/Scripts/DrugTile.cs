using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DrugTile : MonoBehaviour {
	public TMP_Text nameLabelTMP;
	[SerializeField] public List<string> drugMatches;
	[SerializeField] public SceneController controller;

	private const float moveSpeed = 2500;
	private const float scaleSpeed = 10;

	public enum Direction
	{
		Up,
		Down,
		Left,
		Right
	}

	private void Update() {
		if(transform.localPosition != Vector3.zero) {
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, moveSpeed * Time.deltaTime);
		}
		if(transform.localScale != Vector3.one) {
			transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, scaleSpeed * Time.deltaTime);
		}
	}

	public void Tapped() {
		// Debug.Log("clicked " + nameLabelTMP);
		// controller.CardRevealed(this);
		Slide(Direction.Up);
	}

	// returns true if this drug matches another drugtile
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

	// slides a tile in given direction
	// returns true if the tile slides into a match, false otherwise
	public bool Slide(Direction dir) {
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
			if(newCell.childCount < 1) { // if there isn't a tile in the next cell
				transform.parent = newCell;
				// GetComponent<RectTransform>().offsetMin = new Vector2(-0, 0);
				// GetComponent<RectTransform>().offsetMax = new Vector2(-0, 0);
			}
			else if (newCell.GetComponentInChildren<DrugTile>() && CheckMatch(newCell.GetComponentInChildren<DrugTile>())) { // if there is a tile and they match
				foreach (Transform child in newCell) {
					newCell.GetComponentInChildren<DrugTile>().controller.tilesOnScreen.Remove(newCell.GetComponentInChildren<DrugTile>()); //remove this tile from the list of active tiles
					GameObject.Destroy(child.gameObject); //and destroy it
 				}
				this.controller.tilesOnScreen.Remove(this); //remove this card from the list of active cards
				GameObject.Destroy(this.gameObject); //and destroy it
				return true;
			}
		}
		return false;
	}
}
