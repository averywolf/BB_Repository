using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private Transform portalDestination;

    [SerializeField]
    private string transitionSFX;

    [SerializeField]
    private GameObject teleportFX;


    private bool teleportReady=true;


    //might move this logic over to PlayerCore like most of the collision checking
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerCore>() )
        {
            if (teleportReady)
            {
                if (portalDestination.GetComponent<Portal>())
                {
                    portalDestination.GetComponent<Portal>().SetTeleportStatus(false);
                }
                AudioManager.instance.Play(transitionSFX);
                PlayerController playerController = collision.gameObject.GetComponentInParent<PlayerController>();
                playerController.transform.position = portalDestination.transform.position;
                SpawnParticles(teleportFX, transform.position);
                SpawnParticles(teleportFX, portalDestination.position);
            }
  
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerCore>())
        {
            if (teleportReady == false)
            {
                SetTeleportStatus(true);  
            }

        }
    }
    public void SpawnParticles(GameObject particleEffectPrefab, Vector2 spawnPoint)
    {
        GameObject particleEffect = Instantiate(particleEffectPrefab, spawnPoint, particleEffectPrefab.transform.rotation);
        Destroy(particleEffect, particleEffect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
    }

    public void SetTeleportStatus(bool turnOn)
    {
        teleportReady = turnOn;
    }
}
