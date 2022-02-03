using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "BulletHell/ShotTypes/RandomSpread")]
public class RandomSpread : FixedAttack
{
    [SerializeField]
    private float averageBulletMoveSpeed;
    [SerializeField]
    private float moveVariance;
    [SerializeField]
    private float angleToAimTo;
    [SerializeField]
    private float angleVariance;
    [SerializeField]
    [Tooltip("Number of bullets used in a shot at one time.")]
    private int amountToFire;
    //how do I write notes in inspector? moveVariance can't be greatr than averageBulletMoveSpeed
    [SerializeField]
    //private float startAngle = 90f, endAngle = 270f;
    private float angle;
    public override void Fire(Vector2 firePoint)
    {
        BulletPooler pooler = PoolHolder.Pooler;
        for (int i = 0; i < amountToFire + 1; i++) //check if the +1 is necessary
        {

            GameObject bullet = pooler.GrabBullet(bulletPrefab);
            angle = Random.Range(angleToAimTo + angleVariance, angleToAimTo - angleVariance);
            if (bullet != null)
            {
                //bullet.GetComponent<BulletMove>().SetInitialVelocity(Random.Range(averageBulletMoveSpeed - moveVariance, averageBulletMoveSpeed + moveVariance));
                ShootBullet(bullet, firePoint, angle, Random.Range(averageBulletMoveSpeed - moveVariance, averageBulletMoveSpeed + moveVariance));
            }
        }
        PlayAttackSFX();
    }
    
}
