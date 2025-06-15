using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IngredientContainer : MonoBehaviour, IInteractable
{
    [SerializeField] private string ingredientsName;
    public string IngredientsName => ingredientsName;

    [SerializeField] private List<GameObject> ingredientsToSpawn;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private Outline outline;

    private void Start()
    {
        DisableOutline();
    }

    public void Interact()
    {
        SpawnIngredient();
    }

    public string GetDescription()
    {
        return ingredientsName;
    }

    private void SpawnIngredient()
    {
        if (ingredientsToSpawn.Count != 0)
        {
            GameObject ingredient = ingredientsToSpawn[Random.Range(0, ingredientsToSpawn.Count)];
            Instantiate(ingredient, spawnPoint.position, Quaternion.identity);
        }
    }

    public void EnableOutline()
    {
        if (outline != null)
        {
            outline.enabled = true;
        }
    }

    public void DisableOutline()
    {
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    private void OnMouseExit()
    {
        DisableOutline();
    }
}
