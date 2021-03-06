using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalResults : MonoBehaviour
{
    //displayed at end credits
    public SuperTextMesh resultsText;
    public SuperTextMesh resultsText2;
    public SuperTextMesh resultsTextTotal;
 
    public List<CollectIcon> collectibleIcons;
    SaveManager saveManager;
    public GameObject resultsPanel;


    private void Awake()
    {
        saveManager = SaveManager.instance;
        resultsPanel.SetActive(false);
    }
    public void HideTotalResults()
    {
        resultsText.gameObject.SetActive(false);
        for (int i = 0; i < 10; i++)
        {
            collectibleIcons[i].gameObject.SetActive(false);
        }
    }


    public void DisplayRunResults()
    {
        AudioManager.instance.Play("ChangeDirection");
        resultsPanel.gameObject.SetActive(true);
        resultsText.gameObject.SetActive(true);
        for (int i = 0; i < 10; i++)
        {
            collectibleIcons[i].PickIcon(i);
            collectibleIcons[i].gameObject.SetActive(true);
        }
        ShowCollectibles();
        
        gameObject.SetActive(true);
        string resultsTally = "";
        string resultsTally2 = "";
        float totalTime = 0;
        for (int i = 0; i < 10; i++) //might grab level name lenght from a manager
        {
            float levelTime = saveManager.RetrieveCurrentData(i).timeBeaten; //shows each time the player got from each level during the run

            if (i > 4)
            {
                if (i == 9)
                {
                    resultsTally2 += "STAGE X: " + TimeWriter(levelTime);
                }   
                else
                {
                    resultsTally2 += "STAGE " + (i + 1).ToString() + ": " + TimeWriter(levelTime);
                }
                
            }
            else
            {
                resultsTally += "STAGE " + (i + 1).ToString() + ": " + TimeWriter(levelTime);
            }
          
      

            totalTime += levelTime;
        }
        resultsTextTotal.text= "TOTAL TIME: " + FormatTime(totalTime);
        //resultsTally += "TOTAL: " + FormatTime(totalTime);
        saveManager.LogTotalTime(totalTime);
        resultsText.text = resultsTally;
        resultsText2.text = resultsTally2;
       
        //display total
    }
    public string TimeWriter(float score)
    {
        string levelT = "\n";
        if(score== 999999)
        {
            levelT= "Not cleared \n";
        }
        else
        {
            levelT = FormatTime(score) + "\n";
        }
        return levelT;
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

