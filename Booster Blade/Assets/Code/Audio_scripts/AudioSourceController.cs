using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioSourceController : MonoBehaviour
{
    private AudioSource _source;
    private Transform _transform;
    private Transform _parentObject; //don't believe this does anything yet
    private float startSourceVolume; //this variable might be useful if I need to fade the same audio source in and out back to the point it started at
    public bool _claimed;
    public AudioData audioData;

    public int funValue = 0;
    private float defaultPitch = 1;
    public bool isSourcePaused = false;
    void Awake()
    {
        _transform = transform;
        _source = GetComponent<AudioSource>();

        if (_source == null)
        {
            _source = gameObject.AddComponent<AudioSource>();
        }
        //DontDestroyOnLoad(gameObject); //this seems to fix the bug where the source isn't found after reloading a level

    }

    public void SetSourceProperties(AudioClip clip, float volume, float pitch, bool loop, float spacialBlend, AudioMixerGroup mixgroup)
    {

        _source.clip = clip;

        _source.volume = volume;

        startSourceVolume = volume;

        _source.pitch = pitch;
        defaultPitch = pitch;
        _source.loop = loop;
      
        _source.spatialBlend = spacialBlend;

        AssignMixer(_source, mixgroup);
        _source.ignoreListenerPause = false; //testing

    }
    private void AssignMixer(AudioSource source, AudioMixerGroup mixgroup)
    {
        source.outputAudioMixerGroup = mixgroup;
    }

    public void Play()
    {
        _claimed = true;
        isSourcePaused = false;
        _source.Play();

    }
    //don't exactly know the purpose of putting it back in the pool, but Resetting it is important
    public void Stop()
    {
        _source.Stop();
        Reset();
        AudioManager.instance.PutController(this);
    }
    public void PauseSource(bool shouldPause)
    {
        if (shouldPause)
        {
            Debug.Log("Pausing current track.");
            isSourcePaused = true;
            _source.Pause();
        }
        else
        {
            Debug.Log("Unpausing current track.");
            _source.UnPause();
            isSourcePaused = false;
        }  
    }
    public void SetSourcePitch(float pitchval)
    {
        _source.pitch = pitchval;
    }
    public void SetSourcePitchDefault()
    {
        _source.pitch = defaultPitch;
    }
    //targetVolume amount is from 0 to 1
    public IEnumerator FadeAudioSource(float fadeDuration, float targetVolumeAmount)
    {
        float ogVolume = _source.volume;
        float endVolume = startSourceVolume / targetVolumeAmount;
        //currently does nothing
        float currentTime = 0;
        _source.volume = 0;
        //math clamp?
        while (currentTime <= fadeDuration)
        {
            _source.volume = Mathf.Lerp(ogVolume, targetVolumeAmount, currentTime / fadeDuration);
            currentTime += Time.deltaTime;
            yield return null;
        }

    }
    private void Reset()
    {
        _parentObject = null;
        _claimed = false;
    }

    void LateUpdate()
    {
        //NEEDS TO CHECK IF AUDIO LISTENER IS PAUSED
        //_source_isPlaying== false;
        //might be source of problems in future, it definitely tripped me up when figuring out pausing
        if (_claimed && _source.isPlaying == false && AudioListener.pause == false &&!isSourcePaused)
        {
            Stop();
            return;
        }
        if (_parentObject != null)
        {
            _transform.position = _parentObject.position;
        }
    }
}
