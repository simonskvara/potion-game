using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IngredientContainer : MonoBehaviour, IInteractable
{
    [SerializeField] private string ingredientsName;

    [SerializeField] private List<GameObject> ingredientsToSpawn;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private Outline outline;

    [SerializeField] private float waitBetweenSpawn;
    private bool canSpawm = true;

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
        if (!canSpawm) return;
        
        if (ingredientsToSpawn.Count != 0)
        {
            GameObject ingredient = ingredientsToSpawn[Random.Range(0, ingredientsToSpawn.Count)];
            Instantiate(ingredient, spawnPoint.position, Quaternion.identity);
            StartCoroutine(Wait());
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

    private IEnumerator Wait()
    {
        canSpawm = false;
        yield return new WaitForSeconds(waitBetweenSpawn);
        canSpawm = true;
    }
}
