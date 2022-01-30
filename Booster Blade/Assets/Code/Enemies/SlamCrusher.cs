using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamCrusher : MonoBehaviour
{
    public float moveSpeed = 0;
    //sprivate Vector3 movementVelValue;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Hit wall!");
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z + 180); ;
            rb.velocity = Vector3.right * moveSpeed;
        }
  
    }
    public void WakeCrusher()
    {
        GetComponent<DashTrail>().SetEnabled(true);
        rb.velocity = Vector3.right * moveSpeed;
    }
}
