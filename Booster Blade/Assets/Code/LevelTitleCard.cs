using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTitleCard : MonoBehaviour
{
    public void SignalLevelStart()
    {
        LevelManager.instance.BlastOff();
        //Time.timeScale = 1;
    }
}
