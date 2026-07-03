using NaughtyAttributes;
using UnityEngine;

public class Potion : InteractableBase
{
    [BoxGroup("Potion Info")]
    [SerializeField]
    private PotionEffect potionEffect;

    public void Initialize(PotionEffect effect)
    {
        potionEffect = effect;
        // Set visual appearance based on effect
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("TestSubject"))
        {
            other.gameObject.GetComponent<TestSubject>().ApplyEffect(potionEffect);
            Destroy(gameObject);
        }
    }
}
