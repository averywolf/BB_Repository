using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator doorAnim;

    private bool isDoorOpen = false;
    private void Awake()
    {
        doorAnim = GetComponent<Animator>();
    }
    public void OpenDoor(bool shouldOpen)
    {
        if (shouldOpen)
        {
            AudioManager.instance.Play("BreakCrystal1");
            doorAnim.Play("basicdoor_open");
            isDoorOpen = true;
        }
    }
}
