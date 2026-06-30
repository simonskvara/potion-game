using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    private const string MasterKey = "MasterVolume";
    private const string MusicKey = "MusicVolume";
    private const string SFXKey = "SFXVolume";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            LoadVolumes();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMasterVolume(float volume)
    {
        SetVolume(MasterKey, volume);
        PlayerPrefs.SetFloat(MasterKey, volume);
    }

    public void SetMusicVolume(float volume)
    {
        SetVolume(MusicKey, volume);
        PlayerPrefs.SetFloat(MusicKey, volume);
    }

    public void SetSFXVolume(float volume)
    {
        SetVolume(SFXKey, volume);
        PlayerPrefs.SetFloat(SFXKey, volume);
    }

    private void SetVolume(string parameter, float volume)
    {
        if (volume <= 0)
            audioMixer.SetFloat(parameter, -80f);
        else
            audioMixer.SetFloat(parameter, Mathf.Log10(volume) * 20);
    }

    private void LoadVolumes()
    {
        SetMasterVolume(PlayerPrefs.GetFloat(MasterKey, 1f));
        SetMusicVolume(PlayerPrefs.GetFloat(MusicKey, 1f));
        SetSFXVolume(PlayerPrefs.GetFloat(SFXKey, 1f));
    }
}
