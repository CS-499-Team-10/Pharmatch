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
	// private List<string>[] drugs;

	public const int gridRows = 4;
	public const int gridCols = 4;
	public const float offsetX = 2.70f;
	public const float offsetY = 3.50f;

	[SerializeField] private DrugTile originalCard;
	// [SerializeField] private Sprite[] images;
	// [SerializeField] private TextMesh scoreLabel;
	
	private DrugTile _firstRevealed;
	private DrugTile _secondRevealed;
	private int _score = 0;
	[SerializeField] private TMP_Text scoreText;

	// public bool canReveal {
	// 	// get {return _secondRevealed == null;}
	// }

	// Use this for initialization
	void Start() {

        //temporary addition to test LoadDrugs
        TestLoadDrugs();
		Debug.Log("start");
        // drugs = new List<List<string>>();
        string fp = "Assets/DrugInfo/60DrugNames.csv";
        drugs = LoadDrugs(fp);
        int drugsSize = drugs.Count;
        /*
		List<string> firstSet = new List<string>();
		List<string> secondSet = new List<string>();
		firstSet.Add("triangle");
		firstSet.Add("three");
		drugs.Add(firstSet);
		secondSet.Add("square");
		secondSet.Add("four");
		drugs.Add(secondSet);
        */
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
				card = Instantiate(originalCard) as DrugTile;
				card.drugMatches = randomSet;
				card.nameLabelTMP.text = randomSet[Random.Range(0, 2)];
				card.controller = this;

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
		Vector3 card1pos, card2pos;

        //int drugsSize = GetDrugsSize();
		
		if (_firstRevealed.drugMatches.Contains(_secondRevealed.nameLabelTMP.text)) {
			_score++;
			Debug.Log(_score);
			scoreText.text = "Score: " + _score;

			card1pos = _firstRevealed.transform.position;
			card2pos = _secondRevealed.transform.position;

			Destroy(_firstRevealed.gameObject);
			Destroy(_secondRevealed.gameObject);

			List<string> randomSet1 = drugs[Random.Range(0, 2)];
			List<string> randomSet2 = drugs[Random.Range(0, 2)];
			DrugTile firstReplace, secondReplace;
			firstReplace = Instantiate(originalCard) as DrugTile;
			secondReplace = Instantiate(originalCard) as DrugTile;
			firstReplace.drugMatches = randomSet1;
			firstReplace.nameLabelTMP.text = randomSet1[Random.Range(0, 2)];
			secondReplace.drugMatches = randomSet2;
			secondReplace.nameLabelTMP.text = randomSet2[Random.Range(0, 2)];
			firstReplace.transform.position = card1pos;
			secondReplace.transform.position = card2pos;
			firstReplace.controller = this;
			secondReplace.controller = this;
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
    /*
    private Dictionary<string, List<string>> LoadDrugs(string fp, int keyVal) {
        //Read drugs in from CSV, key is brand name of the drugs
        Dictionary<string, List<string>> drugs = new Dictionary<string, List<string>>();
        string line = "";
        using (StreamReader sr = new StreamReader(@filePath)) { 
            line = sr.ReadLine();
            while (sr.Peek() != -1) {
                line = sr.ReadLine();
                string[] splitted = line.Split(',');
                //codeine is the only drug without a brand name, and contains parentheses, so this can be refined later, but to get it to work, added this to check
                if (!splitted[0].Contains("(")) {
                    string generic = splitted[0];
                    string brand = splitted[1];

                    //if the brand name is the key
                    if (keyVal == 0) {
                        //check if key is already in dictionary
                        if (drugs.ContainsKey(brand))
                            drugs[brand].Add(generic);   
                        //key is not in the dictionary, create new value list
                        else {
                            List<string> valList = new List<string>();
                            valList.Add(generic);
                            drugs.Add(brand, valList);
                        }
                    }
                    //if the generic name is the key
                    else {
                        //check if key is already in dictionary
                        if (drugs.ContainsKey(generic))
                            drugs[generic].Add(brand);
                        //key is not in the dictionary, create new value list
                        else {
                            List<string> valList = new List<string>();
                            valList.Add(brand);
                            drugs.Add(generic, valList);
                        }
                    }
                }
                else {
                    List<string> special = new List<string>();
                    special.Add(splitted[0]);
                    drugs.Add(splitted[0], special); //this should only happen for codeine, and it is added as both the key and the value   
                }
            }
        }

        return drugs;
    }*/


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


    private int GetDrugsSize() {
        return drugs.Count;
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
