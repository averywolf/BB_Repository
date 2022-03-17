using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeTell : MonoBehaviour
{
    Transform chargeTransform;

    private void Awake()
    {
        chargeTransform = transform;
    }

    public IEnumerator ScaleOverTime(float duration, float ogScale, float endScale)
    {
        float currentTime = 0.0f;
        Vector3 originalScale =   new Vector3(ogScale, ogScale, 1f);
        Vector3 destinationScale = new Vector3(endScale, endScale, 1f);

        while (currentTime <= duration)
        {
            chargeTransform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / duration);
            currentTime += Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
        //should probably use FixedUpdate?
    }
    public void ActivateTell(float timeActive, float ogScale, float endScale)
    {
        gameObject.SetActive(true);
        StartCoroutine(ScaleOverTime(timeActive, ogScale, endScale));
    }
}
