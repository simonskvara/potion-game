using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Cauldron : MonoBehaviour
{
    [SerializeField] private Recipes recipesSO;
    
    [SerializeField] private Transform potionSpawnPoint;
    [SerializeField] private GameObject potionPrefab;
    
    private List<Ingredient> currentIngredients = new List<Ingredient>();

    private List<GameObject> ingredientObjects = new List<GameObject>();
    
    public void AddIngredient(Ingredient ingredient, GameObject ingredientObject)
    {
        if (currentIngredients.Count >= 3)
        {
            Destroy(ingredientObject);
            return;
        }
        
        currentIngredients.Add(ingredient);
        ingredientObjects.Add(ingredientObject);
        Debug.Log("added ingredient");
        
        if (currentIngredients.Count == 3)
        {
            StartCoroutine(BrewPotion());
        }
    }
    
    private IEnumerator BrewPotion()
    {
        yield return new WaitForSeconds(1f); // Brewing delay
        
        PotionEffect effect = FindMatchingEffect();
        SpawnPotion(effect);
        currentIngredients.Clear();


        foreach (GameObject ingredient in ingredientObjects)
        {
            Destroy(ingredient);
        }
        ingredientObjects.Clear();
    }
    
    private PotionEffect FindMatchingEffect()
    {
        foreach (PotionRecipe recipe in recipesSO.recipes)
        {
            if (recipe.MatchesRecipe(currentIngredients))
            {
                return recipe.resultEffect;
            }
        }
        return PotionEffect.None;
    }
    
    private void SpawnPotion(PotionEffect effect)
    {
        GameObject potion = Instantiate(potionPrefab, 
            potionSpawnPoint.position, 
            potionSpawnPoint.rotation);
        
        potion.GetComponent<Potion>().Initialize(effect);
    }
}
