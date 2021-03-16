using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
//stream reader/IO stuff
using System.IO;
using System;
using System.Text;

using Random = UnityEngine.Random; //to distinguish between UnityEngine.Random and System.Random

public class SceneController : MonoBehaviour {
	private List<List<string>> drugs = new List<List<string>>();

	[SerializeField] private DrugTile drugPrefab;
	[SerializeField] private Transform[] cells;
	
	private DrugTile _firstRevealed;
	private DrugTile _secondRevealed;
	private int _score = 0;
	[SerializeField] private TMP_Text scoreText;

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			CreateCard();
		}
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			SlideUp();
			Invoke("CreateCard", 0.001f);
		}
		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			SlideDown();
			Invoke("CreateCard", 0.001f);
		}
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			SlideLeft();
			Invoke("CreateCard", 0.001f);
		}
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			SlideRight();
			Invoke("CreateCard", 0.001f);
		}
	}

	void CreateCard()
    {
		DrugTile newCard;
		Transform newCell = null;
		foreach (Transform cell in cells)
		{
			if(cell.childCount == 0)
			{
				newCell = cell;
			}
		}

		// int whichCell = Random.Range(0, cells.Length);
		// if(cells[whichCell].childCount != 0)
		// {
		// 	Debug.Log(whichCell);
		// 	CreateCard();
		// 	return;
		// }
		if(newCell != null)
		{
			List<string> randomSet1 = drugs[Random.Range(0, 2)];
			newCard = Instantiate(drugPrefab, newCell) as DrugTile;
			newCard.drugMatches = randomSet1;
			newCard.nameLabelTMP.text = randomSet1[Random.Range(0, 2)];
			newCard.controller = this;
		}
	}

	void SlideUp() {
		foreach (Transform cell in cells)
		{
			GridPosition pos = cell.GetComponentInParent<GridPosition>();
			if (!pos.up) {
				for (GridPosition nextMover = pos.down; nextMover != null; nextMover = nextMover.down)
				{
					DrugTile tileToMove = nextMover.gameObject.transform.GetComponentInChildren<DrugTile>();
					if (tileToMove != null) {
						tileToMove.Slide(DrugTile.Direction.Up);
					}
				}
			}
		}
	}

	void SlideDown() {
		foreach (Transform cell in cells)
		{
			GridPosition pos = cell.GetComponentInParent<GridPosition>();
			if (!pos.down) {
				for (GridPosition nextMover = pos.up; nextMover != null; nextMover = nextMover.up)
				{
					DrugTile tileToMove = nextMover.gameObject.transform.GetComponentInChildren<DrugTile>();
					if (tileToMove != null) {
						tileToMove.Slide(DrugTile.Direction.Down);
					}
				}
			}
		}
	}

	void SlideRight() {
		foreach (Transform cell in cells)
		{
			GridPosition pos = cell.GetComponentInParent<GridPosition>();
			if (!pos.right) {
				for (GridPosition nextMover = pos.left; nextMover != null; nextMover = nextMover.left)
				{
					DrugTile tileToMove = nextMover.gameObject.transform.GetComponentInChildren<DrugTile>();
					if (tileToMove != null) {
						tileToMove.Slide(DrugTile.Direction.Right);
					}
				}
			}
		}
	}

	void SlideLeft() {
		foreach (Transform cell in cells)
		{
			GridPosition pos = cell.GetComponentInParent<GridPosition>();
			if (!pos.left) {
				for (GridPosition nextMover = pos.right; nextMover != null; nextMover = nextMover.right)
				{
					DrugTile tileToMove = nextMover.gameObject.transform.GetComponentInChildren<DrugTile>();
					if (tileToMove != null) {
						tileToMove.Slide(DrugTile.Direction.Left);
					}
				}
			}
		}
	}

	// Use this for initialization
	void Start() {
        // //temporary addition to test LoadDrugs
        // TestLoadDrugs();

        drugs = LoadDrugs("Assets/DrugInfo/60DrugNames.csv");

		// place cards in a grid
		// for (int i = 0; i < cells.Length; i++) {
		// 	CreateCard();
		// }
	}

	public void CardRevealed(DrugTile card) {
		if (_firstRevealed == null) {
			_firstRevealed = card;
			_firstRevealed.Select();
		} else {
			_secondRevealed = card;
			_secondRevealed.Select();
			TryMatch();
		}
	}

	public void IncrementScore(int addedScore) {
		_score += addedScore;
		Debug.Log(_score);
		scoreText.text = "Score: " + _score;
	}
	
	private void TryMatch() {
		if (_firstRevealed.CheckMatch(_secondRevealed)) {
			IncrementScore(1);

			Destroy(_firstRevealed.gameObject);
			Destroy(_secondRevealed.gameObject);

			// Invoke("CreateCard", 0.001f);
			// Invoke("CreateCard", 0.001f);
		}
		else {
			// yield return new WaitForSeconds(.5f);
			Debug.Log("no");
			// return 0;
			_firstRevealed.UnSelect();
			_secondRevealed.UnSelect();
		}
		_firstRevealed = null;
		_secondRevealed = null;
	}

    List<List<string>> LoadDrugs(string fp) {
        List<List<string>> drugs = new List<List<string>>();
        string line = "";
        using (StreamReader sr = new StreamReader(@fp))
        {
            line = sr.ReadLine(); //ignore first line with column labels
            while (sr.Peek() != -1)
            {
                line = sr.ReadLine();
                string[] splitted = line.Split(',');

                //first index in splitted is generic
                //second is Trade
                //third, if available, is alternate Trade

                List<string> toAdd = new List<string>();
                if (splitted[0] != "")
                    toAdd.Add(splitted[0]); //generic name added
                if (splitted[1] != "")
                    toAdd.Add(splitted[1]); //trade name added
                if (splitted[2] != "")
                    toAdd.Add(splitted[2]); //alternate trade name added
                drugs.Add(toAdd); //add to list of lists
            }
        }
        return drugs;
    }

    public void TestLoadDrugs() {
        string fp = "Assets/DrugInfo/60DrugNames.csv";
        List<List<string>> testDrugs = LoadDrugs(fp);

        foreach(List<string> l in testDrugs) {
            string output = string.Join(",", l);
            Debug.Log(output);
        }
    }

	public void Restart() {
		SceneManager.LoadScene("Scene");
	}
}
