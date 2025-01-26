using UnityEngine;
using UnityEngine.EventSystems;

public class SelectSounds : MonoBehaviour, ISelectHandler
{
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.Instance;
    }

    public void OnSelect(BaseEventData eventData)
    {
        PlaySelectSound();
    }

    public void PlaySelectSound()
    {
        if(audioManager == null)
        {
            audioManager = AudioManager.Instance;
        }
        audioManager.PlaySoundFromAnimationEvent("Select");
    }

    public void PlayClickSound()
    {
        if (audioManager == null)
        {
            audioManager = AudioManager.Instance;
        }
        audioManager.PlaySoundFromAnimationEvent("Plop");
    }
}
