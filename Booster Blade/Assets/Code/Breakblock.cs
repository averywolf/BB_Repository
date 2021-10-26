using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakblock : MonoBehaviour
{
    [SerializeField]
    private BreakType breakType;
    private enum BreakType
    {
        standard,
        boost,
        slash
    }

    public GameObject breakeffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SmashBlock()
    {
        AudioManager.instance.Play("EnemyDeath_D");
        SpawnParticles(breakeffect, transform.position);
        Destroy(gameObject);
    }

    public void SpawnParticles(GameObject particleEffectPrefab, Vector2 spawnPoint)
    {
        GameObject particleEffect = Instantiate(particleEffectPrefab, spawnPoint, particleEffectPrefab.transform.rotation);
        Destroy(particleEffect, particleEffect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<SwordSlash>())
        {
            if (!breakType.Equals(BreakType.boost))
            {
                Debug.Log("Smashed slash/regular block");
                SmashBlock();
            }
        }
        if(collision.GetComponent<PlayerSword>())
        {
            if (breakType.Equals(BreakType.boost))
            {
                if (collision.GetComponent<PlayerSword>().swordBoosting)
                {
                    Debug.Log("Smashed boostblock!");
                    SmashBlock();
                }
            }
            else if(breakType.Equals(BreakType.standard))
            {
                SmashBlock();
            }
        }
    }
}
