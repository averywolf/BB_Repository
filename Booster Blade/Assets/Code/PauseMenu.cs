using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    private bool gameIsPaused = false;

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
            pauseGraphic.SetActive(false);

        }
        else
        {
            Time.timeScale = 0;
            Debug.Log("Game Paused");
            pauseGraphic.SetActive(true);
        }
        //tacky solution
        gameIsPaused = !gameIsPaused;
        return gameIsPaused;
    }
}
