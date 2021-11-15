using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceDoor : MonoBehaviour
{
    private Animator entranceAnim;
    private bool isDoorOpen = false;
    private void Awake()
    {
        entranceAnim = GetComponent<Animator>();
    }
    public void OpenDoor(bool shouldOpen)
    {
        if (shouldOpen)
        {
            entranceAnim.Play("entrance_opening");
            isDoorOpen = true;
        }
    }
}
