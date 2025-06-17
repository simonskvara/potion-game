using System;
using System.Collections.Generic;
using UnityEngine;

public class PotionBook : MonoBehaviour
{
    public static PotionBook Instance;

    [SerializeField] private GameObject bookObject;

    public bool IsOpen { get; private set; } = false;

    private PlayerCam playerCam;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerCam = FindAnyObjectByType<PlayerCam>();
        playerMovement = FindAnyObjectByType<PlayerMovement>();
    }

    public void OpenBook()
    {
        playerCam.FreezeCamera();
        playerMovement.FreezeMovement();
        bookObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseBook()
    {
        playerCam.UnfreezeCamera();
        playerMovement.UnfreezeMovement();
        bookObject.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
}
