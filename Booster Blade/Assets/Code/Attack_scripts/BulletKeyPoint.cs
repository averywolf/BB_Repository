using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletKeyPoint
{
    /// <summary>
    /// Used as a List in BulletBehavior to outline a number of sequential BulletActions
    /// </summary>

    public float keyPointDelay; //how long it takes for this keyPoint to activate after the previous one (should be zero if you want to do it right away)

    public BulletAction bulletAction; // action that will happen at this point in time
}
