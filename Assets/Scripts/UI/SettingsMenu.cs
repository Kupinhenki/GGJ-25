using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] Slider _masterVolumeSlider;
        [SerializeField] Slider _musicVolumeSlider;
        [SerializeField] Slider _soundVolumeSlider;

        void Start()
        {
            _masterVolumeSlider.SetValueWithoutNotify(AudioManager.Instance.normalizedMasterVolume);
            _musicVolumeSlider.SetValueWithoutNotify(AudioManager.Instance.normalizedMusicVolume);
            _soundVolumeSlider.SetValueWithoutNotify(AudioManager.Instance.normalizedSoundVolume);
            
            _masterVolumeSlider.onValueChanged.AddListener(delegate
            {
                AudioManager.Instance.SetMasterVolume(_masterVolumeSlider.value);
            });
            
            _musicVolumeSlider.onValueChanged.AddListener(delegate
            {
                AudioManager.Instance.SetMusicVolume(_musicVolumeSlider.value);
            });
            
            _soundVolumeSlider.onValueChanged.AddListener(delegate
            {
                AudioManager.Instance.SetSoundVolume(_soundVolumeSlider.value);
            });
        }
    }
}
