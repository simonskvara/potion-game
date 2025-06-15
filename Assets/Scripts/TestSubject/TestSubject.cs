using UnityEngine;

public class TestSubject : MonoBehaviour, IInteractable
{
    [SerializeField] private string description;
    
    public void Interact()
    {
        
    }

    public string GetDescription()
    {
        return description;
    }

    public void EnableOutline()
    {
        
    }

    public void DisableOutline()
    {
        
    }
}
