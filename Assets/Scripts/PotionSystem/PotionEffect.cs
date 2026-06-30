using System.Collections.Generic;
using UnityEngine;

public enum PotionEffectKind
{
    Transform, // changes the subject into a mapped model
    Reset,     // restores the subject to its base form
    Nothing    // no transformation (the "slop" / no-recipe result)
}

// NOTE: intentionally uses only built-in attributes (no NaughtyAttributes). This keeps the
// asset on Unity's default inspector, which renders the [SerializeReference] onApply list with
// its native "Add" type picker — the core of the data-driven, no-code effect workflow.
[CreateAssetMenu(fileName = "PotionEffect", menuName = "Potions/PotionEffect")]
public class PotionEffect : ScriptableObject
{
    public string PotionEffectID => potionEffectID;
    public string DisplayName => displayName;
    public Sprite Icon => icon;
    public string Description => description;
    public PotionEffectKind Kind => kind;
    public AudioClip TransformVoiceLine => transformVoiceLine;
    public bool PlayLight => playLight;
    public IReadOnlyList<EffectBehaviour> OnApply => onApply;

    [Header("Potion Effect Info")]
    [SerializeField] private string potionEffectID;
    [SerializeField] private string displayName;
    [SerializeField] private Sprite icon;
    [SerializeField, TextArea] private string description;
    [SerializeField] private PotionEffectKind kind = PotionEffectKind.Transform;

    [Header("Presentation")]
    [Tooltip("Optional voice line the subject plays when this effect is applied.")]
    [SerializeField] private AudioClip transformVoiceLine;
    [Tooltip("Trigger the subject's transformation light animation.")]
    [SerializeField] private bool playLight = true;

    [Header("Extra Behaviours (sounds, VFX, …)")]
    [Tooltip("Composable actions run when this effect is applied. Click + to add a block type.")]
    [SerializeReference] private List<EffectBehaviour> onApply = new List<EffectBehaviour>();
}
