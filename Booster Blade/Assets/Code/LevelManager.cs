using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.EventSystems;
public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private LevelUI levelUI;

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
    public bool canPauseGame = false;
    public bool gamePaused; //might not be needed?

    //store level checkpoints
    private SaveManager saveManager;
    private CheckpointManager checkpointManager;
    private EntityManager entityManager;

    public int currentLevelIndex; //if this index is 9, the player is at the final stage

    [SerializeField, Header("After beating level, this scene loads (usually an Intermission)")]
    public string sceneToGoToNext = "";
    // loading the game makes levelManager place the player at the appropriate checkpoint
    private bool startingFromEntrance = false;
    public CinemachineVirtualCamera introCam;
    public CinemachineVirtualCamera playerCam;

    [SerializeField] //make sure to asssign this
    public LevelEntrance levelEntrance;
    private Coroutine gameTimer;

    public InputAction startBoost;


    [SerializeField]
    private string levelMusic = "Post Apocalypse";
    [SerializeField]
    private string actName = "";


    private bool debugIVon = false;
    private bool debugStop = false;
    [SerializeField, Tooltip("Only applies during the final stage")]
    private float finalTimeLimit;

    [HideInInspector]
    public bool tempGotStageCollectible= false;
    public Transform debugDestination;

    private float currentTime;
    void Awake() 
    {
        tempGotStageCollectible = false;
        entityManager = GetComponent<EntityManager>();
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
        introCam=levelEntrance.entranceCam;
        playerCam.gameObject.SetActive(false);
        introCam.gameObject.SetActive(false);
        levelUI.gameObject.SetActive(true);
    }
    public void Start()
    {
        saveManager.isGoingToIntermissionFromLevel = false;
        //This should ensure that the player will Continue from this level. Think I need to save to make this happen, though.

        saveManager.SetUpSavesAtLevelStart((SceneManager.GetActiveScene().buildIndex));
        if (saveManager.hasNotBeganLevel)
        {
            //might need more troubleshooting
            saveManager.RetrieveCurrentData(currentLevelIndex).gotStageCollectible = false; // Makes the collectible status reset if player is playing the stage for the first time during the run
            
        }

        MirrorSavesWithTempStats(); //do this first before showing the levelUI, since it uses tempGotStageCollectible

        levelUI.DisplayCollectible(tempGotStageCollectible);
        AudioManager.instance.StopMusic();
        Time.timeScale = 0;
        levelUI.SetActText(actName);
        levelUI.SetZoneText(NameZone());
        InitializePlayer();
        Time.timeScale = 1;
    
    }
    private string NameZone() //not the most elegant way of doing it, but it works
    {
        string zoneName = "";
        if (currentLevelIndex != 9)
        {

            zoneName = "ZONE " + (currentLevelIndex +1).ToString();
           
        }
        else {
            zoneName = "ZONE X";

        }

        return zoneName;
    }
    private void Update()
    {
        #region
        //if (Keyboard.current.uKey.wasPressedThisFrame)
        //{
        //    saveManager.DeleteRunProgress();
        //}
        //else if (Keyboard.current.tKey.wasPressedThisFrame) 
        //{
        //    saveManager.WipeSave();

        //}
        
        //else if (Keyboard.current.iKey.wasPressedThisFrame)
        //{
        //    if (!debugIVon)
        //    {
        //        playerController.SetDebugInvincibility(true);
        //        debugIVon = true;
        //        Debug.LogWarning("Player Invincibility enabled.");
        //    }
        //    else
        //    {
        //        playerController.SetDebugInvincibility(false);
        //        debugIVon = false;
        //        Debug.LogWarning("Player Invincibility disabled.");
        //    }

        //}
        //else if (Keyboard.current.pKey.wasPressedThisFrame)
        //{
        //    ExitLevel();
        //    //ends level immediately
        //}
        //else if (Keyboard.current.oKey.wasPressedThisFrame)
        //{
        //    playerController.KillPLayer();
        //}

        void SkipToLevel(string levelName) //apparently you can put functions in here? neat
        {
            saveManager.lastSavedAtCheckpoint = false;
            saveManager.tempCheckpointID = -1; 
            SaveManager.instance.hasNotBeganLevel = true; //just in case
            SceneManager.LoadScene(levelName); //unsure if there's anything else I need to do
        }
        if (Keyboard.current.leftShiftKey.isPressed) //extremely rudimentary level select system. need to hold left shift
        {
            if (Keyboard.current.iKey.wasPressedThisFrame)
            {
                if (!debugIVon)
                {
                    playerController.SetDebugInvincibility(true);
                    debugIVon = true;
                    Debug.LogWarning("Player Invincibility enabled.");
                }
                else
                {
                    playerController.SetDebugInvincibility(false);
                    debugIVon = false;
                    Debug.LogWarning("Player Invincibility disabled.");
                }
            }

            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                SkipToLevel("L-01");
            }
            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                SkipToLevel("L-02");
            }
            if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                SkipToLevel("L-03");
            }
            if (Keyboard.current.digit4Key.wasPressedThisFrame)
            {
                SkipToLevel("L-04");
            }
            if (Keyboard.current.digit5Key.wasPressedThisFrame)
            {
                SkipToLevel("L-05");
            }
            if (Keyboard.current.digit6Key.wasPressedThisFrame)
            {
                SkipToLevel("L-06");
            }
            if (Keyboard.current.digit7Key.wasPressedThisFrame)
            {
                SkipToLevel("L-07");
            }
            if (Keyboard.current.digit8Key.wasPressedThisFrame)
            {
                SkipToLevel("L-08");
            }
            if (Keyboard.current.digit9Key.wasPressedThisFrame)
            {
                SkipToLevel("L-09");
            }
            if (Keyboard.current.digit0Key.wasPressedThisFrame)
            {
                SkipToLevel("L-10");
            }
            if( Keyboard.current.cKey.wasPressedThisFrame)
            {
                PretendYouGotCollectible();
            }
            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                ExitLevel();
            }
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (debugStop)
                {
                    playerController.spacepaused = false;
                    debugStop = false;
                }
                else
                {
                    playerController.spacepaused = true;
                    debugStop = true;
                }

            }
            if (Keyboard.current.hKey.wasPressedThisFrame)
            {
                levelUI.ShowHideAllUI();
            }

        }
  
        
        #endregion
    }
    public void PretendYouGotCollectible()
    {
        Debug.Log("Pretending you got the collectible in this stage!");
        levelUI.DisplayCollectible(true);
        tempGotStageCollectible = true;
        
    }
    public void PauseInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (canPauseGame)
            {
                playerController.playerPaused = pauseMenu.PauseGame();
            }
        }
    }

    //Called at Start to setup player and save data.
    public void InitializePlayer() 
    {
        //if player saved at checkpoint, put them over where the checkpoint is
        if (saveManager.lastSavedAtCheckpoint && !saveManager.startStageFromBeginning)
        {
            Checkpoint startingCheckpoint = checkpointManager.SearchForCheckpoint(saveManager.tempCheckpointID);
            if(startingCheckpoint != null)
            {
                startingCheckpoint.DeactivateSpawnedCheckpoint();
                playerController.transform.position = startingCheckpoint.transform.position;
                Vector2 checkDirection = playerController.DirectionConverter(startingCheckpoint.transform.eulerAngles.z);
                playerController.SetFacingDirection(checkDirection.x, checkDirection.y);
                playerCam.gameObject.SetActive(true);
                introCam.gameObject.SetActive(false);
            }
            else
            {
                introCam.gameObject.SetActive(true);
                playerCam.gameObject.SetActive(false);
                startingFromEntrance = true;
            }
        }
        else
        {    
            if (saveManager.hasNotBeganLevel)
            {
                saveManager.currentTimeInLevel = 0;
                Debug.Log("First time playing level");

                saveManager.hasNotBeganLevel = false;
            }
            playerController.SetFacingDirection(1,0);
            //puts player at initialspawn
            playerController.transform.position = levelEntrance.initialSpawn.transform.position;
            playerCam.gameObject.SetActive(false);
            introCam.gameObject.SetActive(true);
            startingFromEntrance = true;
            saveManager.lastSavedAtCheckpoint = false;
            saveManager.startStageFromBeginning = false; //wait why is this here
        }
        playerController.FreezePlayer(true);
        levelUI.UpdateHPHUD(startingPlayerHP);
        playerController.currentPlayerHP = startingPlayerHP;
        
        if (currentLevelIndex == 9) //if it's the final stage
        {
            Debug.Log("It's the final stage! Removing limiters.");
            playerController.limitersRemoved = true;
        }
        else
        {
            playerController.limitersRemoved = false;
        }
        //loads whereever the player is supposed to be
    }
    //player might just refer to LevelUI directly
    public void ManagerUpdateHud(int hp)
    {
        levelUI.UpdateHPHUD(hp);
    }
    public void LevelDeathProcess(bool timeout)
    {
        AudioManager.instance.StopMusic();
        canPauseGame = false;
        levelUI.StartDeathUI(timeout);
        if(gameTimer != null)
        {
            StopCoroutine(gameTimer);
        }
     
        //do this but with currentLevelTime
        saveManager.currentTimeInLevel = levelTime;
        //saveManager.activeSave.SetCurrentLevelTime(levelTime); //registers time before death but doesn't formally save it
        Invoke("RestartFromLevelSave", 2); //Might uses a coroutine instead of Invoke later
    }
    
    public void BeginLevel()
    {
        
        levelStarted = true;
        levelUI.ClearTitleCard();
    }
    public Transform GetPlayerTransform()
    {
        return playerController.transform;
    }
    public void EnablePlayerControl(bool shouldControl)
    {
        playerController.canControlPlayer = shouldControl;

    }
    //called to shift player from cutscene mode, only necessary to call if player started from the entrance
    public void EnableControlAtLevelStart()
    {
        EnablePlayerControl(true);
        introCam.gameObject.SetActive(false);
        playerCam.gameObject.SetActive(true);
        levelEntrance.BlockBacktracking();
    }
    public void BlastOff()
    {
        if (levelMusic != "")
        {
            AudioManager.instance.PlayMusic(levelMusic);
        }
        //AudioManager.instance.PlayMusic("Post Apocalypse"); // should instead play relevant level music

        gameTimer = StartCoroutine(GameTimer());
        playerController.FreezePlayer(false);
        playerController.BeginBoost(2, true);
        entityManager.ActivateEntities();
        levelEntrance.OpenDoor(true, startingFromEntrance); //only gets door to play effect if starting from beginning
        canPauseGame = true;
      
        if (!startingFromEntrance)
        {
            EnablePlayerControl(true);
        }
    }
    public IEnumerator GameTimer()
    {
        //levelTime = 0;
        //float currentTime = 0;
        currentTime = 0;
        levelTime = saveManager.currentTimeInLevel;
        ///Final Countdown seems to work fine but might need further testing
        if (currentLevelIndex == 9)
        {
            levelUI.SetTimer(finalTimeLimit);
            levelUI.evilTimer = true;
            while (currentTime<finalTimeLimit)
            {
                currentTime += Time.deltaTime;
                //levelTime += currentTime;
                levelTime = currentTime;
                levelUI.SetTimer(finalTimeLimit-currentTime);
                yield return null;
            }
            levelUI.SetTimer(0);
            playerController.KillPLayer(true); //will probably initiate a special game over but this should do for now
        }
        else
        {
            while (true)
            {
         
                levelTime += Time.deltaTime;
                levelUI.SetTimer(levelTime);
                yield return null;
            }
        }

    }
    public void OnStartBoost(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!levelStarted)
            {
                BeginLevel();
            }
        }
 
   
    }
  
    // called when manually resetting the level
    // restarts from the very beginning of the stage, ignoring all checkpoints. checkpoint progress is lost.
    // collectible progress is wiped, too
    public void RestartLevel()
    {
        saveManager.lastSavedAtCheckpoint = false;
        saveManager.tempCheckpointID = -1; //unsure if setting this value to its default is safe
        saveManager.SaveCollectibleStatus(currentLevelIndex, false); // rezetz if player picked it up. maybe thiz rezet needz to happen at the Initialization procezz inztead?
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //called specifically when the player dies mid-stage.
    // collectible save data is not erased, it persists if the player had touched a checkpoint after getting the collectible
    public void RestartFromLevelSave()
    {
        StartCoroutine(TransitionManager.instance.Death(SceneManager.GetActiveScene().name));

       // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ExitLevel()
    {
        //saves level information (time that level was beaten, deathcount)

        // transitions to intermission scene
        if (gameTimer != null)
        {
            StopCoroutine(gameTimer);
            
        }
        saveManager.SaveCollectibleStatus(currentLevelIndex, tempGotStageCollectible); //loglevelCompletionData already calls "save both data" so it doesn't seem like there's need
        saveManager.LogLevelCompletionData(currentLevelIndex, levelTime);

        Debug.Log("Current time when exiting: " + saveManager.currentTimeInLevel);

        StartCoroutine(TransitionManager.instance.TransitionToIntermission(sceneToGoToNext));
    }
    public void ExitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    //Useful so any object can grab a reference to it
    public PlayerController GetPlayerController()
    {
        return playerController;
    }
    
    /// <summary>
    /// Called when touching collectible.
    /// Shows the player picked it up through the UI.
    /// Does NOT save collectible status to memory until a checkpoint is touched or the level ends
    /// </summary>
    public void SaveCollectibleTemporary()
    {
        levelUI.DisplayCollectible(true);
        tempGotStageCollectible = true;
    }
    public void SaveTheWorld()
    {
        if (gameTimer != null)
        {
            levelUI.evilTimer = false;
            levelUI.SetTimer(currentTime);
            StopCoroutine(gameTimer);
            
        }

        AudioManager.instance.StopMusic();
        //play sound
        Debug.Log("Time stopped " + saveManager.currentTimeInLevel);
        //play a sound


    }
    //called when restarting level

    /// <summary>
    /// Called when touching checkpoint
    /// </summary>
    /// <param name="checkpointID"></param>
    public void SaveWithCheckpoint(int checkpointID)
    {
        saveManager.RegisterCheckPoint(checkpointID);
        saveManager.SaveCollectibleStatus(currentLevelIndex, tempGotStageCollectible); //saves collectible to memory for currentRun
        saveManager.SaveBothData();
    }
    //remember to save after beating level, too
    public void ResetTempLevelStats()
    {
        
    }
    public void MirrorSavesWithTempStats()
    {
        tempGotStageCollectible=saveManager.RetrieveCurrentData(currentLevelIndex).gotStageCollectible; //this way, if the player dies, levelManager already remembers they got the collectible here. might be unnecessary if you just check saveManager only?
    }

}