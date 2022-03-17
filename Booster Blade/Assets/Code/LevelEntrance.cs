using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class LevelEntrance : MonoBehaviour
{
    [SerializeField]
    private Animator entranceDoorAnim;
    private bool isDoorOpen = false;

    public CinemachineVirtualCamera entranceCam;
    public ParticleSystem doorBurstFX;
    public GameObject backtrackCollider;
    [Tooltip("Place player spawns at initially")]
    public Transform initialSpawn;

    public void Awake()
    {
        backtrackCollider.SetActive(false);
    }
    public void OpenDoor(bool shouldOpen, bool startingFromEntrance)
    {
        if (shouldOpen && !isDoorOpen)
        {
            entranceDoorAnim.Play("entrance_opening");
            isDoorOpen = true;
            //playing sound effect and stuff is pointless if player is starting from checkpoint
            if (startingFromEntrance)
            {
                doorBurstFX.Play(true);
                AudioManager.instance.Play("DoorEntranceBurst");
                //Probably play explosion SFX, too
            }
        }
    }
    //isn't called yet
    public void BlockBacktracking()
    {
        backtrackCollider.SetActive(true);
        //sets a collider to on 
    }
}
