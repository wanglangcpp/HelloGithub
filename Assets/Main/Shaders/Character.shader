Shader "Game/Character"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
		_EmissionBody("Emission", Color) = (0.3, 0.3, 0.3, 1)
        _RimColor ("Rim Color", Color) = (0, 0, 0, 0)
        _InnerColor ("Inner Color", Color) = (0, 0, 0, 0)
        _InnerColorPower ("Inner Color Power", Range(0.0, 1.0)) = 0.0
        _RimPower ("Rim Power", Range(0.0, 5.0)) = 0.0
        _AlphaPower ("Alpha Rim Power", Range(0.0,8.0)) = 0.0
        _AllPower ("All Power", Range(0.0, 10.0)) = 0.0
        _StencilRef ("Stencil Reference", Int) = 1

        _DissolveTex ("Dissolve (R)", 2D) = "white" {}
        _DissolveColor ("Dissolve Color", Color) = (0, 0, 0, 0)
        _DissolveTile ("Dissolve Tile", Range(0.01, 1)) = 1.0
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0.0
        _DissolveSize ("Dissolve Size", Range(0, 1)) = 0.1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry+600"}
        LOD 150
        Stencil
        {
            Ref [_StencilRef]
            WriteMask 3
            ZFail Keep
            Pass Replace
            Fail Keep
        }

        CGPROGRAM
        #pragma surface surf Lambert noforwardadd

        sampler2D _MainTex;
        fixed4 _RimColor;
        fixed _RimPower;
        fixed _AlphaPower;
        fixed _AlphaMin;
        fixed _InnerColorPower;
        fixed _AllPower;
        fixed4 _InnerColor;
		fixed4 _EmissionBody;

        sampler2D _DissolveTex;
        fixed _DissolveTile;
        half _DissolveAmount;
        half _DissolveSize;
        fixed4 _DissolveColor;

        struct Input
        {
            float2 uv_MainTex;
            float3 viewDir; INTERNAL_DATA
        };

        void dissolve(Input IN, inout SurfaceOutput o)
        {
            if (_DissolveAmount <= 0.0)
            {
                return;
            }

            half clipValue = tex2D(_DissolveTex, IN.uv_MainTex / _DissolveTile).r;
            half clipAmount = clipValue - _DissolveAmount;
            if (clipAmount <= 0.0)
            {
                discard;
            }
            else if (clipAmount < _DissolveSize)
            {
                o.Albedo += _DissolveColor;
            }
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
            o.Emission = _RimColor.rgb * pow (rim, _RimPower) * _AllPower + (_InnerColor.rgb * 2 * _InnerColorPower)+(c * 2 * _EmissionBody);

            o.Alpha = c.a;
            half a = clamp((pow(rim, _AlphaPower)) * _AllPower, 0.0, 1.0);
            o.Albedo = (1.0 - a) * c.rgb;
            dissolve(IN, o);
        }
        ENDCG
    }

    Fallback "Mobile/VertexLit"
}
