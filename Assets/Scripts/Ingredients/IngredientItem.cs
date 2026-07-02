using System;
using UnityEngine;

public class IngredientItem : InteractableBase
{
    public Ingredient ingredient;

    [Header("Don't Touch")] 
    [SerializeField] private ObjectGrabbable grabbable;

    private void Start()
    {
        if (!forceDescription)
            description = ingredient.IngredientName;

        grabbable.OnGrabbed.AddListener(() => SetIsInteractable(false));
        grabbable.OnDropped.AddListener(() => SetIsInteractable(true));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cauldron"))
        {
            other.gameObject.GetComponent<Cauldron>().AddIngredient(ingredient, gameObject);
            grabbable.DisableGrabbing();
            grabbable.Drop();
        }
    }
}
