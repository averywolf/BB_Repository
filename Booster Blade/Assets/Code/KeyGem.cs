using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGem : MonoBehaviour
{
    /// when these conditions are met
    //open door
    protected bool conditionsHaveBeenMet = false;

    [SerializeField]
    private Door doorToOpen;

    [SerializeField]
    private GameObject gemFX;
    public GameObject keyBolt;
    public void UnlockDoor()
    {
        Debug.Log("Enough buttons have been pressed.");
        doorToOpen.OpenDoor(true);
        conditionsHaveBeenMet = true;
        SpawnParticles(gemFX, transform.position);
        Destroy(gameObject);
    }
    public void ButtonSignal(Transform buttonTransform)
    {

        if (!conditionsHaveBeenMet)
        {
            StartCoroutine(ShootKeyBolt(buttonTransform, 6));

            // CheckAllButtons();
        }
    }
    public virtual void CheckConditions()
    {

    }
    public void SpawnParticles(GameObject particleEffectPrefab, Vector2 spawnPoint)
    {
        GameObject particleEffect = Instantiate(particleEffectPrefab, spawnPoint, particleEffectPrefab.transform.rotation);
        Destroy(particleEffect, particleEffect.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
    }
    public IEnumerator ShootKeyBolt(Transform startingPoint, float timeSpeed)
    {
        Transform doortarget = transform;
        GameObject bolt = Instantiate(keyBolt, startingPoint.position, transform.rotation);
        Transform boltTrans = bolt.transform;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * timeSpeed)
        {
            boltTrans.position = Vector3.Lerp(startingPoint.position, doortarget.position, t);

            yield return null;
        }
        bolt.GetComponent<ParticleSystem>().Stop();
        bolt.GetComponentInChildren<ParticleSystem>().gameObject.SetActive(true); //doesn't quite work yet, use triggerexplosion method
        //needs to have a particle impact
        Destroy(bolt, bolt.GetComponentInChildren<ParticleSystem>().main.startLifetimeMultiplier);
        CheckConditions();
    }

}
