using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;


    public GameObject laserHitFX;
    // Start is called before the first frame update
    public bool isLaserActive;
    public bool effectsEnabled = false;
    private Vector3 laserHitPos;

    public LayerMask laserMask;
    public LaserTell laserTell;
    public AudioSource laserAudio;//used so we can have proximal audio when switching on and off laser
    //maybe start laser as inactive?
    void Awake()
    {


        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true; // important I think?
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.up, Mathf.Infinity, laserMask);
        laserTell.ShootLaserTell(transform.position, ray.point);
        //make sure this doesn't happen when game is paused, too, probably
        if (isLaserActive)
        {

            laserHitPos = ray.point;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, laserHitPos);
            if (effectsEnabled)
            {
                SpawnParticles(laserHitFX, laserHitPos);
            }
            if (ray.collider != null)
            {
                if (ray.collider.GetComponent<PlayerCore>())
                {
                    
                    ray.collider.GetComponentInParent<PlayerController>().HurtPlayer(gameObject); //not sure if it's the best idea to do this here
                }
            }
        }
        else
        {
           
            //hideLineRenderer
        }
  
    }
    public void TurnOnLaser(bool turnOn)
    {
        isLaserActive = turnOn;
        if (turnOn)
        {
            lineRenderer.enabled = true;
            laserTell.ShowLaserTell(false);
        }
        else
        {
            lineRenderer.enabled = false;
            
            laserTell.ShowLaserTell(true);


        }

    }
    public void ToggleLaser()
    {
        //isLaserActive= !isLaserActive;
        laserAudio.Play();
        TurnOnLaser(!isLaserActive);

    }

    public void SpawnParticles(GameObject particleEffectPrefab, Vector2 spawnPoint)
    {
        GameObject particleEffect = Instantiate(particleEffectPrefab, spawnPoint, particleEffectPrefab.transform.rotation);
        Destroy(particleEffect, particleEffect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
    }
}
