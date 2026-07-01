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

    private PotionEffect currentPotionDisplayed;

    public void Setup(PotionEffect potion)
    {
        currentPotionDisplayed = potion;
        potionTitleText.text = potion.DisplayName;
        potionRiddleText.text = potion.Description;

        if (potion.Icon != null)
            potionImage.sprite = potion.Icon;

        if (PotionDiscovery.Instance.IsEffectDiscovered(potion))
        {
            string[] ingredients = GetIngredients(potion);
            ingredient1Text.text = ingredients[0];
            ingredient2Text.text = ingredients[1];
            ingredient3Text.text = ingredients[2];
        }
        else
        {
            ingredient1Text.text = "???";
            ingredient2Text.text = "???";
            ingredient3Text.text = "???";
        }
    }

    public void Refresh()
    {
        if (currentPotionDisplayed != null)
        {
            Setup(currentPotionDisplayed);
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
