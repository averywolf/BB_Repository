using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System;
using UnityEngine.EventSystems;
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
    [HideInInspector]
    public float horizontal;
    [HideInInspector]
    public float vertical;

    public bool testStun = false;
    //forget what exactly this is
    private Vector2 movementVelValue;
    [SerializeField]
    private DashTrail dashTrail;

    private float xEnvironmentMod;//represents outside forces on object like energy fields
    private float yEnvironmentMod;
    [HideInInspector]
    public GameObject playerBody;

    public GameObject ChargeReleaseEffect;
    private IEnumerator exhaustBoost; //after a certain time, exit boost and return to normal movement
    private IEnumerator boostCooldown; // can't spam charge over and over

    public Animator playerAnimator;
    [HideInInspector]
    public bool canControlPlayer = false;
    private bool isDead = false;
    public int currentPlayerHP;
   

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
    [HideInInspector]
    public bool isInvincible = false;

    public int numberOfMurderers = 0; //used to keep track of how many assassins are attacking
    #endregion

    [HideInInspector]
    public PlayerMoveStates currentMoveState = PlayerMoveStates.moving;
    public PlayerSword playerSword;
   
    private bool canTurn = true;

    private Vector2 initialturnpoint;

    private WallChecker wallChecker;

    private bool isBouncingOffWall = false;

    public static event Action OnPlayerTurn;
    private LevelManager levelManager;
    [HideInInspector]
    public bool isReversing = false;
    public bool playerPaused = false;
    //currently unsure if a distinction is necessary between freeze and paused. freeze is set outside of pause contexts
    private bool isPlayerFrozen = false;

    [HideInInspector]
    public bool swordSlashing=false;
    public CinemachineImpulseSource boostSource;

    [HideInInspector]
    public bool limitersRemoved = false; //set to true by LevelManager if it's the final stage

    private IEnumerator boostCooldownCou;
    [HideInInspector]
    public MonolithCore monolithCoreRef;//only relevant in final stage

    public CinemachineImpulseSource hurtShake;

    public float currentAngleForDeathAnim = 0;

    public bool spacepaused=false;
    private IEnumerator bladeReturn;
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
        playerPaused = false; //probably need to set values just like this
        levelManager = LevelManager.instance;
    }
    public void Start()
    {

        playerAnimator.SetBool("heroBoosting", false);
        playerSword.swordBoosting = false;
        isReversing = false;
    }

    private void Update()
    {
        if (!playerPaused && !isPlayerFrozen)
        {

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
        currentAngleForDeathAnim = angleToRotate;

    }


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
                if (spacepaused) //DELETE THIS
                {
                    playerRb.velocity = new Vector3(0, 0, 0);
                }
                else
                {
                    playerRb.velocity = movementVelValue; //might instead set when changing facing directions
                }
                
            }
        }   
    }

    public void Move(InputAction.CallbackContext context) //gets the input fom the input system
    {
        
        float inputX = context.ReadValue<Vector2>().x;
        float inputY = context.ReadValue<Vector2>().y;
        
        if (!isDead && canTurn && !swordSlashing && !isBouncingOffWall && canControlPlayer &&!playerPaused && !isPlayerFrozen) 
        {

            //bool directionChanged = CheckDirectionInput();
             ReadMoveInputs(inputX, inputY);

        }
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
                    //Gamepad.current.SetMotorSpeeds(0.25f, 0.75f); //could add gamepad rumble, potentially!
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

    //differentiates between entering with boost panel and inputting manually
    public void BeginBoost(float boostingDuration, bool manualBoost)
    {
        testUI.StaminaFlash(true); //if the bar isn't blue already, this makes it so
        CeaseRoutine(exhaustBoost);
        CeaseRoutine(boostCooldownCou); //exits cooldown immediately, this is important for boost panels so they recharge appropriately if player is in cooldown
        //CeaseRoutine(boostCooldown); //not sure fi this should be here
        GameObject boostStartEffect = Instantiate(ChargeReleaseEffect, transform.position, playerBody.transform.rotation, transform);
        boostSource.GenerateImpulse();
        Destroy(boostStartEffect, boostStartEffect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
        dashTrail.SetEnabled(true);
        dashTrail.testDashBoosting = true;
        currentMoveState = PlayerMoveStates.boosting;
        StartCoroutine(exhaustBoost = SlowFromBoost(boostingDuration, manualBoost));
  
        AudioManager.instance.Play("Boost");
  
        playerSword.swordBoosting = true;
        playerAnimator.SetBool("heroBoosting", true);
        //should go after the last horizontal/vertical
    }
    public void InstantFullMeter()
    {
        testUI.SetStaminaSlider(1);
        AudioManager.instance.Play("BoostMeterFull");
        testUI.StaminaFlash(true);
        canBoost = true;
    }

  

    //might need two guages to be layered on top of each other?
    public IEnumerator SlowFromBoost(float boostStateDuration, bool manualBoost) //exit this if player has another state
    {

        if (manualBoost)
        {
            canBoost = false; //does this even do anything?
        }
        float currentTime = 1;
        float boostLastRate = 1 / (boostStateDuration / Time.deltaTime);

        while (currentTime > 0)
        {
            testUI.SetStaminaSlider(currentTime);
            boostLastRate = 1 / (boostStateDuration / Time.deltaTime);
            currentTime -= boostLastRate;
            yield return null;
        }

        //yield return new WaitForSeconds(boostStateDuration);

        ExitBoost();
        InitiateCooldown(manualBoost);
    }
    
    public void ExitBoost()
    {
        testUI.SetStaminaSlider(0);
        testUI.StaminaFlash(false);

        playerAnimator.SetBool("heroBoosting", false);
        currentMoveState = PlayerMoveStates.moving;
        //dashTrail.SetEnabled(false);
        dashTrail.testDashBoosting = false;
        playerSword.swordBoosting = false;
        
    }
    public void InterruptBoost()
    {
        if(currentMoveState.Equals (PlayerMoveStates.boosting))
        {
            if(exhaustBoost != null)
            {
                StopCoroutine(exhaustBoost); //might move over to ExitBoost

            }
            ExitBoost();
            InitiateCooldown(true);
        }
    }
    public void InitiateCooldown(bool manual)
    {
        if (limitersRemoved)
        {
            StartCoroutine(boostCooldownCou = BoostCooldown(0.2f));
        }
        else if (!manual)
        {
            InstantFullMeter();
        }
        else
        {
            StartCoroutine(boostCooldownCou = BoostCooldown(boostCooldownTime));
        }
    }
    public IEnumerator BoostCooldown(float coolDown)
    {
        
        float currentTime = 0;
        float cooldownRate = 1 / (coolDown/(Time.fixedDeltaTime* Time.timeScale));

        while (currentTime<1)
        {
            testUI.SetStaminaSlider(currentTime);
            cooldownRate = 1 / (coolDown / (Time.fixedDeltaTime * Time.timeScale));
            currentTime += cooldownRate;
            yield return null;
        }
        testUI.SetStaminaSlider(1);
        AudioManager.instance.Play("BoostMeterFull");
        testUI.StaminaFlash(true);
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
            hurtShake.GenerateImpulse();
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




        CeaseRoutine(bladeReturn); //should stop the blade return lunge at the end of the game if the player dies during the animation
        //trying thiz out
  //      transform.position = playerBody.transform;
        playerBody.transform.localPosition = new Vector2(0, 0);

        playerAnimator.Play("heroDeath");
        LevelManager.instance.LevelDeathProcess();
    }
    public void HealToFull()
    {
        currentPlayerHP = LevelManager.instance.startingPlayerHP;
        LevelManager.instance.ManagerUpdateHud(currentPlayerHP);
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
        InterruptBoost(); //might lead to some interesting behavior for speedruns
        //Need to exit slash, too (unsure if this comment is still relevant)

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
        yield return null;
    }
    #endregion
    public void SetDebugInvincibility(bool setTrue)
    {
        isInvincible = setTrue;
    }

 
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
    
    //Turns the player based on the specified direction, that's it
    //Used after PlayerController decides through logic if you're allowed to make a turn, or called instantly by other objects to make player face a certain way
    public void SetFacingDirection(float horzValue, float vertValue)
    {
        float newAngleToFace=0;
        //sets newAngleToFace based on the corresponding direction values
        if (horzValue == 1f && vertValue== 0f)
        {
          //  horizontal = 1; vertical = 0;
            newAngleToFace = 0;
        }
        else if (horzValue == -1f && vertValue == 0f)
        {
            newAngleToFace = 180;
        }
        else if (horzValue == 0 && vertValue == -1f)
        {
            // horizontal = 0; vertical = -1;
            newAngleToFace = 270;

        }
        else if (horzValue == 0f && vertValue == 1f)
        {
            // horizontal = 0; vertical = 1;
            newAngleToFace = 90;
        }
        RotateBody(newAngleToFace);
        debugUI.lastDirection.text = "angle: " + newAngleToFace;
        horizontal = horzValue; vertical = vertValue;
        lastMove = new Vector2(horzValue, vertValue);
        wallChecker.TemporarilyDisableChecker();
    }
    
    //I don't think this is used yet
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
    //should probably handle movement logic in here, huh. that way I can use SetPlayerDirection for everything
    void ReadMoveInputs(float xinput, float yinput)
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
               // SetFacingDirection(xinput, 0);
                AttemptTurn(xinput, 0);
            }
            else
            {
               // SetFacingDirection(0, yinput);
                AttemptTurn(0f, yinput);
            }
        }
        //only changes wasMovingVertical status if not inputting diagonally.
        else if (isMovingHorizontal)
        {
            wasMovingVertical = false;
            AttemptTurn(xinput, 0);
            //SetFacingDirection(xinput, 0);
        }
        else if (isMovingVertical)
        {
            wasMovingVertical = true;
            //SetFacingDirection(0, yinput);
            AttemptTurn(0f, yinput);
        }
        else
        {
            //the player is not pressing anything
        }
    }
    //Only called normally through reading inputs. Checks to see if player isn't trying to move in the opposite direction or the same direction, first.
    public void AttemptTurn(float horzVal, float vertVal)
    {
        debugUI.xDebugDisplay.text = "horzval= " + horzVal.ToString();
        debugUI.yDebugDisplay.text = "vertval " + vertVal.ToString();
        if (horzVal != lastMove.x && vertVal != lastMove.y)
        {
            SetFacingDirection(horzVal, vertVal);
            AudioManager.instance.Play("ChangeDirection");
        }
    }
    public void CommunicateBladeReturn()
    {
       //plays a sound
       //stops music
       //stops timer
    }
    public void CutsceneMode(Transform dest)
    {
        RotateBody(0); //makes sure they're facing right
        playerAnimator.Play("hero_return");
        FreezePlayer(true);
        dashTrail.SetEnabled(false);
        currentMoveState = PlayerMoveStates.idle;
        StartCoroutine(bladeReturn = MoveToPoint(dest.position, 0.1f)); //og= 4
    }

    public IEnumerator MoveToPoint(Vector2 destination, float timeSpeed)
    {
        Vector2 startingPoint = playerBody.transform.position;
        //while (true)
        //{
        //    //if (vector2.Distance(target.transform.position, transform.position) < 1)
        //        playerBody.transform.position = Vector2.MoveTowards(transform.position, destination, timeSpeed* Time.deltaTime);
        //    yield return null;
        //}

        //lerping time is weird
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * timeSpeed) //ficked?
        {
            playerBody.transform.position = Vector3.Lerp(startingPoint, destination, t);

            yield return null;
        }
        monolithCoreRef.InsertSword();

        //  transform.position = Vector2.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
    }

}
