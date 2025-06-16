using System;
using UnityEngine;
using UnityEngine.UI;

public class MouseSettings : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private float minSensitivity = 10f;
    [SerializeField] private float maxSensitivity = 200f;

    private void Start()
    {
        sensitivitySlider.minValue = minSensitivity;
        sensitivitySlider.maxValue = maxSensitivity;
        
        sensitivitySlider.value = SettingsManager.GetMouseSensitivity();
        
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
    }
    
    private void OnSensitivityChanged(float value)
    {
        SettingsManager.SetMouseSensitivity(value);
    }
    
    private void OnDestroy()
    {
        // Clean up listeners
        sensitivitySlider.onValueChanged.RemoveListener(OnSensitivityChanged);
    }
}
