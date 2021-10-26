using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardPickup : MonoBehaviour
{
    public GameObject pickupEffect;

    public void PickUpPickup()
    {
        SpawnParticles(pickupEffect, transform.position);
        Destroy(gameObject);
    }

    public void SpawnParticles(GameObject particleEffectPrefab, Vector2 spawnPoint)
    {
        GameObject particleEffect = Instantiate(particleEffectPrefab, spawnPoint, particleEffectPrefab.transform.rotation);
        Destroy(particleEffect, particleEffect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
    }

}
