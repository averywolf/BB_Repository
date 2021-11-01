using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExitDoor : MonoBehaviour
{
    private LevelManager levelManager;

    private void Start()
    {
        levelManager = LevelManager.instance;
    }

    public void GoThroughExitDoor()
    {
        //call Levelmanager, proceed to next level
        levelManager.ExitLevel();

    }
}
