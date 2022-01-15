using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class MainMenu : MonoBehaviour
{
    public string firstLevelName="";
    public PlayerInput playerInput;
    private bool ableToInteractWithMenu=false;

    //no loading save yet



    // Start is called before the first frame update
    void Start()
    {
        ableToInteractWithMenu = true;
        AudioManager.instance.StopMusic();
        AudioManager.instance.PlayMusic("Occult");
        SaveManager.instance.hasNotBeganLevel = true; //just in case
    }

    public void Update()
    {
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            if (ableToInteractWithMenu)
            {
                ableToInteractWithMenu = false;
                SceneManager.LoadScene(firstLevelName);
            }
        }
    }
}
