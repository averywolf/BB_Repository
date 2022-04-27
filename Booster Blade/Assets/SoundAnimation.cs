using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAnimation : MonoBehaviour
{
  public void PlayCreditsMusic()
    {
        AudioManager.instance.PlayMusic("CreditsTheme");
    }
}
