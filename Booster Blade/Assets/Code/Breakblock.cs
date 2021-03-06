using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Breakblock : MonoBehaviour
{
    [SerializeField]
    private BreakType breakType;

    [SerializeField]
    private string breakSound = "BreakCrystal1";
    private CinemachineImpulseSource breakSource;
    private enum BreakType
    {
        standard,
        boost,
        slash
    }

    public GameObject breakeffect;

    private void Awake()
    {
        breakSource = GetComponent<CinemachineImpulseSource>();    
    }
    public void SmashBlock()
    {
        AudioManager.instance.Play(breakSound);
        SpawnParticles(breakeffect, transform.position);
        breakSource.GenerateImpulse();
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
                SmashBlock();
            }
        }
        if(collision.GetComponent<PlayerSword>())
        {
            if (breakType.Equals(BreakType.boost))
            {
                if (collision.GetComponent<PlayerSword>().swordBoosting)
                {
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
