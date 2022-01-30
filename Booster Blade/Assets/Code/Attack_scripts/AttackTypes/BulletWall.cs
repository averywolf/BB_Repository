using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "BulletHell/ShotTypes/BulletWall")]
public class BulletWall : StandardAttack
{
    [SerializeField]
    private float bulspacing;
    [SerializeField]
    private float bulletMoveSpeed;
    [SerializeField]
    private float offset;

    public float bulletAngle;
    public float wallAngle;

    public float incremRate;
    public int incremMax;//should be the same as amountToFire?

    private float bulletLine;
    [HideInInspector]
    public float testBulAmount;

    public GameObject FireWallIncrem(Vector2 p1, Vector2 p2, int i, float offset)
    {
        Vector3 bulletLine = p2 - p1;
        Vector3 lineDirection = bulletLine.normalized;

        float totalDistance = bulletLine.magnitude;

        BulletPooler pooler = PoolHolder.Pooler;
        int bulletNumber = (int)(((totalDistance / bulspacing)));
        GameObject bullet = pooler.GrabBullet(bulletPrefab);
        if (bullet != null)
        {
            PlaceWallBullet(p1, p2, bullet, i, offset);
            ShootBulletOld(bullet, bulletMoveSpeed);
            PlayAttackSFX();
            return bullet;
        }
        return null;
    }

    public void FireWallInstant(Vector2 p1, Vector2 p2, float offset)
    {
        Vector3 bulletLine = p2 - p1;
        Vector3 lineDirection = bulletLine.normalized;

        float totalDistance = bulletLine.magnitude;
        //float spacing = totalDistance / amountToFire;
        BulletPooler pooler = PoolHolder.Pooler;

        int bulletNumber= (int)(((totalDistance / bulspacing)));
        for (int i = 0; i <= bulletNumber; i++)
        {         
            GameObject bullet = pooler.GrabBullet(bulletPrefab);
            if(bullet != null)
            {
                PlaceWallBullet(p1, p2, bullet, i, offset);
                ShootBulletOld(bullet, bulletMoveSpeed);
            }
        }
        PlayAttackSFX();
    }

    private void PlaceWallBullet(Vector2 p1, Vector2 p2, GameObject bul, int val, float offsetValue)
    {
        Vector2 bulletLine = p2 - p1;
       
        Vector2 lineDirection = bulletLine.normalized;
        Vector2 offsetDirection= Rotate90CW(bulletLine).normalized;
        //Vector3 offsetDirection = Vector3.Cross(p1.position, p2.position).normalized;
        bul.transform.position = p1 + (lineDirection * (val*bulspacing)) + (offsetDirection*offsetValue);

        float perpAngle = (Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Mathf.PI) + 90f;
        bul.transform.rotation = Quaternion.Euler(0, 0, perpAngle);

    }
    Vector3 Rotate90CW(Vector3 aDir)
    {
        return new Vector3(aDir.y, -aDir.x, 0);
    }
    // counter clockwise
    Vector3 Rotate90CCW(Vector3 aDir) //incorrect?
    {
        return new Vector3(-aDir.z, 0, aDir.x);
    }
    public int GetWallBulletAmount(Transform p1, Transform p2)
    {
        Vector3 bulletLine = p2.position - p1.position;
        Vector3 lineDirection = bulletLine.normalized;

        float totalDistance = bulletLine.magnitude;

        return (int)(((totalDistance / bulspacing)));
    }
    //need to "center" bullets if they don't all fit between the bounds cleanly
    //consider basing wall around spacing rather than number
}
