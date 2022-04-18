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

    public SuperTextMesh promptForSkip;
    public Coroutine skipPromptProcess;
    private bool skipReady = false;
    public string startIntermissionSong = "";
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
        AudioManager.instance.StopMusic();
        skipReady = false;
        RevertPrompt();
        promptForSkip.gameObject.SetActive(false);
        currentInterState = IntermissionState.beforeresults;
        
        SaveManager.instance.hasNotBeganLevel = true; //just in case
        Invoke("BeginVictory", 0.3f);


    }
    public void BeginVictory()
    {
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
                promptForSkip.gameObject.SetActive(true);
                currentInterState = IntermissionState.duringDialogue;
                AudioManager.instance.PlayMusic(startIntermissionSong);
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
                if (skipReady)
                {
                    StopSkip(skipPromptProcess);
                    dialogueManager.EndDialogue();
                }
                else //shows a prompt to check if the player is sure before they skip
                {
                    //make a sound effect?
                    skipPromptProcess = StartCoroutine(ResetSkipPrompt(2));
                    skipReady = true;
                   
                }
            }
            
        }
    }
    public void RevertPrompt()
    {
        skipReady = false;
        promptForSkip.text = "ESC: SKIP INTERMISSION";
    }
    public void StopSkip(Coroutine skip)
    {
        if (skip != null)
        {
            StopCoroutine(skip);
        }

    }
    public IEnumerator ResetSkipPrompt(float timeToReset)
    {
        promptForSkip.text = "PRESS AGAIN TO SKIP";
        yield return new WaitForSeconds(timeToReset);
        RevertPrompt();
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
