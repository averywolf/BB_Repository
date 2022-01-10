using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class IntermissionManager : MonoBehaviour
{
    public static IntermissionManager instance;
    
    //takes cutscene parameters as an argument?
    [SerializeField]
    private DialogueManager dialogueManager;
    [SerializeField]
    private ResultsScreen resultsScreen;

    //private bool canAdvance = false;
    //private bool isDisplayingResults= true;
    [SerializeField]
    private string nextLevel="";
    private IntermissionState currentInterState;

    //index of the level the player just beat
    [SerializeField]
    public int LevelResultsIndex;
    public enum IntermissionState
    {
        beforeresults,
        results,
        duringDialogue,
        afterDialogue,
        other
    }



    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Start()
    {
        currentInterState = IntermissionState.beforeresults;
        AudioManager.instance.PlayMusic("CardVictoryJingle");
        resultsScreen.DisplayResults();
    }
    public void Update()
    {
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            if (currentInterState.Equals(IntermissionState.results))
            {
                resultsScreen.HideResults();
                dialogueManager.BeginDialogue();
                currentInterState = IntermissionState.duringDialogue;
                AudioManager.instance.PlayMusic("Occult");
            }
            else if(currentInterState.Equals(IntermissionState.duringDialogue))
            {
                dialogueManager.AdvanceCutscene();
            }
        }
        else if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (currentInterState.Equals(IntermissionState.duringDialogue))
            {
                dialogueManager.EndDialogue();
            }
        }
    }
    public void SetInterState(IntermissionState interState)
    {
        currentInterState = interState;
    }
    public void GoToNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }
    //have separate system specific to here that plays events when the dialogue reaches a certain increment (might wait for the event to play out before actually advancing text
}
