using System;
using System.Collections.Generic;
using UnityEngine;

public class IngredientContainer : MonoBehaviour, IInteractable
{
    [SerializeField] private string ingredientsName;
    public string IngredientsName => ingredientsName;

    [SerializeField] private List<GameObject> ingredientsToSpawn;


    public void Interact()
    {
        
    }

    public string GetDescription()
    {
        return ingredientsName;
    }
}
