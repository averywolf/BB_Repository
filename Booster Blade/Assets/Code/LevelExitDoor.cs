using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExitDoor : MonoBehaviour
{
    public string levelName = "";
    public void GoThroughExitDoor()
    {
        //call Levelmanager, proceed to next level
        SceneManager.LoadScene(levelName);
    }
}
