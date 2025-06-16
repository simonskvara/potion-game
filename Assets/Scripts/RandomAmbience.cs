using UnityEngine;

public class RandomAmbience : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float minInterval = 5f;
    [SerializeField] private float maxInterval = 15f;

    private float timer;

    private void Start()
    {
        ScheduleNextPlay();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            if (audioSource != null)
                audioSource.Play();

            ScheduleNextPlay();
        }
    }

    private void ScheduleNextPlay()
    {
        timer = Random.Range(minInterval, maxInterval);
    }
}
