using UnityEngine;

/// <summary>
/// A composable, data-driven action that runs when a potion effect is applied to a
/// <see cref="TestSubject"/>. Behaviours are edited inline on a <see cref="PotionEffect"/> via
/// <c>[SerializeReference]</c> — to add a new kind of extra, write one <c>[System.Serializable]</c>
/// subclass and it appears automatically in the inspector's "Add" type dropdown. No editor code.
/// </summary>
[System.Serializable]
public abstract class EffectBehaviour
{
    public abstract void Apply(TestSubject subject);
}

/// <summary>Plays a one-shot sound on the subject's dedicated effect AudioSource.</summary>
[System.Serializable]
public class PlaySfx : EffectBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField, Range(0f, 1f)] private float volume = 1f;

    public override void Apply(TestSubject subject)
    {
        subject.PlayOneShot(clip, volume);
    }
}

/// <summary>Instantiates a VFX prefab at the subject's vfx anchor, destroying it after a delay.</summary>
[System.Serializable]
public class SpawnVfx : EffectBehaviour
{
    [SerializeField] private GameObject prefab;
    [Tooltip("Seconds before the spawned instance is destroyed. Set <= 0 to keep it alive.")]
    [SerializeField] private float lifetime = 5f;

    public override void Apply(TestSubject subject)
    {
        subject.SpawnVfx(prefab, lifetime);
    }
}
