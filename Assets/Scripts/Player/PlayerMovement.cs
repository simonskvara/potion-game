using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public Vector3 MoveDirection { get; private set; }
    private Vector2 _moveInput;
    
    private InputSystem_Actions _inputSystem;
    private Rigidbody _rb;
    
    private bool isFrozen = false;

    [SerializeField] private Transform orientation;
    [SerializeField] private float moveSpeed;
    
    private void Awake()
    {
        _inputSystem = new InputSystem_Actions();
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _inputSystem.Player.Move.Enable();
        _inputSystem.Player.Move.performed += HandleInput;
        _inputSystem.Player.Move.canceled += HandleInput;
    }

    private void OnDisable()
    {
        _inputSystem.Player.Move.performed -= HandleInput;
        _inputSystem.Player.Move.canceled -= HandleInput;
        _inputSystem.Player.Move.Disable();
    }

    private void FixedUpdate()
    {
        if (isFrozen) return;
        
        MoveDirection = orientation.forward * _moveInput.y + orientation.right * _moveInput.x;
        Move();
    }

    private void HandleInput(InputAction.CallbackContext context)
    {
        if (isFrozen) return;
        
        _moveInput = context.ReadValue<Vector2>();
    }
    
    private void Move()
    {
        _rb.linearVelocity = new Vector3(MoveDirection.x * moveSpeed, _rb.linearVelocity.y, MoveDirection.z * moveSpeed);
    }
    
    public void FreezeMovement()
    {
        isFrozen = true;
        _moveInput = Vector2.zero;
        _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0); // Maintain vertical velocity for gravity
    }

    /// <summary>
    /// Resumes normal player movement
    /// </summary>
    public void UnfreezeMovement()
    {
        isFrozen = false;
    }
}
