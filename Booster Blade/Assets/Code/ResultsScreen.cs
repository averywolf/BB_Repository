using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsScreen : MonoBehaviour
{
    [HideInInspector]
    public bool canMoveOnFromResults = false;
    [SerializeField]
    private Animator resultsAnim;

    [SerializeField]
    private SuperTextMesh timeText;
    private SaveManager saveManager;
    private IntermissionManager intermissionManager;
    public void Awake()
    {
        saveManager = SaveManager.instance;
        intermissionManager = IntermissionManager.instance;
    }
    public void DisplayResults()
    {
        Debug.Log("Current time when loading into results: " + saveManager.currentTimeInLevel);
        //diplay score based on time text

        ///Since currently this doesn't represenet the player's actual best time, I changed the wording. (It'll return to Best Time later)

        resultsAnim.Play("resultsTest");
        int curIndex = intermissionManager.LevelResultsIndex;
        if(saveManager.isGoingToIntermissionFromLevel)
        {
            Debug.Log("Got here from level.");
            CompareTimes(saveManager.RetrieveCurrentTime(curIndex).bestTime, saveManager.RetrieveRecordTime(curIndex).bestTime);
            //timeText.text = ("Time: " + FormatTime(SaveManager.instance.activeSave.RetrieveLevelData(IntermissionManager.instance.LevelResultsIndex).bestTime));
        }
        else
        {
            Debug.Log("Did not get here from level, shouldn't save anything since this has to be for testing");
        }
    }

    public void CompareTimes(float curTime, float timeToBeat)
    {
        saveManager.LoadBothData(); //probably pointless to put this here
        Debug.Log("The time to beat is " + timeToBeat);
        if (timeToBeat == 999999)
        {
            //I guess this is how I can determine if the player didn't beat the level yet?
            Debug.Log("Beat level for first time!");
            timeText.text = ("Time : " + FormatTime(curTime));

            saveManager.SaveCurrentTimes(intermissionManager.LevelResultsIndex, curTime);
            saveManager.SaveNewRecord(intermissionManager.LevelResultsIndex, curTime);
        }
        else if (curTime <= timeToBeat)
        {
            timeText.text = ("New best time : " + FormatTime(curTime) + "\nOld time:" + FormatTime(timeToBeat));

            saveManager.SaveCurrentTimes(intermissionManager.LevelResultsIndex, curTime);
            saveManager.SaveNewRecord(intermissionManager.LevelResultsIndex, curTime);
        }
        else
        {
            timeText.text = ("Time : " + FormatTime(curTime) + "Record:" + FormatTime(timeToBeat));
            saveManager.SaveCurrentTimes(intermissionManager.LevelResultsIndex, curTime);
        }

        saveManager.SaveBothData();
        //do I need to save manually?
    }
    // compare the currentLevelTime to the BestTime, (if there is a best time)
    // if cur <= best, display cur, and save cur as the new BestTime. signify high score.
    // if cur > best, display best, don't update BestTime, and 

    //saving should probably take place at the end of the level, huh... because otherwise if you looked at intermission scene to start it would easily get overriden with 0 from testing
    //maybe have temp variable in SaveManager that's set only when you're going from a level to intermission. so if you just viewed the intermission it wouldnt' override
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
    public void SignalResultsOver()
    {
        canMoveOnFromResults = true;
        IntermissionManager.instance.SetInterState(IntermissionManager.IntermissionState.results);
    }
    public void HideResults()
    {
        gameObject.SetActive(false);
    }
}
