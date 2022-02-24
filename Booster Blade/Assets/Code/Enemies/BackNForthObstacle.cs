using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackNForthObstacle : MonoBehaviour
{
    //Note: these drones appear to get off-sync easily

    private Rigidbody2D obstacleRB;

    [SerializeField]
    private float moveSpeed = 5f;

    private float turnDelayTime = 0.5f;

    private Vector2 droneVelocity;

    public float currentFacingAngle;
    private void Awake()
    {
        obstacleRB = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        currentFacingAngle = transform.eulerAngles.z;    
    }

    public void TestFunction()
    {
       // Debug.LogWarning("A BEYBLADE WITH 3 BITBEASTS HAS BEEN FOUND.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            obstacleRB.velocity = new Vector2(0,0);

            ReverseDirection();
        }
    }
    //called OnStartOfLevel
    public void StartMoving()
    {
        droneVelocity = transform.right * moveSpeed;
        obstacleRB.velocity = droneVelocity;
    }
    private void ReverseDirection()
    {
        currentFacingAngle += 180;
        transform.rotation = Quaternion.Euler(0, 0, currentFacingAngle);
        StartMoving();
    }
}
