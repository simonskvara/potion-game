using System;
using UnityEngine;

public class IngredientItem : MonoBehaviour
{
    public Ingredient ingredientType;

    [Header("Don't Touch")] 
    [SerializeField] private ObjectGrabbable grabbable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cauldron"))
        {
            other.gameObject.GetComponent<Cauldron>().AddIngredient(ingredientType, gameObject);
            grabbable.DisableGrabbing();
            grabbable.Drop();
        }
    }
}
