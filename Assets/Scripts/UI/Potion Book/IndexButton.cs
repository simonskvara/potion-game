using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IndexButton : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private TextMeshProUGUI label;

    public void Setup(string text, Action onClick)
    {
        label.text = text;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick());

        gameObject.name = text;
    }
}
