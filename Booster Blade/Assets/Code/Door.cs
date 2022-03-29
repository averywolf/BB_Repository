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
        if (shouldOpen && !isDoorOpen)
        {
            AudioManager.instance.Play("BreakCrystal1");
            AudioManager.instance.Play("DoorObstacleOpen");
            LevelUI.instance.DisplaySmallNotification("DOOR UNLOCKED!");
            doorAnim.Play("basicdoor_open");
            isDoorOpen = true;
        }
    }

    //public affectlock method that can be called through an event in an enemy?
}
