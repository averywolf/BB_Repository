using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    [SerializeField]
    public Rigidbody2D rb2d;

    public float ogZValue;

    private float scrollSpeed;
    public float testOGHorzPos;
    public Renderer sizeDeterminer;
    // Use this for initialization
    private void Awake()
    {    
        ogZValue = transform.position.z;
        testOGHorzPos = transform.position.x;
    }

    public void StartScrolling(float scrollingSpeed)
    {
     
        rb2d.velocity = new Vector2(scrollingSpeed, 0);
       // Debug.Log("Object has started scrolling with scrollspeed " + scrollingSpeed);
    }

}
