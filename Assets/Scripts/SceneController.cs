using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneController : MonoBehaviour {
	private List<List<string>> drugs = new List<List<string>>();
	// private List<string>[] drugs;

	public const int gridRows = 4;
	public const int gridCols = 4;
	public const float offsetX = 3.00f;
	public const float offsetY = 3.50f;

	[SerializeField] private DrugTile originalCard;
	// [SerializeField] private Sprite[] images;
	// [SerializeField] private TextMesh scoreLabel;
	
	private DrugTile _firstRevealed;
	private DrugTile _secondRevealed;
	private int _score = 0;

	// public bool canReveal {
	// 	// get {return _secondRevealed == null;}
	// }

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
				DrugTile card;	

				// MyElement = Elements[Random.Range(0,Elements.Length)];
				List<string> randomSet = drugs[Random.Range(0, 2)];	

				// use the original for the first grid space
				if (i == 0 && j == 0) {
					card = originalCard;
				} else {
					card = Instantiate(originalCard) as DrugTile;
				}
				card.drugMatches = randomSet;
				card.nameLabelTMP.text = randomSet[Random.Range(0, 2)];

				// next card in the list for each grid space
				int index = j * gridCols + i;
				int id = numbers[index];
				// card.SetCard(id, images[id]);

				float posX = (offsetX * i) + startPos.x;
				float posY = -(offsetY * j) + startPos.y;
				card.transform.position = new Vector3(posX, posY, startPos.z);
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
			StartCoroutine(CheckMatch());
		}
	}
	
	private IEnumerator CheckMatch() {

		// increment score if the cards match
		if (_firstRevealed.drugMatches.Contains(_secondRevealed.nameLabelTMP.text)) {
			_score++;
			Debug.Log(_score);
			// scoreLabel.text = "Score: " + _score;
		}

		// otherwise turn them back over after .5s pause
		else {
			yield return new WaitForSeconds(.5f);

			_firstRevealed.Unreveal();
			_secondRevealed.Unreveal();

			Debug.Log("no");
		}
		
		_firstRevealed = null;
		_secondRevealed = null;
	}

	public void Restart() {
		SceneManager.LoadScene("Scene");
	}
}
