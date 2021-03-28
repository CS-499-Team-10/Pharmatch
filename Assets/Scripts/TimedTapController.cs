using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedTapController : SceneController
{
    protected DrugTile firstSelected;
    protected DrugTile secondSelected;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < GetCells().Length / 2; i++)
        {
            CreateCard();
        }
        InvokeRepeating("CreateCard", 1, 1);
    }

    protected void CreateCard()
    {
        if (tilesOnScreen.Count == GetCells().Length) GameOver();
        base.CreateCard();
    }

    public override void CardTapped(DrugTile card)
    {
        if (firstSelected == null)
        {
            firstSelected = card;
            firstSelected.Select();
        }
        else
        {
            secondSelected = card;
            secondSelected.Select();
            TryMatch();
        }
    }

    protected void TryMatch()
    {
        if (firstSelected.CheckMatch(secondSelected))
        {
            IncrementScore(1);

            tilesOnScreen.Remove(firstSelected);
            tilesOnScreen.Remove(secondSelected);

            Destroy(firstSelected.gameObject);
            Destroy(secondSelected.gameObject);
        }
        else
        {
            Debug.Log("no");
            firstSelected.UnSelect();
            secondSelected.UnSelect();
        }
        firstSelected = null;
        secondSelected = null;
    }
}
