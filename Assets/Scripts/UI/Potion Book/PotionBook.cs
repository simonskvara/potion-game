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
    [BoxGroup("Recipes")]
    [SerializeField]
    private List<PotionEffect> explicitPotions;

    [BoxGroup("Buttons")]
    [SerializeField]
    private Button closeButton;
    [BoxGroup("Buttons")]
    [SerializeField]
    private Button leftButton;
    [BoxGroup("Buttons")]
    [SerializeField]
    private Button rightButton;
    [BoxGroup("Buttons")]
    [SerializeField]
    private Button indexButton;

    [BoxGroup("References")]
    [SerializeField]
    private RectTransform indexPagesTransform;
    [BoxGroup("References")]
    [SerializeField]
    private RectTransform potionPagesTransform;
    [BoxGroup("References")]
    [SerializeField]
    private PotionPage potionPage;
    [BoxGroup("References")]
    [SerializeField]
    private IndexPage indexPage;

    private PlayerCam playerCam;
    private PlayerMovement playerMovement;

    private List<PotionEffect> potions;

    private int currentViewIndex;
    private int indexSpreadCount;

    private int TotalViews => indexSpreadCount + potions.Count;

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

        potions = recipes.AllRecipes.ConvertAll(recipe => recipe.ResultPotionEffect);
        explicitPotions.ForEach(potion =>
        {
            if (!potions.Contains(potion))
                potions.Add(potion);
        });
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

        indexPage.Build(potions, NavigateToPotion);
        indexSpreadCount = indexPage.SpreadCount;
        currentViewIndex = 0;
        ShowView(currentViewIndex);
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

        ShowView(currentViewIndex);
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

    public void GoToIndex()
    {
        currentViewIndex = 0;
        ShowView(currentViewIndex);
    }
    

    private void CycleForward()
    {
        if (currentViewIndex < TotalViews - 1)
        {
            currentViewIndex++;
            ShowView(currentViewIndex);
        }
    }

    private void CycleBackward()
    {
        if (currentViewIndex > 0)
        {
            currentViewIndex--;
            ShowView(currentViewIndex);
        }
    }

    private void NavigateToPotion(int potionIndex)
    {
        currentViewIndex = indexSpreadCount + potionIndex;
        ShowView(currentViewIndex);
    }

    private void ShowView(int view)
    {
        bool isIndex = view < indexSpreadCount;
        indexPagesTransform.gameObject.SetActive(isIndex);
        potionPagesTransform.gameObject.SetActive(!isIndex);

        if (isIndex)
        {
            indexPage.ShowSpread(view);
            indexButton.gameObject.SetActive(false);
        }
        else
        {
            potionPage.Setup(potions[view - indexSpreadCount]);
            indexButton.gameObject.SetActive(true);
        }

        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        leftButton.gameObject.SetActive(currentViewIndex > 0);
        rightButton.gameObject.SetActive(currentViewIndex < TotalViews - 1);
    }
}
