using UnityEngine;
using UnityEngine.Serialization;
using NaughtyAttributes;

public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    [BoxGroup("Interaction")]
    [SerializeField] protected string description;

    [BoxGroup("Interaction")]
    [SerializeField] protected Outline outline;

    protected virtual void Awake()
    {
        DisableOutline();
    }

    public virtual void Interact() { }

    public virtual string GetDescription() => description;

    public void EnableOutline()
    {
        if (outline != null) outline.enabled = true;
    }

    public void DisableOutline()
    {
        if (outline != null) outline.enabled = false;
    }
}
