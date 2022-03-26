using System.Collections;
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



    [HideInInspector]
    public bool limitersRemoved = false; //set to true by LevelManager if it's the final stage

    private IEnumerator boostCooldownCou;
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
       // playerAnimator.Play("hero_lungeUp");
        //SetLungeAnimation(currentFacingAngle);
    }

    private void Update()
    {
        if (!playerPaused && !isPlayerFrozen)
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
        
        if (!isDead && canTurn && !swordSlashing && !isBouncingOffWall && canControlPlayer &&!playerPaused)
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
    //should come out of a boost panel with reduced cooldown

    //differentiate between entering with boost panel and inputting manually
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
        ///Might need to deplete bar instantly here; this is called if player bumps into enemy while boosting
        /// There could potentially be a strat were
        playerAnimator.SetBool("heroBoosting", false);
        currentMoveState = PlayerMoveStates.moving;
        //dashTrail.SetEnabled(false);
        dashTrail.testDashBoosting = false;
        playerSword.swordBoosting = false;
        
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
        float cooldownRate = 1 / (coolDown/Time.fixedDeltaTime);

        while (currentTime<1)
        {
            testUI.SetStaminaSlider(currentTime);
            cooldownRate = 1 / (coolDown / Time.fixedDeltaTime);
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
        playerAnimator.Play("heroDeath");
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


}
