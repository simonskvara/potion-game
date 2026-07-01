using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PotionBook : MonoBehaviour
{
    public static PotionBook Instance;

    public bool IsOpen { get; private set; } = false;
    public Recipes Recipes => recipes;

    [SerializeField] 
    private RectTransform bookObject;

    [BoxGroup("Recipes")]
    [SerializeField]
    private Recipes recipes;

    [BoxGroup("Buttons")]
    [SerializeField]
    private Button closeButton;
    [BoxGroup("Buttons")]
    [SerializeField]
    private Button leftButton;
    [BoxGroup("Buttons")]
    [SerializeField]
    private Button rightButton;

    [BoxGroup("References")]
    [SerializeField]
    private RectTransform indexPagesTransform;
    [BoxGroup("References")]
    [SerializeField]
    private RectTransform potionPagesTransform;
    [BoxGroup("References")]
    [SerializeField]
    private PotionPage potionPage;

    private PlayerCam playerCam;
    private PlayerMovement playerMovement;

    private int currentPageIndex;

    private InputSystem_Actions inputActions;

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

        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.UI.Cancel.performed += CloseBook;
    }

    private void OnDisable()
    {
        inputActions.UI.Cancel.performed -= CloseBook;
        inputActions.Disable();
    }

    private void Start()
    {
        rightButton.onClick.AddListener(CycleForward);
        leftButton.onClick.AddListener(CycleBackward);

        potionPage.Setup(recipes.AllRecipes[0].ResultPotionEffect);
        currentPageIndex = 0;
        UpdateButtonState();
    }

    public void OpenBook()
    {
        if(PauseMenu.Instance != null)
            PauseMenu.Instance.DisablePauseMenu();

        IsOpen = true;
        playerCam.FreezeCamera();
        playerMovement.FreezeMovement();
        bookObject.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        potionPage.Refresh();
        // refresh the current potion page
    }

    public void CloseBook()
    {
        playerCam.UnfreezeCamera();
        playerMovement.UnfreezeMovement();
        bookObject.gameObject.SetActive(false);
        IsOpen = false;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if(PauseMenu.Instance != null)
            PauseMenu.Instance.EnablePauseMenu();
    }
    
    public void CloseBook(InputAction.CallbackContext context)
    {
        if (IsOpen)
        {
            CloseBook();
        }
    }
    

    private void CycleForward()
    {
        currentPageIndex++;
        potionPage.Setup(recipes.AllRecipes[currentPageIndex].ResultPotionEffect);
        UpdateButtonState();
    }

    private void CycleBackward()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            potionPage.Setup(recipes.AllRecipes[currentPageIndex].ResultPotionEffect);
            UpdateButtonState();
        }
    }

    private void UpdateButtonState()
    {
        leftButton.gameObject.SetActive(currentPageIndex > 0);
        rightButton.gameObject.SetActive(currentPageIndex < recipes.AllRecipes.Count - 1);
    }
}
