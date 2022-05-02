using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;
    [SerializeField]
    private Animator transAnim;
    public bool startingFight = false;
    public Image flatColor;
    public bool fadingToIntermission = false;
    public bool interToStage = false;
    public bool dyingEffect = false;
    public Image tToLevel;
    public Image tDeath;
    private RectTransform transCanvas;
    private void Awake()
    {
        tToLevel.gameObject.SetActive(false);
        tDeath.gameObject.SetActive(false);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        transCanvas = GetComponent<RectTransform>();
    }
    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }



    public void PlayTransitionAnimation(string animName)
    {
        transAnim.Play(animName);
    }
    public float GetAnimationTime()
    {
        return transAnim.GetCurrentAnimatorStateInfo(0).length;
    }

    //need to freeze player when this is happening;
    public IEnumerator TransitionToIntermission(string sceneName)
    {

        //playerfreeze
        FadeToColor(1, 0.5f);
        fadingToIntermission = true;
        LevelManager.instance.GetPlayerController().FreezePlayer(true);
        yield return new WaitForSeconds(0.5f);
        
        SceneManager.LoadScene(sceneName);
    }
    public IEnumerator InterToStageOld(string sceneName)
    {
        FadeToColor(1, Color.red, 0.5f);
        interToStage = true;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }
    public IEnumerator InterToStage(string sceneName)
    {
        tToLevel.gameObject.SetActive(true);
        MoveUIThing(new Vector3(-380, 0, 0), new Vector3(0, 0, 0), tToLevel, 0.5f);
        interToStage = true;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }
    public IEnumerator Death(string sceneName)
    {
        tDeath.gameObject.SetActive(true);
        MoveUIThing(new Vector3(0, -340, 0), new Vector3(0, 0, 0), tDeath, 0.5f);
        dyingEffect = true;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }

    public void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Level Loaded");
        if (startingFight)
        {
            startingFight = false;
            PlayTransitionAnimation("FadeFromBlack");
            Debug.Log("loading loading loading");
        }
        if (fadingToIntermission)
        {
            fadingToIntermission = false;
            FadeToColor(0, 0.5f);
        }
        if (interToStage)
        {
            tToLevel.gameObject.SetActive(true);
            interToStage = false;
            //FadeToColor(0, Color.red, 0.5f);
            MoveUIThing(new Vector3(0, 0, 0), new Vector3(380, 0, 0), tToLevel, 0.5f);
            // FadeToColor(0, Color.red, 0.5f);
            Invoke("HideLevelTo", 0.5f);
        }
        if (dyingEffect)
        {
            MoveUIThing(new Vector3(0, 0, 0), new Vector3(0, 340, 0), tDeath, 0.5f);
            dyingEffect = false;
            Invoke("HideDeath", 0.5f);
        }
    }
    public void HideLevelTo()
    {
        HideImage(tToLevel);
    }
    public void HideDeath()
    {
        HideImage(tDeath);
    }
    public void HideImage(Image theImage)
    {
        theImage.gameObject.SetActive(false);
    }
    //make player yait until tranzition iz done?
    public void FadeToColor(float fadeToValue, float fadingTime)
    {
        if (fadeToValue == 0) //probably better way
        {
            flatColor.color =  new Color(1,1,1,1);
        }
        Debug.Log("Should fade!");
        LeanTween.alpha(flatColor.rectTransform, fadeToValue, fadingTime);
    }
    public void FadeToColor(float fadeToValue, Color color, float fadingTime)
    {
        if (fadeToValue == 0) //probably better way
        {
            flatColor.color = new Color(color.r, color.b, color.g, 1);
        }
        flatColor.color = new Color(color.r, color.b, color.g, 0); //might set alpha wrong
        Debug.Log("Colorfade");
        LeanTween.alpha(flatColor.rectTransform, fadeToValue, fadingTime);
    }

    public void MoveUIThing(Vector3 startPos, Vector3 endPos, Image whatImage, float movetime)
    {
        whatImage.rectTransform.SetParent(gameObject.GetComponent<RectTransform>(), false);
        whatImage.rectTransform.anchoredPosition = startPos;
        LeanTween.move(whatImage.rectTransform, endPos, movetime);
    }
}
