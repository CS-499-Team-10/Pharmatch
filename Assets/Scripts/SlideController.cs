using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideController : SceneController
{
    private enum Direction
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
            newTile.nameLabelTMP.text = FewerOccurences(newTile.drugMatches[0], newTile.drugMatches[1]);
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
            Slide(DrugTile.Direction.Up);
            CreateCard();
        }
        if (SwipeInput.swipedDown)
        {
            Slide(DrugTile.Direction.Down);
            CreateCard();
        }
        if (SwipeInput.swipedLeft)
        {
            Slide(DrugTile.Direction.Left);
            CreateCard();
        }
        if (SwipeInput.swipedRight)
        {
            Slide(DrugTile.Direction.Right);
            CreateCard();
        }
    }

    void Slide(DrugTile.Direction dir)
    {
        foreach (Transform cell in GetCells())
        {
            GridPosition pos = cell.GetComponentInParent<GridPosition>();
            if (!pos.GetNext(dir))
            {
                for (GridPosition nextMover = pos.GetOpposite(dir); nextMover != null; nextMover = nextMover.GetOpposite(dir))
                {
                    DrugTile tileToMove = nextMover.gameObject.transform.GetComponentInChildren<DrugTile>();
                    if (tileToMove != null)
                    {
                        if (tileToMove.Slide(dir))
                        {
                            IncrementScore(1);
                        }
                    }
                }
            }
        }
    }
}
