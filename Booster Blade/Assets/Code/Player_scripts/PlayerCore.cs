using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    public PlayerController playerController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.GetComponent<StandardPickup>())
        {           
            Debug.Log("Picked up the thing!");
            collision.GetComponent<StandardPickup>().PickUpPickup();
        }

        if (collision.GetComponent<LevelExitDoor>())
        {
            Debug.Log("Should go through door to next level!");
            {
                collision.GetComponent<LevelExitDoor>().GoThroughExitDoor();
            }
        }
        //doesnt' work yet
        if (collision.GetComponent<DangerHitbox>())
        {
            playerController.HurtPlayer(collision.gameObject);
        }
        if (collision.GetComponent<BoostPanel>())
        {
            playerController.SetFacingDirection(collision.GetComponent<BoostPanel>().GetBoostDirection());
      
            //StartCoroutine(playerController.DirectionChangeDelay(0.3f));
            playerController.BeginBoost(1, false);

        }
        if(collision.GetComponent<Checkpoint>())
        {
            collision.GetComponent<Checkpoint>().RegisterCheckpoint();
        }
        if (collision.GetComponent<BasicButton>())
        {
            collision.GetComponent<BasicButton>().PressButton();
        }
        //if(touching danger object) danger object has an on damage player event?
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
     
    }
}
