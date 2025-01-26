using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        Instance = null;
    }
#endif

    public AudioClip[] jumpClips;
    public AudioClip[] shootClips;
    public AudioClip[] ghostClips;
    public AudioClip[] deathClips;
    public AudioClip[] bubbleBounceClips;
    public AudioClip[] scoreClips;
    public AudioClip[] plopClips;
    public AudioClip[] selectClips;

    /// <summary>
    /// Reference to the audio mixer.
    /// </summary>
    public AudioMixer mixer;

    /// <summary>
    /// A pool of audio sources.
    /// </summary>
    [ContextMenuItem("Test audio source", "TestAudio")]
    public List<AudioSource> soundSourcePool = new List<AudioSource>();
    
    /// <summary>
    /// Enables pitch variation for sounds.
    /// </summary>
    public bool variatePitch = true;

    public AudioMixerGroup masterGroup;
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup soundGroup;


    /// <summary>
    /// Singleton initialization
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    
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
    private void PlaySound(AudioClip clip)
    {
        foreach(AudioSource source in soundSourcePool)
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
        soundSourcePool.Add(audioSource);
        audioSource.outputAudioMixerGroup = soundGroup;
        audioSource.pitch = variatePitch ? VariatePitch() : 1;
        audioSource.PlayOneShot(clip);
    }

    private void PlayRandomSound(AudioClip[] clips)
    {
        int index = Random.Range(0, clips.Length);
        PlaySound(clips[index]);
    }

    /// <summary>
    /// Variates the pitch for an audio source.
    /// </summary>
    /// <returns></returns>
    private float VariatePitch()
    {
        return Random.Range(0.9f, 1.1f);
    }

    public void PlaySoundFromAnimationEvent(string arrayName)
    {
        switch (arrayName)
        {
            case "Jump":
                PlayRandomSound(jumpClips);
                break;
            case "Shoot":
                PlayRandomSound(shootClips);
                break;
            case "Ghost":
                PlayRandomSound(ghostClips);
                break;
            case "Death":
                PlayRandomSound(deathClips);
                break;
            case "BubbleBounce":
                PlayRandomSound(bubbleBounceClips);
                break;
            case "Score":
                PlayRandomSound(scoreClips);
                break;
            case "Plop":
                PlayRandomSound(plopClips);
                break;
            case "Select":
                PlayRandomSound(selectClips);
                break;
            default:
                break;
        }
    }

    // Test function
    private void TestAudio()
    {
        PlayRandomSound(jumpClips);
    }
}
