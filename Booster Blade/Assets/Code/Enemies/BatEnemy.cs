using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class BatEnemy : MonoBehaviour
{
    Seeker seeker;
    AIDestinationSetter aIDestinationSetter;
    AIPath aIPath;

    [SerializeField]
    private Transform batGFX;
    [SerializeField]
    private float batMoveSpeed = 10;

    [SerializeField]
    private GameObject batDeathFX;

    private bool batReady = false;
    private bool chasingPlayer = false;

    public WeakX weakX;

    public bool debugBatAlt =false;
    private void Awake()
    {
        batReady= false;
        chasingPlayer = false;
        aIPath = GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();
        aIDestinationSetter = GetComponent<AIDestinationSetter>();
        aIPath.canMove = false;
        //set acceleration, too?
        aIPath.maxSpeed = batMoveSpeed;
    }
    public void Start()
    {
        if (debugBatAlt)
        {
            aIDestinationSetter.target = LevelManager.instance.debugDestination;
        }
        else
        {
            aIDestinationSetter.target = LevelManager.instance.GetPlayerController().transform; //might be better to grab playerCore transform?}
        }

    }
    //make this be called by EnitityBehavior?
    public void BatWakeUp()
    {
        batReady = true;
    }
    public void BatSpotPlayer(bool wasSpotted)
    {
        if (wasSpotted &&!chasingPlayer)
        {
            chasingPlayer=true;
            weakX.SummonWeakX();
            AudioManager.instance.Play("BatSqueak");
            StartBatMoving();
        }
    }
    public void StartBatMoving()
    {
        if (batReady)
        {
            aIPath.canMove = true;
        }
      
    }
    
    public void Update()
    {

        if(aIPath.desiredVelocity.x >= 0.005f)
        {
            batGFX.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (aIPath.desiredVelocity.x <= -0.005f)
        {
            batGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
    //called through OnEnemyDeath event
    public void BatDeath()
    {
        SpawnParticles(batDeathFX, transform.position);
        AudioManager.instance.Play("EnemyDeath_D");
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
    public void SpawnParticles(GameObject particleEffectPrefab, Vector2 spawnPoint)
    {
        GameObject particleEffect = Instantiate(particleEffectPrefab, spawnPoint, particleEffectPrefab.transform.rotation);
        Destroy(particleEffect, particleEffect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
    }
}
