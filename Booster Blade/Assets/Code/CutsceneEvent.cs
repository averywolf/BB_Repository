using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class CutsceneEvent
{
    [TextArea]
    public string dialogueline = "";
    //speedread and skiptotheend are supertextmeshfeatures;

    //variable that determines which DialogueActor to use
    public Actor characterActor = Actor.Archie;
    public int expressionNum = 0;

    public enum Actor
    {
        Archie,
        Zaria,
        Badguy
    }

}
