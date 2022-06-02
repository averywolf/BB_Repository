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
    SuperTextMesh zoneNumberText;

    [SerializeField]
    SuperTextMesh levelDialogue;


    [SerializeField]
    Animator titlecardanim;

    [SerializeField]
    GameObject deathBG;
    [SerializeField]
    SuperTextMesh deathText;

    [SerializeField]
    GameObject dPlayer;
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
    public CollectIcon collectibleTracker;

    public Animator dPlayerAnim;

    [SerializeField]
    private Canvas levelCan;
    [HideInInspector]
    public bool evilTimer = false;
    private Canvas uiCanvas;
    public void Awake()
    {
        uiCanvas = GetComponent<Canvas>();
        titlecardanim.gameObject.SetActive(true);
        
        deathBG.SetActive(false);
        dPlayer.SetActive(false);
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
    public void EvilTimer()
    {
        timer.color = Color.red;
    }

    public void ClearTitleCard()
    {
        titlecardanim.Play("titlecard_clearaway");
    }

    public void UpdateHPHUD(int currentHP)
    {
        healthtext.text = "HP: " + currentHP.ToString();
    }
    public void ShowHideAllUI()
    {
        if (uiCanvas.enabled)
        {
            uiCanvas.enabled = false;
        }
        else
        {
            uiCanvas.enabled = true;
        }
    }
    public void StartDeathUI(bool timeout)
    {
        if (timeout)
        {
            deathText.text = "TIME OVER";
        }
        else
        {
            deathText.text = "GAME OVER";
        }
        deathBG.SetActive(true);
        dPlayer.SetActive(true);
        SetDeathPlayer();
    }
    public void SetDeathPlayer()
    {
      //  RectTransformUtility.WorldToScreenPoint(Camera.main, )
         //dPlayerAnim.gameObject.GetComponent<RectTransform>().anchoredPosition = LevelManager.instance.GetPlayerController().playerBody.transform.position;
        //dPlayerAnim.gameObject.GetComponent<RectTransform>().anchoredPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, LevelManager.instance.GetPlayerController().playerBody.transform.position);

      

        //RectTransform myRect = dPlayerAnim.gameObject.GetComponent<RectTransform>();
        //Vector2 positionOfPlayer= LevelManager.instance.GetPlayerController().playerBody.transform.position;
        //Vector2 myPositionOnScreen = Camera.main.WorldToScreenPoint(positionOfPlayer);
        
        //Canvas copyOfMainCanvas = levelCan;
        //Debug.Log("World space of player= (x= " + positionOfPlayer.x + ", y= "+positionOfPlayer.y+")");
        //Debug.Log("Screen space of player= (x= " + myPositionOnScreen.x + ", y= " + myPositionOnScreen.y + ")");

        //float scaleFactor = copyOfMainCanvas.scaleFactor;
        //Vector2 finalPosition = new Vector2(myPositionOnScreen.x / scaleFactor, myPositionOnScreen.y / scaleFactor);
        //myRect.anchoredPosition = finalPosition;
        SetDeathAnimation(LevelManager.instance.GetPlayerController().currentAngleForDeathAnim);


    }
    public void SetDeathAnimation(float angleToRotate)
    {
        dPlayer.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, angleToRotate);
        dPlayerAnim.SetFloat("heroDirection", angleToRotate);
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
        if (evilTimer)
        {
            timer.text = "<j>Time: " + FormatTime(timerValue);
        }
        else
        {
            timer.text = "Time: " + FormatTime(timerValue);
        }

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
    public void SetZoneText(string message)
    {
        zoneNumberText.text = message;
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
        collectibleTracker.PickIcon(LevelManager.instance.currentLevelIndex);
        if (found)
        {
            collectibleTracker.IconOn();

        }

    }
}
