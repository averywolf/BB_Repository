using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator doorAnim;

    private bool isDoorOpen = false;

    public GameObject keyBolt;
    private void Awake()
    {
        doorAnim = GetComponent<Animator>();
    }
    public void OpenDoor(bool shouldOpen)
    {
        if (shouldOpen)
        {
            AudioManager.instance.Play("BreakCrystal1");
            AudioManager.instance.Play("DoorObstacleOpen");
            doorAnim.Play("basicdoor_open");
            isDoorOpen = true;
        }
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
        Destroy(bolt);
    }
    //public affectlock method that can be called through an event in an enemy?
}
