using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    [SerializeField]
    protected ChainWeakpoint weakPoint;

    [SerializeField]
    protected Transform pointA; //start point of chain

    [SerializeField]
    protected Transform pointZ; //end point of chain

    [SerializeField]
    protected LineRenderer chainLineA; //stretches from A to Weakpoint
    [SerializeField]
    protected LineRenderer chainLineZ; //stretches from Z to Weakpoint

    [SerializeField]
    protected GameObject chainCutFX;

    public bool isChainCut;

    public void Awake()
    {
        isChainCut = false;
        chainLineA.enabled = true;
        chainLineA.useWorldSpace = true;
        chainLineZ.enabled = true;
        chainLineZ.useWorldSpace = true;   
    }

    // Update is called once per frame
    void Update()
    {
        DrawChains();
    }
    public void DrawChains()
    {
        chainLineA.SetPosition(0, pointA.position);
        chainLineA.SetPosition(1, weakPoint.transform.position);
        chainLineZ.SetPosition(0, pointZ.position);
        chainLineZ.SetPosition(1, weakPoint.transform.position);
    }
    
    //Triggered by ChainWeakpoint
    public void CutChain()
    {
        if (!isChainCut)
        {
            weakPoint.WeakpointCutEffect(); //triggers whatever VFX the weakpoint itself has for being attacked, might add a delay to this happening
            chainLineA.enabled = false;
            chainLineZ.enabled = false;
            StartCoroutine(ExplodeDownLength(pointA, weakPoint.transform.position));
            StartCoroutine(ExplodeDownLength(pointZ, weakPoint.transform.position));
            isChainCut = true;
        }
    }
    public IEnumerator ExplodeDownLength(Transform pontA, Vector3 pB)
    {
        Vector3 pA = pontA.position;
        Vector3 chain = pA - pB;
        Vector3 chainDirection = chain.normalized;
        float totalDistance = chain.magnitude;
        float fireRate = 0.02f;
        int effectNumber = (int)(totalDistance / 1);
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        int j = 0;
        while (j <= effectNumber)
        {
            Vector2 spawnPoint = pB + (chainDirection * (j * 1)); //no offset yet

            SpawnParticles(chainCutFX, spawnPoint);
            j++;
            for (float duration = fireRate; duration > 0; duration -= Time.fixedDeltaTime)
            {
                yield return waitForFixedUpdate;
            }

        }
        pontA.gameObject.SetActive(false);
    }

    public void SpawnParticles(GameObject particleEffectPrefab, Vector2 spawnPoint)
    {
        GameObject particleEffect = Instantiate(particleEffectPrefab, spawnPoint, particleEffectPrefab.transform.rotation);
        Destroy(particleEffect, particleEffect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
    }

    //doesn't matter but I'm keeping this method on hand just in case
    private float GetPerpAngle(Vector2 p1, Vector2 p2)
    {
        return (Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Mathf.PI) + 90f;
    }
}
