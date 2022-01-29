using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    /// <summary>
    /// Dictates the behavior the bullet takes after being launched by StandardAttacks.
    /// StandardAttack specifies the BulletKeyPoints that have "BulletActions" associated with them
    /// </summary>
    /// 

    [SerializeField, Tooltip("Specify which part of the bullet will change color")]
    private SpriteRenderer spriteRenderer;

    // Set directly by StandardAttack, should normally be empty when looking at the prefab
    public List<BulletKeyPoint> bulletKeyPoints;

    BulletMovement bulletMovement;

    private void Awake()
    {
        bulletMovement = GetComponent<BulletMovement>();
    }

    /// <summary>
    /// Starts the bullet behavior, called by BulletMovement directly after setting initial speed.
    /// </summary>
    public void BeginBulletProcesses()
    {
        // Only start if there's actually bullet actions
        if (bulletKeyPoints.Count > 0)
        {
            StartCoroutine(BulletProcesses());
        }
    }

    public void OnDisable()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// Used by StandardAttack to set the color of the specified spriterenderer.
    /// This allows a certain part of the bullet to have color, with the "core" being a different color.
    /// </summary>
    /// <param name="bulletColor"></param>
    public void SetBulletColor(Color bulletColor)
    {
        spriteRenderer.color = bulletColor;
        //...apparently you can get the grayscale versions of a color?
    }

    /// <summary>
    /// Goes through the list of BulletKeyPoints one by one, with a delay between them
    /// </summary>
    /// <returns></returns>
    public IEnumerator BulletProcesses()
    {
        int currentKeyPoint = 0;
        while (currentKeyPoint < bulletKeyPoints.Count)
        {
            float delay = bulletKeyPoints[currentKeyPoint].keyPointDelay;
            YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
            for (float duration = delay; duration >= 0; duration -= Time.fixedDeltaTime)
            {
                yield return waitForFixedUpdate;
            }
            DoBulletAction(bulletKeyPoints[currentKeyPoint].bulletAction);
            currentKeyPoint++;
        }
    }

    /// <summary>
    /// Does the appropriate bullet action specified by the BulletAction enum at the action in BulletKeyPoint.
    /// Looks at the BulletAction's parameters when calling those functions.
    /// </summary>
    /// <param name="bulletAction"></param>
    private void DoBulletAction(BulletAction bulletAction)
    {
        BulletAction.BulActionType actionType = bulletAction.GetActionType();
        if (actionType.Equals(BulletAction.BulActionType.setVelocity))
        {
            bulletMovement.SetVelocity(bulletAction.setSpeed);
        }
        else if (actionType.Equals(BulletAction.BulActionType.faceTarget))
        {
            bulletMovement.FaceTarget(true, true);
        }
        else if (actionType.Equals(BulletAction.BulActionType.spinOverTime))
        {
            bulletMovement.StartSpinning(bulletAction.spinRate);
        }
        else if (actionType.Equals(BulletAction.BulActionType.scaleOverTime))
        {
            bulletMovement.StartCoroutine(bulletMovement.ScaleOverTime(bulletAction.scaleDuration, bulletAction.endScale));
        }
        else if (actionType.Equals(BulletAction.BulActionType.changeSpeedOverTime))
        {
            bulletMovement.StartCoroutine(bulletMovement.LerpVelocity(bulletAction.accelDuration, bulletAction.endSpeed));
        }
        else if (actionType.Equals(BulletAction.BulActionType.triggerAttack))
        {
            AttackPattern atPattern = gameObject.GetComponent<AttackPattern>();
            if (atPattern != null)
            {
                atPattern.ActivatePattern();
            }
        }
    }
}
