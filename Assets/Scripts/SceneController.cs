using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneController : MonoBehaviour {
	private List<HashSet<string>> drugs = new List<HashSet<string>>();

	public const int gridRows = 4;
	public const int gridCols = 4;
	public const float offsetX = 200f;
	public const float offsetY = 250f;

	// public IDictionary<int, string> numberNames = new Dictionary<int, string>();
// List<Part> parts = new List<Part>();
	[SerializeField] private DrugTile originalCard;
	// [SerializeField] private Sprite[] images;
	// [SerializeField] private TextMesh scoreLabel;
	
	// private MemoryCard _firstRevealed;
	// private MemoryCard _secondRevealed;
	private int _score = 0;

	// public bool canReveal {
	// 	// get {return _secondRevealed == null;}
	// }

	// Use this for initialization
	void Start() {
		// drugs = new List<HashSet<string>>();
		HashSet<string> firstSet = new HashSet<string>();
		HashSet<string> secondSet = new HashSet<string>();
		firstSet.Add("triangle");
		firstSet.Add("three");
		drugs.Add(firstSet);
		secondSet.Add("square");
		secondSet.Add("four");
		drugs.Add(secondSet);
		// Vector3 startPos = originalCard.transform.position;
		Vector3 startPos = new Vector3(0, 0, -10);

		// create shuffled list of cards
		int[] numbers = {0, 0, 1, 1, 2, 2, 3, 3, 0, 0, 1, 1, 2, 2, 3, 3};
		numbers = ShuffleArray(numbers);

		// place cards in a grid
		for (int i = 0; i < gridCols; i++) {
			for (int j = 0; j < gridRows; j++) {
				DrugTile card;

				// use the original for the first grid space
				if (i == 0 && j == 0) {
					card = originalCard;
				} else {
					card = Instantiate(originalCard) as DrugTile;
				}

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

	// public void CardRevealed(MemoryCard card) {
	// 	if (_firstRevealed == null) {
	// 		_firstRevealed = card;
	// 	} else {
	// 		_secondRevealed = card;
	// 		StartCoroutine(CheckMatch());
	// 	}
	// }
	
	// private IEnumerator CheckMatch() {

	// 	// increment score if the cards match
	// 	if (_firstRevealed.id == _secondRevealed.id) {
	// 		_score++;
	// 		scoreLabel.text = "Score: " + _score;
	// 	}

	// 	// otherwise turn them back over after .5s pause
	// 	else {
	// 		yield return new WaitForSeconds(.5f);

	// 		_firstRevealed.Unreveal();
	// 		_secondRevealed.Unreveal();
	// 	}
		
	// 	_firstRevealed = null;
	// 	_secondRevealed = null;
	// }

	public void Restart() {
		SceneManager.LoadScene("Scene");
	}
}
