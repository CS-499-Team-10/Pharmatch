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
            const string sceneName = "MatchGame";
            SceneManager.LoadScene(sceneName);
            // wait for the scene to load before proceeding
            while (SceneManager.GetActiveScene().name != sceneName)
            {
                yield return null;
            }
            GameObject sceneController = GameObject.Find("SceneController");
            if (sceneController == null)
            {
                Assert.Fail();
            }
            // yield return null;
        }

        [UnityTest]
        public IEnumerator FailToFindGameObject()
        {
            const string sceneName = "MatchGame";
            SceneManager.LoadScene(sceneName);
            // wait for the scene to load before proceeding
            while (SceneManager.GetActiveScene().name != sceneName)
            {
                yield return null;
            }
            GameObject obj = GameObject.Find("NotHere");
            if (obj != null)
            {
                Assert.Fail();
            }
            // yield return null;
        }
    }
}
