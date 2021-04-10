using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tests
{
    public class MainMenuTest
    {
        [UnityTest]
        public IEnumerator LoadTimed()
        {
            SceneManager.LoadScene("GameMenu");
            while (SceneManager.GetActiveScene().name != "GameMenu")
                yield return null; // wait for the scene to load

            // get and press play button
            GameObject playButtonObject = GameObject.Find("PlayTimedButton");
            var playButton = playButtonObject.GetComponent<Button>();
            playButton.onClick.Invoke();

            // did correct scene load?
            while (SceneManager.GetActiveScene().name != "MatchGame")
                yield return null; // wait for the scene to load
            Assert.IsTrue(SceneManager.GetActiveScene().name == "MatchGame");

            // check game mode
            GameObject sceneController = GameObject.Find("SceneController");
            SceneController controllerComponent = sceneController.GetComponent<TimedTapController>();
            Assert.IsTrue(controllerComponent.enabled);
            controllerComponent = sceneController.GetComponent<TapController>();
            Assert.IsFalse(controllerComponent.enabled);
            controllerComponent = sceneController.GetComponent<SlideController>();
            Assert.IsFalse(controllerComponent.enabled);

        }
    }
}
