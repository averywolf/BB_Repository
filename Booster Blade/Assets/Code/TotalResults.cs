﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalResults : MonoBehaviour
{
    //displayed at end credits
    public SuperTextMesh resultsText;

    SaveManager saveManager;
    private void Awake()
    {
        saveManager = SaveManager.instance;
    }

    public void Start()
    {
        DisplayRunResults();
    }
    public void DisplayRunResults()
    {
        string resultsTally = "";
        float totalTime = 0;
        for (int i = 0; i < 10; i++) //might grab level name lenght from a manager
        {
            float levelTime = saveManager.RetrieveCurrentData(i).bestTime;
            resultsTally += "Stage " + (i + 1).ToString() + ": " + FormatTime(levelTime) + "\n";

            totalTime += levelTime;
        }
        resultsTally += "TOTAL: " + FormatTime(totalTime);

        resultsText.text = resultsTally;

        //display total
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


}

