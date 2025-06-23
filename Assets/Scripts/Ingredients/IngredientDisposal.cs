using System;
using UnityEngine;
using UnityEngine.Events;

public class IngredientDisposal : MonoBehaviour
{
    public UnityEvent OnDisposal;

    public GameObject poofEffect;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ingredient"))
        {
            OnDisposal?.Invoke();
            
            other.GetComponent<ObjectGrabbable>().Drop();

            if (poofEffect != null)
            {
                GameObject poof = Instantiate(poofEffect, other.transform.position, Quaternion.identity);
                Destroy(poof, 5f);
            }
            
            
            Destroy(other.gameObject);
        }
    }
}
