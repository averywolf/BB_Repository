using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineWeakpoint : MonoBehaviour
{
    //will probably make a separate Vine class
    [SerializeField]
    Transform pointA;
    [SerializeField]
    Transform pointB;
    [SerializeField]
    LineRenderer vineLineA;
    [SerializeField]
    LineRenderer vineLineB;
    public float speed = 1.19f;
    OvergrownCore overgrownCore;
    [SerializeField]
    public bool isCut =false;
    [SerializeField]
    private GameObject vineFX;
    public GameObject testPointer;
    private void Awake()
    {
        isCut = false;
        vineLineA.enabled = true;
        vineLineA.useWorldSpace = true;
        vineLineB.enabled = true;
        vineLineB.useWorldSpace = true;
    }
    private void Start()
    {
        StartCoroutine(VineParticles());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<SwordSlash>())
        {

            CutVine();
            //enemyHealth.HurtEnemy();
        }
    }

    public void CutVine()
    {
        StartCoroutine(CutVineFX());
        
    }
    public IEnumerator CutVineFX()
    {
        Vector3 destinationA = new Vector3(transform.position.x + 6, transform.position.y, 0);
        float currentTime = 0;
        float duration = 2f;

        //while (currentTime <= duration)
        //{
        //    //bulTransform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / duration);
        //    vineLineA.SetPosition(0, Vector3.Lerp(transform.position, destinationA, currentTime / duration));
        //    currentTime += Time.deltaTime;
        //    yield return null;
        //}
        Vector2 vineLine = pointA.position - transform.position;
       // Vector2 vineDirection= vineLine.normalized;
       // Vector2 breakDirection = Rotate90CW(vineLine). normalized;
        Vector2 testOG = testPointer.transform.position;
        float perpangle = GetPerpAngle(pointA.position, transform.position);
        testPointer.transform.rotation = Quaternion.Euler(0, 0, perpangle);

        Vector2 destTest = transform.position + testPointer.transform.right * 4;

        while (currentTime <= duration)
        {
            //bulTransform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / duration);
            // testPointer.transform.position = Vector3.Lerp(testOG, destTest, currentTime / duration);
            testPointer.transform.position += testPointer.transform.right * Time.deltaTime *10;
           // vineLineA.SetPosition(0, Vector3.Lerp(transform.position, destinationA, currentTime / duration));
            currentTime += Time.deltaTime;
            yield return null;
        }
        isCut = true;
        gameObject.SetActive(false);

    }
    private float GetPerpAngle(Vector2 p1, Vector2 p2)
    {
        return (Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Mathf.PI) +90f;
    }
    public IEnumerator VineParticles()
    {
        Vector3 vineLine= pointA.transform.position - transform.position;
        Vector3 vineDirection = vineLine.normalized;
        float totalDistance = vineLine.magnitude; 
        float fireRate = 1;
        int effectNumber = (int)(((totalDistance/1))); //spacing

        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        while (true)
        {
            // AudioManager.instance.Play("Shoot1");
            //fixedAttack.Fire(turretFirePoint);
            for (int i = 0; i <= effectNumber; i++)
            {
                Vector2 spawnPoint = transform.position + (vineDirection * (i * 1)); //no offset yet

                SpawnParticles(vineFX, spawnPoint);
            }
            for (float duration = fireRate; duration > 0; duration -= Time.fixedDeltaTime)
            {
                yield return waitForFixedUpdate;
            }
        }
    }


    private void Update()
    {
        //PingPong between 0 and 1
       // float time = Mathf.PingPong(Time.time * speed, 1);
        //transform.position = Vector3.Lerp(pointA.position, pointB.position, time);

        vineLineA.SetPosition(0, pointA.position);
        vineLineA.SetPosition(1, transform.position);
        vineLineB.SetPosition(0, transform.position);
        vineLineB.SetPosition(1, pointB.position);
    }

    //private void PlaceWallBullet(Vector2 p1, Vector2 p2, GameObject bul, int val, float offsetValue)
    //{
    //    Vector2 bulletLine = p2 - p1;

    //    Vector2 lineDirection = bulletLine.normalized;
    //    Vector2 offsetDirection = Rotate90CW(bulletLine).normalized;
    //    //Vector3 offsetDirection = Vector3.Cross(p1.position, p2.position).normalized;
    //    bul.transform.position = p1 + (lineDirection * (val * bulspacing)) + (offsetDirection * offsetValue);

    //    float perpAngle = (Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Mathf.PI) + 90f;
    //    bul.transform.rotation = Quaternion.Euler(0, 0, perpAngle);

    //}
    Vector3 Rotate90CW(Vector3 aDir)
    {
        return new Vector3(aDir.y, -aDir.x, 0);
    }
    public void SpawnParticles(GameObject particleEffectPrefab, Vector2 spawnPoint)
    {
        GameObject particleEffect = Instantiate(particleEffectPrefab, spawnPoint, particleEffectPrefab.transform.rotation);
        Destroy(particleEffect, particleEffect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
    }
    //public IEnumerator TestShootBolt(Transform target, float timeSpeed)
    //{
    //    GameObject bolt = Instantiate(KeyBolt, transform.position, transform.rotation);
    //    Transform boltTrans = bolt.transform;
    //    for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * timeSpeed)
    //    {
    //        boltTrans.position = Vector3.Lerp(transform.position, target.position, t);

    //        yield return null;
    //    }
    //    Destroy(bolt);
    //}

}
