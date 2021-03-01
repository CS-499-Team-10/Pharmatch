using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class SceneController : MonoBehaviour {
	private List<List<string>> drugs = new List<List<string>>();
	// private List<string>[] drugs;

	public const int gridRows = 4;
	public const int gridCols = 4;

	[SerializeField] private DrugTile drugPrefab;
	[SerializeField] private Transform[] cells;
	// [SerializeField] private Sprite[] images;
	
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

	// public bool canReveal {
	// 	// get {return _secondRevealed == null;}
	// }
	void createCard()
    {
		DrugTile newCard;

		int whichCell = Random.Range(0, cells.Length);
		if(cells[whichCell].childCount != 0)
		{
			Debug.Log(whichCell);
			createCard();
			return;
		}
		List<string> randomSet1 = drugs[Random.Range(0, 2)];
		newCard = Instantiate(drugPrefab, cells[whichCell]) as DrugTile;
		newCard.drugMatches = randomSet1;
		newCard.nameLabelTMP.text = randomSet1[Random.Range(0, 2)];
		newCard.controller = this;

		// return newCard;
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
		Vector3 startPos = new Vector3(-4, 4, -10);

		// create shuffled list of cards
		int[] numbers = {0, 0, 1, 1, 2, 2, 3, 3, 0, 0, 1, 1, 2, 2, 3, 3};
		numbers = ShuffleArray(numbers);

		// place cards in a grid
		for (int i = 0; i < gridCols; i++) {
			for (int j = 0; j < gridRows; j++) {
				// next card in the list for each grid space
				// int index = j * gridCols + i;
				// int id = numbers[index];
				// card.SetCard(id, images[id]);

				// float posX = (offsetX * i) + startPos.x;
				// float posY = -(offsetY * j) + startPos.y;
				//card.transform.position = new Vector3(posX, posY, startPos.z);

				createCard();
			}
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
		Vector3 card1pos, card2pos;
		
		if (_firstRevealed.drugMatches.Contains(_secondRevealed.nameLabelTMP.text) && _firstRevealed.nameLabelTMP.text != _secondRevealed.nameLabelTMP.text) {
			_score++;
			Debug.Log(_score);
			scoreText.text = "Score: " + _score;

			card1pos = _firstRevealed.transform.position;
			card2pos = _secondRevealed.transform.position;

			Destroy(_firstRevealed.gameObject);
			Destroy(_secondRevealed.gameObject);

			createCard();
			createCard();
		}

		// otherwise turn them back over after .5s pause
		else {
			// yield return new WaitForSeconds(.5f);

			_firstRevealed.Unreveal();
			_secondRevealed.Unreveal();

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
