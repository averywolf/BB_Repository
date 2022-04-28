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
    private bool ableToInteractWithMenu=false; //doesn't do anything as of writing but might be used to stop player from inputting during transitions
    private SaveManager saveManager;

    public GameObject resultsMenu;
    public GameObject firstButton;

    public SuperTextMesh resultsText;

    public GameObject recordsEnterButton;
    public GameObject resultsReturnButton;
    //no loading save yet
    [SerializeField]
    private OptionsMenu optionsMenu;

    public SuperTextMesh totalResults;

    [SerializeField]
    MenuArrow menuArrow;
    [SerializeField]
    private string menuSong = "";
    public bool menuInitialized= false;

    public GameObject OptionsDeleteConfirmation;
    public GameObject optionsDontDelete;
    public GameObject optionsOpenDeleteConfirmation;

    public GameObject deletedText;
    private void Awake()
    {
        saveManager = SaveManager.instance;   
    }
    public Button continueButton;
    void Start()
    {
        deletedText.gameObject.SetActive(false);
        OptionsDeleteConfirmation.SetActive(false);
        SetNewFirstSelected(firstButton);
        resultsMenu.SetActive(false); //options menu hides itself
        ableToInteractWithMenu = true;
        AudioManager.instance.StopMusic();
        AudioManager.instance.PlayMusic(menuSong);
        Debug.Log("Making sure the player hasNotBeganLevel here");
        SaveManager.instance.hasNotBeganLevel = true; //just in case
        Time.timeScale = 1; //resets time to make sure it moves if the player got here from the pause menu
        menuInitialized = true;
        if(saveManager.currentRunData.continueIndex == 0)
        {
            continueButton.interactable = false;
        }
        else
        {
            continueButton.interactable = true;
        }
    }

    //starts run from the beginning, plays the intro cutscene, goes onto the first level
    public void StartNewGame()
    {
        PlayButtonClickSFX();
        ableToInteractWithMenu = false;
        saveManager.DeleteRunProgress(); //should clear current run but NOT records
        SceneManager.LoadScene(firstLevelName);
    }
    //picks up from the level where the current run left off. starts at beginning of the last level, doesn't take into account checkpoints
    public void ContinueGame()
    {
        PlayButtonClickSFX();
        //Maybe if continue index is 0 don't continue?
        SceneManager.LoadScene(saveManager.currentRunData.continueIndex);

    }

    public void ShowDeletedText()
    {
        deletedText.gameObject.SetActive(true);
        Invoke("HideDeletedText", 1);   
    }
    public void HideDeletedText()
    {
        deletedText.gameObject.SetActive(false);
    }
    //deletes all save data
    public void WipeRecords()
    {
        PlayButtonClickSFX();
        saveManager.WipeSave();
        SetNewFirstSelected(optionsOpenDeleteConfirmation);
        SetContinueButtonStatus();
        OptionsDeleteConfirmation.SetActive(false);
        ShowDeletedText();
        //play sound effect
    }
    public void OpenDeleteConfirmation()
    {
        menuArrow.PlaceArrowNoSound(optionsDontDelete.GetComponent<RectTransform>());
        SetNewFirstSelected(optionsDontDelete);
        PlayButtonClickSFX();
        OptionsDeleteConfirmation.SetActive(true);
    }
    public void DontWipeRecords()
    {
        PlayButtonClickSFX();
        SetNewFirstSelected(optionsOpenDeleteConfirmation);
        OptionsDeleteConfirmation.SetActive(false);
    }
    public void OpenOptions()
    {

        optionsMenu.OptionsOpen(); //options menu takes care of SetNewFirstSelected and arrow placement stuff
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenResultsScreen()
    {
        PlayButtonClickSFX();
        resultsMenu.SetActive(true);
        menuArrow.PlaceArrow(resultsMenu.GetComponent<RectTransform>());
        SetNewFirstSelected(resultsReturnButton);
        DisplayBestTimes();
    }
    public void CloseResultsScreen()
    {
        PlayButtonClickSFX();
        resultsMenu.SetActive(false);
        
        SetNewFirstSelected(recordsEnterButton);
        menuArrow.PlaceArrow(recordsEnterButton.GetComponent<RectTransform>());
    }
    public void DisplayBestTimes()
    {
        //Currently you can view all results even if you haven't beaten the stages

        string resultsTally = "";

        //Specifically, this shows the individual best times for each stage
        for (int i = 0; i < 10; i++) //might grab level name lenght from a manager
        {
            float levelTime = saveManager.RetrieveRecordData(i).timeBeaten;
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
        if(saveManager.recordsData.bestTotalTime == -1)
        {
            totalResults.text = "COMPLETE RUN TIME NOT YET RECORDED";
        }
        else
        {
            totalResults.text = "FASTEST COMPLETE RUN: " + FormatTime(saveManager.recordsData.bestTotalTime).ToString();
        }
      
        //display total
    }
    private void SetContinueButtonStatus()
    {
        if (saveManager.currentRunData.continueIndex == 0)
        {
            continueButton.interactable = false;
        }
        else
        {
            continueButton.interactable = true;
        }
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
    public void PlayButtonClickSFX()
    {
        AudioManager.instance.Play("UIButtonClick");
    }

    public void ConditionalSelect(RectTransform t) //used by New Game arrow to prevent sound from playing automatically
    {
        if (menuInitialized)
        {
            menuArrow.PlaceArrow(t);
        }
        else
        {
            menuArrow.PlaceArrowNoSound(t);
        }
    }
}
