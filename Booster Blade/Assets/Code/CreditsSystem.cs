using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class CreditsSystem : MonoBehaviour
{
    public List<CreditsPanel> creditsPanels;
    public TotalResults totalResults;
    public string sceneToGoTo;
    public string musicToPlay;
    private bool atTotalResults;
    public Animator creditsAnim;
    public void Awake()
    {
        for (int i = 0; i < creditsPanels.Count; i++)
        {
            creditsPanels[i].gameObject.SetActive(false);
        }
        totalResults.HideTotalResults();
    }

    public void BeginCredits()
    {
        creditsAnim.Play("credits");
    }
    public void Start()
    {
        atTotalResults = false;
        Time.timeScale = 1;
        // AudioManager.instance.PlayMusic(musicToPlay);
        Invoke("BeginCredits", 0.3f);
   
       // StartCoroutine(CreditsProcess());
    }

    public void CreditsAdvanceInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(atTotalResults == true)
            ReturnToTitle();
        }
    }
    
    public void EndTheCredits()
    {
        ShowFinalResults();
    }
    public void ShowFinalResults()
    {
        creditsAnim.Play("hidecredits");
        atTotalResults = true;
        totalResults.DisplayRunResults();
    }
    public void ReturnToTitle()
    {
        SceneManager.LoadScene(sceneToGoTo);
    }
}
