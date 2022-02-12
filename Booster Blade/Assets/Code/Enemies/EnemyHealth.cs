using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EnemyHealth : MonoBehaviour
{
    //add OnDeath function?
    public UnityEvent OnEnemyDeath;
    public void HurtEnemy()
    {
        //not bothering with HP just yet
        KillEnemy();
    }
    public void KillEnemy()
    {
        OnEnemyDeath.Invoke();
        //make sure to call the specific relevant Destroy() function through this event
        


    }
}
