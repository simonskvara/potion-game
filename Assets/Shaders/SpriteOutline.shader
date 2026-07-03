Shader "Sprites/SpriteOutline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineThickness ("Outline Thickness (texels)", Range(0,8)) = 2
        _AlphaThreshold ("Alpha Threshold", Range(0,1)) = 0.1
        [MaterialToggle] _OutlineEnabled ("Outline Enabled", Float) = 0
        // Outline depth test: 4 = LessEqual (only where visible), 8 = Always (through walls).
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("Outline ZTest", Float) = 4

        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        // Pass 1 - the sprite itself, with normal depth handling (occluded by scene geometry).
        Pass
        {
            Name "Sprite"
        CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment SpriteFrag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"
        ENDCG
        }

        // Pass 2 - the outline ring only. ZTest is selectable so the outline can either
        // respect scene depth (only where visible) or draw through everything (Always).
        Pass
        {
            Name "Outline"
            ZTest [_ZTest]
        CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment SpriteFragOutline
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"

            // _MainTex, _Color, SampleSpriteTexture, v2f, SpriteVert come from UnitySprites.cginc.
            float4 _MainTex_TexelSize;
            fixed4 _OutlineColor;
            float _OutlineThickness;
            float _AlphaThreshold;
            float _OutlineEnabled;

            fixed4 SpriteFragOutline(v2f IN) : SV_Target
            {
                if (_OutlineEnabled < 0.5) return 0;

                // Only border texels (transparent pixel next to an opaque one) get the outline.
                fixed centerA = SampleSpriteTexture(IN.texcoord).a;
                if (centerA >= _AlphaThreshold) return 0;   // inside the sprite - leave it to pass 1

                float2 t = _MainTex_TexelSize.xy * _OutlineThickness;
                fixed a = 0;
                a = max(a, SampleSpriteTexture(IN.texcoord + float2( t.x, 0)).a);
                a = max(a, SampleSpriteTexture(IN.texcoord + float2(-t.x, 0)).a);
                a = max(a, SampleSpriteTexture(IN.texcoord + float2(0,  t.y)).a);
                a = max(a, SampleSpriteTexture(IN.texcoord + float2(0, -t.y)).a);
                a = max(a, SampleSpriteTexture(IN.texcoord + float2( t.x,  t.y)).a);
                a = max(a, SampleSpriteTexture(IN.texcoord + float2(-t.x,  t.y)).a);
                a = max(a, SampleSpriteTexture(IN.texcoord + float2( t.x, -t.y)).a);
                a = max(a, SampleSpriteTexture(IN.texcoord + float2(-t.x, -t.y)).a);

                if (a <= _AlphaThreshold) return 0;

                // Premultiplied alpha to match Blend One OneMinusSrcAlpha.
                return fixed4(_OutlineColor.rgb * _OutlineColor.a, _OutlineColor.a);
            }
        ENDCG
        }
    }
}
