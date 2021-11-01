using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    //The fastest time the player took to beat the level
    //This value of 9999 should never be seen, probably
    public float bestTime = 9999;
    public int levelIndex; //order it's stored in dictionary, independent of scene order
    //so even if the title screen is scene = 0, and 1-1 and 1-2 are 1 and 2
    // 1-1's level index is 0, and 1-2's index is 1.
    //name transition scenes based on levelindex

        //called to initialize leveldata if it doesn't exist yet
    public LevelData(int lvlIndex, float timeAchieved)
    {
        levelIndex = lvlIndex;
        bestTime = timeAchieved;
    }
    public LevelData()
    {

    }
}
