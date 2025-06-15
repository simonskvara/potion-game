using System;
using UnityEngine;

public class IngredientItem : MonoBehaviour
{
    public Ingredient ingredientType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cauldron"))
        {
            other.gameObject.GetComponent<Cauldron>().AddIngredient(ingredientType, gameObject);
        }
    }
}
