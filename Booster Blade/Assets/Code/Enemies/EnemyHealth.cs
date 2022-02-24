using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EnemyHealth : MonoBehaviour
{
    //add OnDeath function?
    public UnityEvent OnEnemyDeath;

    [HideInInspector]
    public EnemyGem enemyGem;

    public bool enemyDead = false;
    public void HurtEnemy()
    {
        //not bothering with HP just yet
        KillEnemy();
    }
    public void KillEnemy()
    {
        enemyDead = true;
        //make sure to call the specific relevant Destroy() function through this event
        
        if(enemyGem != null)
        {
            enemyGem.ButtonSignal(transform);
        }
        OnEnemyDeath.Invoke();
    }
}
