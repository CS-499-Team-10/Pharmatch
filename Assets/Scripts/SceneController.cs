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
			List<string> randomSet1 = drugs[Random.Range(0, 3)];
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
		// Vector3 startPos = new Vector3(-4, 4, -10);

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
		// Vector3 card1pos, card2pos;

        //int drugsSize = GetDrugsSize();
		
		if (_firstRevealed.drugMatches.Contains(_secondRevealed.nameLabelTMP.text)) {
			_score++;
			Debug.Log(_score);
			scoreText.text = "Score: " + _score;

			Destroy(_firstRevealed.gameObject);
			Destroy(_secondRevealed.gameObject);

			Invoke("createCard", 0.001f);
			Invoke("createCard", 0.001f);

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


    // private int GetDrugsSize() {

    //     return drugs.Count;

    // }


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
