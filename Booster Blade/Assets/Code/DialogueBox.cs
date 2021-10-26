﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    public SuperTextMesh dialogueText;
    public Image charPortrait;
    public CanvasGroup canvasGroup;

    public bool readingText=false;
    public void Awake()
    {
        dialogueText.text = "";
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
