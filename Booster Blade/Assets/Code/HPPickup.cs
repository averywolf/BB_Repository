using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPickup : MonoBehaviour
{
    public GameObject collectFX;
   
    public void HealPickedUp()
    {
        SpawnParticles(collectFX, transform.position);
        AudioManager.instance.Play("Heal");
        LevelUI.instance.DisplaySmallNotification("HP RESTORED");
        Destroy(gameObject);
    }

    public void SpawnParticles(GameObject particleEffectPrefab, Vector2 spawnPoint)
    {
        if (particleEffectPrefab != null)
        {
            GameObject particleEffect = Instantiate(particleEffectPrefab, spawnPoint, particleEffectPrefab.transform.rotation);
            Destroy(particleEffect, particleEffect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
        }
    }
}
