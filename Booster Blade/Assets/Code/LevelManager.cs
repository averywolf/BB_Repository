﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Cinemachine;
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

    public int currentLevelIndex;

    [SerializeField, Header("After beating level, this scene loads (usually an Intermission)")]
    public string sceneToGoToNext = "";
    // loading the game makes levelManager place the player at the appropriate checkpoint
    private bool startingFromEntrance = false;
    public CinemachineVirtualCamera introCam;
    public CinemachineVirtualCamera playerCam;

    [SerializeField] //make sure to asssign this
    public LevelEntrance levelEntrance;
    private Coroutine gameTimer;
   
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
            playerController.playerPaused = pauseMenu.PauseGame();
        }
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
           // ShakeCamera(10f, 0.2f);
        }
    }
    public void BoostShake()
    {

    }
    //public void ShakeCamera(float intensity, float time)
    //{
    //    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
    //        playerCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>(); //maybe make function that sets this?
    //    cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
    //    StartCoroutine(CamShake(time));
    //}
    public IEnumerator CamShake(float time)
    {
        float shakeTime = 0;
        while (shakeTime < time)
        {
            shakeTime += Time.deltaTime;
            yield return null;
        }
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
             playerCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
    }

    public void InitializePlayer() //sets up player at the start of level
    {
        //if player saved at checkpoint, put them over where the checkpoint is
        if (saveManager.activeSave.lastSavedAtCheckpoint && !saveManager.startStageFromBeginning)
        {
            Checkpoint startingCheckpoint = checkpointManager.SearchForCheckpoint(saveManager.activeSave.savedCheckpointID);
            if(startingCheckpoint != null)
            {
                playerController.transform.position = startingCheckpoint.transform.position;
                playerController.SetFacingDirection(startingCheckpoint.GetCheckpointDirection());
                playerController.DisplayHorzVert();
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
            Debug.Log("Starting from beginning of level");
            playerController.SetFacingDirection(PlayerController.PlayerDirection.right);
            //puts player at initialspawn
            playerController.transform.position = levelEntrance.initialSpawn.transform.position;

            playerController.DisplayHorzVert();
            playerCam.gameObject.SetActive(false);
            introCam.gameObject.SetActive(true);
            startingFromEntrance = true;
            saveManager.activeSave.lastSavedAtCheckpoint = false;
            saveManager.startStageFromBeginning = false; //wait why is this here
        }
        playerController.FreezePlayer(true);
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
        AudioManager.instance.StopMusic();
        canPauseGame = false;
        levelUI.StartDeathUI();
        StopCoroutine(gameTimer);
        saveManager.activeSave.SetCurrentLevelTime(levelTime); //registers time before death but doesn't formally save it
        Invoke("RestartLevel", 2); //placeholder
    }
    
    public void BeginLevel()
    {
        
        levelStarted = true;
        levelUI.ClearTitleCard();
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
        AudioManager.instance.PlayMusic("Post Apocalypse"); // should instead play relevant level music

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
    { //currently resets when you die?
        //levelTime = 0;
        levelTime = saveManager.activeSave.currentLevelTime;
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

    public void ExitLevel()
    {
        //saves level information (time that level was beaten, deathcount)

        // transitions to intermission scene

        /// SET SAVED AT CHECKPOINT TO FALSE WHEN MOVING ON TO NEXT SCENE
        /// 
        /// 
        ///IMPORTANT: I don't think the data is truly saved yet (as in, if you close the game, the data will go away)
        ///Need to make sure that happens
        saveManager.activeSave.lastSavedAtCheckpoint = false;
        saveManager.activeSave.SaveCompleteLevelData(currentLevelIndex, levelTime);
        StartCoroutine(TransitionManager.instance.TransitionToIntermission(sceneToGoToNext));
    }
    //Useful so any object can grab a reference to it
    public PlayerController GetPlayerController()
    {
        return playerController;
    }

}