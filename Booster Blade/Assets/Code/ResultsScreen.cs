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
    public void DisplayResults()
    {


        //diplay score based on time text
        
        ///Since currently this doesn't represenet the player's actual best time, I changed the wording. (It'll return to Best Time later)
        timeText.text = ("Time: " + FormatTime(SaveManager.instance.activeSave.RetrieveLevelData(IntermissionManager.instance.LevelResultsIndex).bestTime));
        resultsAnim.Play("resultsTest");
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
