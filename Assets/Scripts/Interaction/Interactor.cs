using System;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private InputSystem_Actions inputSystem;

    private void Awake()
    {
        inputSystem = new InputSystem_Actions();
    }

    private void Start()
    {
        uiManager = UIManager.Instance;
    }

    private void OnEnable()
    {
        inputSystem.Player.Enable();
        inputSystem.Player.Interact.started += TryInteract;
    }

    private void OnDisable()
    {
        inputSystem.Player.Interact.started -= TryInteract;
        inputSystem.Player.Disable();
    }

    private void Update()
    {
        // enabling and disabling outline, has to be a bit more complicated since there are multiple objects one can outline
        Ray r = new Ray(interactorSource.position, interactorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                // Skip if it's the same interactable
                if (interactObj == currentInteractable) 
                {
                    // Only enable outline if the object is still valid
                    if (IsInteractableValid(currentInteractable))
                    {
                        currentInteractable.EnableOutline();
                    }
                    return;
                }

                // if it's not the same
                // Clear previous interactable if valid
                if (IsInteractableValid(currentInteractable))
                {
                    currentInteractable.DisableOutline();
                }

                // Set new interactable
                currentInteractable = interactObj;
                uiManager.UpdateInteractionDescription(interactObj.GetDescription());
                
                // Enable outline only if valid
                if (IsInteractableValid(currentInteractable))
                {
                    currentInteractable.EnableOutline();
                }
                return;
            }
        }
        
        // Clear current interactable if nothing is hit
        if (IsInteractableValid(currentInteractable))
        {
            currentInteractable.DisableOutline();
            uiManager.UpdateInteractionDescription("");
        }
        currentInteractable = null;
    }

    private void TryInteract(InputAction.CallbackContext context)
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
    
    private bool IsInteractableValid(IInteractable interactable)
    {
        // Check if the interface reference points to a destroyed Unity object
        return interactable != null && !interactable.Equals(null);
    }
    
}
