﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedTurret : MonoBehaviour
{
    public FixedAttack fixedAttack;
    public Transform turretFirePoint;

    public bool startFiringAutomatically = true;
    private bool turretPrimed = false;

    private bool isShooting = false;
    // Start is called before the first frame update

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
        float fireRate = fixedAttack.GetRateOfFire();
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        while (true)
        {
            // AudioManager.instance.Play("Shoot1");
            fixedAttack.Fire(turretFirePoint);
            for (float duration = fireRate; duration > 0; duration -= Time.fixedDeltaTime)
            {
                yield return waitForFixedUpdate;
            }
        }
    }
    public void StartBlasting()
    {
        if (!isShooting)
        {
            StartCoroutine(RepeatTurret());
            isShooting = true;
        }
   
    }
}
