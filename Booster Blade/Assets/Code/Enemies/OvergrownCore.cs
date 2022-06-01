using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvergrownCore : MonoBehaviour
{
    public GameObject plantdoor;
    public LineRenderer doorColumn;
    public GameObject plantcore;

    public List<OvergrownVine> overgrownVines;
    public FixedAttack fixedAttack;
    private Coroutine attackProcess;
    private Animator animator;
    [SerializeField]
    private GameObject deathFX;
    [SerializeField]
    private GameObject columnDeathFX;
    AudioManager audiomanager;
    private bool attackingPlayer = false;
    
    //get only to attack within range

    private bool plantIsAlive = true;
    private void Awake()
    {
        audiomanager = AudioManager.instance;
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
    }
    public IEnumerator RepeatTurret()
    {

        float fireRate = fixedAttack.GetRateOfFire();
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        while (true)
        {
            audiomanager.Play("PlantFire1");
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
        plantdoor.SetActive(false);
        plantcore.GetComponent<SpriteRenderer>().enabled = false;
        ExplodePillar(transform, plantdoor.transform.position);
        
    }
    public void ExplodePillar(Transform pontA, Vector3 pB)
    {
        Vector3 pA = pontA.position;
        Vector3 vineLine = pA - pB;
        Vector3 vineDirection = vineLine.normalized;
        float totalDistance = vineLine.magnitude;

        int effectNumber = (int)(totalDistance / 1);
        int j = 0;
        while (j <= effectNumber)
        {
            Vector2 spawnPoint = pB + (vineDirection * (j * 1)); //no offset yet

            SpawnParticles(columnDeathFX, spawnPoint);
            j++;

        }
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
