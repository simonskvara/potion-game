using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform orientation;
    
    private float _xRotation;
    private float _yRotation;

    private Vector2 _lookInput;
    
    private InputSystem_Actions _inputSystem;

    private void Awake()
    {
        _inputSystem = new InputSystem_Actions();
    }
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void OnEnable()
    {
        _inputSystem.Player.Enable();
        
        _inputSystem.Player.Look.performed += OnLookPerformed;
        _inputSystem.Player.Look.canceled += ctx => _lookInput = Vector2.zero;
    }

    private void OnDisable()
    {
        _inputSystem.Player.Look.performed -= OnLookPerformed;
        _inputSystem.Player.Disable();
    }

    private void Update()
    {
        float mouseX = _lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = _lookInput.y * mouseSensitivity * Time.deltaTime;

        _yRotation += mouseX;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        
        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }
    
}
