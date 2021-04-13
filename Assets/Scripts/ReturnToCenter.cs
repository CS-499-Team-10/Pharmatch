using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToCenter : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(MoveTowards(transform, new Vector3(transform.localPosition.x, 0f, 0f), 0.25f));
    }

    // wrapper for MoveTowards to use as a coroutine
    // edited from https://stackoverflow.com/a/51166030
    IEnumerator MoveTowards(Transform objectToMove, Vector3 toPosition, float duration)
    {
        float counter = 0;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            Vector3 currentPos = objectToMove.localPosition;

            float time = Vector3.Distance(currentPos, toPosition) / (duration - counter) * Time.deltaTime;

            objectToMove.localPosition = Vector3.MoveTowards(currentPos, toPosition, time);

            // Debug.Log(counter + " / " + duration);
            yield return null;
        }
        objectToMove.localPosition = toPosition;
    }
}
