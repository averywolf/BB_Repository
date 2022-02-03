﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField]
    SuperTextMesh healthtext;

    [SerializeField]
    SuperTextMesh actTitleText;


    [SerializeField]
    Animator titlecardanim;

    [SerializeField]
    GameObject deathBG;

    [SerializeField]
    Slider staminaSlider;

    [SerializeField]
    SuperTextMesh timer;
    public void Awake()
    {
        deathBG.SetActive(false);
    }
    public void ClearTitleCard()
    {
        titlecardanim.Play("titlecard_clearaway");
    }

    public void UpdateHPHUD(int currentHP)
    {
        healthtext.text = "HP: " + currentHP.ToString();
    }
    public void StartDeathUI()
    {
        deathBG.SetActive(true);
    }

    public void SetStaminaSlider(float staminaVal)
    {
        staminaSlider.value = staminaVal;
        //staminaSlider;
    }
    public void SetTimer(float timerValue)
    {
        //trunctuates to two decimal places

        //timer.text = "Time: " + timerValue.ToString("F2");
        //TimeSpan timeToDisplay
        //timer.text = "Time: " + timerValue.ToString(@"hh\:mm\:ss");
        timer.text = "Time: " + FormatTime(timerValue);
    }
    private string FormatTime(float time)
    {
        int intTime = (int)time;
        int minutes = intTime / 60;
        int seconds = intTime % 60;
        float fraction = time * 1000;
        fraction = (fraction % 1000);
        string timeText = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, fraction);
        return timeText;
    }
    public void SetActText(string message)
    {
        actTitleText.text = message;
    }
}
