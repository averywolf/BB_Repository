using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineWeakpoint : MonoBehaviour
{
    //po
    OvergrownCore overgrownCore;
    [SerializeField]
    public bool isCut =false;
    private void Awake()
    {
        isCut = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<SwordSlash>())
        {
            isCut = true;
            gameObject.SetActive(false);
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
