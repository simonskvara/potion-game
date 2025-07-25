using System;
using System.Collections.Generic;
using UnityEngine;

public class PotionDiscovery : MonoBehaviour
{
    public static PotionDiscovery Instance;
    
    private HashSet<PotionEffect> discoveredEffects = new HashSet<PotionEffect>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadDiscoveredEffects();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void LoadDiscoveredEffects()
    {
        // Load from PlayerPrefs
        string discoveredKey = "DiscoveredPotionEffects";
        if (PlayerPrefs.HasKey(discoveredKey))
        {
            string data = PlayerPrefs.GetString(discoveredKey);
            if (!string.IsNullOrEmpty(data))
            {
                string[] effects = data.Split(',');
                foreach (string effect in effects)
                {
                    if (System.Enum.TryParse(effect, out PotionEffect parsedEffect))
                    {
                        discoveredEffects.Add(parsedEffect);
                    }
                }
            }
        }
    }
    
    private void SaveDiscoveredEffects()
    {
        List<string> effects = new List<string>();
        foreach (PotionEffect effect in discoveredEffects)
        {
            effects.Add(effect.ToString());
        }
        PlayerPrefs.SetString("DiscoveredPotionEffects", string.Join(",", effects));
        PlayerPrefs.Save();
    }
    
    public bool IsEffectDiscovered(PotionEffect effect)
    {
        return discoveredEffects.Contains(effect);
    }
    
    public void DiscoverEffect(PotionEffect effect)
    {
        if (!discoveredEffects.Contains(effect))
        {
            discoveredEffects.Add(effect);
            SaveDiscoveredEffects();
            PotionBook.Instance.RevealRecipe(effect);
        }
    }
}
