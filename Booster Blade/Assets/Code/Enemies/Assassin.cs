using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : MonoBehaviour
{
    public AimedAttack testAimedAttack;
    // Start is called before the first frame update
    public float teleDistance=1;
    void Start()
    {

    }
    public IEnumerator AttemptToMurder()
    {
        PlayerController playControl = LevelManager.instance.GetPlayerController();
        transform.position = LevelManager.instance.GetPlayerTransform().position;
        transform.parent = LevelManager.instance.GetPlayerTransform();
        transform.localPosition = new Vector2(teleDistance, 0);
        float duration = 4;
        float currentTime = 0.0f;
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

        //forget why I do this instead of waittime
        while (currentTime <= duration)
        {
            Vector2 telePos = new Vector2(playControl.horizontal, playControl.vertical);
            transform.localPosition = -telePos * 5;
            currentTime += Time.fixedDeltaTime;

            yield return waitForFixedUpdate;
        }

        transform.parent = null;
        //might need rewrite
        Vector2 aimPoint= LevelManager.instance.GetPlayerTransform().position;
        yield return new WaitForSeconds(0.2f);
        testAimedAttack.FireAimed(transform.position, aimPoint);
        StartCoroutine(AssassinCooldown());
    }

    public void Teleport(float attackAngle)
    {
        //if (attackAngle == 0)
        //{
        //    transform.localPosition = new Vector2(teleDistance, 0);
        //}
        //else if(attackAngle == 90)
        //{
        //    transform.localPosition = new Vector2(0, teleDistance);
        //}
        //else if(attackAngle == 180)
        //{
        //    transform.localPosition = new Vector2(-teleDistance, 0);
        //}
        //else if(attackAngle == 270)
        //{
        //    transform.localPosition = new Vector2(0, -teleDistance);
        //}

    }
    public void BeginMurderAttempt()
    {
 
        StartCoroutine(AttemptToMurder());
    }
    public void AttackPlayer()
    {

    }
    public IEnumerator AssassinCooldown()
    {
        yield return new WaitForSeconds(3);
        BeginMurderAttempt();
    }
}
