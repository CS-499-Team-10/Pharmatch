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

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			SlideUp();
			Invoke("CreateCard", 0.001f);
		}
		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			SlideDown();
			Invoke("CreateCard", 0.001f);
		}
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			SlideLeft();
			Invoke("CreateCard", 0.001f);
		}
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			SlideRight();
			Invoke("CreateCard", 0.001f);
		}
    }

    // void CreateCard() {
    //     CreateCard();
    // }

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
