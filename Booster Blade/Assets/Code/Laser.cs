using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform laserHit;

    public GameObject laserHitFX;
    // Start is called before the first frame update
    public bool isLaserActive;
    public bool effectsEnabled = false;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true; // important I think?
    }

    // Update is called once per frame
    void Update()
    {
        //make sure this doesn't happen when game is paused, too, probably
        if (isLaserActive)
        {
            //will need to expand so enemies don't hit lasers, probably
            RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.up);
            Debug.DrawLine(transform.position, ray.point);
            laserHit.position = ray.point;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, laserHit.position);
            if (effectsEnabled)
            {
                SpawnParticles(laserHitFX, laserHit.position);
            }
            if (ray.collider != null)
            {
                if (ray.collider.GetComponent<PlayerCore>())
                {
                    Debug.Log("Hitting Player core");
                    ray.collider.GetComponentInParent<PlayerController>().HurtPlayer(gameObject); //not sure if it's the best idea to do this here
                }
            }
        }
  
    }
    public void TurnOnLaser(bool turnOn)
    {
        isLaserActive = turnOn;
    }


    public void SpawnParticles(GameObject particleEffectPrefab, Vector2 spawnPoint)
    {
        GameObject particleEffect = Instantiate(particleEffectPrefab, spawnPoint, particleEffectPrefab.transform.rotation);
        Destroy(particleEffect, particleEffect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
    }
}
