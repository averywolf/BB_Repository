using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : MonoBehaviour
{
    private Animator animator;
    public AimedAttack testAimedAttack;
    private bool assassinReadied = false;
    private bool tryingToKill = false;
    // Start is called before the first frame update
    [SerializeField]
    private float cooldownTime;
    [SerializeField]
    private float timeToReadyAttack;
    [SerializeField]
    private float fireDelay;
    public float teleDistance=1;
    [SerializeField]
    private GameObject teleportParticles;
    [SerializeField]
    private GameObject deathParticles;
    [SerializeField]
    private float noticeDuration;
    [SerializeField]
    private int tempTimesToFire = 8;


    [SerializeField]
    ChargeTell chargeTell;
    [SerializeField]
    float chargeTime;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.Play("idle");
    }
    public IEnumerator AttemptToMurder()
    {

        animator.Play("idle");
        PlayerController playControl = LevelManager.instance.GetPlayerController();

        SpawnParticles(teleportParticles, transform.position);
        transform.position = LevelManager.instance.GetPlayerTransform().position;
        transform.parent = LevelManager.instance.GetPlayerTransform();
        transform.localPosition = new Vector2(teleDistance, 0);
       
    
        //might make this fixedUpdate
        SpawnParticles(teleportParticles, transform.position);
        AudioManager.instance.Play("AssassinTeleport");
        float duration = timeToReadyAttack;
        float currentTime = 0.0f;

        chargeTell.ActivateTell(chargeTime, 3, 0); // currently just does this as soon as it locks on
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

        //forget why I do this instead of waittime, I think contantly setting position might be irrelevant since it's already parented
        while (currentTime <= duration)
        {
            Vector2 telePos = new Vector2(playControl.horizontal, playControl.vertical);
            transform.localPosition = -telePos * 5;
            currentTime += Time.fixedDeltaTime;

            yield return waitForFixedUpdate;
        }

        transform.parent = null;
        //might need rewrite
        Vector2 aimPoint= LevelManager.instance.GetPlayerTransform().position;
        yield return new WaitForSeconds(fireDelay);
        StartCoroutine(RepeatAimedAttack(testAimedAttack, transform, aimPoint, tempTimesToFire));
        testAimedAttack.FireAimed(transform.position, aimPoint);
        StartCoroutine(AssassinCooldown());
    }

    public void Teleport(float attackAngle)
    {
        //if (attackAngle == 0)
        //{
        //    transform.localPosition = new Vector2(teleDistance, 0);
        //}
        //else if(attackAngle == 90)
        //{
        //    transform.localPosition = new Vector2(0, teleDistance);
        //}
        //else if(attackAngle == 180)
        //{
        //    transform.localPosition = new Vector2(-teleDistance, 0);
        //}
        //else if(attackAngle == 270)
        //{
        //    transform.localPosition = new Vector2(0, -teleDistance);
        //}

    }
    public void BeginMurderAttempt()
    {
        if (assassinReadied)
        {
           
            StartCoroutine(AttemptToMurder());
        }
    }
    public IEnumerator NoticePlayer()
    {
        
        AudioManager.instance.Play("AssassinNotice");
        animator.Play("notice");
        yield return new WaitForSeconds(noticeDuration);
        BeginMurderAttempt();
    }
    public void CommitToMurderingPlayer()
    {
        if (!tryingToKill)
        {
            tryingToKill = true;
            StartCoroutine(NoticePlayer());
        }
  
    }
    public void AssassinDeath()
    {
        SpawnParticles(deathParticles, transform.position);
        AudioManager.instance.Play("EnemyDeath_D");
        Destroy(gameObject);
    }
    public void ReadyAssassin()
    {
        assassinReadied = true;
    }
    public IEnumerator AssassinCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        BeginMurderAttempt();
    }

    public void SpawnParticles(GameObject particleEffectPrefab, Vector2 spawnPoint)
    {
        GameObject particleEffect = Instantiate(particleEffectPrefab, spawnPoint, particleEffectPrefab.transform.rotation);
        Destroy(particleEffect, particleEffect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
    }
    ///maybe add function where if you leave the range it goes on standby?
    ///


    //public IEnumerator RepeatFixedAttack(FixedAttack fixedAttack, Transform firePoint, int numTimes)
    //{

    //    YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
    //    int j = 0;
    //    float fireRate = fixedAttack.GetRateOfFire();

    //    while (j < numTimes)
    //    {
    //        // AudioManager.instance.Play("Shoot1");
    //        fixedAttack.Fire(firePoint.position);
    //        for (float duration = fireRate; duration > 0; duration -= Time.fixedDeltaTime)
    //        {
    //            yield return waitForFixedUpdate;
    //        }
    //        j++;
    //    }
    //}
    //public IEnumerator RepeatFixedAttack(FixedAttack fixedAttack, Vector2 firePoint, int numTimes)
    //{
    //    YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
    //    int j = 0;
    //    float fireRate = fixedAttack.GetRateOfFire();
    //    while (j < numTimes)
    //    {
    //        fixedAttack.Fire(firePoint);
    //        for (float duration = fireRate; duration > 0; duration -= Time.fixedDeltaTime)
    //        {
    //            yield return waitForFixedUpdate;
    //        }
    //        j++;
    //    }
    //}

    ////different one that takes Transform instead?
    //public IEnumerator RepeatAimedAttack(AimedAttack aimedAttack, Transform firePoint, Transform aimTransform, int numTimes)
    //{
    //    YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

    //    int j = 0;
    //    float fireRate = aimedAttack.GetRateOfFire();
    //    while (j < numTimes)
    //    {
    //        aimedAttack.FireAimed(firePoint.position, aimTransform.position);
    //        // audioManager.Play("BossShoot_Arco1");
    //        for (float duration = fireRate; duration > 0; duration -= Time.fixedDeltaTime)
    //        {
    //            yield return waitForFixedUpdate;
    //        }
    //        j++;
    //    }
    //}
    public IEnumerator RepeatAimedAttack(AimedAttack aimedAttack, Transform firePoint, Vector2 aimPoint, int numTimes)
    {

        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        int j = 0;
        float fireRate = aimedAttack.GetRateOfFire();
        while (j < numTimes)
        {
            aimedAttack.FireAimed(firePoint.position, aimPoint);
            //audioManager.Play("BossShoot_Arco1");
            for (float duration = fireRate; duration > 0; duration -= Time.fixedDeltaTime)
            {
                yield return waitForFixedUpdate;
            }
            j++;
        }
    }
    //public IEnumerator BugMoveToPosition(Vector3 target, float moveTime)
    //{
    //    float t = 0;
    //    Vector3 start = transform.position;

    //    while (t <= 1)
    //    {
    //        t += Time.fixedDeltaTime / moveTime; //i am fearful
    //        rb.MovePosition(Vector3.Lerp(start, target, t));

    //        yield return null;
    //    }
    //}
}
