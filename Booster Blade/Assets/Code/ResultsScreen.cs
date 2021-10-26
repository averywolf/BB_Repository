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
