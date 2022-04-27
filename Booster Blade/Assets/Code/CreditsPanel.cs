using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CreditsPanel : MonoBehaviour
{
    public UnityEvent OnPanelShow;

    public float stayTime; //how long this panel stays on screen before proceeding to the next panel

    public void ShowPanel()
    {
        gameObject.SetActive(true);
        OnPanelShow.Invoke();
    }
    public void HidePanel()
    {
        gameObject.SetActive(false);
    }
    public void PanelPlaySFX(string name)
    {
        AudioManager.instance.Play(name);
    }
}
