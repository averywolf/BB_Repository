using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushSense : MonoBehaviour
{
    [SerializeField]
    private SlamCrusher slamCrusher;
    private bool sensing = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (sensing)
        {
            if (collision.gameObject.GetComponent<PlayerCore>())
            {
                slamCrusher.CheckPosition(collision.gameObject.GetComponent<PlayerCore>().transform);
            }
        }
 
    }
    public void ActivateSense(bool shouldActivate)
    {
        sensing = shouldActivate;
    }
}
