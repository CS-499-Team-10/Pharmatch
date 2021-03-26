using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideController : SceneController
{
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
    }

    protected override void PopulateTile(DrugTile newTile)
    {
        // with a % chance, or always if either the board is empty or the number of drug families is greater than a certain amount
        if ((Random.Range(0f, 1f) > 0.5f && tilesOnScreen.Count != 0) || (GetDrugFamilyCount() > 4))
        {
            // create a unique set of tiles on the board
            List<string> names = new List<string>();
            foreach (DrugTile tile in tilesOnScreen)
            {
                if (!names.Contains(tile.nameLabelTMP.text))
                {
                    names.Add(tile.nameLabelTMP.text);
                }
            }

            // pick a name from the set
            string newName = names[Random.Range(0, names.Count)];
            newTile.drugMatches = drugnameToMatches[newName];
            newTile.nameLabelTMP.text = FewerOccurrences(newTile.drugMatches[0], newTile.drugMatches[1]);
        }
        else //otherwise add a random card
        {
            List<string> drugFamily = drugs[Random.Range(0, drugs.Count)];
            newTile.drugMatches = drugFamily;
            newTile.nameLabelTMP.text = drugFamily[Random.Range(0, drugFamily.Count)];
        }
    }

    // Update is called once per frame
    void Update()
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

    // returns true if a move in the given direction is valid, false otherwise
    bool CanMove(SlideController.Direction dir)
    {
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
        if (CheckGameOver()) GameOver();
    }

    // creates a card at the opposite end of dir
    void CreateCard(Direction dir)
    {
        DrugTile newCard;
        Transform newCell = null;

        // create a list of empty cells that can accept a new tile
        List<Transform> activeCells = new List<Transform>();
        foreach (Transform cell in cells)
        {
            if (cell.childCount == 0 && !cell.GetComponent<GridPosition>().GetOpposite(dir)) // only pick cells at the opposite end from the direction
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

        if (newCell != null)
        {
            newCard = Instantiate(drugPrefab, newCell) as DrugTile;
            newCard.transform.localScale = Vector3.zero; // start the scale at 0 and grow from there
            PopulateTile(newCard);
            newCard.controller = this;
            tilesOnScreen.Add(newCard);
        }
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
}
