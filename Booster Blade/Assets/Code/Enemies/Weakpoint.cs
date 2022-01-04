using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weakpoint : MonoBehaviour
{
    // Updated version of EnemyHitbox
    // EnemyBase to replace EnemyHealth?

    // OnWeakPointHit
    // grab from static class or something what effect to use?

    public Weakness weakness = Weakness.slashweak; //most enemies die from Slash

    public float reticuleSize;

    public bool weakExposed =true;
    public enum Weakness
    {
        boostweak,
        slashweak,
    }

    public void ShowWeakpoint(bool showIt)
    {
        
    }
}
