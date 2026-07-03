using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private Button newGameButton;

    private void Start()
    {
        SetupContinueButton();
    }

    private void SetupContinueButton()
    {
        continueButton.interactable = SaveManager.HasSaveFile();
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        if (PauseMenu.Instance != null)
        {
            PauseMenu.Instance.Resume();
        }
        SceneLoader.LoadScene(sceneName);
    }

    public void NewGame(string sceneName)
    {
        Time.timeScale = 1f;

        SaveManager.DeleteSave();
        SaveManager.Save();

        SceneLoader.LoadScene(sceneName);
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneLoader.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        SceneLoader.Quit();
    }
}
