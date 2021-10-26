using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    public Transform playerTarget;
    // Start is called before the first frame update
    public Rigidbody2D enemyRb;
    void Update()
    {
        Vector3 direction = playerTarget.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x)//returns a value in radians--the angle 
            * Mathf.Rad2Deg; //multiply by this to convert to degrees
        transform.rotation = Quaternion.Euler(0, 0, angle);

    }
    private void FixedUpdate()
    {
        enemyRb.velocity = transform.right * 4;   
    }
}
