using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelCutsceneSystem : MonoBehaviour
{
    public List<CutscenePanel> cutscenePanels;

    public int currentPanel; //current id of panel displayed

    public void Awake()
    {
        for (int i = 0; i < cutscenePanels.Count; i++)
        {
            cutscenePanels[i].gameObject.SetActive(false);
        }
    }
    public void PanelTransition()
    {

    }
    public void SkipCutscene()
    {

    }
    public void Start()
    {
        AudioManager.instance.PlayMusic("Mus_Intro");
        StartCoroutine(TestPanelProcess());
    }

    public IEnumerator TestPanelProcess()
    {
        Debug.Log("Starting cutscene.");
        for (int i = 0; i < cutscenePanels.Count; i++)
        {
            cutscenePanels[i].ShowPanel();
            yield return new WaitForSeconds(cutscenePanels[i].stayTime);
            cutscenePanels[i].HidePanel();
        }
        Debug.Log("Done with cutscene.");
    }
}
