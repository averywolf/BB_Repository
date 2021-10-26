using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPanel : MonoBehaviour
{

    /// <summary>
    /// Currently doesn't work on multiples of these values
    /// </summary>
    /// <returns></returns>

    public PlayerController.PlayerDirection GetBoostDirection()
    {
        if (transform.eulerAngles.z == 90f)
        {
            return PlayerController.PlayerDirection.up;
        }
        else if(transform.eulerAngles.z == 180)
        {
            return PlayerController.PlayerDirection.left;
        }
        else if (transform.eulerAngles.z == 270)
        {
            return PlayerController.PlayerDirection.down;
        }
        else
        {
            return PlayerController.PlayerDirection.right;
        }

    }
}
