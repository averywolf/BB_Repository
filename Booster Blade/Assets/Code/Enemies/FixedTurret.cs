using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedTurret : MonoBehaviour
{
    public FixedAttack fixedAttack;
    public Transform turretFirePoint;

    // Start is called before the first frame update

    public void TurretWake()
    {
        StartCoroutine(RepeatTurret());
    }
    // Update is called once per frame

    public IEnumerator RepeatTurret()
    {
        Debug.Log("So I started blasting...");
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
}
