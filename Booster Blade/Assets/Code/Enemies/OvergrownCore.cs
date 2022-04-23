using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvergrownCore : MonoBehaviour
{
    public GameObject plantdoor;
    public LineRenderer doorColumn;

    public List<OvergrownVine> overgrownVines;
    public FixedAttack fixedAttack;
    private Coroutine attackProcess;
    private Animator animator;
    [SerializeField]
    private GameObject deathFX;

    private bool attackingPlayer = false;
    
    //get only to attack within range

    private bool plantIsAlive = true;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        doorColumn.useWorldSpace = true;

        doorColumn.SetPosition(0, transform.position);
        doorColumn.SetPosition(1, plantdoor.transform.position);
    }

    public void PlantWake()
    {
        animator.Play("plant_idle");
        //
    }
    public void Update()
    {
        if (plantIsAlive)
        {
            for (int i = 0; i < overgrownVines.Count; i++)
            {
                if (!overgrownVines[i].isVineCut)
                {
                    return;
                }
            }
            KillPlant();
        }
     
       // KillPlant();
    }
    public IEnumerator RepeatTurret()
    {

        float fireRate = fixedAttack.GetRateOfFire();
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        while (true)
        {
            // AudioManager.instance.Play("Shoot1");
            fixedAttack.Fire(transform);
            for (float duration = fireRate; duration > 0; duration -= Time.fixedDeltaTime)
            {
                yield return waitForFixedUpdate;
            }
        }
    }
    public void KillPlant()
    {
        AudioManager.instance.Play("PlantDeath");
        Debug.Log("Plant is dead.");
        plantIsAlive = false;
        StartCoroutine(PlantDeathProcess());
    }
    public IEnumerator PlantDeathProcess()
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        animator.Play("plant_death");
        StopCoroutine(attackProcess);
        SpawnParticles(deathFX, transform.position);
        for (float duration = 1; duration > 0; duration -= Time.fixedDeltaTime)
        {
            yield return waitForFixedUpdate;
        }
        Destroy(gameObject);
    
    }

    public void SpawnParticles(GameObject particleEffectPrefab, Vector2 spawnPoint)
    {
        GameObject particleEffect = Instantiate(particleEffectPrefab, spawnPoint, particleEffectPrefab.transform.rotation);
        Destroy(particleEffect, particleEffect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
    }
    public void StartAttacking()
    {
        if(attackingPlayer == false)
        {
            AudioManager.instance.Play("PlantNotice");
            LevelUI.instance.DisplaySmallNotification("IT AWAKES");
            attackingPlayer = true;
            attackProcess = StartCoroutine(RepeatTurret());
        }
    }
}
