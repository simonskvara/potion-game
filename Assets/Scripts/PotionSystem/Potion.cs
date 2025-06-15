using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField] private PotionEffect _effect;
    
    public void Initialize(PotionEffect effect)
    {
        _effect = effect;
        // Set visual appearance based on effect
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TestSubject"))
        {
            ApplyEffect(other.gameObject);
            Destroy(gameObject);
        }
    }
    
    private void ApplyEffect(GameObject target)
    {
        switch (_effect)
        {
            case PotionEffect.Goblinization:
                break;
                
            case PotionEffect.Combustion:
                break;
                
            // Add other effects here
                
            default:
                // No effect for failed potions
                break;
        }
    }
}
