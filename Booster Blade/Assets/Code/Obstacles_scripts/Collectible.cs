using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    //CollectibleID might be needed, might be set for each collectible differently?

    public GameObject collectFX;

    //Called by PlayerCore upon making contact
    public void PickUpCollectible()
    {
        SpawnParticles(collectFX, transform.position);
        AudioManager.instance.Play("Collect");
        LevelUI.instance.DisplaySmallNotification("Collectible GET!");
        Destroy(gameObject);
        //interact with LevelManager to register this as collected, most likely
    }

    public void SpawnParticles(GameObject particleEffectPrefab, Vector2 spawnPoint)
    {
        if(particleEffectPrefab != null)
        {
            GameObject particleEffect = Instantiate(particleEffectPrefab, spawnPoint, particleEffectPrefab.transform.rotation);
            Destroy(particleEffect, particleEffect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
        }  
    }
}
