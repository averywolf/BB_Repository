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
            FadeToColor(0, 0.5f);
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
    public void FadeToColor(float fadeToValue, float fadingTime)
    {
        if (fadeToValue == 0) //probably better way
        {
            flatColor.color =  new Color(1,1,1,1);
        }
        Debug.Log("Should fade!");
        LeanTween.alpha(flatColor.rectTransform, fadeToValue, fadingTime);
    }
}
