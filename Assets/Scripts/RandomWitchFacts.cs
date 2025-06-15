using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWitchFacts : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI factText;

    [SerializeField] private List<string> facts;

    private void OnEnable()
    {
        string fact = facts[Random.Range(0, facts.Count)];
        factText.text = fact;
    }
}
