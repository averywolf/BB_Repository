using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManosCore : MonoBehaviour
{
    public GameObject manosDoor;
    public LineRenderer doorColumn;

    public List<Chain> chains;
    public FixedAttack fixedAttack;
    public AimedAttack aimedAttack;
    public AimedAttack aimedAttack2;
    private Coroutine spiralAttack;
    private Coroutine attackProcess;
    public CircleSpread circleSpread;
    public float rotateRate = 2;
    [SerializeField]
    private GameObject deathFX;

    private Animator manosAnim;
    private bool attackingPlayer = false;

    [SerializeField]
    private int aimCircleIterations;
    [SerializeField]
    private int spiralIterations;
    [SerializeField]
    private float attackSwitchTime;
    //get only to attack within range

    private bool coreIsAlive = true;
    private void Awake()
    {
        manosAnim = GetComponent<Animator>();
        doorColumn.useWorldSpace = true;

        doorColumn.SetPosition(0, transform.position);
        doorColumn.SetPosition(1, manosDoor.transform.position);
    }

    public void ManosWake()
    {

    }
    public void Update()
    {
        if (coreIsAlive)
        {
            for (int i = 0; i < chains.Count; i++)
            {
                if (!chains[i].isChainCut)
                {
                    return;
                }
            }
            KillCore();
        }
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
    public IEnumerator RepeatTurretAimed()
    {

        float fireRate = aimedAttack.GetRateOfFire();
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        while (true)
        {
            // AudioManager.instance.Play("Shoot1");
            aimedAttack.FireAimed(transform.position, LevelManager.instance.GetPlayerTransform().position);
            for (float duration = fireRate; duration > 0; duration -= Time.fixedDeltaTime)
            {
                yield return waitForFixedUpdate;
            }
        }
    }
    public void KillCore()
    {
        Debug.Log("Plant is dead.");
        coreIsAlive = false;
        StartCoroutine(CoreDeathProcess());
    }
    public IEnumerator CoreDeathProcess()
    {
        manosAnim.Play("manos_exit");
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        // animator.Play("plant_death");
        //StopCoroutine(spiralAttack);
        StopCoroutine(attackProcess);
        SpawnParticles(deathFX, transform.position);
        for (float duration = 1; duration > 0; duration -= Time.fixedDeltaTime)
        {
            yield return waitForFixedUpdate;
        }
        //Destroy(gameObject);

    }

    public IEnumerator SpiralAttack()
    {
        float fireRate = circleSpread.GetRateOfFire();
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        float j = 0;

        while (true)
        {
            j+=rotateRate;
            j = j % 360;
            // AudioManager.instance.Play("Shoot1");
            circleSpread.CircleAttack(j, transform.position);
           // aimedAttack.FireAimed(transform.position, LevelManager.instance.GetPlayerTransform().position);
            for (float duration = fireRate; duration > 0; duration -= Time.fixedDeltaTime)
            {
                yield return waitForFixedUpdate;
            }
        }
    }
    public IEnumerator SpiralAttack(int numTimes)
    {
        float fireRate = circleSpread.GetRateOfFire();
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        float j = 0;
        int timesDone = 0;
        while (timesDone <numTimes)
        {
            j += rotateRate;
            j = j % 360;
            // AudioManager.instance.Play("Shoot1");
            circleSpread.CircleAttack(j, transform.position);
            // aimedAttack.FireAimed(transform.position, LevelManager.instance.GetPlayerTransform().position);
            for (float duration = fireRate; duration > 0; duration -= Time.fixedDeltaTime)
            {
                yield return waitForFixedUpdate;
            }
            timesDone++;
        }
    }
    public IEnumerator ManosPattern()
    {
      //  Coroutine aimedCou = StartCoroutine(RepeatAimedAttack(aimedAttack, transform, LevelManager.instance.GetPlayerTransform(), aimCircleIterations));
        while (true)
        {

          
             yield return StartCoroutine(RepeatAimedAttack(aimedAttack, transform, LevelManager.instance.GetPlayerTransform(), aimCircleIterations));
            yield return new WaitForSeconds(attackSwitchTime);
            //spawn assassin like drone instead?
            yield return StartCoroutine(SpiralAttack(spiralIterations));
            //yield return StartCoroutine(RepeatFixedAttack(fixedAttack, transform, spiralIterations));
            yield return new WaitForSeconds(attackSwitchTime);
            yield return StartCoroutine(RepeatAimedAttack(aimedAttack2, transform, LevelManager.instance.GetPlayerTransform(), aimCircleIterations));
            yield return new WaitForSeconds(attackSwitchTime);
            yield return StartCoroutine(SpiralAttack(spiralIterations));
            //yield return StartCoroutine(RepeatFixedAttack(fixedAttack, transform, spiralIterations));
            yield return new WaitForSeconds(attackSwitchTime);
        }
    }
    public void SpawnParticles(GameObject particleEffectPrefab, Vector2 spawnPoint)
    {
        GameObject particleEffect = Instantiate(particleEffectPrefab, spawnPoint, particleEffectPrefab.transform.rotation);
        Destroy(particleEffect, particleEffect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
    }
    public void StartAttacking()
    {
        if (attackingPlayer == false)
        {
            //maybe weakpoints animate in, too?
            manosAnim.Play("manos_enter");
            LevelUI.instance.SayLevelDialogue("I'm gonna kill you!!!");
            attackingPlayer = true;
           // attackProcess = StartCoroutine(RepeatTurret());

        }
    }
    public void StartToKill()
    {
        attackProcess = StartCoroutine(ManosPattern());

        //if (fixedAttack != null)
        //{
        //    StartCoroutine(RepeatTurret());
        //}
        //if (aimedAttack != null)
        //{
        //    StartCoroutine(RepeatTurretAimed());
        //}
        //if (circleSpread != null)
        //{
        //    spiralAttack = StartCoroutine(SpiralAttack());
        //}
    }
    public IEnumerator RepeatFixedAttack(FixedAttack fixedAttack, Transform firePoint, int numTimes)
    {

        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        int j = 0;
        float fireRate = fixedAttack.GetRateOfFire();

        while (j < numTimes)
        {
            // AudioManager.instance.Play("Shoot1");
            fixedAttack.Fire(firePoint.position);
            for (float duration = fireRate; duration > 0; duration -= Time.fixedDeltaTime)
            {
                yield return waitForFixedUpdate;
            }
            j++;
        }
    }

    public IEnumerator RepeatAimedAttack(AimedAttack aimedAttack, Transform firePoint, Transform aimTransform, int numTimes)
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

        int j = 0;
        float fireRate = aimedAttack.GetRateOfFire();
        while (j < numTimes)
        {
            aimedAttack.FireAimed(firePoint.position, aimTransform.position);
            // audioManager.Play("BossShoot_Arco1");
            for (float duration = fireRate; duration > 0; duration -= Time.fixedDeltaTime)
            {
                yield return waitForFixedUpdate;
            }
            j++;
        }
    }



}
