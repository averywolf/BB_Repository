using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    //The fastest time the player took to beat the level
    //This value of 9999 should never be seen, probably
    public float bestTime = 999999;
    public bool hasLevelBeenBeaten = false;// NOTE: Only relevant for RecordsData. this value doesn't change from false at all in currentRunData. (really should make separate...)
    public int levelIndex; //order it's stored in dictionary, independent of scene order

    //so even if the title screen is scene = 0, and 1-1 and 1-2 are 1 and 2
    // 1-1's level index is 0, and 1-2's index is 1.
    //name transition scenes based on levelindex

    public bool gotStageCollectible = false; //did the player pick up the collectible during this stage? (currently only matters in the context of currentRunData)

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
