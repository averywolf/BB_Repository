using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class CreditsSystem : MonoBehaviour
{
    public string sceneToGoTo;
    public string musicToPlay;
    public void Start()
    {
        AudioManager.instance.PlayMusic(musicToPlay);
    }
    private void Update()
    {
        ///MAKE THIS REFER TO PLAYER INPUT INSTEAD
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {

            ReturnToTitle();

        }
    }
    public void ShowFinalResults()
    {

    }
    public void ReturnToTitle()
    {
        SceneManager.LoadScene(sceneToGoTo);
    }
}
