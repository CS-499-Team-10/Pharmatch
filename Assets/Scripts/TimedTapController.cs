﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedTapController : SceneController
{
    protected DrugTile firstSelected;
    protected DrugTile secondSelected;

    [SerializeField] float spawnCooldown = 2f; // seconds until the next tile will spawn
    [SerializeField] float timeSinceSpawn = 0f; // seconds since last tile spawned

    [SerializeField] const float minSpawnTime = 0.75f;
    [SerializeField] const float maxSpawnTime = 4f;
    [SerializeField] float currentMax = maxSpawnTime;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < GetCells().Length / 4; i++)
        {
            CreateCard();
        }
    }

    // maps a value from one range to another
    float MapRange(float value, float oldMin, float oldMax, float newMin, float newMax)
    {
        return newMin + (value - oldMin) * (newMax - newMin) / (oldMax - oldMin);
    }

    void Update()
    {
        timeSinceSpawn += Time.deltaTime; // update timer
        if (timeSinceSpawn >= spawnCooldown) // create card if elapsed
        {
            CreateCard();
        }

        // scale max spawn time based on current score
        currentMax = maxSpawnTime - (0.4f * Mathf.Log(score, 2));
        if (currentMax == Mathf.Infinity) currentMax = maxSpawnTime;
        Mathf.Clamp(currentMax, minSpawnTime, maxSpawnTime);

        // set spawn cooldown
        spawnCooldown = MapRange(tilesOnScreen.Count, 0, GetCells().Length, minSpawnTime, currentMax);
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
