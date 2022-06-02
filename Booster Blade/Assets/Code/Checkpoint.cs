using System.Collections;
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
    public void DeactivateSpawnedCheckpoint() //prevents player from activating checkpoint when spawning on it
    {
        isCheckpointActive = true;

    }
    public void RegisterCheckpoint()
    {
        if (isCheckpointActive == false)
        {
            AudioManager.instance.Play("CheckpointFound");
            SpawnParticles(checkpointtouchFX, transform.position);
            LevelUI.instance.DisplaySmallNotification("CHECKPOINT");
            checkpointAnim.Play("checkP_active");
            isCheckpointActive = true;
            //used to send in the name of the scene, too, but that's irrelevant
            //SaveManager.instance.activeSave.RegisterCheckPoint(gameObject.transform.position, SceneManager.GetActiveScene().name);
            LevelManager.instance.SaveWithCheckpoint(checkPointID);
        }

    }
    public void SpawnParticles(GameObject particleEffectPrefab, Vector2 spawnPoint)
    {
        GameObject particleEffect = Instantiate(particleEffectPrefab, spawnPoint, particleEffectPrefab.transform.rotation);
        Destroy(particleEffect, particleEffect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
    }
    
    //might have specific starting angles?
}
