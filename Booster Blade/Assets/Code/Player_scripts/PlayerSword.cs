using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    public GameObject leftSparkEmitter;
    public GameObject rightSparkEmitter;
    public GameObject turnSparkEffect;


    public PolygonCollider2D slashCollider;

    
    [HideInInspector]
    public bool swordBoosting;
    [HideInInspector]
    public bool swordSwinging = false;

    public void Start()
    {
        slashCollider.enabled = false;
    }
    public void SwordSpark()
    {
        SpawnSparkParticles(turnSparkEffect, leftSparkEmitter.transform);
    }

    public void SpawnSparkParticles(GameObject particleEffectPrefab, Transform spawnPoint)
    {
        GameObject particleEffect = Instantiate(particleEffectPrefab, spawnPoint);
        Destroy(particleEffect, particleEffect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
    }
}
