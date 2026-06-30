using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipes", menuName = "Potion/Recipes")]
public class Recipes : ScriptableObject
{
    public List<PotionRecipe> recipes = new List<PotionRecipe>();

    /// <summary>
    /// The distinct, player-discoverable effects across all recipes (Transform effects only —
    /// excludes Reset and the "Nothing"/slop result). Multiple recipes may share one effect.
    /// </summary>
    public IEnumerable<PotionEffect> DiscoverableEffects()
    {
        return recipes
            .Select(recipe => recipe.ResultPotionEffect)
            .Where(effect => effect != null && effect.Kind == PotionEffectKind.Transform)
            .Distinct();
    }
}


[System.Serializable]
public class PotionRecipe
{
    public PotionEffect ResultPotionEffect => potionEffect;

    public Ingredient[] ingredients;

    [BoxGroup("Potion Effect")]
    [SerializeField]
    private PotionEffect potionEffect;

    public bool MatchesRecipe(List<Ingredient> input)
    {
        return new HashSet<Ingredient>(ingredients).SetEquals(input);
    }
}
