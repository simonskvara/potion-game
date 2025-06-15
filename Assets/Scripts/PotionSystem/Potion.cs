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
            ApplyEffect(other.gameObject, other.GetComponent<TestSubject>());
            Destroy(gameObject);
        }
    }
    
    private void ApplyEffect(GameObject targetObj, TestSubject targetSubject)
    {
        switch (_effect)
        {
            case PotionEffect.Goblinization:
                break;
                
            case PotionEffect.Combustion:
                break;
                
            case PotionEffect.Zombification:
                break;
                
            default:
                // No effect for failed potions
                break;
        }
    }
}
