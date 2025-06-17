using UnityEngine;

public class Settings : MonoBehaviour
{
    public void ResetPotionRecipes()
    {
        PlayerPrefs.DeleteKey("DiscoveredPotionEffects");
        PlayerPrefs.Save();
    }
}
