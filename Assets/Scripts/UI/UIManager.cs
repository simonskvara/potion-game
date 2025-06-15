using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private TextMeshProUGUI interactionDescriptionText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateInteractionDescription(string description)
    {
        interactionDescriptionText.text = description;
    }
}
