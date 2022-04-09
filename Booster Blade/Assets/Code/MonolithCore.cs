using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonolithCore : MonoBehaviour
{
    public Transform accessPoint; // looked at by PlayerCore to figure out where to move player

    private LevelManager levelManager;

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
}
