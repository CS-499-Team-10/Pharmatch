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
		// update position of tile when it is moved/changes parent
		if(transform.localPosition != Vector3.zero) {
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, moveSpeed * Time.deltaTime);
		}
		// tiles are spawned at scale 0 and grow to 1
		if(transform.localScale != Vector3.one) {
			transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, scaleSpeed * Time.deltaTime);
		}
	}

	public void Tapped() {
		Debug.Log("clicked " + nameLabelTMP);
		controller.CardTapped(this);
		// Slide(Direction.Up);
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

    IEnumerator wait(RectTransform newCell)
    {
        controller.audios.Play();
        yield return new WaitForEndOfFrame();
        // transform.parent = newCell;
        while (transform.localScale.x > .1f && transform.localScale.y > .1f)
        {
            transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);
            yield return new WaitForEndOfFrame();
        }
        this.controller.tilesOnScreen.Remove(this); //remove this card from the list of active cards
        GameObject.Destroy(this.gameObject); //and destroy it
        yield return 0;
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
			}
			else if (newCell.GetComponentInChildren<DrugTile>() && CheckMatch(newCell.GetComponentInChildren<DrugTile>())) { // if there is a tile and they match
				transform.parent = newCell;
				foreach (Transform child in newCell) {
					controller.tilesOnScreen.Remove(newCell.GetComponentInChildren<DrugTile>()); //remove this tile from the list of active tiles
					GameObject.Destroy(child.gameObject); //and destroy it
 				}
                StartCoroutine(wait(newCell));
				return true;
			}
		}
		return false;
	}
}
