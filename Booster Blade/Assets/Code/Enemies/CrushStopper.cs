using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushStopper : MonoBehaviour
{
    [SerializeField]
    private SlamCrusher slamCrusher;
    BoxCollider2D boxCollider2D;
    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false; // will set to false instead, using this for testing
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            slamCrusher.EndCrushing();
            Debug.Log("Touched Stopper");
        }


    }
    public void EnableStopper(bool stopperOn)
    {

        boxCollider2D.enabled = stopperOn;
    }
}
