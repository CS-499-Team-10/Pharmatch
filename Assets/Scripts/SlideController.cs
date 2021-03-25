using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideController : SceneController
{
    // Start is called before the first frame update
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
        if(SwipeInput.swipedUp)
		{
			SlideUp();
			// Invoke("CreateCard", 0.001f);
			CreateCard();
		}
		if(SwipeInput.swipedDown)
		{
			SlideDown();
			// Invoke("CreateCard", 0.001f);
			CreateCard();
		}
		if(SwipeInput.swipedLeft)
		{
			SlideLeft();
			// Invoke("CreateCard", 0.001f);
			CreateCard();
		}
		if(SwipeInput.swipedRight)
		{
			SlideRight();
			// Invoke("CreateCard", 0.001f);
			CreateCard();
		}
    }

    void SlideUp() {
		foreach (Transform cell in GetCells())
		{
			GridPosition pos = cell.GetComponentInParent<GridPosition>();
			if (!pos.up) {
				for (GridPosition nextMover = pos.down; nextMover != null; nextMover = nextMover.down)
				{
					DrugTile tileToMove = nextMover.gameObject.transform.GetComponentInChildren<DrugTile>();
					if (tileToMove != null) {
						if (tileToMove.Slide(DrugTile.Direction.Up))
                        {
                            IncrementScore(1);
						}							
					}
				}
			}
		}
	}

	void SlideDown() {
		foreach (Transform cell in GetCells())
		{
			GridPosition pos = cell.GetComponentInParent<GridPosition>();
			if (!pos.down) {
				for (GridPosition nextMover = pos.up; nextMover != null; nextMover = nextMover.up)
				{
					DrugTile tileToMove = nextMover.gameObject.transform.GetComponentInChildren<DrugTile>();
					if (tileToMove != null) {
						if (tileToMove.Slide(DrugTile.Direction.Down))
						{
                            IncrementScore(1);
						}
					}
				}
			}
		}
	}

	void SlideRight() {
		foreach (Transform cell in GetCells())
		{
			GridPosition pos = cell.GetComponentInParent<GridPosition>();
			if (!pos.right) {
				for (GridPosition nextMover = pos.left; nextMover != null; nextMover = nextMover.left)
				{
					DrugTile tileToMove = nextMover.gameObject.transform.GetComponentInChildren<DrugTile>();
					if (tileToMove != null) {
						if (tileToMove.Slide(DrugTile.Direction.Right))
						{
                            IncrementScore(1);
						}
					}
				}
			}
		}
	}

	void SlideLeft() {
		foreach (Transform cell in GetCells())
		{
			GridPosition pos = cell.GetComponentInParent<GridPosition>();
			if (!pos.left) {
				for (GridPosition nextMover = pos.right; nextMover != null; nextMover = nextMover.right)
				{
					DrugTile tileToMove = nextMover.gameObject.transform.GetComponentInChildren<DrugTile>();
					if (tileToMove != null) {
						if (tileToMove.Slide(DrugTile.Direction.Left))
						{
                            IncrementScore(1);
						}
					}
				}
			}
		}
	}
}
