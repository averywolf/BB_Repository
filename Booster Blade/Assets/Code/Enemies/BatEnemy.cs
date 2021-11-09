using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class BatEnemy : MonoBehaviour
{
    Seeker seeker;
    AIDestinationSetter aIDestinationSetter;
    AIPath aIPath;

    [SerializeField]
    private Transform batGFX;
    [SerializeField]
    private float batMoveSpeed = 10;
    private void Awake()
    {
        aIPath = GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();
        aIDestinationSetter = GetComponent<AIDestinationSetter>();
        aIPath.canMove = false;
        //set acceleration, too?
        aIPath.maxSpeed = batMoveSpeed;
    }
    public void Start()
    {
        aIDestinationSetter.target = LevelManager.instance.GetPlayerController().transform; //might be better to grab playerCore transform?
    }
    public void StartBatMoving()
    {
        aIPath.canMove = true;
    }
    public void Update()
    {
        if(aIPath.desiredVelocity.x >= 0.005f)
        {
            batGFX.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (aIPath.desiredVelocity.x <= -0.005f)
        {
            batGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}
