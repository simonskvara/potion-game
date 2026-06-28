using UnityEngine;
using UnityEngine.InputSystem;

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
        IInteractable hit = RaycastForInteractable();

        // Nothing changed: keep the current target as-is (its outline is already on).
        if (ReferenceEquals(hit, currentInteractable))
        {
            if (!IsInteractableValid(currentInteractable)) ClearCurrent();
            return;
        }

        // Target changed: drop the previous outline.
        if (IsInteractableValid(currentInteractable)) currentInteractable.DisableOutline();

        currentInteractable = hit;

        if (hit != null)
        {
            hit.EnableOutline();
            uiManager.UpdateInteractionDescription(hit.GetDescription());
        }
        else
        {
            uiManager.UpdateInteractionDescription("");
        }
    }

    private void TryInteract(InputAction.CallbackContext context)
    {
        RaycastForInteractable()?.Interact();
    }

    private IInteractable RaycastForInteractable()
    {
        Ray r = new Ray(interactorSource.position, interactorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange) &&
            hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
        {
            return interactObj;
        }

        return null;
    }

    private void ClearCurrent()
    {
        if (IsInteractableValid(currentInteractable)) currentInteractable.DisableOutline();
        uiManager.UpdateInteractionDescription("");
        currentInteractable = null;
    }

    private bool IsInteractableValid(IInteractable interactable)
    {
        // Check if the interface reference points to a destroyed Unity object
        return interactable != null && !interactable.Equals(null);
    }
}
