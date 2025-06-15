using System;
using UnityEngine;

interface IInteractable
{
    public void Interact();
    public string GetDescription();
    public void EnableOutline();
    public void DisableOutline();
}

/// <summary>
/// Sorry for the bad handling of it, don't like the optimizations either
/// </summary>
public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform interactorSource;
    [SerializeField] private float interactRange;
    
    private IInteractable currentInteractable;

    private UIManager uiManager;

    private void Start()
    {
        uiManager = UIManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryInteract();
        }
        
        Ray r = new Ray(interactorSource.position, interactorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                if (interactObj != currentInteractable)
                {
                    uiManager.UpdateInteractionDescription(interactObj.GetDescription());
                    currentInteractable = interactObj;
                }
                interactObj.EnableOutline();
                return;
            }
        }
        
        if (currentInteractable != null)
        {
            uiManager.UpdateInteractionDescription("");
            currentInteractable = null;
        }
    }

    private void TryInteract()
    {
        Ray r = new Ray(interactorSource.position, interactorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                interactObj.Interact();
            }
        }
    }
    
}
