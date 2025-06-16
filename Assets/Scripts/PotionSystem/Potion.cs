using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField] private PotionEffect _effect;
    
    public void Initialize(PotionEffect effect)
    {
        _effect = effect;
        // Set visual appearance based on effect
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("TestSubject"))
        {
            other.gameObject.GetComponent<TestSubject>().ApplyEffect(_effect);
            Destroy(gameObject);
        }
    }
    
    
}
