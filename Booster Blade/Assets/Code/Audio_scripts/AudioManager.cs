using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    // Singleton class that stores all of the sounds used in the game, and allows other scripts to play them at any time.

    [SerializeField]
    private List<AudioSourceController> audioControllerPool = new List<AudioSourceController>();

    public static AudioManager instance;

    //volume percentages, to be adjusted in-game
    public float mvol = 1;
    //private float evol=1;
    [SerializeField]
    private AudioSourceController currentMusicTrack;
    private string currentSongName = "";
    public AudioMixer mixer; //unsure if this is used

    //will this assignment persist?
    public AudioMixerGroup MasterGroup;
    public AudioMixerGroup MusicGroup;
    public AudioMixerGroup SFXGroup;

    public string musicParam; //forget wthat this does

    public event EventHandler DisplaySongNameEvent;

    public List<GameObject> audioGroups;
    [SerializeField]
    private GameObject audioSourceController;
    public enum MixerGroupType //the type of mixer group
    {
        Master,
        Music,
        SFX,
    }

    /// <summary>
    /// Call this in most cases to play audio.
    /// All you need is the exact name of the AudioData you're calling, and AudioManager will search for it across all of its audiogroups.
    /// </summary>
    /// <param name="name"></param>
    public AudioSourceController Play(string audioName)
    {
        return Play(SearchAudioGroups(audioName));
    }
    public AudioData SearchAudioGroups(string audioName)
    {
        AudioData audData = null;
        for (int i = 0; i < audioGroups.Count; i++)
        {
            audData = audioGroups[i].GetComponent<AudioGroup>().SearchForAudio(audioName);
            if (audData != null)
            {
                return audData;
            }
        }
        Debug.LogWarning("Sound + " + audioName + " not found across all AudioGroups.");
        return null;
    }

    //might be irrelevant?
    public AudioSourceController SearchForController(AudioData audioData)
    {
        for (int i = 0; i < audioControllerPool.Count; i++)
        {
            //if (audioControllerPool[i].audioData != null && audioControllerPool[i].Equals(audioData))
            if (!audioControllerPool[i]._claimed)
            {
                return audioControllerPool[i];
            }
        }

        return null;
    }
    
    public void PutController(AudioSourceController controller)
    {

        if (audioControllerPool.Contains(controller) == false)

            audioControllerPool.Add(controller);

    }
    //Play at position

    public AudioSourceController Play(AudioData audioData)
    {
        
        if (audioData == null)
        {
            return null;
        }
        AudioSourceController audioController = null;

        audioController = SearchForController(audioData);
        if (audioController == null)
        {
            //PLEASE update this debug text, it makes zero sense

           // Debug.Log("Apparently no audcontroller with " + audioData + " exists.");

            GameObject go = Instantiate(audioSourceController, transform);
            //go.GetComponent<AudioSourceController>() = audioSourceController;
            audioController = go.GetComponent<AudioSourceController>();
            //audioController = go.AddComponent<AudioSourceController>();

            //sfx group used for testing

        }
        if (!audioData.isMusic)
        {
            audioController.SetSourceProperties(audioData.Clip, audioData.Volume, audioData.Pitch, audioData.Loop, audioData.SpacialBlend, SFXGroup);
        }
        else
        {
            audioController.SetSourceProperties(audioData.Clip, audioData.Volume, audioData.Pitch, audioData.Loop, audioData.SpacialBlend, MusicGroup);
            if (currentMusicTrack != null)
            {
                currentMusicTrack.Stop();
            }
            currentMusicTrack = audioController;
            currentSongName = audioData.name;
            SetMusicPitch(1); //restting this is probably a good idea
        }

        audioController.Play();

        //if audioData is music, override the current track

        return audioController;
    }
    public void MusicPitchSetTest(float pitchValue)
    {
        if (currentMusicTrack != null)
        {
            currentMusicTrack.SetSourcePitch(pitchValue);
        }
    }
    //does not work
    public void PauseMusic(bool pause)
    {   
        if(currentMusicTrack != null)
        {
            if (pause)
            {         
                currentMusicTrack.PauseSource(true);
            }
            else
            {
              
                currentMusicTrack.PauseSource(false);
            }
           
        }
        else
        {
            Debug.LogWarning("No current music track available to pause/resume");
        }
  
    }

    //might be outdated
    public AudioSourceController PlayMusic(string songName)
    {
        AudioSourceController musicController = Play(SearchAudioGroups(songName));
        if (SearchAudioGroups(songName).isMusic)
        {
            //currentSong.name = name;
            DisplaySongNameEvent?.Invoke(this, EventArgs.Empty);
        }
        return musicController;
    }

    //public void PlayMusic(string name)
    //{
    //    if (currentSong == null) //if the current sound has not been assigned (null check inside, too)
    //    {
    //        Debug.Log("Setting up the song!");
    //        currentSong = Array.Find(musicPlaylist, sound => sound.name == name);

    //        if (currentSong == null) //null exception check
    //        {
    //            Debug.LogWarning("Music: " + name + " not found!");
    //            return;
    //        }
    //        currentSong.source.Play();
    //        //built-in null check
    //        DisplaySongNameEvent?.Invoke(this, EventArgs.Empty);
    //        return;
    //    }
    //    currentSong.source.Stop(); //stops the song that's playing to replace it with the other one

    //    currentSong = Array.Find(musicPlaylist, sound => sound.name == name);
    //    if (currentSong == null) //null exception check
    //    {
    //        Debug.LogWarning("Music: " + name + " not found!");
    //        return;
    //    }
    //    currentSong.source.Play();
    //    DisplaySongNameEvent?.Invoke(this, EventArgs.Empty);
    //}

    //public AudioSourceController Play(AudioData audioData)
    //{
    //    if (audioData == null)
    //    {
    //        return null;
    //    }
    //    AudioSourceController audioController = null;

    //    audioController = SearchForController(audioData);
    //    if (audioController == null)
    //    {
    //        Debug.Log("Apparently no audcontroller with " + audioData + " exists.");
    //        GameObject go = new GameObject("AudioControllerEX");
    //        audioController = go.AddComponent<AudioSourceController>();
    //        //sfx group used for testing
    //        audioController.SetSourceProperties(audioData.Clip, audioData.Volume, audioData.Pitch, audioData.Loop, audioData.SpacialBlend, SFXGroup);
    //    }

    //    if (audioData.isMusic)
    //    {
    //        if (currentMusicTrack != null)
    //        {
    //            currentMusicTrack.Stop();
    //            currentMusicTrack = audioController;

    //        }

    //    }
    //    audioController.Play();

    //    //if audioData is music, override the current track

    //    return audioController;
    //}


    void Awake() //sets up the list of sounds and makes sure the audio manager is a singleton
    {
        if (instance == null)
        {
            instance = this;
        }
        else //if there's already an instance of AudioManager in the scene
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject); // lets the audio manager persist between scenes

        //Goes through the list of sounds and assigns an audioclip to each of them with the appropriate parameters

        //CreateAudioSources(sounds, 1, SFXGroup);
        //CreateAudioSources(musicPlaylist, 1, MusicGroup);
    }

    //function that sets up the sounds in each list
    
    //sets up the source's audio mixer. hopefully. 
    //not sure if assigning the Mixer publicly is a good idea, but I don't know my other options
    private void AssignMixer(AudioSource source, AudioMixerGroup mixgroup)
    {
        source.outputAudioMixerGroup = mixgroup;
    }

    //might need to take in an argument to see if you make the music card appear


    //both of these scripts are just for testing
    //pausing audio is handled by pausing the audio listener, in the pause menu script


    //need to look into this, should work now. 
    public void StopMusic()
    {
        if (currentMusicTrack != null)
        {
            currentMusicTrack.Stop();
        }
        currentSongName = "";
    }

    //displays the name of the song. might edit later to display the "true" song name
    public string GetSongName()
    {
        return currentSongName;
    }

    //No longer works for me
    //effectly serves as master volume changer
    public void MusicVolumeChanged(float sliderVal)
    {
        //mvol = sliderVal;
        //foreach (Sound m in musicPlaylist)
        //{
        //    m.source.volume = m.baseVolume * mvol;
        //}
    }
    public void SoundVolumeChanged(float sliderVal)
    {
        //evol = sliderVal;
        //foreach (Sound s in sounds)
        //{
        //    s.source.volume = s.baseVolume * evol;
        //}
    }

    //functions that set the volume of the individual mixers
    public void SetMusicVolume(float value)
    {
        MusicGroup.audioMixer.SetFloat("MusVolume", Mathf.Lerp(-80, 0, value));
    }
    public void SetSoundVolume(float value)
    {
        MusicGroup.audioMixer.SetFloat("SFXVolume", Mathf.Lerp(-80, 0, value));
    }
    public void SetMasterVolume(float value)
    {
        MusicGroup.audioMixer.SetFloat("MasterVolume", Mathf.Lerp(-80, 0, value));
    }

    public void SetMusicPitch(float value)
    {

        //if (currentMusicTrack != null)
        //{
        //    currentMusicTrack.SetSourcePitch(value);
        //}
        //lerping might not be necessary

        //MusicGroup.audioMixer.SetFloat("MusicPitch", value);
    }

    public void FadeCurrentBGM(float fadeDuration, float targetVolumeAmount)
    {
        StartCoroutine(currentMusicTrack.FadeAudioSource(fadeDuration, targetVolumeAmount));
    }

    //unsure what specific effect, but this is called on game over in Bullet Catcher
    public void ActivateGameOverFX()
    {
        MusicGroup.audioMixer.SetFloat("MusicPitch", 0.6f);
    }
    //called by BugFightManager and HubManager on start, resets music to normal
    public void ResetAudioFX()
    {
        MusicGroup.audioMixer.SetFloat("MusicPitch", 1f);
    }
}
