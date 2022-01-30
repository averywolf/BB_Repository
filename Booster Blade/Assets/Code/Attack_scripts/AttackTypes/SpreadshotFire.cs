using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "BulletHell/ShotTypes/SpreadshotFire")]
public class SpreadshotFire : AimedAttack
{

    [SerializeField]
    private float spreadShotRange;
    [SerializeField]
    private float bulletMoveSpeed;

    private float angle;
    [SerializeField, Tooltip("Number of bullets used in a shot at one time.")]
    private int amountToFire = 1;

    //get a reference to player in scene?
    //set up bullet script?
    public override void FireAimed(Vector2 firePoint, Vector2 aimPoint)
    {
        BulletPooler bulletPooler = PoolHolder.Pooler;
        Vector2 direction = aimPoint - firePoint;
        float angle = Mathf.Atan2(direction.y, direction.x)//returns a value in radians--the angle 
            * Mathf.Rad2Deg; //multiply by this to convert to degrees

        float startAngle = (angle) - (spreadShotRange / 2);
        float angleIncrem = spreadShotRange / (amountToFire - 1);
        for (int i = 0; i <= amountToFire - 1; i++)
        {
            GameObject bullet = bulletPooler.GrabBullet(bulletPrefab); //it appears necessarily to get a reference WITHIN the for-loop from the pool

            if (bullet != null)
            {
                bullet.transform.position = firePoint;
                float fireAngle = startAngle;

                if (amountToFire > 1)
                {
                    fireAngle= startAngle + (i * angleIncrem);
                }
                ShootBullet(bullet, firePoint, fireAngle, bulletMoveSpeed);
            }

        }
        PlayAttackSFX();
    }
   

}
