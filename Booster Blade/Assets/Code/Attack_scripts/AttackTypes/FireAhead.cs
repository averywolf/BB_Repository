using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "BulletHell/ShotTypes/FireAhead")]
public class FireAhead : FixedAttack
{
    // Simple attack that fires a single bullet in the direction the firePoint is facing.
    // Make sure to make firePoint equal to the transform of the object.


        /// <summary>
        /// TEMPORARILY BROKEN
        /// </summary>
    [SerializeField, Tooltip("Bullet starting velocity")]
    private float bulletMoveSpeed;
    //DON"T USE THIS ONE
    public override void Fire(Vector2 firePoint)
    {
        BulletPooler pooler = PoolHolder.Pooler;
        GameObject bullet = pooler.GrabBullet(bulletPrefab);
        if (bullet != null)
        {
            bullet.transform.position = firePoint;
            bullet.transform.rotation = Quaternion.Euler(0, 0, 0);
            ShootBulletOld(bullet, bulletMoveSpeed);
            PlayAttackSFX();
        }
    }   
    public override void Fire(Transform firePoint)
    {
        BulletPooler pooler = PoolHolder.Pooler;
        GameObject bullet = pooler.GrabBullet(bulletPrefab);
        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;
            /// bullet.transform.rotation = firePoint.rotation;
            ShootBullet(bullet, firePoint.position, firePoint.eulerAngles.z, bulletMoveSpeed);
            PlayAttackSFX();
        }
    }
    public void FireAtAngle(Vector2 firePoint, float shotAngle)
    {
        BulletPooler pooler = PoolHolder.Pooler;
        GameObject bullet = pooler.GrabBullet(bulletPrefab);
        if (bullet != null)
        {
            ShootBullet(bullet, firePoint, shotAngle, bulletMoveSpeed);
            PlayAttackSFX();
        }
    }

    //need specific method that takes angle to fire as a parameter
}
