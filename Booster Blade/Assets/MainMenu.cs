using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MainMenu : MonoBehaviour
{
    public string firstLevelName="";
    public PlayerInput playerInput;
    private bool ableToInteractWithMenu=false;
    private SaveManager saveManager;

    public GameObject resultsMenu;
    public GameObject firstButton;

    public SuperTextMesh resultsText;


    public GameObject resultsReturnButton;
    //no loading save yet



    // Start is called before the first frame update
    private void Awake()
    {
        saveManager = SaveManager.instance;   
    }
    void Start()
    {
        SetNewFirstSelected(firstButton);
        resultsMenu.SetActive(false);
        ableToInteractWithMenu = true;
        AudioManager.instance.StopMusic();
        AudioManager.instance.PlayMusic("Occult");
        SaveManager.instance.hasNotBeganLevel = true; //just in case
    }

    //starts run from the beginning, plays the intro cutscene, goes onto the first level
    public void StartNewGame()
    {
        ableToInteractWithMenu = false;
        //temp solution
        saveManager.DeleteRunProgress();
        SceneManager.LoadScene(firstLevelName);
    }
    //picks up from the level where the current run left off. starts at beginning of the last level, doesn't take into account checkpoints
    public void ContinueGame()
    {
        //Maybe if continue index is 0 don't continue?
        SceneManager.LoadScene(saveManager.currentRunData.continueIndex);

    }
    //deletes all save data
    public void WipeRecords()
    {
        saveManager.WipeSave();
    }

    public void QuitGame()
    {
        Debug.Log("Attempting to quit game.");
        Application.Quit();
    }

    public void OpenResultsScreen()
    {
        resultsMenu.SetActive(true);
        SetNewFirstSelected(resultsReturnButton);
        DisplayBestTimes();
    }
    public void CloseResultsScreen()
    {
        resultsMenu.SetActive(false);
        SetNewFirstSelected(firstButton);
    }
    public void DisplayBestTimes()
    {
        //Currently you can view all results even if you haven't beaten the stages

        string resultsTally = "";
        for (int i = 0; i < 10; i++) //might grab level name lenght from a manager
        {
            float levelTime = saveManager.RetrieveRecordData(i).bestTime;
            if (levelTime == 999999)
            {
                resultsTally += "Stage " + (i + 1).ToString() + ": Not cleared \n";
            }
            else
            {
                resultsTally += "Stage " + (i + 1).ToString() + ": " + FormatTime(levelTime) + "\n";
            }
           
        }

        resultsText.text = resultsTally;
        //display total
    }
    private void SetNewFirstSelected(GameObject firstSelection)
    {
        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(firstSelection);
        firstSelection.GetComponent<Button>().Select();
        //firstPauseButton.GetComponent<Button>().Select();
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
