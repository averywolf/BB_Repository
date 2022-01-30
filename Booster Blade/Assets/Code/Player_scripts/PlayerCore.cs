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

        if (collision.GetComponent<DangerHitbox>())
        {
            //logic for hitting lasers works on the laser's end
            playerController.HurtPlayer(collision.gameObject);
        }
        //maybe one day Dangerhitbox and DangerBullet won't be separate
        if(collision.GetComponent<DangerBullet>())
        {
            playerController.HurtPlayer(collision.gameObject);
            if (!playerController.isInvincible)
            {
                collision.GetComponent<DangerBullet>().RemoveBullet();
            }
         
        }
        if (collision.GetComponent<BoostPanel>())
        {
            Vector2 boostDirection = playerController.DirectionConverter(collision.GetComponent<BoostPanel>().boostPanelTransform.eulerAngles.z);
            playerController.SetFacingDirection(boostDirection.x, boostDirection.y);
      
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
