using System;
using UnityEngine;

public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private float pickupDistance;

    private ObjectGrabbable objectGrabbable;
    
    private void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit raycastHit, pickupDistance, pickupLayerMask))
            {
                if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                {
                    PickUp();
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
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
