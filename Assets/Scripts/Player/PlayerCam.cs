using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float defaultMouseSensitivity = 100f;
    private float mouseSensitivity;
    [SerializeField] private Transform orientation;
    
    [Header("Zoom Settings")]
    [SerializeField] private float zoomFOV = 40f;
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float zoomSensitivityFactor = 0.7f;
    
    private float xRotation;
    private float yRotation;
    private Vector2 lookInput;
    
    // zoom variables
    private float currentFOV;
    private float normalFOV;
    private bool isZooming = false;
    private Camera playerCamera;
    private float currentSensitivity;
    
    private InputSystem_Actions inputSystem;

    private void Awake()
    {
        inputSystem = new InputSystem_Actions();
        LoadSensitivity();

        playerCamera = gameObject.GetComponent<Camera>();
        normalFOV = playerCamera.fieldOfView;
        currentFOV = normalFOV;
    }
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void OnEnable()
    {
        inputSystem.Player.Enable();
        
        inputSystem.Player.Look.performed += OnLookPerformed;
        inputSystem.Player.Look.canceled += ctx => lookInput = Vector2.zero;
        
        inputSystem.Player.Zoom.performed += StartZoom;
        inputSystem.Player.Zoom.canceled += EndZoom;
        
        SettingsManager.OnSensitivityChanged.AddListener(UpdateSensitivity);
    }

    private void OnDisable()
    {
        inputSystem.Player.Look.performed -= OnLookPerformed;
        
        inputSystem.Player.Zoom.performed -= StartZoom;
        inputSystem.Player.Zoom.canceled -= EndZoom;
        
        inputSystem.Player.Disable();
        
        SettingsManager.OnSensitivityChanged.RemoveListener(UpdateSensitivity);
    }

    private void Update()
    {
        HandleCameraRotation();
        HandleZoom();
    }
    
    private void HandleCameraRotation()
    {
        float mouseX = lookInput.x * currentSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * currentSensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    
    private void HandleZoom()
    {
        // Smoothly transition FOV
        currentFOV = Mathf.Lerp(currentFOV, isZooming ? zoomFOV : normalFOV, zoomSpeed * Time.deltaTime);
        playerCamera.fieldOfView = currentFOV;
        
        // Adjust sensitivity based on zoom state
        currentSensitivity = isZooming ? mouseSensitivity * zoomSensitivityFactor : mouseSensitivity;
    }
    
    private void StartZoom(InputAction.CallbackContext context) => isZooming = true;
    private void EndZoom(InputAction.CallbackContext context) => isZooming = false;

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    
    public void UpdateSensitivity(float newSensitivity)
    {
        mouseSensitivity = newSensitivity;
        
        if (!isZooming) currentSensitivity = mouseSensitivity;
    }

    // Load saved sensitivity or use default
    private void LoadSensitivity()
    {
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", defaultMouseSensitivity);
        currentSensitivity = mouseSensitivity;
    }
}
