using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "BulletHell/ShotTypes/BasicRingSurround")]
public class RingAroundPoint : AimedAttack
{

    //randomStartAngle?
    [SerializeField, Tooltip("How fast the bullets are")]
    private float bulletMoveSpeed;
    [SerializeField]
    private float initialAngle;
    [SerializeField]
    private float circleRadius;
    [SerializeField]
    private bool scaleWithBulletNumber, randomPositions;
    [SerializeField, Tooltip("Number of bullets used in a shot at one time.")]
    private int amountToFire = 1;
    public override void FireAimed(Vector2 firePoint, Vector2 aimPoint)
    {
        BulletPooler pooler = PoolHolder.Pooler;
        float radius;
        float circAngle;
        float refAngle;
        if (scaleWithBulletNumber)
        {
            radius = amountToFire;
        }
        else
        {
            radius = circleRadius;
        }
        if (randomPositions)
        {
            refAngle = Random.Range(0, 360);
        }
        else
        {
            refAngle = initialAngle;
        }
        for (int j = 0; j < amountToFire; j++)
        {

            circAngle = (j * Mathf.PI * 2f / amountToFire) + refAngle;
            GameObject bullet = pooler.GrabBullet(bulletPrefab);

            Vector2 newPos = aimPoint + (new Vector2(Mathf.Cos(circAngle) * radius, Mathf.Sin(circAngle) * radius));
            Vector2 direction = aimPoint - newPos;
            float angle = Mathf.Atan2(direction.y, direction.x)//returns a value in radians--the angle 
                * Mathf.Rad2Deg; //multiply by this to convert to degrees
            if (bullet != null)
            {
                ShootBullet(bullet, newPos, angle, bulletMoveSpeed);
            }
           
        }
        PlayAttackSFX();
    }

}
