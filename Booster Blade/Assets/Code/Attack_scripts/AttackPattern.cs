using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern : MonoBehaviour
{
    /// <summary>
    /// Attached to some bullets, and other "drones" as well.
    /// Bullets can call this script on collision or after a certain time using BulletBehavior.
    /// ActivatePattern() can be called by anything that has a reference to the bullet.
    /// This is the default behavior.
    /// </summary>

    //These variables are not set by Attack and need to be prefab-specific
    public bool deactivateAfterFiring;

    public FixedAttack fixedAttack;

    public void ActivatePattern()
    {
        fixedAttack.Fire(transform.position);
        if (deactivateAfterFiring)
        {
            gameObject.SetActive(false);
        }
    }
}
