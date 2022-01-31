using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushSense : MonoBehaviour
{
    [SerializeField]
    private SlamCrusher slamCrusher;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            Debug.Log("I should be crushing you.");
        }
    }
}
