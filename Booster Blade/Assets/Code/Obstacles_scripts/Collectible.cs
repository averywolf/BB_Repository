using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    //CollectibleID might be needed, might be set for each collectible differently?

    public GameObject collectFX;
    public List<Sprite> colImg; // equal to level ID
    public SpriteRenderer spriteRenderer;

    //add function to hide collectible if already obtained

    public void Start()
    {
        if (LevelManager.instance.tempGotStageCollectible)
        {
            gameObject.SetActive(false); //should hide collectible if it's already saved

        }
        SetUpGraphic();
    }
    public void SetUpGraphic()
    {
        spriteRenderer.sprite = colImg[LevelManager.instance.currentLevelIndex];
    }
    //Called by PlayerCore upon making contact
    public void PickUpCollectible()
    {
        SpawnParticles(collectFX, transform.position);
        AudioManager.instance.Play("Collect");
        LevelUI.instance.DisplaySmallNotification("Collectible Found!");

        LevelManager.instance.SaveCollectibleTemporary();
        Destroy(gameObject);
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
