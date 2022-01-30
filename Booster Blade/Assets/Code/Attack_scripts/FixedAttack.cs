using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FixedAttack : StandardAttack
{
    public virtual void Fire(Vector2 firePoint)
    {
        //PoolHolder.Pooler.GrabBullet();
        Debug.Log("The Shot being used does not have its own Fire() method.");
    }
    public virtual void Fire(Transform firePoint)
    {

    }

}
