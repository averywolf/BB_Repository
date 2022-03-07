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
    public bool canPauseGame =false;
    public bool gamePaused;

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

    private bool debugIVon= false;
    [SerializeField, Tooltip("Only applies during the final stage")]
    private float finalTimeLimit;
    void Awake() 
    {
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
        AudioManager.instance.StopMusic();
        Time.timeScale = 0;
        levelUI.SetActText(actName);
        InitializePlayer();
        Time.timeScale = 1;
    
    }
    private void Update()
    {
        if (Keyboard.current.uKey.wasPressedThisFrame)
        {
            saveManager.DeleteRunProgress();
        }
        else if (Keyboard.current.tKey.wasPressedThisFrame) //is this the same key for WASD attack?
        {
            saveManager.WipeSave();

        }
        else if (Keyboard.current.iKey.wasPressedThisFrame)
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
        else if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            AudioManager.instance.PauseMusic(true);
        }
        else if (Keyboard.current.nKey.wasPressedThisFrame)
        {

            AudioManager.instance.PauseMusic(false);
        }
        else if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            ExitLevel();
            //ends level immediately
        }
        else if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            playerController.KillPLayer();
        }
        //if (startBoost.triggered && !levelStarted)
        //{

        //    BeginLevel();
        //}
        if (canPauseGame && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            playerController.playerPaused = pauseMenu.PauseGame();
        }

    }

    public void InitializePlayer() //sets up player at the start of level
    {
        //if player saved at checkpoint, put them over where the checkpoint is
        if (saveManager.lastSavedAtCheckpoint && !saveManager.startStageFromBeginning)
        {
            Checkpoint startingCheckpoint = checkpointManager.SearchForCheckpoint(saveManager.tempCheckpointID);
            if(startingCheckpoint != null)
            {
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
    public void LevelDeathProcess()
    {
        AudioManager.instance.StopMusic();
        canPauseGame = false;
        levelUI.StartDeathUI();
        StopCoroutine(gameTimer);
        //do this but with currentLevelTime
        saveManager.currentTimeInLevel = levelTime;
        //saveManager.activeSave.SetCurrentLevelTime(levelTime); //registers time before death but doesn't formally save it
        Invoke("RestartLevel", 2); //placeholder
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
        playerController.BeginBoost(2, false);
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
        float currentTime = 0;
  
        Debug.Log("Player has spent " + saveManager.currentTimeInLevel +" in level.");
        levelTime = saveManager.currentTimeInLevel;
        ///Final Countdown seems to work fine, but it doesn't seem to save the player's time properly.
        if (currentLevelIndex == 9)
        {
            levelUI.SetTimer(finalTimeLimit);
            while (currentTime<finalTimeLimit)
            {
                currentTime += Time.deltaTime;
                //levelTime += currentTime;
                levelTime = currentTime;
                levelUI.SetTimer(finalTimeLimit-currentTime);
                yield return null;
            }
            levelUI.SetTimer(0);
            playerController.KillPLayer(); //will probably initiate a special game over but this should do for now
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
            Debug.Log("Should BOOST OFF");
            if (!levelStarted)
            {
                BeginLevel();
            }
        }
 
   
    }
    //i guess this resets all enemies?
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitLevel()
    {
        //saves level information (time that level was beaten, deathcount)

        // transitions to intermission scene

        StopCoroutine(gameTimer);
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

}