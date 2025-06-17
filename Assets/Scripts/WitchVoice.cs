using UnityEngine;

public class WitchVoice : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] voiceClips;
    [SerializeField] private float minInterval = 5f;
    [SerializeField] private float maxInterval = 15f;

    private float timer;

    private void Start()
    {
        ScheduleNextPlay();
    }

    private void Update()
    {
        if (voiceClips.Length == 0 || audioSource == null)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            PlayRandomVoice();
            ScheduleNextPlay();
        }
    }

    private void PlayRandomVoice()
    {
        int index = Random.Range(0, voiceClips.Length);
        audioSource.clip = voiceClips[index];
        audioSource.Play();
    }

    private void ScheduleNextPlay()
    {
        timer = Random.Range(minInterval, maxInterval);
    }
}
