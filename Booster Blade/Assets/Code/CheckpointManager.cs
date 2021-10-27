using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public List<Checkpoint> checkpointsInLevel;

    public Checkpoint SearchForCheckpoint(int checkID)
    {
        for (int i = 0; i < checkpointsInLevel.Count; i++)
        {
            if(checkpointsInLevel[i].checkPointID == checkID)
            {
                return checkpointsInLevel[i];
            }
        }
        Debug.LogError("Unable to find checkpoint at ID " + checkID);
        return null;
    }
}
