using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public GameObject keyBolt;

    public IEnumerator ShootKeyBolt(Transform startingPoint, float timeSpeed)
    {
        Transform doortarget = transform;
        GameObject bolt = Instantiate(keyBolt, startingPoint.position, transform.rotation);
        Transform boltTrans = bolt.transform;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * timeSpeed)
        {
            boltTrans.position = Vector3.Lerp(startingPoint.position, doortarget.position, t);

            yield return null;
        }
        Destroy(bolt);
    }
}
