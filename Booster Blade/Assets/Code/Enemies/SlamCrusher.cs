using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamCrusher : MonoBehaviour
{
    public float moveSpeed = 0;
    [Range(0, 1)]
    public float leeway;
    //sprivate Vector3 movementVelValue;
    [SerializeField]
    private CrushSense crushSense;
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
            EndCrushing();
        }
  
    }
    public void WakeCrusher()
    {
        GetComponent<DashTrail>().SetEnabled(true);
        crushSense.ActivateSense(true);
       // StartCrushing(90f);
    }

    public void StartCrushing(float attackAngle)
    {
        transform.rotation = Quaternion.Euler(0, 0, attackAngle) ;
        rb.velocity = transform.right * moveSpeed;
    }
    public void EndCrushing()
    {
        rb.velocity = Vector3.zero;
        crushSense.ActivateSense(true); //probably will have short delay until activated again
    }
    //quick debug text?
    //don't need to check for distance thanks to the collider itself
    public void CheckPosition(Transform playerTrans)
    {
        Vector3 enemyPos = transform.position;
        Vector3 playerPos = playerTrans.position;
        Vector3 relPosition = (playerPos - enemyPos).normalized;
        ///compare enemy and player position
        ///
        if(LeewayCheck(relPosition.x) && relPosition.y > 0 )
        {
            crushSense.ActivateSense(false);
            Debug.LogWarning("RIGHT ABOVE, BABY! " + relPosition);
            StartCrushing(90);
        }
        else if( relPosition.x > 0 && LeewayCheck(relPosition.y))
        {
            crushSense.ActivateSense(false);

            StartCrushing(0);
        }
        else if (LeewayCheck(relPosition.x) && relPosition.y < 0)
        {
            crushSense.ActivateSense(false);
            Debug.LogWarning("RIGHT ABOVE, BABY! " + relPosition);
            StartCrushing(270);
        }
        else if (relPosition.x < 0 && LeewayCheck(relPosition.y))
        {
            crushSense.ActivateSense(false);

            StartCrushing(180);
        }
    }
    public bool LeewayCheck(float positionVa)
    {
        if(positionVa >= -leeway && positionVa <= leeway)
        {
            return true;
        }
        return false;
    }
}
