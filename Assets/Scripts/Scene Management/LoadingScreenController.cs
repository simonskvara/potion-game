using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    [SerializeField] private Slider progressBar;

    private void Start()
    {
        StartCoroutine(LoadTargetScene());
    }

    private IEnumerator LoadTargetScene()
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(SceneLoader.TargetScene);

        while (!asyncOp.isDone)
        {
            float progress = Mathf.Clamp01(asyncOp.progress / 0.9f);
            UpdateUI(progress);
            yield return null;
        }

    }

    void UpdateUI(float progress)
    {
        if (progressBar != null)
            progressBar.value = progress;
    }
}
