using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGroup : MonoBehaviour
{
    /// <summary>
    /// A prefab used to hold multiple audioDatas at once
    /// That way, different sets of audio (music, things associated with a certain character)
    /// can be organized more easily, seperate from each other.
    /// Make sure that AudioManager stores all of them
    /// </summary>
    public List<AudioData> audioDatas;

    //Called for each AudioGroup in AudioManager while searching for the data with the name requested.
    public AudioData SearchForAudio(string name)
    {
        for (int i = 0; i < audioDatas.Count; i++)
        {
            if (audioDatas[i].audioName.Equals(name))
            {
                return audioDatas[i];
            }

        }
        //this audiogroup does not have the audiodata requested.
        return null;
    }


}
