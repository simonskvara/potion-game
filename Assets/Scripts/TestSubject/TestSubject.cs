using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestSubject : InteractableBase
{
    [System.Serializable]
    public class EffectModel
    {
        [Tooltip("Optional label for inspector")]
        public string label;
        [Tooltip("The potion effect this model represents.")]
        public PotionEffect effect;
        [Tooltip("The model GameObject (child of the subject) shown for this effect.")]
        public GameObject model;
        [Tooltip("Optional escape hatch for scene-specific wiring that can't live on the PotionEffect asset.")]
        public UnityEvent onApply;
    }

    [Header("Subject models, please don't touch")]
    [SerializeField] private GameObject baseModel;
    [Tooltip("Maps each transform PotionEffect to the model GameObject it activates.")]
    [SerializeField] private List<EffectModel> effectModels = new List<EffectModel>();

    [Header("Other")]
    [SerializeField] private Animator transformationLight;

    private bool isTransformed;

    [Header("Sound")]
    [Tooltip("Plays voice lines (interaction + transform reactions).")]
    [SerializeField] private AudioSource audioSource;
    [Tooltip("Plays effect SFX (explosions, etc.) so they don't cut off voice lines.")]
    [SerializeField] private AudioSource effectAudioSource;
    [SerializeField] private AudioClip[] priestVoiceLines;

    [Header("VFX")]
    [Tooltip("Spawned VFX are parented here. Falls back to this transform if unset.")]
    [SerializeField] private Transform vfxAnchor;

    [Header("Unity Events")]
    [Tooltip("Shared flourish played for every transformation (light flash, etc.).")]
    public UnityEvent transformationEvent;

    private List<int> _voiceLinePlayOrder;
    private int _currentVoiceLineIndex;

    private Dictionary<PotionEffect, EffectModel> _modelsByEffect;

    protected override void Awake()
    {
        base.Awake();

        _modelsByEffect = new Dictionary<PotionEffect, EffectModel>();
        foreach (EffectModel entry in effectModels)
        {
            if (entry.effect != null && !_modelsByEffect.ContainsKey(entry.effect))
            {
                _modelsByEffect.Add(entry.effect, entry);
            }
        }
    }

    public override void Interact()
    {
        if (isTransformed) return;

        PlayVoiceLine();
    }

    public void ApplyEffect(PotionEffect effect)
    {
        if (effect == null) return;

        // Block re-applying while transformed, except for a Reset.
        if (isTransformed && effect.Kind != PotionEffectKind.Reset) return;

        switch (effect.Kind)
        {
            case PotionEffectKind.Reset:
                ResetSubject(effect);
                break;

            case PotionEffectKind.Nothing:
                NothingHappens(effect);
                break;

            default:
                ApplyTransformation(effect);
                break;
        }
    }

    private void ApplyTransformation(PotionEffect effect)
    {
        isTransformed = true;

        HideAllModels();

        if (_modelsByEffect.TryGetValue(effect, out EffectModel entry) && entry.model != null)
        {
            entry.model.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"TestSubject has no model mapped for effect '{effect.PotionEffectID}'.", this);
        }

        if (effect.PlayLight && transformationLight != null)
        {
            transformationLight.SetTrigger("LightOn");
        }

        transformationEvent?.Invoke();

        RunPresentation(effect);

        PotionDiscovery.Instance?.DiscoverEffect(effect);
    }

    private void NothingHappens(PotionEffect effect)
    {
        // Subject is unchanged; just play the slop reaction.
        isTransformed = false;
        RunPresentation(effect);
    }

    private void ResetSubject(PotionEffect effect)
    {
        HideAllModels();

        if (transformationLight != null) transformationLight.SetTrigger("LightOff");
        if (baseModel != null) baseModel.SetActive(true);

        isTransformed = false;

        RunPresentation(effect);
    }

    /// <summary>Plays the effect's voice line, runs its behaviour list, and fires the optional scene hook.</summary>
    private void RunPresentation(PotionEffect effect)
    {
        if (effect.TransformVoiceLine != null)
        {
            PlaySound(effect.TransformVoiceLine);
        }

        foreach (EffectBehaviour behaviour in effect.OnApply)
        {
            behaviour?.Apply(this);
        }

        if (_modelsByEffect.TryGetValue(effect, out EffectModel entry))
        {
            entry.onApply?.Invoke();
        }
    }

    private void HideAllModels()
    {
        if (baseModel != null) baseModel.SetActive(false);

        foreach (EffectModel entry in effectModels)
        {
            if (entry.model != null) entry.model.SetActive(false);
        }
    }

    #region Behaviour hooks

    /// <summary>Plays a one-shot effect sound. Used by <see cref="EffectBehaviour"/> blocks.</summary>
    public void PlayOneShot(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        AudioSource source = effectAudioSource != null ? effectAudioSource : audioSource;
        if (source != null) source.PlayOneShot(clip, volume);
    }

    /// <summary>Spawns a VFX prefab at the vfx anchor. Used by <see cref="EffectBehaviour"/> blocks.</summary>
    public GameObject SpawnVfx(GameObject prefab, float lifetime = 5f)
    {
        if (prefab == null) return null;

        Transform anchor = vfxAnchor != null ? vfxAnchor : transform;
        GameObject instance = Instantiate(prefab, anchor.position, anchor.rotation, anchor);

        if (lifetime > 0f) Destroy(instance, lifetime);
        return instance;
    }

    #endregion

    #region Voice lines

    private void InitializeVoiceLinePlayOrder()
    {
        _voiceLinePlayOrder = new List<int>();
        _currentVoiceLineIndex = 0;

        for (int i = 0; i < priestVoiceLines.Length; i++)
        {
            _voiceLinePlayOrder.Add(i);
        }

        // Shuffle the list using Fisher-Yates algorithm
        for (int i = 0; i < _voiceLinePlayOrder.Count; i++)
        {
            int randomIndex = Random.Range(i, _voiceLinePlayOrder.Count);
            (_voiceLinePlayOrder[i], _voiceLinePlayOrder[randomIndex]) = (_voiceLinePlayOrder[randomIndex], _voiceLinePlayOrder[i]);
        }
    }

    private void PlayVoiceLine()
    {
        if (priestVoiceLines == null || priestVoiceLines.Length == 0)
            return;

        // Initialize if not done yet
        if (_voiceLinePlayOrder == null || _voiceLinePlayOrder.Count == 0)
        {
            InitializeVoiceLinePlayOrder();
        }

        // Reset if we've played all clips
        if (_voiceLinePlayOrder != null && _currentVoiceLineIndex >= _voiceLinePlayOrder.Count)
        {
            InitializeVoiceLinePlayOrder();
        }

        int clipIndex = _voiceLinePlayOrder[_currentVoiceLineIndex];
        _currentVoiceLineIndex++;

        PlaySound(priestVoiceLines[clipIndex]);
    }

    private void PlaySound(AudioClip sound)
    {
        if (audioSource == null) return;

        audioSource.Stop();
        audioSource.clip = sound;
        audioSource.Play();
    }

    #endregion
}
