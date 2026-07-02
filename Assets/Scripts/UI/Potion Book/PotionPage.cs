using NaughtyAttributes;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionPage : MonoBehaviour
{
    [SerializeField]
    private Recipes recipes;

    [BoxGroup("References")]
    [SerializeField]
    private TextMeshProUGUI potionTitleText;
    [BoxGroup("References")]
    [SerializeField]
    private TextMeshProUGUI potionRiddleText;
    [BoxGroup("References")]
    [SerializeField]
    private Image potionImage;
    [BoxGroup("References")]
    [SerializeField]
    private TextMeshProUGUI ingredient1Text;
    [BoxGroup("References")]
    [SerializeField]
    private TextMeshProUGUI ingredient2Text;
    [BoxGroup("References")]
    [SerializeField]
    private TextMeshProUGUI ingredient3Text;

    [BoxGroup("Explicit Potions")]
    [SerializeField]
    private PotionEffect nonePotion;
    [BoxGroup("Explicit Potions")]
    [SerializeField]
    private PotionEffect resetPotion;

    private PotionEffect currentPotionDisplayed;

    public void Setup(PotionEffect potion)
    {
        currentPotionDisplayed = potion;
        potionTitleText.text = potion.DisplayName;
        potionRiddleText.text = potion.Description;

        if (PotionDiscovery.Instance.IsEffectDiscovered(potion))
        {
            if (potion.PotionEffectID == nonePotion.PotionEffectID)
            {
                ingredient1Text.text = "Nothing and Everything";
                ingredient2Text.text = "";
                ingredient3Text.text = "";

                return;
            }

            string[] ingredients = GetIngredients(potion);
            ingredient1Text.text = ingredients[0];
            ingredient2Text.text = ingredients[1];
            ingredient3Text.text = ingredients[2];

            SetPotionImage(potion.Icon);
        }
        else
        {
            ingredient1Text.text = "???";
            ingredient2Text.text = "???";
            ingredient3Text.text = "???";

            SetPotionImage(potion.IconSilhouette);
        }
    }

    public void Refresh()
    {
        if (currentPotionDisplayed != null)
        {
            Setup(currentPotionDisplayed);
        }
    }

    private void SetPotionImage(Sprite icon)
    {
        if (icon != null)
        {
            potionImage.sprite = icon;
            potionImage.gameObject.SetActive(true);
        }
        else
        {
            potionImage.gameObject.SetActive(false);
        }
    }

    private string[] GetIngredients(PotionEffect potion)
    {
        return recipes.AllRecipes
            .Where(recipe => recipe.ResultPotionEffect == potion)
            .SelectMany(recipe => recipe.Ingredients)
            .Select(ingredient => ingredient.IngredientName)
            .ToArray();
    }
}
