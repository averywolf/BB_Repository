using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvergrownCore : MonoBehaviour
{
    public List<VineWeakpoint> weakpoints;
    public FixedAttack fixedAttack;
    
    public void PlantWake()
    {
        StartCoroutine(RepeatTurret());
    }
    public void Update()
    {
        for (int i = 0; i < weakpoints.Count; i++)
        {
            if (!weakpoints[i].isCut)
            {
                return;
            }
        }
        KillPlant();
    }
    public IEnumerator RepeatTurret()
    {

        float fireRate = fixedAttack.GetRateOfFire();
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        while (true)
        {
            // AudioManager.instance.Play("Shoot1");
            fixedAttack.Fire(transform);
            for (float duration = fireRate; duration > 0; duration -= Time.fixedDeltaTime)
            {
                yield return waitForFixedUpdate;
            }
        }
    }
    public void KillPlant()
    {
        Destroy(gameObject);
    }
}
