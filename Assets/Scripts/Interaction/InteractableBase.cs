using UnityEngine;
using UnityEngine.Serialization;
using NaughtyAttributes;

public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    [BoxGroup("Interaction")]
    [SerializeField, InfoBox("If you want to force a description that is getting set automatically")] 
    protected bool forceDescription;

    [Space]

    
    [BoxGroup("Interaction")]
    [SerializeField]
    protected string description;
    [BoxGroup("Interaction")]
    [SerializeField] 
    protected Outline outline;
    [BoxGroup("Interaction")]
    [SerializeField]
    private bool isInteractable = true;

    public bool IsInteractable
    {
        get => isInteractable;
        protected set => isInteractable = value;
    }

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

    public void SetIsInteractable(bool isInteractable)
    {
        this.IsInteractable = isInteractable;
    }
}
