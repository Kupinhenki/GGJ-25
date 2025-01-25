using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public void PlayEventSounds(string eventName)
    {
        AudioManager.Instance.PlaySoundFromAnimationEvent(eventName);
    }
}
