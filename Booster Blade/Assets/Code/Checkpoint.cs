using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Checkpoint : MonoBehaviour
{
    [SerializeField, Header("Make sure this is different for every checkpoint in the current scene")]
    public int checkPointID = 0;
    [SerializeField]
    private Animator checkpointAnim;

    private bool isCheckpointActive=false;
    // Start is called before the first frame update
    void Start()
    {
        checkpointAnim.Play("checkP_inactive");
    }
    //if touching player
    public void RegisterCheckpoint()
    {
        if (isCheckpointActive == false)
        {
            Debug.Log("Registering checkpoint");
            checkpointAnim.Play("checkP_active");
            isCheckpointActive = true;
            
            //SaveManager.instance.activeSave.RegisterCheckPoint(gameObject.transform.position, SceneManager.GetActiveScene().name);
            SaveManager.instance.activeSave.RegisterCheckPoint(checkPointID, SceneManager.GetActiveScene().name);
            SaveManager.instance.Save();
        }

    }

    public PlayerController.PlayerDirection GetCheckpointDirection()
    {
        if (transform.eulerAngles.z == 90f)
        {
            return PlayerController.PlayerDirection.up;
        }
        else if (transform.eulerAngles.z == 180)
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
    //might have specific starting angles?
}
