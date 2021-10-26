using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class AudioData : ScriptableObject
{
    public string audioName = "sound";

    public AudioClip Clip;

    [Range(0, 1)]
    public float Volume = 1f;

    [Range(.25f, 3)]
    public float Pitch = 1f;

    public bool Loop = false;

    [Range(0f, 1f)]
    public float SpacialBlend = 1f;

    public bool isMusic = false;

}
