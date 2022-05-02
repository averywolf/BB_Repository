using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class Outro : MonoBehaviour
{
    public string sceneToGoTo;
    // Start is called before the first frame update
    [SerializeField]
    private string cutsceneSong = "";
    public Animator outroAnim;

    public void Start()
    {
        Time.timeScale = 1;
        AudioManager.instance.PlayMusic(cutsceneSong);
        ReadOutro();
    }
    public void ReadOutro()
    {
        outroAnim.Play("outro");
    }

    public void SkipCutscene()
    {
        Debug.Log("Done with cutscene.");
        SceneManager.LoadScene(sceneToGoTo);
    }
}
