using UnityEngine;
using UnityEngine.UI;

public class PageFlip : MonoBehaviour
{
    [SerializeField] private GameObject[] doublePages;
    [SerializeField] private GameObject forwardButton;
    [SerializeField] private GameObject backButton;

    private int currentIndex = 0;

    private void Start()
    {
        UpdatePages();
        UpdateButtons();
    }

    public void FlipForward()
    {
        if (currentIndex < doublePages.Length - 1)
        {
            currentIndex++;
            UpdatePages();
            UpdateButtons();
        }
    }

    public void FlipBackward()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdatePages();
            UpdateButtons();
        }
    }

    public void JumpToPage(int index)
    {
        if (index >= 0 && index < doublePages.Length)
        {
            currentIndex = index;
            UpdatePages();
            UpdateButtons();
        }
    }

    private void UpdatePages()
    {
        for (int i = 0; i < doublePages.Length; i++)
        {
            doublePages[i].SetActive(i == currentIndex);
        }
    }

    private void UpdateButtons()
    {
        backButton.SetActive(currentIndex > 0);
        forwardButton.SetActive(currentIndex < doublePages.Length - 1);
    }
}
