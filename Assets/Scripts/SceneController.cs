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
	public List<DrugTile> tilesOnScreen = new List<DrugTile>(); //list of all the tiles currently in the game
    private DrugTile holder;

    [SerializeField] public AudioSource audios;
	[SerializeField] private DrugTile drugPrefab;
	[SerializeField] private Transform[] cells;

	[SerializeField] private String drugFilename = "testDrugNames.csv";
	
	private int _score = 0;
	[SerializeField] private TMP_Text scoreText;

	void Update()
	{
		// if(Input.GetKeyDown(KeyCode.Space))
		// {
		// 	CreateCard();
		// }
	}

	protected Transform[] GetCells() {return cells;}

	public void CreateCard()
    {
		DrugTile newCard;
		Transform newCell = null;

		// create a list of empty cells that can accept a new tile
		List<Transform> activeCells = new List<Transform>(); 
		foreach (Transform cell in cells)
		{
			if(cell.childCount == 0)
			{
				activeCells.Add(cell);
			}
		}

		// if there is space to generate a new card, pick an index
		if (activeCells.Count > 0)
		{
			int whichCell = Random.Range(0, activeCells.Count);
			newCell = activeCells[whichCell];
		}

		if(newCell != null)
		{
			newCard = Instantiate(drugPrefab, newCell) as DrugTile;
			newCard.transform.localScale = Vector3.zero; // start the scale at 0 and grow from there
			PopulateTile(newCard);
			newCard.controller = this;
			tilesOnScreen.Add(newCard);
		}
	}

	// populate a new drug tile with a new drug
	// can be overloaded for different behavior in various game modes
	protected virtual void PopulateTile(DrugTile newTile) {
		if (Random.Range(0, 9) > 1 && tilesOnScreen.Count != 0) //if we hit the 80% chance
		{
			holder = tilesOnScreen[Random.Range(0, tilesOnScreen.Count)];
			newTile.drugMatches = holder.drugMatches;
			if (holder.nameLabelTMP.text == holder.drugMatches[0]) //set our new card to match the randomly selected one
			{
				newTile.nameLabelTMP.text = holder.drugMatches[1];
			}
			else 
			{
				newTile.nameLabelTMP.text = holder.drugMatches[0];
			}
		} 
		else //otherwise add a random card
		{
			List<string> randomSet1 = drugs[Random.Range(0, drugs.Count)];
			newTile.drugMatches = randomSet1;
			newTile.nameLabelTMP.text = randomSet1[Random.Range(0, randomSet1.Count)];
		}
	}

	// Use this for initialization
	protected virtual void Start() {
        // //temporary addition to test LoadDrugs
        // TestLoadDrugs();

        drugs = LoadDrugs("Assets/DrugInfo/" + drugFilename);
	}

	public virtual void CardTapped(DrugTile card) {
		Debug.Log("wrong");
	}

	public void IncrementScore(int addedScore) {
		_score += addedScore;
		Debug.Log(_score);
		scoreText.text = "Score: " + _score;
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
                    toAdd.Add(splitted[0].ToUpper()); //generic name added
                if (splitted[1] != "")
                    toAdd.Add(splitted[1].ToUpper()); //trade name added
                if (splitted[2] != "")
                    toAdd.Add(splitted[2].ToUpper()); //alternate trade name added
                drugs.Add(toAdd); //add to list of lists
            }
        }
        return drugs;
    }

    public void TestLoadDrugs() {
        string fp = "Assets/DrugInfo/" + drugFilename;
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
