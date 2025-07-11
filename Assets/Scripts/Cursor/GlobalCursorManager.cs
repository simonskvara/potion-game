using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalCursorManager : MonoBehaviour
{
    [Header("Cursor Textures")]
    public Texture2D normalCursor;
    public Texture2D clickedCursor;
    
    [Header("Cursor Settings")]
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;
    
    private static GlobalCursorManager instance;

    [SerializeField] private InputActionReference _clickAction;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        _clickAction.action.Enable();
        _clickAction.action.started += OnClickStarted;
        _clickAction.action.canceled += OnClickCanceled;
    }

    private void OnDisable()
    {
        _clickAction.action.started -= OnClickStarted;
        _clickAction.action.canceled -= OnClickCanceled;
        _clickAction.action.Disable();
    }

    void Start()
    {
        if (normalCursor != null)
        {
            Cursor.SetCursor(normalCursor, hotspot, cursorMode);
        }
    }
    
    void OnClickStarted(InputAction.CallbackContext context)
    {
        if (clickedCursor != null)
            Cursor.SetCursor(clickedCursor, hotspot, cursorMode);
    }
    
    void OnClickCanceled(InputAction.CallbackContext context)
    {
        if (normalCursor != null)
            Cursor.SetCursor(normalCursor, hotspot, cursorMode);
    }
}