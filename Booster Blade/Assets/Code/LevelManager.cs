using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private LevelUI levelUI;


    private float currentLevelTime;
    public static LevelManager instance;

    public int startingPlayerHP = 3;

    [SerializeField]
    private PlayerController playerController;

    private bool levelStarted = false;
    private float levelTime = 0;
    //GameManager carries data from scene to scene

    //should reference UI probably (maybe even set up Player with it)
    [SerializeField]
    private PauseMenu pauseMenu;

    [HideInInspector]
    public bool canPauseGame =false;

    //store level checkpoints
    private SaveManager saveManager;
    private CheckpointManager checkpointManager;
    // loading the game makes levelManager place the player at the appropriate checkpoint
    void Awake() 
    {
        checkpointManager = GetComponent<CheckpointManager>();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        saveManager = SaveManager.instance; // MAKE SURE THIS IS INTIALIZED FIRST
        levelStarted = false;
    }
    public void Start()
    {
        AudioManager.instance.StopMusic();
       // Time.timeScale = 0;
       
        InitializePlayer();
    
    }
    private void Update()
    {
        if(Keyboard.current.zKey.isPressed && !levelStarted)
        {
            BeginLevel();
        }
        if(canPauseGame && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            pauseMenu.PauseGame();
        }
    }
   
    public void InitializePlayer() //sets up player at the start of level
    {
        //if player saved at checkpoint, put them over where the checkpoint is
        if (saveManager.activeSave.lastSavedAtCheckpoint && !saveManager.startStageFromBeginning)
        {
            Checkpoint startingCheckpoint = checkpointManager.SearchForCheckpoint(saveManager.activeSave.savedCheckpointID);
            if(startingCheckpoint != null)
            {
               // playerController.InitializePlayer(startingCheckpoint.g)
                // playerController.transform.position = new Vector3(saveManager.activeSave.spawnX, saveManager.activeSave.spawnY);
                playerController.transform.position = startingCheckpoint.transform.position;
                playerController.SetFacingDirection(startingCheckpoint.GetCheckpointDirection());
                //might need to manually set facing angle for animation here
            }

        }
        else
        {
            saveManager.startStageFromBeginning = false;
        }
    
        levelUI.UpdateHPHUD(startingPlayerHP);
        playerController.currentPlayerHP = startingPlayerHP;
        //loads whereever the player is supposed to be
    }
    //player might just refer to LevelUI directly
    public void ManagerUpdateHud(int hp)
    {
        levelUI.UpdateHPHUD(hp);
    }
    public void LevelDeathProcess()
    {
        canPauseGame = false;
        levelUI.StartDeathUI();
        Invoke("RestartLevel", 2); //placeholder
    }

    public void BeginLevel()
    {
        levelStarted = true;
        levelUI.ClearTitleCard();
    }

    public void BlastOff()
    {
        AudioManager.instance.PlayMusic("Post Apocalypse");
        StartCoroutine(GameTimer());
        playerController.SetFacingDirection(PlayerController.PlayerDirection.right);
        playerController.BeginBoost(2, false);
        playerController.levelStarted = true;
        canPauseGame = true;
    }
    public IEnumerator GameTimer()
    { //currently resets when you die?
        levelTime = 0;
        while (true)
        {
            levelTime += Time.deltaTime;
            levelUI.SetTimer(levelTime);
            yield return null;
        }
    }

    //i guess this resets all enemies?
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ResultsScreen()
    {
        //
    }

    /// SET SAVED AT CHECKPOINT TO FALSE WHEN MOVING ON TO NEXT SCENE

}