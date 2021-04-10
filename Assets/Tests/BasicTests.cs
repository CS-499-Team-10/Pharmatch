using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class BasicTests
    {
        [UnityTest]
        public IEnumerator FindSceneController()
        {
            SceneManager.LoadScene("MatchGame");
            while (SceneManager.GetActiveScene().name != "MatchGame")
                yield return null; // wait for the scene to load

            Assert.NotNull(GameObject.Find("SceneController"));
        }

        [UnityTest]
        public IEnumerator FailToFindGameObject()
        {
            SceneManager.LoadScene("MatchGame");
            while (SceneManager.GetActiveScene().name != "MatchGame")
                yield return null; // wait for the scene to load

            Assert.Null(GameObject.Find("NotHere"));
        }
    }
}
