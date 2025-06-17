using System;
using UnityEngine;
using UnityEngine.UI;

public class MouseSettings : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    
    private float minSensitivity;
    private float maxSensitivity;

    private void Start()
    {
        minSensitivity = SettingsManager.MinSensitivity;
        maxSensitivity = SettingsManager.MaxSensitivity;
        
        
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
