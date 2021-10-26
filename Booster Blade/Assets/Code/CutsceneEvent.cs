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
    public DialoguePosition dialoguePosition = DialoguePosition.top;
    public Actor characterActor = Actor.Player;
    public int expressionNum = 0;
    public enum DialoguePosition
    {
        top,
        bottom,
    }
    public enum Actor
    {
        Player,
        Support,
        Badguy
    }
    public DialogueActor dialogueActorTest;


    //DialogueActor
}
