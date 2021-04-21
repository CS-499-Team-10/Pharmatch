using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapController : SceneController
{
    int livesLeft = 3;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < GetCells().Length; i++)
        {
            CreateCard();
        }
        secondaryText = "Lives: " + livesLeft;
    }

    protected DrugTile firstSelected;
    protected DrugTile secondSelected;

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

            firstSelected.markedToDestroy = true;
            secondSelected.markedToDestroy = true;

            MatchSFX.Play();
            firstSelected.GenerateMatchParticles();
            secondSelected.GenerateMatchParticles();
            Destroy(firstSelected.gameObject);
            Destroy(secondSelected.gameObject);

            CreateCard();
            CreateCard();
        }
        else
        {
            if (firstSelected.drugName != secondSelected.drugName)
            {
                livesLeft -= 1;
                secondaryText = "Lives: " + livesLeft;
                if (livesLeft == 0)
                {
                    GameOver();
                }
            }
            firstSelected.UnSelect();
            secondSelected.UnSelect();
        }
        firstSelected = null;
        secondSelected = null;
    }
}
