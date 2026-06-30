using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Potions/Ingredient")]
public class Ingredient : ScriptableObject
{
    public string IngredientID => ingredientID;
    public string IngredientName => ingredientName;

    [BoxGroup("Ingredient Info")]
    [SerializeField]
    private string ingredientID;
    [BoxGroup("Ingredient Info")]
    [SerializeField]
    private string ingredientName;
}

