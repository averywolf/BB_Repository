using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvergrownVine : MonoBehaviour
{
    [SerializeField]
    PlantWeakpoint weakPoint;

    [SerializeField]
    Transform pointA;

    [SerializeField]
    Transform pointZ;

    [SerializeField]
    LineRenderer vineLineA;
    [SerializeField]
    LineRenderer vineLineZ;

    [SerializeField]
    private GameObject vineIdleFX;

    public bool isVineCut=false;

    private void Awake()
    {
        isVineCut = false;
        vineLineA.enabled = true;
        vineLineA.useWorldSpace = true;
        vineLineZ.enabled = true;
        vineLineZ.useWorldSpace = true;
    }
    private void Start()
    {
      //  StartCoroutine(VineParticles(pointA.position, weakPoint.transform.position));
    }
    // Update is called once per frame
    void Update()
    {
        DrawVines();
    }
    public void DrawVines()
    {
        vineLineA.SetPosition(0, pointA.position);
        vineLineA.SetPosition(1, weakPoint.transform.position);
        vineLineZ.SetPosition(0, pointZ.position);
        vineLineZ.SetPosition(1, weakPoint.transform.position);
    }
    private float GetPerpAngle(Vector2 p1, Vector2 p2)
    {
        return (Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Mathf.PI) + 90f;
    }
    public void CutVine()
    {
        if (!isVineCut)
        {
            weakPoint.WeakdownCutEffect();
            vineLineA.enabled = false;
            vineLineZ.enabled = false;
            StartCoroutine(VineParticles(pointA.position, weakPoint.transform.position));
            StartCoroutine(VineParticles(pointZ.position, weakPoint.transform.position));
            //gameObject.SetActive(false);
            isVineCut = true;
        }
  
    }
    public IEnumerator VineParticles(Vector3 pA, Vector3 pB)
    {
        Vector3 vineLine = pA- pB;
        Vector3 vineDirection = vineLine.normalized;
        float totalDistance = vineLine.magnitude;
        float fireRate = 0.02f;
        //int effectNumber = (int)(((totalDistance / 0.1))); //spacing
        int effectNumber = (int)(totalDistance / 1);
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        int j = 0;
        while (j<= effectNumber)
        {
            Vector2 spawnPoint = pB + (vineDirection * (j * 1)); //no offset yet
     
            SpawnParticles(vineIdleFX, spawnPoint);
            j++;
            for (float duration = fireRate; duration > 0; duration -= Time.fixedDeltaTime)
            {
                yield return waitForFixedUpdate;
            }
           
        }

        //    while (true)
        //{
        //    // AudioManager.instance.Play("Shoot1");
        //    //fixedAttack.Fire(turretFirePoint);
        //    for (int i = 0; i <= effectNumber; i++)
        //    {
        //        Vector2 spawnPoint = pB + (vineDirection * (i * 1)); //no offset yet

        //        SpawnParticles(vineIdleFX, spawnPoint);
        //    }
        //    for (float duration = fireRate; duration > 0; duration -= Time.fixedDeltaTime)
        //    {
        //        yield return waitForFixedUpdate;
        //    }
        //}
        
    }

    public void SpawnParticles(GameObject particleEffectPrefab, Vector2 spawnPoint)
    {
        GameObject particleEffect = Instantiate(particleEffectPrefab, spawnPoint, particleEffectPrefab.transform.rotation);
        Destroy(particleEffect, particleEffect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
    }
    public int GetFXAmount(Vector2 pA, Vector2 pB)
    {
        Vector3 vineLine = pA - pB;
        Vector3 vineDirection = vineLine.normalized;
        float totalDistance = vineLine.magnitude;
       
        return (int)(((totalDistance / 1)));
    }
    public IEnumerator VineOldCutParticles(Vector3 pA, Vector3 pB)
    {
        Vector3 vineLine = pA - pB;
        Vector3 vineDirection = vineLine.normalized;
        float totalDistance = vineLine.magnitude;
        float fireRate = 0.4f;
        int effectNumber = (int)(((totalDistance / 1))); //spacing

        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        while (true)
        {
            // AudioManager.instance.Play("Shoot1");
            //fixedAttack.Fire(turretFirePoint);
            for (int i = 0; i <= effectNumber; i++)
            {
                Vector2 spawnPoint = pB + (vineDirection * (i * 1)); //no offset yet

                SpawnParticles(vineIdleFX, spawnPoint);
                for (float duration = fireRate; duration > 0; duration -= Time.fixedDeltaTime)
                {
                    yield return waitForFixedUpdate;
                }
            }
         
        }
    }
 
}
