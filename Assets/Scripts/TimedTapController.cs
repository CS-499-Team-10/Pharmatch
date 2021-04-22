using System.Collections;
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

        base.ShowHighScore(1);
    }

    void Update()
    {
        timeSinceSpawn += Time.deltaTime; // update timer
        if (timeSinceSpawn >= spawnCooldown) // create card if elapsed
        {
            CreateCard();
        }
    }

    // maps a value from one range to another
    float MapRange(float value, float oldMin, float oldMax, float newMin, float newMax)
    {
        return newMin + (value - oldMin) * (newMax - newMin) / (oldMax - oldMin);
    }

    // refresh spawn time depending on score and number of tiles on screen
    void UpdateSpawnTime()
    {
        // scale max spawn time based on current score
        currentMax = maxSpawnTime - (0.4f * Mathf.Log(score, 2));
        if (currentMax == Mathf.Infinity) currentMax = maxSpawnTime;
        Mathf.Clamp(currentMax, minSpawnTime, maxSpawnTime);

        // set spawn cooldown
        spawnCooldown = MapRange(tilesOnScreen.Count, 0, GetCells().Length, minSpawnTime, currentMax);
    }

    protected void CreateCard()
    {
        if (!PauseMenu.isPaused)
        {
            if (tilesOnScreen.Count == GetCells().Length) GameOver();
            timeSinceSpawn = 0;
            UpdateSpawnTime();
            base.CreateCard();
        }
    }

    protected override void GameOver()
    {
        UpdateHighScore("TimedHigh");
        base.GameOver();
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

    public override void IncrementScore(int addedScore)
    {
        base.IncrementScore(addedScore);
        UpdateSpawnTime();
    }

    protected void TryMatch()
    {
        if (firstSelected.CheckMatch(secondSelected))
        {
            IncrementScore(1);
            UpdateSpawnTime();

            tilesOnScreen.Remove(firstSelected);
            tilesOnScreen.Remove(secondSelected);

            MatchSFX.Play();
            firstSelected.GenerateMatchParticles();
            secondSelected.GenerateMatchParticles();
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

    // int GetHighScore()
    // {
    //     return PlayerPrefs.GetInt("TimedHigh");
    // }

    // void SetHighScore(int newHigh)
    // {
    //     PlayerPrefs.SetInt("TimedHigh", newHigh);
    // }
}
