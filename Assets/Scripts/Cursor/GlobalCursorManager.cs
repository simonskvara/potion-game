using System;
using UnityEngine;
using UnityEngine.InputSystem;

//TODO: Make it function with new input system
public class GlobalCursorManager : MonoBehaviour
{
    [Header("Cursor Textures")]
    public Texture2D normalCursor;
    public Texture2D clickedCursor;
    
    [Header("Cursor Settings")]
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;
    
    private static GlobalCursorManager instance;
    
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

    void Start()
    {
        if (normalCursor != null)
        {
            Cursor.SetCursor(normalCursor, hotspot, cursorMode);
        }
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnClickStarted();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            OnClickCanceled();
        }
    }
    
    void OnClickStarted()
    {
        if (clickedCursor != null)
            Cursor.SetCursor(clickedCursor, hotspot, cursorMode);
    }
    
    void OnClickCanceled()
    {
        if (normalCursor != null)
            Cursor.SetCursor(normalCursor, hotspot, cursorMode);
    }
}