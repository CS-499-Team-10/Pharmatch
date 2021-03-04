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

	// void Update()
	// {
	// 	if(Input.GetKeyDown(KeyCode.Space))
	// 	{
	// 		createCard();
	// 	}
	// }

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
		// drugs = List from read csv

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
		// increment score if the cards match
		Vector3 card1pos, card2pos;

        //int drugsSize = GetDrugsSize();
		
		if (_firstRevealed.drugMatches.Contains(_secondRevealed.nameLabelTMP.text)) {
			_score++;
			Debug.Log(_score);
			scoreText.text = "Score: " + _score;

			Destroy(_firstRevealed.gameObject);
			Destroy(_secondRevealed.gameObject);

			Invoke("createCard", 0.001f);
			Invoke("createCard", 0.001f);
			// createCard();
			// createCard();
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
