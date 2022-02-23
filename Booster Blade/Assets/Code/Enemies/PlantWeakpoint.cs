using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantWeakpoint : MonoBehaviour
{
    private OvergrownVine overgrownVine;
    private void Awake()
    {
        overgrownVine = GetComponentInParent<OvergrownVine>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<SwordSlash>())
        {
            
            overgrownVine.CutVine();
            //enemyHealth.HurtEnemy();
        }
    }
}
