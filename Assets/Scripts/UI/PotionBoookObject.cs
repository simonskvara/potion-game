using UnityEngine;

public class PotionBoookObject : MonoBehaviour, IInteractable
{
    [SerializeField] private string objectNameDescription;

    [Header("Outline")] [SerializeField] private Outline outline;
    
    public void Interact()
    {
        if (PotionBook.Instance.IsOpen)
        {
            PotionBook.Instance.CloseBook();
        }
        else
        {
            PotionBook.Instance.OpenBook();
        }
    }

    public string GetDescription()
    {
        return objectNameDescription;
    }

    public void EnableOutline()
    {
        outline.enabled = true;
    }

    public void DisableOutline()
    {
        outline.enabled = false;
    }
}
