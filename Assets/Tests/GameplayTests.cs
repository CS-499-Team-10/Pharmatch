using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class GameplayTests
    {
        [UnityTest]
        public IEnumerator CheckGameOver()
        {
            ModeSelector.mode = ModeSelector.Mode.Timed;
            SceneManager.LoadScene("MatchGame");
            yield return new WaitForSeconds(1);
            while (SceneManager.GetActiveScene().name != "MatchGame")
                yield return null; // wait for the scene to load

            GameObject sceneController = GameObject.Find("SceneController");
            SceneController controllerComponent = sceneController.GetComponent<TimedTapController>();
            Time.timeScale = 99.0f; // speed up and wait for game to end
            while (SceneManager.GetActiveScene().name != "GameMenu")
                yield return null; // wait for the scene to load
            Assert.True(SceneManager.GetActiveScene().name == "GameMenu");
        }

        [UnityTest]
        public IEnumerator CheckMatchesAvailable()
        {
            ModeSelector.mode = ModeSelector.Mode.Tap;
            SceneManager.LoadScene("MatchGame");
            yield return new WaitForSeconds(1);
            while (SceneManager.GetActiveScene().name != "MatchGame")
                yield return null; // wait for the scene to load

            GameObject sceneController = GameObject.Find("SceneController");
            SceneController controllerComponent = sceneController.GetComponent<TapController>();
            Assert.True(controllerComponent.tilesOnScreen.Count == 16);
            // check that each tile has a match available
            foreach (var tile in controllerComponent.tilesOnScreen)
            {
                int foundMatches = 0;
                foreach (var otherTile in controllerComponent.tilesOnScreen)
                {
                    if (tile.CheckMatch(otherTile))
                    {
                        foundMatches += 1;
                    }
                }
                Assert.True(foundMatches == 1);
            }
        }
    }
}
