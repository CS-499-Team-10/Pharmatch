using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideController : SceneController
{
    [SerializeField] int maxDrugFamilies = 6;

    // time it takes for a tile to slide
    public const float SLIDE_TIME = 0.15f;
    float timeSinceSlide = SLIDE_TIME;

    // the directions in which a tile can slide
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < 3; i++)
        {
            CreateCard();
        }

        base.ShowHighScore(2);
    }

    protected override void PopulateTile(DrugTile newTile)
    {
        // with a % chance, or always if either the board is empty or the number of drug families is greater than a certain amount
        if ((Random.Range(0f, 1f) > 0.5f && tilesOnScreen.Count != 0) || (GetDrugFamilyCount() > maxDrugFamilies))
        {
            // create a unique set of tiles on the board
            List<string> names = new List<string>();
            foreach (DrugTile tile in tilesOnScreen)
            {
                if (!names.Contains(tile.drugName))
                {
                    names.Add(tile.drugName);
                }
            }

            // pick a name from the set
            string newName = names[Random.Range(0, names.Count)];
            newTile.drugMatches = drugnameToMatches[newName];
            newTile.drugName = FewerOccurrences(newTile.drugMatches[0], newTile.drugMatches[1]);
        }
        else //otherwise add a random card
        {
            List<string> drugFamily = drugs[Random.Range(0, drugs.Count)];
            newTile.drugMatches = drugFamily;
            newTile.drugName = drugFamily[Random.Range(0, drugFamily.Count)];
        }
        SetColor(newTile);
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceSlide += Time.deltaTime;
        if (timeSinceSlide >= (SLIDE_TIME * 1.01)) // wait a little longer than the slide time before allowing another slide
        {
            if (SwipeInput.swipedUp)
            {
                Slide(SlideController.Direction.Up);
            }
            if (SwipeInput.swipedDown)
            {
                Slide(SlideController.Direction.Down);
            }
            if (SwipeInput.swipedLeft)
            {
                Slide(SlideController.Direction.Left);
            }
            if (SwipeInput.swipedRight)
            {
                Slide(SlideController.Direction.Right);
            }
        }
    }

    // returns true if a move in the given direction is valid, false otherwise
    bool CanMove(SlideController.Direction dir)
    {
        foreach (Transform cell in GetCells()) // for each cell on the board
        {
            GridPosition pos = cell.GetComponentInParent<GridPosition>(); // get its grid position
            if (pos && !pos.GetNext(dir)) // if this position is on the edge (has no next cell in the passed direction)
            {
                for (GridPosition nextMover = pos.GetOpposite(dir); nextMover != null; nextMover = nextMover.GetOpposite(dir)) // for each position, traveling in the opposite direction
                {
                    DrugTile tileToMove = nextMover.gameObject.transform.GetComponentInChildren<DrugTile>();
                    if (tileToMove != null)
                    {
                        if (tileToMove.CanSlide(dir))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    // slides each tile on the board in the passed direction
    void Slide(SlideController.Direction dir)
    {
        timeSinceSlide = 0f;
        if (!CanMove(dir)) return;
        foreach (Transform cell in GetCells()) // for each cell on the board
        {
            GridPosition pos = cell.GetComponentInParent<GridPosition>(); // get its grid position
            if (!pos.GetNext(dir)) // if this position is on the edge (has no next cell in the passed direction)
            {
                for (GridPosition nextMover = pos.GetOpposite(dir); nextMover != null; nextMover = nextMover.GetOpposite(dir)) // for each position, traveling in the opposite direction
                {
                    DrugTile tileToMove = nextMover.gameObject.transform.GetComponentInChildren<DrugTile>();
                    if (tileToMove != null)
                    {
                        if (tileToMove.Slide(dir)) // returns true if tile matched
                        {
                            IncrementScore(1);
                        }
                    }
                }
            }
        }
        CreateCard(dir); // create a card at the opposite direction of the swipe
        if (CheckGameOver()) GameOver(2);
    }

    // creates a card at the opposite end of dir
    void CreateCard(Direction dir)
    {
        // create a list of empty cells that can accept a new tile
        List<Transform> activeCells = new List<Transform>();
        foreach (Transform cell in GetCells())
        {
            if (cell.childCount == 0 && !cell.GetComponent<GridPosition>().GetOpposite(dir)) // only pick cells at the opposite end from the direction
            {
                activeCells.Add(cell);
            }
        }
        base.CreateCard(activeCells);
    }

    // returns true if player has no moves available
    bool CheckGameOver()
    {
        foreach (Direction dir in System.Enum.GetValues(typeof(Direction)))
        {
            if (CanMove(dir)) return false;
        }
        return true;
    }

    int GetHighScore() {
        return PlayerPrefs.GetInt("SlideHigh");
    }

    void SetHighScore(int newHigh) {
        PlayerPrefs.SetInt("SlideHigh", newHigh);
    }
}
