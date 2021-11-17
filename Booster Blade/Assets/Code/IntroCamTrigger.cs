using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCamTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //enable control
        LevelManager.instance.EnablePlayerControl(true);
    }
}
