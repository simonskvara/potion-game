using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteOutline : MonoBehaviour
{
    public enum Visibility
    {
        OnlyWhenVisible,   // outline hidden when the sprite is behind scene geometry (ZTest LessEqual)
        ThroughWalls       // outline always drawn, even through other objects (ZTest Always)
    }

    [SerializeField] private Material outlineMaterial;             // shared "Sprites/SpriteOutline" material (template)
    [SerializeField] private Color outlineColor = Color.white;
    [SerializeField, Range(0f, 8f)] private float outlineThickness = 2f;
    [SerializeField] private Visibility visibility = Visibility.OnlyWhenVisible;

    private SpriteRenderer sr;
    private Material mat;   // per-object instance (render state can't come from a property block)

    private static readonly int OutlineEnabledID   = Shader.PropertyToID("_OutlineEnabled");
    private static readonly int OutlineColorID      = Shader.PropertyToID("_OutlineColor");
    private static readonly int OutlineThicknessID  = Shader.PropertyToID("_OutlineThickness");
    private static readonly int ZTestID             = Shader.PropertyToID("_ZTest");

    private void EnsureRefs()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (mat == null)
        {
            if (outlineMaterial != null && sr.sharedMaterial != outlineMaterial)
                sr.sharedMaterial = outlineMaterial;
            mat = sr.material;   // instantiates a unique copy for this renderer
        }
    }

    private void ApplySettings()
    {
        EnsureRefs();
        mat.SetColor(OutlineColorID, outlineColor);
        mat.SetFloat(OutlineThicknessID, outlineThickness);
        mat.SetFloat(ZTestID, (float)(visibility == Visibility.ThroughWalls
            ? CompareFunction.Always
            : CompareFunction.LessEqual));
    }

    private void Awake()     { ApplySettings(); SetOutline(false); }
    private void OnEnable()  => SetOutline(true);
    private void OnDisable() => SetOutline(false);

    private void SetOutline(bool on)
    {
        EnsureRefs();
        mat.SetFloat(OutlineEnabledID, on ? 1f : 0f);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying) ApplySettings();
    }
#endif
}
