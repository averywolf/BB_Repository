﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Checkpoint : MonoBehaviour
{
    [SerializeField, Header("Make sure this is different for every checkpoint in the current scene")]
    public int checkPointID = 0;
    [SerializeField]
    private Animator checkpointAnim;

    private bool isCheckpointActive=false;
    public GameObject checkpointtouchFX;
    // Start is called before the first frame update
    void Start()
    {
        checkpointAnim.Play("checkP_inactive");
    }
    //if touching player
    public void RegisterCheckpoint()
    {
        if (isCheckpointActive == false)
        {
            SpawnParticles(checkpointtouchFX, transform.position);
            Debug.Log("Registering checkpoint");
            checkpointAnim.Play("checkP_active");
            isCheckpointActive = true;
            
            //SaveManager.instance.activeSave.RegisterCheckPoint(gameObject.transform.position, SceneManager.GetActiveScene().name);
            SaveManager.instance.activeSave.RegisterCheckPoint(checkPointID, SceneManager.GetActiveScene().name);
            SaveManager.instance.Save();
        }

    }
    public void SpawnParticles(GameObject particleEffectPrefab, Vector2 spawnPoint)
    {
        GameObject particleEffect = Instantiate(particleEffectPrefab, spawnPoint, particleEffectPrefab.transform.rotation);
        Destroy(particleEffect, particleEffect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
    }
    
    //might have specific starting angles?
}
