using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
//stream reader/IO stuff
using System.IO;
using System;
using System.Text;

using Random = UnityEngine.Random; //to distinguish between UnityEngine.Random and System.Random

public abstract class SceneController : MonoBehaviour
{

    protected bool hintMode;
    protected List<List<string>> drugs = new List<List<string>>(); // master list of all drugs loaded in from the .csv
    [SerializeField] private List<string> currentDrugs = new List<string>(); // subset of drugs to cycle through for tapping games
    public List<DrugTile> tilesOnScreen = new List<DrugTile>(); // list of all the tiles currently in the game

    protected Dictionary<string, List<string>> drugnameToMatches = new Dictionary<string, List<string>>(); // maps a string containing a drug name to a list of its matches

    protected Dictionary<string, Color> drugnameToColor = new Dictionary<string, Color>(); // maps a string containing a drug name to a color

    [SerializeField] public AudioSource MatchSFX;
    [SerializeField] protected DrugTile drugPrefab;
    [SerializeField] private Transform[] cells; // list of cells on the board

    [SerializeField] private bool useDebugNames = false; // set to true to use debug names for drugs

    private int _score = 0;
    public int score
    {
        get { return _score; }
    }
    [SerializeField] private TMP_Text scoreText;

    // used for other text elements, such as lives
    [SerializeField] private TMP_Text secondaryTMP;

    public string secondaryText
    {
        get { return secondaryTMP.text; }
        set { secondaryTMP.text = value; }
    }

    protected Transform[] GetCells() { return cells; }

    List<Transform> GetEmptyCells()
    {
        // create a list of empty cells that can accept a new tile
        List<Transform> emptyCells = new List<Transform>();
        foreach (Transform cell in cells)
        {
            if (cell.childCount == 0 || (cell.childCount == 1 && cell.GetComponentInChildren<DrugTile>().markedToDestroy))
            {
                emptyCells.Add(cell);
            }
        }
        return emptyCells;
    }

    // create a drug tile and place it in the game
    protected virtual void CreateCard(List<Transform> cells = null)
    {
        if (cells == null)
        {
            cells = GetEmptyCells();
        }

        DrugTile newCard;
        Transform newCell = null;

        // if there is space to generate a new card, pick an index
        if (cells.Count > 0)
        {
            int whichCell = Random.Range(0, cells.Count);
            newCell = cells[whichCell];
        }

        if (newCell != null)
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
    protected virtual void PopulateTile(DrugTile newTile)
    {
        int index = Random.Range(0, currentDrugs.Count);
        string newName = currentDrugs[index];
        currentDrugs.RemoveAt(index);
        if (currentDrugs.Count == 0) currentDrugs = SampleDrugs(8);
        newTile.drugName = newName;
        newTile.drugMatches = drugnameToMatches[newName];
        SetColor(newTile);
    }

    // Use this for initialization
    protected virtual void Start()
    {
        // //temporary addition to test LoadDrugs
        // TestLoadDrugs();
        hintMode = Convert.ToBoolean((PlayerPrefs.GetInt("HintMode")));
        string fp = "60DrugNames";
        if (useDebugNames) fp = "testDrugNames";
        drugs = LoadDrugs(fp);
        if (hintMode)
        {
            foreach (var list in drugs)
            {
                Color drugColor = Color.HSVToRGB(Random.value, 30f / 255f, 1f);
                foreach (var drug in list)
                {
                    drugnameToMatches.Add(drug, list);
                    drugnameToColor.Add(drug, drugColor);
                }
            }
        }
        else
        {
            foreach (var list in drugs)
            {
                foreach (var drug in list)
                {
                    drugnameToMatches.Add(drug, list);
                    drugnameToColor.Add(drug, Color.HSVToRGB(Random.value, 30f / 255f, 1f));
                }
            }
        }
        currentDrugs = SampleDrugs(8);
        // Debug.Log(currentDrugs.Count);

        secondaryText = "";
    }

    public virtual void CardTapped(DrugTile card)
    {
        // Debug.Log("wrong");
    }

    public virtual void IncrementScore(int addedScore)
    {
        _score += addedScore;
        scoreText.text = "Score: " + _score;
    }

    // returns a randomly sampled list of 2*drugfamilyCount drugs from drugfamilyCount families
    // https://stackoverflow.com/questions/48087/select-n-random-elements-from-a-listt-in-c-sharp
    private List<string> SampleDrugs(int drugfamilyCount)
    {
        List<string> returnList = new List<string>();
        int index = 0;
        foreach (var matchList in drugs)
        {
            if (Random.Range(0f, drugs.Count - index) < (drugfamilyCount - returnList.Count / 2))
            {
                returnList.Add(matchList[0]);
                returnList.Add(matchList[1]);
            }
            if (returnList.Count >= (2 * drugfamilyCount)) return returnList;
            index += 1;
        }
        return returnList; // this shouldn't happen
    }

    List<List<string>> LoadDrugs(string fp)
    {
        List<List<string>> drugs = new List<List<string>>();
        string line = "";

        TextAsset theList = (TextAsset)Resources.Load(fp) as TextAsset;
        string txtContent = theList.text;

        using (StringReader sr = new StringReader(txtContent))
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

    // public void Restart()
    // {
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    // }

    // returns number of drug "families" currently on the board.
    protected int GetDrugFamilyCount()
    {
        Dictionary<List<string>, int> counts = new Dictionary<List<string>, int>();
        int unique = 0;
        foreach (DrugTile tile in tilesOnScreen)
        {
            if (counts.ContainsKey(tile.drugMatches))
            {
                counts[tile.drugMatches] = counts[tile.drugMatches] + 1;
            }
            else
            {
                counts[tile.drugMatches] = 1;
                unique += 1;
            }
        }
        // Debug.Log("Unique classes: " + unique);
        return unique;
    }

    // given two drug names, returns the name of the one which occurs on the board fewer times.
    protected string FewerOccurrences(string name1, string name2)
    {
        int count1 = 0;
        int count2 = 0;

        foreach (DrugTile tile in tilesOnScreen)
        {
            if (tile.drugName == name1)
            {
                count1 += 1;
            }
            else if (tile.drugName == name2)
            {
                count2 += 1;
            }
        }

        if (count2 < count1) return name2;
        else if (count1 < count2) return name1;
        // if tied, pick randomly
        else if (Random.Range(0f, 1f) > 0.5f) return name1;
        else return name2;
    }

    protected void GameOver()
    {
        // Restart();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void SetColor(DrugTile tile)
    {
        tile.GetComponent<Image>().color = drugnameToColor[tile.drugName];
    }
}
