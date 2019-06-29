Shader "Game/Legacy Shaders/Transparent/Cutout/Diffuse + Fading"
{
    Properties {
        _Color ("Main Color", Color) = (1, 1, 1, 1)
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }

    SubShader {
        Tags {"Queue"="Transparent+500" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
        LOD 200
        
        CGPROGRAM
        #pragma surface surf Lambert alphatest:_Cutoff alpha:blend

        sampler2D _MainTex;
        fixed4 _Color;

        struct Input
        
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }

    Fallback "Legacy Shaders/Transparent/Cutout/Diffuse"
}
