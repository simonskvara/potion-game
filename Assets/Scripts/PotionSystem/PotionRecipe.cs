using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PotionRecipe
{
    public Ingredient[] ingredients;
    public PotionEffect resultEffect;

    public bool MatchesRecipe(List<Ingredient> input)
    {
        return new HashSet<Ingredient>(ingredients).SetEquals(input);
    }

}
