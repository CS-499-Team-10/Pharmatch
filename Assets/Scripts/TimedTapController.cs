using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedTapController : SceneController
{
    protected DrugTile firstSelected;
    protected DrugTile secondSelected;

    float spawnCooldown = 2f; // seconds until the next tile will spawn
    float timeSinceSpawn = 0f; // seconds since last tile spawned

    [SerializeField] const float minSpawnTime = 0.75f;
    [SerializeField] const float maxSpawnTime = 4f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        // for (int i = 0; i < GetCells().Length / 2; i++)
        // {
        //     CreateCard();
        // }
    }

    // maps a value s from a1-a2 to b1-b2
    float MapRange(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    void Update()
    {
        timeSinceSpawn += Time.deltaTime; // update timer
        if (timeSinceSpawn >= spawnCooldown) // create card if elapsed
        {
            CreateCard();
        }

        spawnCooldown = MapRange(tilesOnScreen.Count, 0, GetCells().Length, minSpawnTime, maxSpawnTime);
    }

    protected void CreateCard()
    {
        if (tilesOnScreen.Count == GetCells().Length) GameOver();
        timeSinceSpawn = 0;
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
