public interface IInteractable
{
    bool IsInteractable { get; }

    void Interact();
    string GetDescription();
    void EnableOutline();
    void DisableOutline();
}
