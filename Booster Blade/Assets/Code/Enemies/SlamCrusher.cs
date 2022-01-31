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
            //screenshake?
            //StartCrushing(-transform.rotation.z);
 
        }
  
    }
    public void WakeCrusher()
    {
        GetComponent<DashTrail>().SetEnabled(true);
        StartCrushing(90f);
    }

    public void StartCrushing(float attackAngle)
    {
        transform.rotation = Quaternion.Euler(0, 0, attackAngle) ;
        rb.velocity = transform.right * moveSpeed;
    }
    public void EndCrushing()
    {
        rb.velocity = Vector3.zero;
    }
}
