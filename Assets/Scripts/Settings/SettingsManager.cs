using UnityEngine;
using UnityEngine.Events;

public static class SettingsManager
{
    public static UnityEvent<float> OnSensitivityChanged = new UnityEvent<float>();

    public static readonly float MinSensitivity = 10f;
    public static readonly float MaxSensitivity = 250f;
    
    public static void SetMouseSensitivity(float value)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save();
        
        OnSensitivityChanged?.Invoke(value);
    }
    
    public static float GetMouseSensitivity()
    {
        return PlayerPrefs.GetFloat("MouseSensitivity", 100f);
    }
}
