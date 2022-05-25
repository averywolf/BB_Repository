using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class MonolithCore : MonoBehaviour
{
    public Transform accessPoint; // looked at by PlayerCore to figure out where to move player

    private LevelManager levelManager;
    private CinemachineImpulseSource shakeSource;

    private void Awake()
    {
        shakeSource= GetComponent<CinemachineImpulseSource>();
    }
    private void Start()
    {
        levelManager = LevelManager.instance;
    }

    public void GoThroughExitDoor()
    {
        //call Levelmanager, proceed to next level
        AudioManager.instance.Play("ExitLevel");
        levelManager.ExitLevel();

    }
    public void InsertSword()
    {
        shakeSource.GenerateImpulse();
        Invoke("GoThroughExitDoor", 2);
        LevelManager.instance.SaveTheWorld();
    }

}
