using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ObjectGrabbable : MonoBehaviour, IInteractable
{
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;
    [SerializeField] private float lerpSpeed = 20f;
    [SerializeField] private float rotationLerpSpeed = 15f;
    
    private bool isGrabbed;
    private Vector3 targetPosition;
    
    private Transform cameraTransform;

    private bool canGrab = true;

    [Header("Outline")]
    [SerializeField] private Outline outline;

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
        objectRigidbody.excludeLayers = LayerMask.GetMask("Player");
        objectRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Start()
    {
        if (gameObject.layer != LayerMask.NameToLayer("PickUpObject"))
        {
            Debug.LogWarning("Ingredient doesn't have the layer needed to work", gameObject);
        }
        
        DisableOutline();
    }

    public void DisableGrabbing() { canGrab = false;}
    
    public void Grab(Transform objectGrabPointTransform, Transform cameraTransform)
    {
        if(!canGrab) return;
        
        this.objectGrabPointTransform = objectGrabPointTransform;
        this.cameraTransform = cameraTransform;
        objectRigidbody.useGravity = false;
        
        isGrabbed = true;
    }

    public void Drop()
    {
        this.objectGrabPointTransform = null;
        this.cameraTransform = null;
        objectRigidbody.useGravity = true;
        
        isGrabbed = false;
    }

    private void FixedUpdate()
    {
        if (!isGrabbed) return;
        
        targetPosition = objectGrabPointTransform.position;
        
        // Directly set position if very close to avoid micro-movements
        float distance = Vector3.Distance(transform.position, targetPosition);
        if (distance < 0.01f)
        {
            objectRigidbody.MovePosition(targetPosition);
            objectRigidbody.linearVelocity = Vector3.zero;
            return;
        }

        Vector3 direction = (targetPosition - transform.position).normalized;
        float speed = Mathf.Min(lerpSpeed * distance, lerpSpeed);
        
        objectRigidbody.linearVelocity = direction * speed;
        
        if (cameraTransform != null)
        {
            Quaternion targetRotation = cameraTransform.rotation;
            
            Quaternion newRotation = Quaternion.Lerp(
                objectRigidbody.rotation,
                targetRotation,
                Time.fixedDeltaTime * rotationLerpSpeed
            );
            
            objectRigidbody.MoveRotation(newRotation);
        }
        
        objectRigidbody.angularVelocity = Vector3.zero;
    }

    
    // interactable stuff
    
    public void Interact()
    {
        // nothing happens
    }

    public string GetDescription()
    {
        return "";
    }

    public void EnableOutline()
    {
        outline.enabled = true;
    }

    public void DisableOutline()
    {
        outline.enabled = false;
    }
}
