using UnityEngine;

public class SoundLoop : MonoBehaviour
{
    [SerializeField] private AudioSource[] audioSources;

    private int currentIndex = 0;

    private void Start()
    {
        if (audioSources.Length == 0)
            return;

        PlayNext();
    }

    private void PlayNext()
    {
        if (audioSources[currentIndex] != null)
            audioSources[currentIndex].Play();

        float clipLength = audioSources[currentIndex].clip.length;
        currentIndex = (currentIndex + 1) % audioSources.Length;

        Invoke(nameof(PlayNext), clipLength);
    }
}
