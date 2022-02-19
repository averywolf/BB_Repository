using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineWeakpoint : MonoBehaviour
{
    //po

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<SwordSlash>())
        {
            //enemyHealth.HurtEnemy();
        }
    }

    //public IEnumerator TestShootBolt(Transform target, float timeSpeed)
    //{
    //    GameObject bolt = Instantiate(KeyBolt, transform.position, transform.rotation);
    //    Transform boltTrans = bolt.transform;
    //    for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * timeSpeed)
    //    {
    //        boltTrans.position = Vector3.Lerp(transform.position, target.position, t);

    //        yield return null;
    //    }
    //    Destroy(bolt);
    //}

}
