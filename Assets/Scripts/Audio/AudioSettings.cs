using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Header("Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        SetupSliders();
    }

    private void OnEnable()
    {
        RefreshSliders();
    }

    void SetupSliders()
    {
        masterSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.RemoveAllListeners();

        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        masterSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
        musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);

        AudioManager.Instance.SetMasterVolume(masterSlider.value);
        AudioManager.Instance.SetMusicVolume(musicSlider.value);
        AudioManager.Instance.SetSFXVolume(sfxSlider.value);
    }

    void RefreshSliders()
    {
        UpdateSlider(masterSlider, PlayerPrefs.GetFloat("MasterVolume", 1f));
        UpdateSlider(musicSlider, PlayerPrefs.GetFloat("MusicVolume", 1f));
        UpdateSlider(sfxSlider, PlayerPrefs.GetFloat("SFXVolume", 1f));
    }

    void UpdateSlider(Slider slider, float volume)
    {
        slider.value = volume;
    }
}
