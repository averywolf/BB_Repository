using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGem : KeyGem
{
    public GameObject requiredEnemiesHolder;
    private List<EnemyHealth> enemiesToDestroy;

    public void Start()
    {
        EnemyHealth[] enemies = requiredEnemiesHolder.GetComponentsInChildren<EnemyHealth>();
        enemiesToDestroy = new List<EnemyHealth>();
        foreach (EnemyHealth enemyHealth in enemies)
        {
            enemiesToDestroy.Add(enemyHealth.GetComponent<EnemyHealth>());
      
        }

        for (int i = 0; i < enemiesToDestroy.Count; i++)
        {
            enemiesToDestroy[i].enemyGem = this;
            enemiesToDestroy[i].SetKeyStatus();
        }
    }
    public override void CheckConditions()
    {
        //if (!requiredEnemiesHolder.GetComponentInChildren<EnemyHealth>())
        //{
            for (int i = 0; i < enemiesToDestroy.Count; i++)
            {
                if (!enemiesToDestroy[i].enemyDead)
                {
                    return;
                }
            }

            Debug.Log("The enemies needed to open the door have been dealt with.");
            UnlockDoor();
        //}
    }

}
