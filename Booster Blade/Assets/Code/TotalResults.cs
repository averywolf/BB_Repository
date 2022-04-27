﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalResults : MonoBehaviour
{
    //displayed at end credits
    public SuperTextMesh resultsText;

 
    public List<CollectIcon> collectibleIcons;
    SaveManager saveManager;
    private void Awake()
    {
        saveManager = SaveManager.instance;
    }
    public void HideTotalResults()
    {
        resultsText.gameObject.SetActive(false);
        for (int i = 0; i < 10; i++)
        {
            collectibleIcons[i].gameObject.SetActive(false);
        }
    }
    //public void Start()
    //{
    //    for (int i = 0; i < 10; i++)
    //    {
    //        collectibleIcons[i].PickIcon(i);
    //    }
    //    ShowCollectibles();
    
    //}

    public void DisplayRunResults()
    {
        resultsText.gameObject.SetActive(true);
        for (int i = 0; i < 10; i++)
        {
            collectibleIcons[i].PickIcon(i);
            collectibleIcons[i].gameObject.SetActive(true);
        }
        ShowCollectibles();
        gameObject.SetActive(true);
        string resultsTally = "";
        float totalTime = 0;
        for (int i = 0; i < 10; i++) //might grab level name lenght from a manager
        {
            float levelTime = saveManager.RetrieveCurrentData(i).timeBeaten; //shows each time the player got from each level during the run
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
    public void ShowCollectibles()
    {
        for (int i = 0; i < 10; i++)
        {
            if (saveManager.RetrieveCurrentData(i).gotStageCollectible)
            {
                collectibleIcons[i].IconOn();
            }
        }
    }
}

