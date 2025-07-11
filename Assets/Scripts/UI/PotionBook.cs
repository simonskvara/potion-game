using System;
using System.Collections.Generic;
using skv_toolkit.MenuScripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class PotionBook : MonoBehaviour
{
    public static PotionBook Instance;
    
    

    [SerializeField] private GameObject bookObject;

    [SerializeField] private InputActionReference cancelInput;
    
    public bool IsOpen { get; private set; } = false;

    private PlayerCam playerCam;
    private PlayerMovement playerMovement;
    
    
    
    
    [System.Serializable]
    public class RecipeUI
    {
        public PotionEffect effect;
        public GameObject cover;
    }
    
    public List<RecipeUI> recipeUIs = new List<RecipeUI>();

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

    private void OnEnable()
    {
        cancelInput.action.Enable();
        cancelInput.action.started += CloseBook;
    }

    private void OnDisable()
    {
        cancelInput.action.started -= CloseBook;
        cancelInput.action.Enable();
    }

    private void Start()
    {
        foreach (var recipeUI in recipeUIs)
        {
            if (PotionDiscovery.Instance.IsEffectDiscovered(recipeUI.effect))
            {
                RevealRecipe(recipeUI.effect);
            }
        }
    }
    
    public void RevealRecipe(PotionEffect effect)
    {
        // Find and reveal the specific recipe
        foreach (var recipeUI in recipeUIs)
        {
            if (recipeUI.effect == effect && recipeUI.cover != null)
            {
                recipeUI.cover.SetActive(false);
                break;
            }
        }
    }

    public void OpenBook()
    {
        if(PauseMenu.Instance != null)
            PauseMenu.Instance.DisablePauseMenu();

        IsOpen = true;
        playerCam.FreezeCamera();
        playerMovement.FreezeMovement();
        bookObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        foreach (var recipeUI in recipeUIs)
        {
            if (PotionDiscovery.Instance.IsEffectDiscovered(recipeUI.effect))
            {
                recipeUI.cover.SetActive(false);
            }
            else
            {
                recipeUI.cover.SetActive(true);
            }
        }
    }

    public void CloseBook()
    {
        if(PauseMenu.Instance != null)
            PauseMenu.Instance.EnablePauseMenu();
        
        playerCam.UnfreezeCamera();
        playerMovement.UnfreezeMovement();
        bookObject.SetActive(false);
        IsOpen = false;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public void CloseBook(InputAction.CallbackContext context)
    {
        if (IsOpen)
        {
            CloseBook();
        }
    }
    
}
