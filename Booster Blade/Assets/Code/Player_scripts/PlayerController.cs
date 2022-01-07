﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
public class PlayerController : MonoBehaviour
{

    #region unorganized variables
    //baseRunSpeed
    // Start is called before the first frame update
    // X and Y inputs. These are taken all of the time, however...
    [HideInInspector]
    public PlayerInput playerInput;
    private Rigidbody2D playerRb;
    public static float testBaseSpeed = -26;
    [SerializeField]
    private float currentRunSpeed;
    [SerializeField]
    private float boostSpeedModifier = 10;
    // It's *these* variables, which are set equal to those inputX/inputY most of the time that move the player.
    // They are set to zero under some circumestances, like being stunned.
    private float horizontal;
    private float vertical;

    public bool testStun = false;
    //forget what exactly this is
    private Vector2 movementVelValue;
    [SerializeField]
    private DashTrail dashTrail;

    private float xEnvironmentMod;//represents outside forces on object like energy fields
    private float yEnvironmentMod;
    [SerializeField]
    private GameObject playerBody;

    public GameObject ChargeReleaseEffect;
    private IEnumerator exhaustBoost; //after a certain time, exit boost and return to normal movement
    private IEnumerator boostCooldown; // can't spam charge over and over

    public Animator playerAnimator;
    [HideInInspector]
    public bool canControlPlayer = false;
    private bool isDead = false;
    public int currentPlayerHP;
    private float angleToFace=0;

    [SerializeField]
    private float boostDuration = 2;
    [SerializeField, Tooltip("Time before player can use boost again")]
    private float boostCooldownTime = 1;

    private bool canBoost = true;

    [SerializeField]
    private LevelUI testUI;
    [SerializeField]
    private DebugLevelUI debugUI;

    [SerializeField]
    private SpriteFlash bodyFlash;
    [SerializeField]
    private SpriteFlash swordFlash;
    private bool isStunned = false;
    //might make public
    private bool isInvincible = false;
    #endregion

    [HideInInspector]
    public PlayerMoveStates currentMoveState = PlayerMoveStates.moving;
    public PlayerSword playerSword;

    private bool canTurn = true;

    private Vector2 initialturnpoint;

    private WallChecker wallChecker;

    private bool isBouncingOffWall = false;


    private LevelManager levelManager;
    [HideInInspector]
    public bool isReversing = false;
    public bool playerPaused = false;
    //currently unsure if a distinction is necessary between freeze and paused. freeze is set outside of pause contexts
    private bool isPlayerFrozen = false;

    [HideInInspector]
    public bool swordSlashing=false;
    public CinemachineImpulseSource boostSource;
    public enum PlayerMoveStates
    {
        idle,
        moving,
        boosting,
    }

    public void Awake()
    {
        boostSource = GetComponent<CinemachineImpulseSource>();
        playerInput = GetComponent<PlayerInput>();
        playerRb = GetComponent<Rigidbody2D>();
        wallChecker = GetComponentInChildren<WallChecker>();
        dashTrail.SetEnabled(true);
        levelManager = LevelManager.instance;
    }
    public void Start()
    {

        playerAnimator.SetBool("heroBoosting", false);
        playerSword.swordBoosting = false;
        isReversing = false;
       // playerAnimator.Play("hero_lungeUp");
        //SetLungeAnimation(currentFacingAngle);
    }

    private void Update()
    {
        if (!isDead && !playerPaused && !isPlayerFrozen)
        {

            #region DEBUG INPUTS
            if (Keyboard.current.uKey.wasPressedThisFrame) //used for debugging
            {
                Debug.Log("Trying to shake!");
               //s Vector3 shakeVect = new Vector3(horizontal, vertical, 0f);
                //boostSource.GenerateImpulse(movementVelValue);
                boostSource.GenerateImpulse();
                // KillPLayer();
            }
            //if (Keyboard.current.iKey.wasPressedThisFrame)
            //{
            //    SaveManager.instance.DeleteSave();
            //}
            #endregion

            if (wallChecker.boxCollider2D.enabled== false)
            {
                wallChecker.ReEnableChecker();
                isReversing = false;
            }

           // ReadDirectionInputs();       //where should this go?
            SetMovementVel();
          
        }
    }
    public void RotateBody(float angleToRotate)
    {
        playerBody.transform.rotation = Quaternion.Euler(0, 0, angleToRotate);
        playerAnimator.SetFloat("heroDirection", angleToRotate);
    }

    //This controls when the inputs are checked.
    //public void ReadDirectionInputs()
    //{
    //    if (canTurn && !swordSlashing && !isBouncingOffWall && canControlPlayer)
    //    {
    //        //bool directionChanged = CheckDirectionInput();
    //        bool directionChanged = NewCheckDirectionInput();
    //        Debug.Log("Horz= " + horizontal + "Vert = " + vertical);
    //       // if (directionChanged)
    //        {
    //            //delay from changing direction?
    //           // StartCoroutine(DirectionChangeDelay(0.3f)); //might need to be in fixedUpdate maybe?
    //            // playerSword.SwordSpark();

    //        //Move box?
    //        }
    //    }

    //}


    public void SetMovementVel()
    {
        if (currentMoveState.Equals(PlayerMoveStates.moving))
        {
            movementVelValue = new Vector2(horizontal * currentRunSpeed, vertical * currentRunSpeed);
        }
        else if (currentMoveState.Equals(PlayerMoveStates.boosting))
        {
            movementVelValue = new Vector2(horizontal * (currentRunSpeed + boostSpeedModifier), vertical * (currentRunSpeed + boostSpeedModifier));
        }
      
    }
    /// <summary>
    /// Sets the PlayerDirection based on the player's inputs. 
    /// </summary>
    /// <returns> if the direction is different, it returns directionchanged as true </returns>
    /// 
    //public bool CheckIfLastDirectionIsDifferent(PlayerDirection currentDir, PlayerDirection lastDir)
    //{
    //    bool directionWasChanged = false;
    //    if ((int)lastDir == (int)currentDir)
    //    {
    //        directionWasChanged = true;
    //    }
    //    return directionWasChanged;
    //}
    //public bool NewerCheckDirectionInput()
    //{
    //    bool directionChanged = false;
    //    //need to make it so if you are holding two keys and let go of one of them, the other key takes over

    //    if (!lastplayerDirection.Equals(PlayerDirection.right) && inputX == 1 && inputY == 0)
    //    {
    //        if (!lastplayerDirection.Equals(PlayerDirection.left))
    //        {
    //            SetFacingDirection(PlayerDirection.right);
    //            directionChanged = true;
    //        }

    //    }
    //    else if (!lastplayerDirection.Equals(PlayerDirection.up) && inputX == 0 && inputY > 0)
    //    {
    //        if (!lastplayerDirection.Equals(PlayerDirection.down))
    //        {
    //            SetFacingDirection(PlayerDirection.up);
    //            directionChanged = true;
    //        }
    //    }
    //    else if (!lastplayerDirection.Equals(PlayerDirection.left) && inputX < 0 && inputY == 0)
    //    {
    //        if (!lastplayerDirection.Equals(PlayerDirection.right))
    //        {
    //            SetFacingDirection(PlayerDirection.left);
    //            directionChanged = true;
    //        }
    //    }
    //    else if (!lastplayerDirection.Equals(PlayerDirection.down) && inputX == 0 && inputY < 0)
    //    {
    //        if (!lastplayerDirection.Equals(PlayerDirection.up))
    //        {
    //            SetFacingDirection(PlayerDirection.down);
    //            directionChanged = true;
    //        }
    //    }

    //    debugUI.lastDirection.text = lastplayerDirection.ToString();
    //    return directionChanged;
    //}


    //public bool NewCheckDirectionInput()
    //{
    //    bool directionChanged = false;
    //    //need to make it so if you are holding two keys and let go of one of them, the other key takes over

    //    if (!lastplayerDirection.Equals(PlayerDirection.right) && inputX == 1 && inputY == 0)
    //    {
    //        if (!lastplayerDirection.Equals(PlayerDirection.left))
    //        {
    //            SetFacingDirection(PlayerDirection.right);
    //            directionChanged = true;
    //        }

    //    }
    //    else if (!lastplayerDirection.Equals(PlayerDirection.up) && inputX == 0 && inputY > 0)
    //    {
    //        if (!lastplayerDirection.Equals(PlayerDirection.down))
    //        {
    //            SetFacingDirection(PlayerDirection.up);
    //            directionChanged = true;
    //        }
    //    }
    //    else if (!lastplayerDirection.Equals(PlayerDirection.left) && inputX < 0 && inputY == 0)
    //    {
    //        if (!lastplayerDirection.Equals(PlayerDirection.right))
    //        {
    //            SetFacingDirection(PlayerDirection.left);
    //            directionChanged = true;
    //        }
    //    }
    //    else if (!lastplayerDirection.Equals(PlayerDirection.down) && inputX == 0 && inputY < 0)
    //    {
    //        if (!lastplayerDirection.Equals(PlayerDirection.up))
    //        {
    //            SetFacingDirection(PlayerDirection.down);
    //            directionChanged = true;
    //        }
    //    }
     
    //    debugUI.lastDirection.text = lastplayerDirection.ToString();
    //    return directionChanged;
    //}
    //public bool CheckDirectionInput()
    //{
    //    bool directionChanged = false;
    //    //need to make it so if you are holding two keys and let go of one of them, the other key takes over
    //    if (!Keyboard.current.anyKey.isPressed)
    //    {
    //      //  SetFacingDirection(lastplayerDirection);


    //    }
    //    else
    //    {
    //        if (!lastplayerDirection.Equals(PlayerDirection.right) && Keyboard.current.rightArrowKey.wasPressedThisFrame)
    //        {
    //            if (!lastplayerDirection.Equals(PlayerDirection.left))
    //            {
    //                SetFacingDirection(PlayerDirection.right);
    //                directionChanged = true;
    //            }

    //        }
    //        else if (!lastplayerDirection.Equals(PlayerDirection.up) && Keyboard.current.upArrowKey.wasPressedThisFrame)
    //        {
    //            if (!lastplayerDirection.Equals(PlayerDirection.down))
    //            {
    //                SetFacingDirection(PlayerDirection.up);
    //                directionChanged = true;
    //            }
    //        }
    //        else if (!lastplayerDirection.Equals(PlayerDirection.left) && Keyboard.current.leftArrowKey.wasPressedThisFrame)
    //        {
    //            if (!lastplayerDirection.Equals(PlayerDirection.right))
    //            {
    //                SetFacingDirection(PlayerDirection.left);
    //                directionChanged = true;
    //            }
    //        }
    //        else if (!lastplayerDirection.Equals(PlayerDirection.down) && Keyboard.current.downArrowKey.wasPressedThisFrame)
    //        {
    //            if (!lastplayerDirection.Equals(PlayerDirection.up))
    //            {
    //                SetFacingDirection(PlayerDirection.down);
    //                directionChanged = true;
    //            }
    //        }
    //    }
    
    //    return directionChanged;
    //}
    //might replace logic in other function?


    /// <summary>
    /// Used by CheckDirectioninput to actually set specific direction parameters
    /// </summary>
    /// <param name="direction"></param>
    //public void SetFacingDirection(PlayerDirection direction)
    //{
    //    if (direction.Equals(PlayerDirection.right))
    //    {
    //        lastplayerDirection = PlayerDirection.right;
    //        horizontal = 1; vertical = 0;
    //        currentFacingAngle = 0;
    //    }
    //    else if(direction.Equals(PlayerDirection.left))
    //    {
    //        lastplayerDirection = PlayerDirection.left;
    //        horizontal = -1; vertical = 0;
    //        currentFacingAngle = 180;
      
    //    }
    //    else if(direction.Equals(PlayerDirection.down))
    //    {
    //        lastplayerDirection = PlayerDirection.down;
    //        horizontal = 0; vertical = -1;
    //        currentFacingAngle = 270;
      
    //    }
    //    else if (direction.Equals(PlayerDirection.up))
    //    {
    //        lastplayerDirection = PlayerDirection.up;
    //        horizontal = 0; vertical = 1;
    //        currentFacingAngle = 90;
    //    }
    //    AudioManager.instance.Play("ChangeDirection");
  
    //    SetLungeAnimation(currentFacingAngle);
    //    //
    //    RotateBody(currentFacingAngle);
    //    wallChecker.TemporarilyDisableChecker();
    //}

    public void ReverseDirection()
    {
        if (lastMove.x == -1 && lastMove.y == 0)
        {
            SetFacingDirection(1,0);
        }
        else if (lastMove.x == 0 && lastMove.y == 1)
        {
            SetFacingDirection(0,-1);
        }
        else if (lastMove.x == 0 && lastMove.y == -1)
        {
            SetFacingDirection(0,1);
        }
        else if (lastMove.x == 1 && lastMove.y == 0)
        {
            SetFacingDirection(-1,0);
        }
      //  wallChecker.TemporarilyDisableChecker();
       // isBouncingOffWall = true;

    }
    public void FixedUpdate()
    {
        if (currentMoveState.Equals(PlayerMoveStates.moving) || currentMoveState.Equals(PlayerMoveStates.boosting))
        {
            if (!isStunned && !isDead && !isPlayerFrozen)
            {
                playerRb.velocity = movementVelValue; //might instead set when changing facing directions
            }
            
        }   
    }

    public void Move(InputAction.CallbackContext context) //gets the input fom the input system
    {
        
        float inputX = context.ReadValue<Vector2>().x;
        float inputY = context.ReadValue<Vector2>().y;
        
        if (canTurn && !swordSlashing && !isBouncingOffWall && canControlPlayer)
        {

            //bool directionChanged = CheckDirectionInput();
             GamerUpdate(inputX, inputY);
           // SetFacingDirection(inputX, inputY);
            //bool directionChanged = NewCheckDirectionInput();
        }

        debugUI.xDebugDisplay.text = "xinput= " + inputX.ToString();
        debugUI.yDebugDisplay.text = "yinput= " + inputY.ToString();

    }
    #region SPECIAL MOVES

    
    public void BoostInput(InputAction.CallbackContext context)
    {
        if (!currentMoveState.Equals(PlayerMoveStates.boosting) && canControlPlayer && !isDead &&!isStunned && !playerPaused &&!isPlayerFrozen)
        {
            if (context.performed)
            {
                if (canBoost)
                {
                    BeginBoost(boostDuration, true);
                    //Gamepad.current.SetMotorSpeeds(0.25f, 0.75f);
                }
                else
                {
                    Debug.Log("Can't boost!");
                }
            }
        }
    }
    public void SwingSFX()
    {
        AudioManager.instance.Play("Slash");
    }
    public void SlashInput(InputAction.CallbackContext context)
    {
        if (context.performed && canControlPlayer && !isDead && !isStunned && !playerPaused && !isPlayerFrozen)
        {
                playerAnimator.SetTrigger("slashAttack");
        }
    }
    public IEnumerator DirectionChangeDelay(float delayTime)
    {
        canTurn = false;
        yield return new WaitForSeconds(delayTime);
        canTurn = true;
    }

    //differentiate between entering with boost panel and inputting manually
    public void BeginBoost(float boostingDuration, bool spendStamina)
    {
        CeaseRoutine(exhaustBoost);
        //CeaseRoutine(boostCooldown); //not sure fi this should be here
        GameObject boostStartEffect = Instantiate(ChargeReleaseEffect, transform.position, ChargeReleaseEffect.transform.rotation);
        boostSource.GenerateImpulse();
        Destroy(boostStartEffect, boostStartEffect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
        dashTrail.SetEnabled(true);
        currentMoveState = PlayerMoveStates.boosting;
        StartCoroutine(exhaustBoost = SlowFromBoost(boostingDuration));
        if (spendStamina)
        {
            canBoost = false;
            StartCoroutine(boostCooldown = BoostCooldown(boostCooldownTime));
        }
        AudioManager.instance.Play("Boost");
  
        playerSword.swordBoosting = true;
        playerAnimator.SetBool("heroBoosting", true);
        //should go after the last horizontal/vertical
    }

    public IEnumerator SlowFromBoost(float boostStateDuration) //exit this if player has another state
    {    
        yield return new WaitForSeconds(boostStateDuration);

        ExitBoost();
    }
    public void ExitBoost()
    {
        playerAnimator.SetBool("heroBoosting", false);
        currentMoveState = PlayerMoveStates.moving;
        dashTrail.SetEnabled(false);
        playerSword.swordBoosting = false;
    }
    public IEnumerator BoostCooldown(float coolDown)
    {
        float currentTime = 0;
        float cooldownRate = 1 / (coolDown/Time.deltaTime);

        while (currentTime<1)
        {
            testUI.SetStaminaSlider(currentTime);
            cooldownRate = 1 / (coolDown / Time.deltaTime);
            currentTime += cooldownRate;
            yield return null;
        }
        AudioManager.instance.Play("BoostMeterFull");
        canBoost = true;
    }
    #endregion

   

    #region DAMAGE
    public void HurtPlayer(GameObject dangerObj)
    {
        if (!isDead && !isInvincible)
        {
            currentPlayerHP -= 1; //might get value from dangerObj IDK
            Debug.Log("Owch! Player health: " + currentPlayerHP);
            swordSlashing = false; //breaks out of slash?
            LevelManager.instance.ManagerUpdateHud(currentPlayerHP);
            AudioManager.instance.Play("PlayerTakeDamage");
            if (currentPlayerHP < 1)
            {
                KillPLayer();
            }
            else
            {
                StartCoroutine(HurtState(dangerObj.transform));
            }
        }
    }
    public void KillPLayer()
    {
        isDead = true;
        playerRb.velocity = Vector3.zero;
        LevelManager.instance.LevelDeathProcess();
    }

    public void KnockbackPlayer(Transform dangerObj)
    {
        float thrust = 5; //make variable 
        Vector2 forceDirection = transform.position - dangerObj.position;
        Vector2 force = forceDirection.normalized * thrust;
        playerRb.velocity = force;
    }
    public IEnumerator HurtState(Transform dangerObj)
    {
        isInvincible = true;
        ExitBoost();
        //Need to exit slash, too

        bodyFlash.Flash(false, 0.3f);
        swordFlash.Flash(false, 0.3f);
        float invincibleTime = 1.5f;
        //FreezePlayer(true);
        isStunned = true;
        KnockbackPlayer(dangerObj);
        yield return new WaitForSeconds(0.3f);
        isStunned = false;
        //FreezePlayer(false);//might set this back using the stun animation event instead, or maybe use an animation state
        float flashingTime = 0;
        while (flashingTime < invincibleTime)
        {
            bodyFlash.Flash(true, (invincibleTime / 3));
            swordFlash.Flash(true, (invincibleTime / 3));
            yield return new WaitForSeconds(invincibleTime / 3);
            flashingTime += (invincibleTime / 3);
        }
        //yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
        Debug.Log("Can be damaged again.");
        yield return null;
    }
    #endregion


 
    public void CeaseRoutine(IEnumerator enumerator)
    {
        if (enumerator != null)
        {
            StopCoroutine(enumerator);
        }
    }

    public void FreezePlayer(bool shouldFreeze)
    {
        isPlayerFrozen = shouldFreeze;
        if (shouldFreeze)
        {

            playerRb.velocity = new Vector3(0, 0, 0);
        }
    }

    bool wasMovingVertical= false; //if the player was previously moving vertically, determines diagonals
    Vector2 lastMove; //might be used basically in the same way as lastDirection
    public Vector2 DirectionConverter(float angleToConvert)
    {
        //transform.eulerAngles.z
        if (angleToConvert == 90f)
        {
            return new Vector2(0, 1);
        }
        else if (angleToConvert == 180)
        {
            return new Vector2(-1, 0);
        }
        else if (angleToConvert == 270)
        {
            return new Vector2(0, -1);
        }
        else
        {
            return new Vector2(1, 0);
        }
        
    }
    public void SetFacingDirection(float horzValue, float vertValue)
    {
        //if direction isn't different than the one you're going in now, don't bother changing it and playing the sound and stuff

        if (horzValue == 1 && vertical == 0)
        {

          //  horizontal = 1; vertical = 0;
            angleToFace = 0;
        }
        else if (horzValue == -1 && vertical == 0)
        {
           // horizontal = -1; vertical = 0;
            angleToFace = 180;

        }
        else if (horzValue == 0 && vertical == -1)
        {
           // horizontal = 0; vertical = -1;
            angleToFace = 270;

        }
        else if (horzValue == 0 && vertical == 1)
        {
           // horizontal = 0; vertical = 1;
            angleToFace = 90;
        }
        horizontal = horzValue; vertical = vertValue;
        if (horzValue != lastMove.x && vertValue != lastMove.y)
        {
            AudioManager.instance.Play("ChangeDirection");
            // i guess here?
        }
        RotateBody(angleToFace);
        wallChecker.TemporarilyDisableChecker();
        //only do this when rotating


    }
    //lastMove has only 4 possible values
    public Vector2 GetOppositeDirection(Vector2 ogDirection)
    {
        Vector2 opDirection = new Vector2(0,0);
        if (ogDirection.x == -1 && ogDirection.y == 0)
        {
            opDirection = new Vector2(1, 0);
        }
        else if (ogDirection.x == 0 && ogDirection.y == 1)
        {
            opDirection = new Vector2(0, -1);

        }
        else if (ogDirection.x == 0 && ogDirection.y == -1)
        {
            opDirection = new Vector2(0, 1);

        }
        else if (ogDirection.x == 1 && ogDirection.y == 0)
        {
            opDirection = new Vector2(-1, 0);
        }
        return opDirection;
    }
    void GamerUpdate(float xinput, float yinput)
    {
        bool isMovingHorizontal = Mathf.Abs(xinput) > 0.5f;
        bool isMovingVertical = Mathf.Abs(yinput) > 0.5f;

        //changes facing direction, sets lastmove to the new direction
        //only play sound if movement is different
        if (isMovingVertical && isMovingHorizontal)
        {
            //if player is pressing in both directions, look at if the last input was completely horizontal or vertical, and set the player moving with the method that wasn't there
            if (wasMovingVertical)
            { //
                SetFacingDirection(xinput, 0);
                lastMove = new Vector2(horizontal, 0f);
            }
            else
            {
                SetFacingDirection(0, yinput);
                lastMove = new Vector2(0f, vertical);
            }
        }
        //only changes wasMovingVertical status if not inputting diagonally.
        else if (isMovingHorizontal)
        {
            wasMovingVertical = false;
            SetFacingDirection(xinput, 0);

            lastMove = new Vector2(horizontal, 0f);
        }
        else if (isMovingVertical)
        {
            wasMovingVertical = true;
            SetFacingDirection(0, yinput);

            lastMove = new Vector2(0f, vertical);
        }
        else
        {
            //the player is not pressing anything
        }
    }
}
