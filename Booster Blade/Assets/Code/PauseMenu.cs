using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PauseMenu : MonoBehaviour
{

    private bool gameIsPaused = false;
    public GameObject firstPauseButton;
    [SerializeField]
    private GameObject pauseGraphic;

    private void Awake()
    {
        pauseGraphic.SetActive(false);
    }
    public bool PauseGame()
    {
        if(gameIsPaused)
        {
            Time.timeScale = 1;
            Debug.Log("Game Resumed");
            SetNewFirstSelected(null);
            pauseGraphic.SetActive(false);

        }
        else
        {
            Time.timeScale = 0;
            Debug.Log("Game Paused");
            SetNewFirstSelected(firstPauseButton);
            pauseGraphic.SetActive(true);
        }
        //tacky solution
        gameIsPaused = !gameIsPaused;
        return gameIsPaused;
    }
    private void SetNewFirstSelected(GameObject firstSelection)
    {
        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(firstSelection);
        firstPauseButton.GetComponent<Button>().Select();
    }
    public void Unpause()
    {
        Debug.Log("Unpause pressed");
        Time.timeScale = 1;
        Debug.Log("Game Resumed");
        SetNewFirstSelected(null);
        pauseGraphic.SetActive(false);
        LevelManager.instance.GetPlayerController().playerPaused = false;
        gameIsPaused = false;
    }
    public void RetryLevel()
    {
        Debug.Log("Pressed Retry");
        LevelManager.instance.RestartLevel();
    }
    public void QuitToMenu()
    {
        LevelManager.instance.ExitToMenu();
    }
    public void QuitGame()
    {
        
    }
}
