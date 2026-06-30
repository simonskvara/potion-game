using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class PotionDiscovery : MonoBehaviour
{
    public static PotionDiscovery Instance;

    [Tooltip("Source of truth for the set of discoverable effects.")]
    [SerializeField] private Recipes recipesSO;

    private List<string> discoveredEffects = new List<string>();

    public UnityEvent AllPotionsDiscovered;

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
        discoveredEffects = SaveManager.Data.UnlockedPotions;
    }

    public bool IsEffectDiscovered(PotionEffect effect)
    {
        return discoveredEffects.Contains(effect.PotionEffectID);
    }

    public void DiscoverEffect(PotionEffect effect)
    {
        if (!discoveredEffects.Contains(effect.PotionEffectID))
        {
            discoveredEffects.Add(effect.PotionEffectID);
            SaveManager.Data.UnlockedPotions = discoveredEffects;
            SaveManager.Save();

            // Book UI is optional for now — guard so discovery/save works without it.
            if (PotionBook.Instance != null) PotionBook.Instance.RevealRecipe(effect);

            int numberOfPotionEffects = recipesSO.DiscoverableEffects().Count();
            int numberOfDiscoveredEffect = discoveredEffects.Count;

            if (numberOfDiscoveredEffect == numberOfPotionEffects)
            {
                Debug.LogWarning("Discovered All Effect");
                AllPotionsDiscovered?.Invoke();
            }
        }
    }

    [Button("Discover All Effects")]
    private void SetupForAllDiscovery()
    {
        foreach (PotionEffect effect in recipesSO.DiscoverableEffects())
        {
            if (!SaveManager.Data.UnlockedPotions.Contains(effect.PotionEffectID))
            {
                SaveManager.Data.UnlockedPotions.Add(effect.PotionEffectID);
            }
        }
        SaveManager.Save();
    }
}
