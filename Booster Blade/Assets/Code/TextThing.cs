using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextThing : MonoBehaviour
{
    public SuperTextMesh sMesh;
    public float delayBeforeReading;

    private void Awake()
    {
        sMesh = GetComponent<SuperTextMesh>();
    }

    public void ReadText()
    {
        sMesh.Read();
    }

    
}
