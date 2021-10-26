using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEntity : MonoBehaviour
{
    private EntityStates currentEntState = EntityStates.frozen;
    public enum EntityStates
    {
        frozen,
        dormant,
        aggro
    }

    public void SetEntityState(EntityStates stateToSet)
    {
        currentEntState = stateToSet;
    }
}
