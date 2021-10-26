using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChecker : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    [HideInInspector]
    public BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void TemporarilyDisableChecker()
    {
        Debug.Log("Colliderdisabled");
        boxCollider2D.enabled = false;
    }
    public void ReEnableChecker()
    {
        boxCollider2D.enabled = true;
        Debug.Log("ColliderIsBackNow");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Hit wall!");
            playerController.ReverseDirection(playerController.lastplayerDirection);
            
            //movementVelValue=-movementVelValue;
            //StartCoroutine(TempStun());
            //playerRb.velocity = movementVelValue; //might instead set when changing facing directions
            //prevent quickly changing directions after?
        }
    }
}
