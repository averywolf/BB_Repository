using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    [SerializeField]
    private EnemyHealth enemyHealth;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerSword>() || collision.GetComponent<SwordSlash>())
        {
            enemyHealth.HurtEnemy();
        }
    }
}
