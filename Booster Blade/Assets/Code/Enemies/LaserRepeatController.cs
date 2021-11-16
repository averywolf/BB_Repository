using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LaserRepeatController : MonoBehaviour
{
    private Laser laser;

    public bool shouldCycleLaser;
    public float delay1 = 1;
    public float delay2 = 1;
    public bool shouldStartTurnedOn;
    private void Awake()
    {
        laser = GetComponentInChildren<Laser>();
    }
    public void Start()
    {
        laser.TurnOnLaser(shouldStartTurnedOn);

        if (shouldCycleLaser)
        {
            StartCoroutine(ActionProcess());
        }
    }
    public IEnumerator ActionProcess()
    {
        while (true)
        {     
            yield return new WaitForSeconds(delay1);
            laser.ToggleLaser();
            yield return new WaitForSeconds(delay2);
            laser.ToggleLaser();
        }
    }
}
