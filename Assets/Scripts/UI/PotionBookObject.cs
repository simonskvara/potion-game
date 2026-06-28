public class PotionBookObject : InteractableBase
{
    public override void Interact()
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
}
