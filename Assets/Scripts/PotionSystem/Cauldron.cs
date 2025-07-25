using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Cauldron : MonoBehaviour
{
    [SerializeField] private Recipes recipesSO;
    
    [SerializeField] private Transform potionSpawnPoint;
    [SerializeField] private GameObject potionPrefab;

    [SerializeField] private float brewingTime;
    
    private List<Ingredient> currentIngredients = new List<Ingredient>();

    private List<GameObject> ingredientObjects = new List<GameObject>();

    public UnityEvent ingredientAdded;
    public UnityEvent potionBrewEnded;
    
    public void AddIngredient(Ingredient ingredient, GameObject ingredientObject)
    {
        if (currentIngredients.Count >= 3)
        {
            Destroy(ingredientObject);
            return;
        }
        
        currentIngredients.Add(ingredient);
        ingredientObjects.Add(ingredientObject);
        Debug.Log("added: " + ingredient);
        
        ingredientAdded?.Invoke();
        
        if (currentIngredients.Count == 3)
        {
            StartCoroutine(BrewPotion());
        }
    }
    
    private IEnumerator BrewPotion()
    {
        yield return new WaitForSeconds(brewingTime); // Brewing delay
        
        PotionEffect effect = FindMatchingEffect();
        SpawnPotion(effect);
        currentIngredients.Clear();
        potionBrewEnded?.Invoke();

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
