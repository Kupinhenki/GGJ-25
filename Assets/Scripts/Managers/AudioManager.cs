using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the audio mixer.
    /// </summary>
    public AudioMixer mixer;

    /// <summary>
    /// A pool of audio sources.
    /// </summary>
    public List<AudioSource> soundPool = new List<AudioSource>();
    
    /// <summary>
    /// Enables pitch variation for sounds.
    /// </summary>
    public bool variatePitch = true;

    /// <summary>
    /// Modifies master volume based on slider value change.
    /// </summary>
    /// <param name="sliderValue"></param>
    public void SetMasterVolume(float sliderValue)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }

    /// <summary>
    /// Modifies music volume based on slider value change.
    /// </summary>
    /// <param name="sliderValue"></param>
    public void SetMusicVolume(float sliderValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }

    /// <summary>
    /// Modifies sound volume based on slider value change.
    /// </summary>
    /// <param name="sliderValue"></param>
    public void SetSoundVolume(float sliderValue)
    {
        mixer.SetFloat("SoundVolume", Mathf.Log10(sliderValue) * 20);
    }

    /// <summary>
    /// Plays a sound audio clip.
    /// </summary>
    /// <param name="clip">Audio clip to be played.</param>
    public void PlaySound(AudioClip clip)
    {
        foreach(AudioSource source in soundPool)
        {
            // If a source is not playing, use it and then exit the function
            if(!source.isPlaying)
            {
                source.pitch = variatePitch ? VariatePitch() : 1;
                source.PlayOneShot(clip);
                return;
            }
        }

        // No unused audio source was found, so create a new one
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        soundPool.Add(audioSource);
        audioSource.pitch = variatePitch ? VariatePitch() : 1;
        audioSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Variates the pitch for an audio source.
    /// </summary>
    /// <returns></returns>
    private float VariatePitch()
    {
        return Random.Range(0.9f, 1.1f);
    }
}
