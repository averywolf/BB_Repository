using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PanelCutsceneSystem : MonoBehaviour
{
    public List<CutscenePanel> cutscenePanels;
    public string sceneToGoTo;
    public int currentPanel; //current id of panel displayed

    public Image panelFader;
    [SerializeField]
    private string cutsceneSong = "";
    public void Awake()
    {
        for (int i = 0; i < cutscenePanels.Count; i++)
        {
            cutscenePanels[i].gameObject.SetActive(false);
        }
    }

    public void SkipInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SkipCutscene();
        }
    }
    public void SkipCutscene()
    {
        StopAllCoroutines();
        Debug.Log("Done with cutscene.");
        SceneManager.LoadScene(sceneToGoTo);
    }
    public void FadeTransition(float fadeToValue, float fadingTime)
    {
        LeanTween.alpha(panelFader.rectTransform, fadeToValue, fadingTime);
    }
    public void Start()
    {
        Time.timeScale = 1;
        if(cutsceneSong != "")
        {
            AudioManager.instance.PlayMusic(cutsceneSong);
        }
        StartCoroutine(TestPanelProcess());
    }

    public IEnumerator TestPanelProcess()
    {
        Debug.Log("Starting cutscene.");

        for (int i = 0; i < cutscenePanels.Count; i++)
        {
            cutscenePanels[i].ShowPanel();
            if (i!=0 && !cutscenePanels[i - 1].cutImmediately)
            {
                
                FadeTransition(0, 0.2f); //fade to black
                yield return new WaitForSeconds(0.2f);
               
            }
            
            yield return new WaitForSeconds(cutscenePanels[i].stayTime);

            if (!cutscenePanels[i].cutImmediately) {
                FadeTransition(1, 0.2f);// fade to white
                yield return new WaitForSeconds(0.2f);
            }
         
            cutscenePanels[i].HidePanel();
            
        }

        Debug.Log("Done with cutscene.");
        AudioManager.instance.StopMusic();
        SceneManager.LoadScene(sceneToGoTo);
    }
    public void PlayIntroSong()
    {
        AudioManager.instance.Play("IntroTheme");
    }
    public void DramaticMusicStop()
    {
        AudioManager.instance.StopMusic();
    }
}
