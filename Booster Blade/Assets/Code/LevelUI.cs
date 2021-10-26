using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelUI : MonoBehaviour
{
    [SerializeField]
    SuperTextMesh healthtext;

    [SerializeField]
    Animator titlecardanim;

    [SerializeField]
    GameObject deathBG;

    [SerializeField]
    Slider staminaSlider;

    [SerializeField]
    SuperTextMesh timer;
    public void Awake()
    {
        deathBG.SetActive(false);
    }
    public void ClearTitleCard()
    {
        titlecardanim.Play("titlecard_clearaway");
    }

    public void UpdateHPHUD(int currentHP)
    {
        healthtext.text = "HP: " + currentHP.ToString();
    }
    public void StartDeathUI()
    {
        deathBG.SetActive(true);
    }

    public void SetStaminaSlider(float staminaVal)
    {
        staminaSlider.value = staminaVal;
        //staminaSlider;
    }
    public void SetTimer(float timerValue)
    {
        //trunctuates to two decimal places

        timer.text = "Time: " + timerValue.ToString("F2");
    }
}
