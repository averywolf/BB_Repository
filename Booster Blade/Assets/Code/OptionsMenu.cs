using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class OptionsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject optionsButton;//uses this to reselect past button when exiting

    public GameObject closeButton;
    private void Awake()
    {
        gameObject.SetActive(false);   
    }
    public void OptionsOpen()
    {
        SetNewFirstSelected(closeButton);
        gameObject.SetActive(true);
        PlayButtonClickSFX();

    }
    public void OptionsClose()
    {
        gameObject.SetActive(false);
        SetNewFirstSelected(optionsButton);
        PlayButtonClickSFX();
    }
    private void SetNewFirstSelected(GameObject firstSelection)
    {
        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(firstSelection);
        firstSelection.GetComponent<Button>().Select();
        //firstPauseButton.GetComponent<Button>().Select();
    }
    public void PlayButtonClickSFX()
    {
        AudioManager.instance.Play("UIButtonClick");
    }
    public void SliderSetMusicVolume(float value)
    {
        AudioManager.instance.SetMusicVolume(value);
    }
}
