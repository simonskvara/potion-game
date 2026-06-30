using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Tooltip("Scenes in which the pause menu should not open")]
    [Scene]
    public List<string> forbiddenScenes;

    [Tooltip("Turn on when you need to unlock the cursor when pause menu is active")]
    public bool lockCursor;

    public static PauseMenu Instance;

    public static bool GameIsPaused;

    public GameObject pauseMenu;

    [SerializeField] private GameObject eventSystemObject;

    private bool _canPause;

    public UnityEvent OnMenuOpen;
    public UnityEvent OnMenuClosed;

    private InputSystem_Actions inputActions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        EnablePauseMenu();

        inputActions = new InputSystem_Actions();

        inputActions.UI.Enable();
        inputActions.UI.Escape.performed += OnEscapePressed;
    }

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void OnEscapePressed(InputAction.CallbackContext context)
    {
        if (CanOpen())
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        GameIsPaused = false;

        OnMenuClosed?.Invoke();

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Pause()
    {
        OnMenuOpen?.Invoke();
        pauseMenu.SetActive(true);

        Time.timeScale = 0f;
        GameIsPaused = true;

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void RestartScene()
    {
        Resume();
        SceneLoader.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool sceneHasEventSystem = false;
        foreach (var es in FindObjectsByType<EventSystem>(FindObjectsSortMode.None))
        {
            if (es.gameObject.scene == scene)
            {
                sceneHasEventSystem = true;
                break;
            }
        }

        eventSystemObject.SetActive(!sceneHasEventSystem);

        if (scene.name == "MainMenu")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private bool CanOpen()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            return false;
        }

        if (forbiddenScenes.Contains(SceneManager.GetActiveScene().name))
        {
            return false;
        }

        return _canPause;
    }

    public void DisablePauseMenu()
    {
        _canPause = false;
    }

    public void EnablePauseMenu()
    {
        StartCoroutine(DelayEnableMenu());
    }

    private IEnumerator DelayEnableMenu()
    {
        yield return new WaitForEndOfFrame();
        _canPause = true;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        inputActions.UI.Escape.performed -= OnEscapePressed;
        inputActions.UI.Disable();
    }
}
