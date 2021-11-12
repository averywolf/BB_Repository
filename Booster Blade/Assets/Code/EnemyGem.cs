using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGem : KeyGem
{
    public GameObject requiredEnemiesHolder;
    public List<EnemyHealth> enemiesToDestroy;

    public void Update()
    {
        if (!conditionsHaveBeenMet)
        {
            if (!requiredEnemiesHolder.GetComponentInChildren<EnemyHealth>())
            {
                Debug.Log("The enemies needed to open the door have been dealt with.");
                UnlockDoor();
            }

        }
    }

}
