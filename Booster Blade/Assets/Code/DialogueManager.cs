using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class DialogueManager : MonoBehaviour
{
    public DialogueBox upperDialogueBox;
    public DialogueBox lowerDialogueBox;

    //Make sure these are assigned to the correct prefabs
    //When checking the CutsceneEvent's Actor enum it'll determine which one of these to read
    public DialogueActor archie;
    public DialogueActor zaria;
    
    public List<CutsceneEvent> cutsceneEvents;
    [SerializeField]
    private int currentCutsceneEventNumber = 0;

    private SuperTextMesh currentTextMesh;
    public void Awake()
    {
        upperDialogueBox.gameObject.SetActive(false);
        lowerDialogueBox.gameObject.SetActive(false);
    }

    public void BeginDialogue()
    {
        upperDialogueBox.gameObject.SetActive(true);
        lowerDialogueBox.gameObject.SetActive(true);
        ReadCutsceneText(cutsceneEvents[0]);
        currentCutsceneEventNumber = 0;
    }
    public void EndDialogue()
    {
        IntermissionManager.instance.SetInterState(IntermissionManager.IntermissionState.afterDialogue);
        IntermissionManager.instance.GoToNextLevel();
    }
    public void ReadCutsceneText(CutsceneEvent cutsceneEvent)
    {
        DialogueBox dialogueBox = upperDialogueBox;
        DialogueBox oldBox = lowerDialogueBox;
        DialogueActor currentActor = GetDialogueActor(cutsceneEvent.characterActor);
        //cutsceneEvent.dialoguePosition.Equals(CutsceneEvent.DialoguePosition.bottom))
        if (currentActor.actorDialoguePosition.Equals(DialogueActor.DialoguePosition.bottom))
        {
            dialogueBox = lowerDialogueBox;
            oldBox = upperDialogueBox;
        }
        currentTextMesh = dialogueBox.dialogueText;
        dialogueBox.SetDialogueText(cutsceneEvent.dialogueline);
        dialogueBox.charPortrait.sprite = currentActor.expressions[cutsceneEvent.expressionNum];
        dialogueBox.portraitBG.sprite = currentActor.portraitBg;
        oldBox.HideBox();
        //probably should make the dialogue box that's not being used fade out

    }
    public DialogueActor GetDialogueActor(CutsceneEvent.Actor actor)
    {
        if (actor.Equals(CutsceneEvent.Actor.Archie))
        {
            return archie;
        }
        else if (actor.Equals(CutsceneEvent.Actor.Zaria))
        {
            return zaria;
        }
        //no Badguy yet
        return archie;
    }

    //Make cutscene information a scriptable object
    public void AdvanceCutscene()
    {
        if(currentTextMesh !=null && currentTextMesh.reading)
        {
            currentTextMesh.SkipToEnd();
        }
        else
        {
            currentCutsceneEventNumber++;
            // currentCutsceneEventNumber++;

            if ((currentCutsceneEventNumber) < cutsceneEvents.Count)
            {
                ReadCutsceneText(cutsceneEvents[currentCutsceneEventNumber]);

            }
            else
            {
                EndDialogue();
            }
        }
    }

}
