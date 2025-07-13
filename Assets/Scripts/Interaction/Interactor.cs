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
    private IInteractable previousInteractable;

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
        
        Ray r = new Ray(interactorSource.position, interactorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                if (interactObj != currentInteractable)
                {
                    previousInteractable = currentInteractable;
                    uiManager.UpdateInteractionDescription(interactObj.GetDescription());
                    currentInteractable = interactObj;
                }
                previousInteractable?.DisableOutline();
                interactObj.EnableOutline();
                return;
            }
        }
        
        if (currentInteractable != null)
        {
            currentInteractable.DisableOutline();
            uiManager.UpdateInteractionDescription("");
            currentInteractable = null;
        }
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
    
}
