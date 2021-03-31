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
        // A Test behaves as an ordinary method
        [Test]
        public void BasicTestsSimplePasses()
        {
            // Use the Assert class to test conditions
            SceneManager.LoadScene("MatchGame");
            GameObject sceneController = GameObject.Find("SceneController");
            if (sceneController == null)
            {
                Assert.Fail();
            }
        }

        // A Test behaves as an ordinary method
        [Test]
        public void BasicTestsSimpleFails()
        {
            // Use the Assert class to test conditions
            SceneManager.LoadScene("MatchGame");
            GameObject sceneController = GameObject.Find("UI");
            if (sceneController == null)
            {
                Assert.Fail();
            }
        }

        // // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // // `yield return null;` to skip a frame.
        // [UnityTest]
        // public IEnumerator BasicTestsWithEnumeratorPasses()
        // {
        //     // Use the Assert class to test conditions.
        //     // Use yield to skip a frame.
        //     yield return null;
        // }
    }
}
