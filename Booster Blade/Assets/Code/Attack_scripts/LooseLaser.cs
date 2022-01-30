using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseLaser : BulletMovement
{
    //should have lasercollision script? maybe?
    public Vector2 fullSize = new Vector2(1, 1);

    private void Awake()
    {
        StandardInitialization(); //maybe use base.method instead?
    }
    private void OnEnable()
    {
        StandardSetup();//should wait to set velocity until afterwards
        StartCoroutine(ExpandLaserToFull());
    }
    //need bool to see if you want to set duration based on velocity

    private IEnumerator ExpandLaserToFull()
    {
        float duration = initialVelocity / 100;
        //float duration = 1;
        float currentTime = 0.0f;
        Vector2 startSize = new Vector2(0, 0);
        if (duration != 0)
        {
            while (currentTime <= duration)
            {
                //bulTransform.localScale = new Vector2(1, 1);
                bulTransform.localScale = Vector2.Lerp(startSize, fullSize, currentTime / duration);
                currentTime += Time.deltaTime;
                yield return null;
            }
        }

        SetVelocity(initialVelocity);
    }

    //should never disappear when hitting walls

    //CreateLooseLaser (xcoord, ycoord, speed, angle, length, width, graphic, delay)
    //returns object id of loose laser

    //delay is the time in frames before the laser will appear. during its delay, there will be a collisionless cloud that appears where the laser will spawn

}
