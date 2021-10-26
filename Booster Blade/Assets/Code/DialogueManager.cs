using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class DialogueManager : MonoBehaviour
{
    public DialogueBox upperDialogueBox;
    public DialogueBox lowerDialogueBox;
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
        IntermissionManager.instance.GoToNextLevel();
    }
    public void ReadCutsceneText(CutsceneEvent cutsceneEvent)
    {
        DialogueBox dialogueBox = upperDialogueBox;
        DialogueBox oldBox = lowerDialogueBox;
        if (cutsceneEvent.dialoguePosition.Equals(CutsceneEvent.DialoguePosition.bottom))
        {
            dialogueBox = lowerDialogueBox;
            oldBox = upperDialogueBox;
        }
        currentTextMesh = dialogueBox.dialogueText;
        dialogueBox.SetDialogueText(cutsceneEvent.dialogueline);
        dialogueBox.charPortrait.sprite = cutsceneEvent.dialogueActorTest.expressions[cutsceneEvent.expressionNum];
        oldBox.HideBox();
        //probably should make the dialogue box that's not being used fade out

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
