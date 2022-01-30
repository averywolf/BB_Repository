using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "BulletHell/ShotTypes/PlaceInBounds")]
public class PlaceInBounds : StandardAttack
{
  
    public void PlaceBullet(Bounds bounds)
    {
        BulletPooler pooler = PoolHolder.Pooler;
        GameObject bullet = pooler.GrabBullet(bulletPrefab);
        Vector2 bulletPos = new Vector2(Random.Range(bounds.min.x, bounds.max.x),
        Random.Range(bounds.min.y, bounds.max.y));
        ShootBullet(bullet, bulletPos, 0, 1);
    }
}
