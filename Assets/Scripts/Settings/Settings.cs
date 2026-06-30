using UnityEngine;

public class Settings : MonoBehaviour
{
    public void ResetPotionRecipes()
    {
        if (SaveManager.Data.UnlockedPotions != null)
        {
            SaveManager.Data.UnlockedPotions.Clear();
            SaveManager.Save();
        }
    }
}