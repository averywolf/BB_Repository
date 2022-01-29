using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class BulletAction
{
    /// <summary>
    /// Class that specifies a piece of behavior to be performed, based on the enum ActionType. Set by StandardAttack for the bullets that are fired.
    /// BulletBehavior looks at the relavant variables and then executes an action at keypoint.
    /// </summary>

    [SerializeField]
    private BulActionType actionType = BulActionType.setVelocity;

    public enum BulActionType
    {
        setVelocity,
        faceTarget,
        spinOverTime,
        scaleOverTime,
        changeSpeedOverTime,
        triggerAttack,
    }

    public BulActionType GetActionType()
    {
        return actionType;
    }
    public float setSpeed = 0;
    public float endScale = 0;
    public float scaleDuration = 0;
    public float accelDuration = 0;
    public float endSpeed = 0;
    public float spinRate = 0;
}

