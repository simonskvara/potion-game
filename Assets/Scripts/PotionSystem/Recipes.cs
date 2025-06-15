using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipes", menuName = "Potion/Recipes")]
public class Recipes : ScriptableObject
{
    public List<PotionRecipe> recipes = new List<PotionRecipe>();
}
