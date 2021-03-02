using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

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
			createCard();
		}
	}

	void createCard()
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
		// 	createCard();
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

	// Use this for initialization
	void Start() {
		Debug.Log("start");
		// drugs = new List<List<string>>();
		List<string> firstSet = new List<string>();
		List<string> secondSet = new List<string>();
		firstSet.Add("triangle");
		firstSet.Add("three");
		drugs.Add(firstSet);
		secondSet.Add("square");
		secondSet.Add("four");
		drugs.Add(secondSet);

		// place cards in a grid
		for (int i = 0; i < 16; i++) {
			createCard();
		}
	}

	// Knuth shuffle algorithm
	private int[] ShuffleArray(int[] numbers) {
		int[] newArray = numbers.Clone() as int[];
		for (int i = 0; i < newArray.Length; i++ ) {
			int tmp = newArray[i];
			int r = Random.Range(i, newArray.Length);
			newArray[i] = newArray[r];
			newArray[r] = tmp;
		}
		return newArray;
	}

	public void CardRevealed(DrugTile card) {
		if (_firstRevealed == null) {
			_firstRevealed = card;
		} else {
			_secondRevealed = card;
			CheckMatch();
		}
	}
	
	private void CheckMatch() {

		// increment score if the cards match		
		if (_firstRevealed.drugMatches.Contains(_secondRevealed.nameLabelTMP.text) && _firstRevealed.nameLabelTMP.text != _secondRevealed.nameLabelTMP.text) {
			_score++;
			Debug.Log(_score);
			scoreText.text = "Score: " + _score;

			Destroy(_firstRevealed.gameObject);
			Destroy(_secondRevealed.gameObject);

			createCard();
			createCard();
		}

		// otherwise turn them back over after .5s pause
		else {
			// yield return new WaitForSeconds(.5f);

			Debug.Log("no");
			// return 0;
		}
		
		_firstRevealed = null;
		_secondRevealed = null;
	}

	public void Restart() {
		SceneManager.LoadScene("Scene");
	}
}
