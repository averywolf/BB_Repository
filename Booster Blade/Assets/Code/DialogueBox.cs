using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    public SuperTextMesh dialogueText;
    public Image charPortrait;
    public CanvasGroup canvasGroup;

    private SpriteRenderer boxRenderer;
    public bool readingText=false;
    public Image portraitBG;
    public void Awake()
    {
        dialogueText.text = "";
        boxRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetDialogueText(string message)
    {
        //if (dialogueText.reading)
        //{
        //    dialogueText.SkipToEnd();
        //    //only increment cutscene now?
        //}
        //else
        //{
            dialogueText.text = message;

            FocusBox();
        //}

    }
    public void DrawTextImmediately()
    {
        dialogueText.SkipToEnd();
    }
    public void HideBox()
    {
        canvasGroup.alpha = 0.5f;
    }
    public void FocusBox()
    {
        canvasGroup.alpha = 1f;
    }
}
