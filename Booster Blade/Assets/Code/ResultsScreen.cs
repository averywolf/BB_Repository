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
        timeText.text = ("Time: " + SaveManager.instance.activeSave.RetrieveLevelData(IntermissionManager.instance.LevelResultsIndex).bestTime.ToString());
        resultsAnim.Play("resultsTest");
 
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
