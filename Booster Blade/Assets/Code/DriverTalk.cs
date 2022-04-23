using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverTalk : MonoBehaviour
{
    [SerializeField]
    private Animator driveAnim;
    public void ZariaTalk()
    {
        driveAnim.Play("ZariaTalk");
    }
    public void ManTalk()
    {
        driveAnim.Play("ManTalk");
    }
    public void ArchieTalk()
    {
        driveAnim.Play("ArchieTalk");
    }
}
