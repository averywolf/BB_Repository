using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChecker : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    [HideInInspector]
    public BoxCollider2D boxCollider2D;

    public bool wallCheckerSetOff = false;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void TemporarilyDisableChecker()
    {
     //   Debug.Log("Colliderdisabled");
        boxCollider2D.enabled = false;
    }
    public void ReEnableChecker()
    {
        boxCollider2D.enabled = true;
      //  Debug.Log("ColliderIsBackNow");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") &&!wallCheckerSetOff)
        {
           
            if (playerController.isReversing == false)
            {
              //  Debug.Log("Hit wall!");
                AudioManager.instance.Play("WallBounce");
                playerController.isReversing = true; //is it best to set this here?
                playerController.ReverseDirection();
            }
        }
    }
}
