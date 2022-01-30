using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    /// <summary>
    /// Attached to all standard bullets. Most of the BulletActions are called here to make the bullet move in different ways.
    /// </summary>

    [SerializeField, Tooltip("Make sure this value is set to the scale you want your bullet--this sets the default size in game.")]
    protected float startScale = 1;

    private bool spinning = false;
    private float spinSpeed = 0;
    float zRotation = 0;
    private bool accelerating = false;
    protected float initialVelocity;
    protected float baseVelocity;

    IEnumerator accelRoutine;
    IEnumerator spinRoutine;
    protected Rigidbody2D bulRb;
    protected Transform bulTransform;
    BulletBehavior bulletBehavior;
    // Initialization
    private void Awake()
    {
        StandardInitialization();
    }

    // Gets the bullet moving with default values
    private void OnEnable()
    {
        bulTransform.localScale = new Vector3(startScale, startScale, 1f);
        StandardSetup();
        SetVelocity(initialVelocity);
    }


    protected void StandardInitialization()
    {
        bulRb = GetComponent<Rigidbody2D>();
        bulletBehavior = GetComponent<BulletBehavior>();
        bulTransform = transform;
    }
    protected void StandardSetup()
    {
        zRotation = bulTransform.localEulerAngles.z;
        spinning = false;
        accelerating = false;
        bulletBehavior.BeginBulletProcesses();
    }
    protected void OnDisable()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// Used by StandardAttack to set up the speed of the bullet before firing
    /// </summary>
    /// <param name="value"></param>//do I really need a separate method for this?
    public virtual void SetInitialVelocity(float value)
    {
        initialVelocity = value;
    }

    /// <summary>
    /// Unlike SetInitialVelocity this is used by BulletBehavior to set speed directly after being fired
    /// </summary>
    /// <param name="value"></param>
    public void SetVelocity(float value)
    {
        bulRb.velocity = transform.right * value;
        baseVelocity = value;
    }

    /// <summary>
    /// Used by BulletBehavior to make the bullet spin. (Does not lerp, and there's no function to stop spinning/set angle)
    /// </summary>
    /// <param name="rotationSpeed"></param>
    public void StartSpinning(float rotationSpeed)
    {
        if (rotationSpeed == 0)
        {

            spinning = false;
        }
        else
        {
            spinning = true;
            spinSpeed = rotationSpeed;
        }


    }

    /// <summary>
    /// Called by BulletBehavior to face a target(currently just faces the player)
    /// Parameters should both be true at this stage, might update this function later to target more specifically
    /// </summary>
    /// <param name="facePlayer"></param>
    /// <param name="updateVelocity"></param>
    public void FaceTarget(bool facePlayer, bool updateVelocity)
    {
        if (facePlayer)
        {
            Vector3 direction = LevelManager.instance.GetPlayerTransform().position - bulTransform.position; //worried about Vector3
            float angle = Mathf.Atan2(direction.y, direction.x)//returns a value in radians--the angle 
                * Mathf.Rad2Deg; //multiply by this to convert to degrees
            bulTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
        // Automatically gets bullet moving in the correct direction again, now that facing angle has changed
        if (updateVelocity)
        {
            bulRb.velocity = transform.right * baseVelocity;
        }
        spinning = false;
    }

    /// <summary>
    /// Only sets velocity when necessary to change it (continuously accelerating or spinning)
    /// </summary>
    protected void FixedUpdate()
    {
        if (accelerating || spinning)
        {
            if (spinning)
            {
                zRotation += Time.fixedDeltaTime * spinSpeed;
                bulTransform.rotation = Quaternion.Euler(0, 0, zRotation);
                //maybe only apply changing velocity here instead of in methods like SetVelocity                
            }
            bulRb.velocity = transform.right * baseVelocity;
        }
    }


    /// <summary>
    /// Lerps the bullet's scale from the scale at the start to the specified value.
    /// Scales evenly, might need a function for special bullets that "stretch."
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="endScale"></param>
    /// <returns></returns>
    public IEnumerator ScaleOverTime(float duration, float endScale)
    {
        float currentTime = 0.0f;
        Vector3 originalScale = bulTransform.localScale;
        Vector3 destinationScale = new Vector3(endScale, endScale, 1f);

        while (currentTime <= duration)
        {
            bulTransform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / duration);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// BulletBehavior uses this to accelerate. You can only lerp velocity to a specified point (no function to keep going) but that should still be fine.
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="endVelocity"></param>
    /// <returns></returns>
    public IEnumerator LerpVelocity(float duration, float endVelocity)
    {
        accelerating = true;
        float currentTime = 0.0f;
        float currentVelocity = baseVelocity;
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

        while (currentTime <= duration)
        {
            currentTime += Time.fixedDeltaTime;
            baseVelocity = Mathf.Lerp(currentVelocity, endVelocity, currentTime / duration);
            yield return waitForFixedUpdate;
        }
        accelerating = false;
    }
}
