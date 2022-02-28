using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CutscenePanel : MonoBehaviour
{
    public UnityEvent OnPanelShow;

    //types WaitTime WaitForSignal
    public bool fadeToNext;
    public float stayTime; //how long this panel stays on screen before proceeding to the next panel

    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }
    public void HidePanel()
    {
        gameObject.SetActive(false);
    }
}
