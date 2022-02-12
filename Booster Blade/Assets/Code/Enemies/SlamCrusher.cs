using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class SlamCrusher : MonoBehaviour
{
    public float moveSpeed = 0;
    [Range(0, 1)]
    public float leeway;

    [SerializeField]
    private float crushRechargeTime = 1;
    //sprivate Vector3 movementVelValue;
    [SerializeField]
    private CrushSense crushSense;
    private Rigidbody2D rb;

    private BoxCollider2D crushCollider;
    private bool isMoving;
    private bool isAbleToMoveAgain;
    [SerializeField]
    private LayerMask crusherMask;


    private CinemachineImpulseSource shakeSource;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        isMoving = false;
        isAbleToMoveAgain = true;
        shakeSource = GetComponent<CinemachineImpulseSource>();
        crushCollider = GetComponent<BoxCollider2D>();
    }
    //only stop if crusher is actually moving in that direction


    public void WakeCrusher()
    {

        GetComponent<DashTrail>().SetEnabled(true);
        crushSense.ActivateSense(true);
    }

    public void StartCrushing(float attackAngle)
    {
        if (!isMoving && isAbleToMoveAgain)
        {
            crushSense.ActivateSense(false);
            Debug.Log("Attempting to crush");
            transform.rotation = Quaternion.Euler(0, 0, attackAngle);
            rb.velocity = transform.right * moveSpeed;
            isMoving = true;
        }

    }
    public void EndCrushing()
    {
        shakeSource.GenerateImpulse();
        Debug.Log("Stopping movement.");
        rb.velocity = Vector3.zero;
        isMoving = false;
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
            StartCrushing(90);
        }
        else if( relPosition.x > 0 && LeewayCheck(relPosition.y))
        {
            StartCrushing(0);
        }
        else if (LeewayCheck(relPosition.x) && relPosition.y < 0)
        {
            StartCrushing(270);
        }
        else if (relPosition.x < 0 && LeewayCheck(relPosition.y))
        {
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
    private void FixedUpdate()
    {
        if (isMoving) { IsTouchingWall(); }
    
    }

    public IEnumerator SlamShockwave()
    {
        isAbleToMoveAgain = false;
        EndCrushing();
        yield return new WaitForSeconds(crushRechargeTime);
        isAbleToMoveAgain = true;
    }
    public bool IsTouchingWall()
    {
        float extraHeightTest = 0.1f;
        //RaycastHit2D raycastHit2D = Physics2D.BoxCast(crushCollider.bounds.center, crushCollider.bounds.size, 0f, transform.right, crushCollider.bounds.extents.x + extraHeightTest, crusherMask);
        Color rayColor;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(crushCollider.bounds.center, transform.right, 1 + extraHeightTest, crusherMask);

       // RaycastHit2D raycastHit2D = Physics2D.BoxCast(crushCollider.bounds.center, crushCollider.bounds.size, 0f, transform.right, crushCollider.bounds.extents.x + extraHeightTest, crusherMask);
        if (raycastHit2D.collider != null)
        {
            rayColor = Color.blue;
        }
        else
        {
            rayColor = Color.red;
        }

        Debug.DrawRay(crushCollider.bounds.center*1, transform.right, rayColor);
        //Debug.DrawRay(crushCollider.bounds.center + new Vector3(crushCollider.bounds.extents.x, 0), Vector2.right *( crushCollider.bounds.extents.y + extraHeightTest), rayColor);
       // Debug.DrawRay(crushCollider.bounds.center + new Vector3(crushCollider.bounds.extents.x, 0), transform.right * (crushCollider.bounds.extents.y + extraHeightTest), rayColor);
        //Debug.DrawRay(crushCollider.bounds.center - new Vector3(crushCollider.bounds.extents.x, 0), Vector2.right * (crushCollider.bounds.extents.y + extraHeightTest), rayColor);
        //Debug.DrawRay(crushCollider.bounds.center + new Vector3(0, crushCollider.bounds.extents.y), Vector2.right * (crushCollider.bounds.extents.y + extraHeightTest), rayColor);
        if (raycastHit2D.collider != null)
        {

            StartCoroutine(SlamShockwave());
            Debug.Log("Hit wall");
            return true;
        }
        else
        {
            return false;
        }
     

    }
}
