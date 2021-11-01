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
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

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

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
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
            FadeToColor(0, 3);
        }
    }

    public void PlayTransitionAnimation(string animName)
    {
        transAnim.Play(animName);
    }
    public float GetAnimationTime()
    {
        return transAnim.GetCurrentAnimatorStateInfo(0).length;
    }
    public IEnumerator FightTransitionProcess(int sceneID)
    {
        //yield return new WaitForSeconds(0.5f);
        PlayTransitionAnimation("FadeToBlack");
        startingFight = true;
        AudioManager.instance.StopMusic();
        yield return new WaitForSeconds(transAnim.GetCurrentAnimatorStateInfo(0).length);

        Debug.LogWarning("LOADING FIGHT");
        SceneManager.LoadScene(sceneID);


    }
    //need to freeze player when this is happening;
    public IEnumerator TransitionToIntermission(string sceneName)
    {

        //playerfreeze
        FadeToColor(1, 3);
        fadingToIntermission = true;
        yield return new WaitForSeconds(3);
        
        SceneManager.LoadScene(sceneName);
    }
    public void FadeToColor(float fadeToValue, float fadingTime)
    {
        if (fadeToValue == 0) //probably better way
        {
            flatColor.color =  new Color(0,0,0,1);
        }
        Debug.Log("Should fade!");
        LeanTween.alpha(flatColor.rectTransform, fadeToValue, fadingTime);
    }
}
