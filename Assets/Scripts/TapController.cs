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

        base.ShowHighScore(0);
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
                    Invoke("GameOver", 1);
                }
            }
            firstSelected.UnSelect();
            secondSelected.UnSelect();
        }
        firstSelected = null;
        secondSelected = null;
    }

    protected override void GameOver()
    {
        UpdateHighScore("TapHigh");
        base.GameOver();
    }

    // int GetHighScore()
    // {
    //     return PlayerPrefs.GetInt("TapHigh");
    // }

    // void SetHighScore(int newHigh)
    // {
    //     PlayerPrefs.SetInt("TapHigh", newHigh);
    // }
}
