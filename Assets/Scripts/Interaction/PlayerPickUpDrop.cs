using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private float pickupDistance;

    private ObjectGrabbable objectGrabbable;

    private InputSystem_Actions inputSystem;

    private void Awake()
    {
        inputSystem = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputSystem.Player.LeftClick.Enable();
        inputSystem.Player.LeftClick.started += ProcessInput;
        inputSystem.Player.LeftClick.canceled += ProcessInput;
    }

    private void OnDisable()
    {
        inputSystem.Player.LeftClick.started -= ProcessInput;
        inputSystem.Player.LeftClick.canceled -= ProcessInput;
        inputSystem.Player.LeftClick.Disable();
    }

    private void ProcessInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit raycastHit, pickupDistance, pickupLayerMask))
            {
                if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                {
                    PickUp();
                }
            }
        }

        if (context.canceled)
        {
            Drop();
        }
    }
    
    private void PickUp()
    {
        objectGrabbable.Grab(objectGrabPointTransform, playerCamera);
    }

    private void Drop()
    {
        if(objectGrabbable == null) return;
        
        objectGrabbable.Drop();
        objectGrabbable = null;
    }
}
