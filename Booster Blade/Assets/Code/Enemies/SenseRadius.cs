using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SenseRadius : MonoBehaviour
{
    //This script just 
    public UnityEvent SpottedPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerCore>())
        {

            SpottedPlayer.Invoke();
        }
    }
}
