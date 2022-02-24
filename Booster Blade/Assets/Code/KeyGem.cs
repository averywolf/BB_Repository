using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGem : MonoBehaviour
{
    /// when these conditions are met
    //open door
    protected bool conditionsHaveBeenMet = false;

    [SerializeField]
    private Door doorToOpen;

    [SerializeField]
    private GameObject gemFX;
    public void UnlockDoor()
    {
        Debug.Log("Enough buttons have been pressed.");
        doorToOpen.OpenDoor(true);
        conditionsHaveBeenMet = true;
        SpawnParticles(gemFX, transform.position);
        Destroy(gameObject);
    }
    public void SpawnParticles(GameObject particleEffectPrefab, Vector2 spawnPoint)
    {
        GameObject particleEffect = Instantiate(particleEffectPrefab, spawnPoint, particleEffectPrefab.transform.rotation);
        Destroy(particleEffect, particleEffect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
    }
}
