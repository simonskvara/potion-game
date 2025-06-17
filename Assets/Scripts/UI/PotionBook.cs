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
        playerCam.UnfreezeCamera();
        playerMovement.UnfreezeMovement();
        bookObject.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
}
