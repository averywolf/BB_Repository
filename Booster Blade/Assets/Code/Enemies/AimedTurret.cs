using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimedTurret : MonoBehaviour
{
    public AimedAttack aimedAttack;
    public Transform turretFirePoint;

    public bool startFiringAutomatically = true;
    private bool turretPrimed = false;

    private bool isShooting = false;
    // Start is called before the first frame update
    private Transform playerTransform;


    private void Start()
    {
        playerTransform = LevelManager.instance.GetPlayerTransform();
    }
    public void TurretWake()
    {
        turretPrimed = true;
        if (startFiringAutomatically)
        {
            StartBlasting();
        }
    }
    // Update is called once per frame

    public IEnumerator RepeatTurret()
    {
        float fireRate = aimedAttack.GetRateOfFire();
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        while (true)
        {
            aimedAttack.FireAimed(turretFirePoint.position, playerTransform.position);
            for (float duration = fireRate; duration > 0; duration -= Time.fixedDeltaTime)
            {
                yield return waitForFixedUpdate;
            }
        }
    }
    public void StartBlasting()
    {
        if (turretPrimed)
        {
            if (!isShooting)
            {
                StartCoroutine(RepeatTurret());
                isShooting = true;
            }

        }

    }
    public void StopBlasting()
    {
        StopAllCoroutines();
        isShooting = false;
    }
}
