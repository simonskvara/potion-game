using System;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;
    [SerializeField] private float lerpSpeed = 10f;
    
    private bool isGrabbed;
    private Vector3 targetPosition;

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
        objectRigidbody.excludeLayers = LayerMask.GetMask("Player");
    }

    public void Grab(Transform objectGrabPointTransform)
    {
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;
        
        isGrabbed = true;
    }

    public void Drop()
    {
        this.objectGrabPointTransform = null;
        objectRigidbody.useGravity = true;
        
        isGrabbed = false;
    }

    private void FixedUpdate()
    {
        /*if (objectGrabPointTransform != null)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.fixedDeltaTime * lerpSpeed);
            objectRigidbody.MovePosition(newPosition);  
        }*/
        
        
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
        
        objectRigidbody.angularVelocity = Vector3.zero;
    }
}
