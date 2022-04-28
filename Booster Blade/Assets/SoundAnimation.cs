using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAnimation : MonoBehaviour
{
    public CreditsSystem creditsSystem;
    public void EndOfCredits()
    {
        creditsSystem.EndTheCredits();
    }

  public void PlayCreditsMusic()
    {
        AudioManager.instance.PlayMusic("CreditsTheme");
    }

}
