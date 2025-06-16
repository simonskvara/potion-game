using UnityEngine;
using UnityEngine.Events;

public class TestSubject : MonoBehaviour, IInteractable
{
    [Header("Subject models, please don't touch")]
    [SerializeField] private GameObject baseModel;
    [SerializeField] private GameObject[] subjectModels;
    
    [Header("Other")]
    [SerializeField] private Animator transformationLight;
    [SerializeField] private string description;
    
    private bool isTransformed;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] priestVoiceLines;

    [Header("Unity Events")] 
    public UnityEvent transformationEvent;
    public UnityEvent resetEvent;
    public UnityEvent goblinizationEvent;
    public UnityEvent combustionEvent;
    public UnityEvent pregnancyEvent;
    public UnityEvent extraLimbEvent;
    public UnityEvent tentaclesEvent;
    public UnityEvent furrysationEvent;
    public UnityEvent zombificationEvent;
    public UnityEvent gelatinEvent;
    public UnityEvent velocipastorEvent;
    public UnityEvent childificationEvent;
    
    public void Interact()
    {
        if (isTransformed) return;
        
        PlayVoiceLine();
    }

    public void ApplyEffect(PotionEffect effect)
    {
        if (isTransformed && effect != PotionEffect.Reset) return;
        
        switch (effect)
        {
            case PotionEffect.Reset:
                ResetSubject();
                break;
            
            default:
                ApplyTransformation(effect);
                break;
        }
    }

    private void ApplyTransformation(PotionEffect effect)
    {
        Debug.Log("apply transformation");
        baseModel.SetActive(false);
        foreach (GameObject model in subjectModels)
        {
            if (model) model.SetActive(false);
        }
        
        int effectIndex = (int)effect;
        if (subjectModels.Length > effectIndex && subjectModels[effectIndex] != null)
        {
            subjectModels[effectIndex].SetActive(true);
        }
        
        transformationLight.SetTrigger("LightOn");
        transformationEvent?.Invoke();
        
        switch (effect)
        {
            case PotionEffect.Goblinization:
                Goblinization();
                break;
                
            case PotionEffect.Combustion:
                Combustion();
                break;
                
            case PotionEffect.Pregnancy:
                Pregnancy();
                break;
            
            case PotionEffect.ExtraLimb:
                ExtraLimb();
                break;
            
            case PotionEffect.Tentacles:
                Tentacles();
                break;
            
            case PotionEffect.Furrysation:
                Furrysation();
                break;
            
            case PotionEffect.Zombification:
                Zombification();
                break;
                
            case PotionEffect.Gelatin:
                Gelatin();
                break;
            
            case PotionEffect.Velocipastor:
                Velocipastor();
                break;
            
            case PotionEffect.Childification:
                Childification();
                break;
            
            default:
                NothingHappens();
                break;
        }

        isTransformed = true;
    }
    
    private void PlayVoiceLine()
    {
        int rand = Random.Range(0, priestVoiceLines.Length);
        PlaySound(priestVoiceLines[rand]);
    }

    public void PlaySound(AudioClip sound)
    {
        audioSource.Stop();
        
        audioSource.clip = sound;
        audioSource.Play();
    }
    
    #region Effects

    private void NothingHappens()
    {
        
    }

    private void Goblinization()
    {
        goblinizationEvent?.Invoke();
    }

    private void Combustion()
    {
        combustionEvent?.Invoke();
    }

    private void Pregnancy()
    {
        pregnancyEvent?.Invoke();
    }

    private void ExtraLimb()
    {
        extraLimbEvent?.Invoke();
    }

    private void Tentacles()
    {
        tentaclesEvent?.Invoke();
    }

    private void Furrysation()
    {
        furrysationEvent?.Invoke();
    }

    private void Zombification()
    {
        zombificationEvent?.Invoke();
    }

    private void Gelatin()
    {
        gelatinEvent?.Invoke();
    }

    private void Velocipastor()
    {
        velocipastorEvent?.Invoke();   
    }

    private void Childification()
    {
        childificationEvent?.Invoke();
    }
    
    private void ResetSubject()
    {
        foreach (GameObject model in subjectModels)
        {
            if (model) model.SetActive(false);
        }
        
        transformationLight.SetTrigger("LightOff");
        
        baseModel.SetActive(true);
        
        isTransformed = false;
        
        resetEvent?.Invoke();
    }
    

    #endregion
    
    
    
    
    
    
    #region InterfaceStuff

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

    #endregion
}
