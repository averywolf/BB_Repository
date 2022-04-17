using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField]
    SuperTextMesh healthtext;

    [SerializeField]
    SuperTextMesh actTitleText;

    [SerializeField]
    SuperTextMesh levelDialogue;


    [SerializeField]
    Animator titlecardanim;

    [SerializeField]
    GameObject deathBG;

    [SerializeField]
    Slider staminaSlider;

    [SerializeField]
    SuperTextMesh timer;

    [SerializeField]
    Image staminaFill;

    [SerializeField]
    Color fullChargeColor;
    [SerializeField]
    Color chargineColor;
    public GameObject notifBox;
    public SuperTextMesh smallNotification;
    public static LevelUI instance;

    public Coroutine notifProcess;
    public Coroutine dialogueProcess;
    //might make itz unique clazz
    public GameObject collectibleTracker;

    public void Awake()
    {
        deathBG.SetActive(false);
        collectibleTracker.SetActive(false);
        notifBox.SetActive(false);
        staminaSlider.value = 1;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        levelDialogue.gameObject.SetActive(false);
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
    public void StaminaFlash(bool flashOn)
    {
        if (flashOn)
        {
            staminaFill.color = fullChargeColor;
        }
        else
        {
            staminaFill.color = chargineColor;
        }
    }
    public void SetTimer(float timerValue)
    {
        //trunctuates to two decimal places

        //timer.text = "Time: " + timerValue.ToString("F2");
        //TimeSpan timeToDisplay
        //timer.text = "Time: " + timerValue.ToString(@"hh\:mm\:ss");
        timer.text = "Time: " + FormatTime(timerValue);
    }
    private string FormatTime(float time)
    {
        int intTime = (int)time;
        int minutes = intTime / 60;
        int seconds = intTime % 60;
        float fraction = time * 1000;
        fraction = (fraction % 1000);
        string timeText = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, fraction);
        return timeText;
    }
    public void SetActText(string message)
    {
        actTitleText.text = message;
    }
    public void DialogueFade()
    {
        //doesn't quite work yet
        // levelDialogue.GetComponent<SuperTextMesh>().style= FontStyle.Bold;
        // levelDialogue.GetComponent<SuperTextMesh>().color = Color.blue;

      //  levelDialogue.gameObject.SetActive(false);
        levelDialogue.Unread();
        //LeanTween.alpha(levelDialogue.gameObject, 0, 2);
    }
    public void SayLevelDialogue(string dialogueLine)
    {
        levelDialogue.text = dialogueLine;
        if(dialogueProcess != null)
        {
            StopCoroutine(dialogueProcess);
        }
        levelDialogue.gameObject.SetActive(true);
        //need to add function that waits until text is finished
        dialogueProcess = StartCoroutine(DialogueProcess());
    }
    public void NotificationFade()
    {
        //will add better animation later
   
        smallNotification.UnRead();

    }
    //might lump in to levelDialogue system
    public void DisplaySmallNotification(string notification)
    {
        Debug.Log("Should be notification");
        if (notifProcess != null)
        {
            StopCoroutine(notifProcess);
        }
        
        notifBox.SetActive(true);
        smallNotification.text = notification;
        StartCoroutine(NotificationProcess());
       // Invoke("NotificationFade", 3);
    }
    public IEnumerator NotificationProcess()
    {
        while (smallNotification.reading)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        NotificationFade();
    }
    public IEnumerator DialogueProcess()
    {
        while (levelDialogue.reading)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        DialogueFade();
    }
    public void DisplayCollectible(bool found)
    {
        collectibleTracker.SetActive(found);
    }
}
