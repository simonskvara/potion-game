using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IngredientContainer : InteractableBase
{
    [SerializeField] private List<GameObject> ingredientsToSpawn;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private float waitBetweenSpawn;
    private bool canSpawm = true;

    public override void Interact()
    {
        SpawnIngredient();
    }

    private void SpawnIngredient()
    {
        if (!canSpawm) return;

        if (ingredientsToSpawn.Count != 0)
        {
            GameObject ingredient = ingredientsToSpawn[Random.Range(0, ingredientsToSpawn.Count)];
            Instantiate(ingredient, spawnPoint.position, gameObject.transform.rotation);
            StartCoroutine(Wait());
        }
    }

    private IEnumerator Wait()
    {
        canSpawm = false;
        yield return new WaitForSeconds(waitBetweenSpawn);
        canSpawm = true;
    }
}
