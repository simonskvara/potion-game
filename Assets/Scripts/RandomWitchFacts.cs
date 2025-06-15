using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomWitchFacts : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI factText;
    [SerializeField] private List<string> facts;
    [SerializeField] private float interval = 5f;

    private float timer;
    private int currentIndex;

    private void OnEnable()
    {
        if (facts.Count == 0) return;

        currentIndex = Random.Range(0, facts.Count);
        ShowFact(currentIndex);
        timer = interval;
    }

    private void Update()
    {
        if (facts.Count == 0) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            currentIndex = (currentIndex + 1) % facts.Count;
            ShowFact(currentIndex);
            timer = interval;
        }
    }

    private void ShowFact(int index)
    {
        factText.text = facts[index];
    }
}
