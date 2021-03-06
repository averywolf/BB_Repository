using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "BulletHell/ShotTypes/CircleSpread")]
public class CircleSpread : FixedAttack
{
    // Start is called before the first frame update
    [SerializeField, Tooltip("How fast the bullets are")]
    private float bulletMoveSpeed;
    [SerializeField, Tooltip("Number of bullets used in a shot at one time.")]
    private int amountToFire = 1;
    [SerializeField]
    private float initialAngle;
    [SerializeField]
    private bool randomizeAngle;
    public override void Fire(Vector2 firePoint)
    {
        float angleStart = initialAngle;
        if (randomizeAngle)
        {
            angleStart = Random.Range(0, 360);
        }
        CircleAttack(angleStart, firePoint);

    }
    public void CircleAttack(float angleStart, Vector2 firePoint)
    {
        float angleIncrements = 360 / amountToFire;
        BulletPooler pooler = PoolHolder.Pooler;
        for (int j = 0; j < amountToFire; j++)
        {

            GameObject bullet = pooler.GrabBullet(bulletPrefab);
            if (bullet != null)
            {
                float angle = j * angleIncrements + angleStart;
                ShootBullet(bullet, firePoint, angle, bulletMoveSpeed);
            }
        }
        PlayAttackSFX();
    }
    public override void Fire(Transform firePoint)
    {//unsure if quaternions ruin this
        if (randomizeAngle)
        {
            Fire(firePoint.position);
        }
        else
        {
            CircleAttack(firePoint.rotation.z, firePoint.position);
        }
    

    }

}
