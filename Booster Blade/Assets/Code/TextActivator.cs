using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextActivator : MonoBehaviour
{
    public List<TextThing> textThings;


    public void StartReadingText()
    {
        StartCoroutine(ReadingProcess());
    }
    public IEnumerator ReadingProcess()
    {
        for (int i = 0; i < textThings.Count; i++)
        {
            yield return new WaitForSeconds(textThings[i].delayBeforeReading);
            textThings[i].ReadText();
        }
    }
}
