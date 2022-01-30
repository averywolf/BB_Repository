using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "BulletHell/ShotTypes/AimSingleShot")]
public class AimSingleShot : AimedAttack
{
    [SerializeField, Tooltip("Bullet starting velocity")]
    private float bulletMoveSpeed;

    [SerializeField]
    private float shotOffset;
    [SerializeField]
    private float angleVariance;
    [SerializeField]
    private float speedVariance;

    //variable for variance?

    public override void FireAimed(Vector2 firePoint, Vector2 aimPoint)
    {
        BulletPooler pooler = PoolHolder.Pooler;
        Vector2 direction = aimPoint - firePoint; //worried about Vector3
        float angle = Mathf.Atan2(direction.y, direction.x)//returns a value in radians--the angle 
            * Mathf.Rad2Deg; //multiply by this to convert to degrees
        float angleModifier = shotOffset + Random.Range(-angleVariance, angleVariance);
        float speedModifier = Random.Range(-speedVariance, speedVariance);
        GameObject bullet = pooler.GrabBullet(bulletPrefab);
        if (bullet)
        {
            ShootBullet(bullet, firePoint, (angle+ angleModifier), bulletMoveSpeed + speedModifier);
            PlayAttackSFX();
        }
    }
}
