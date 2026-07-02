using System.Collections.Generic;
using UnityEngine;

public enum PotionEffectKind
{
    Transform,
    Reset,
    Nothing
}

[CreateAssetMenu(fileName = "PotionEffect", menuName = "Potions/PotionEffect")]
public class PotionEffect : ScriptableObject
{
    public string PotionEffectID => potionEffectID;
    public string DisplayName => displayName;
    public Sprite Icon => icon;
    public Sprite IconSilhouette => iconSilhouette;
    public string Description => description;
    public PotionEffectKind Kind => kind;
    public AudioClip TransformVoiceLine => transformVoiceLine;
    public bool PlayLight => playLight;
    public IReadOnlyList<EffectBehaviour> OnApply => onApply;

    [Header("Potion Effect Info")]
    [SerializeField] private string potionEffectID;
    [SerializeField] private string displayName;
    [SerializeField] private Sprite icon;
    [SerializeField] private Sprite iconSilhouette;
    [SerializeField, TextArea(4, 10)] private string description;
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
