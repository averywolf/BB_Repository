using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class MainMenu : MonoBehaviour
{
    public string firstLevelName="";
    public PlayerInput playerInput;
    private bool ableToInteractWithMenu=false;
    private SaveManager saveManager;

    public SuperTextMesh resultsTemp;
    //no loading save yet



    // Start is called before the first frame update
    private void Awake()
    {
        saveManager = SaveManager.instance;   
    }
    void Start()
    {
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
    public void ViewRecords()
    {
        Debug.Log("Would be able to use a feature for records... IF I HAD THEM!!!!");
        //Use resultsTemp
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void DisplayTempResults()
    {

    }
}
