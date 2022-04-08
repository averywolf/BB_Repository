using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueActor : MonoBehaviour
{
    public string actorName="";
    public DialoguePosition actorDialoguePosition = DialoguePosition.top;
    public Sprite portraitBg;
    public enum DialoguePosition
    {
        top,
        bottom,
    }

    public List<Sprite> expressions; 
    //Contains all of the information related to the character speaking
}
