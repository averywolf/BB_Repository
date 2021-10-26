using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private int checkPointID = 0;
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
            SaveManager.instance.activeSave.RegisterRespawnPoint(gameObject.transform.position, SceneManager.GetActiveScene().name);
            SaveManager.instance.Save();
        }

    }
    //might have specific starting angles?
}
