Shader "Game/Mobile/Diffuse + Fading"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _TintColor ("Tint Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent" "Queue"="Transparent+500"
        }

        LOD 150
        ZWrite Off

        CGPROGRAM
        #pragma surface surf Lambert noforwardadd alpha:blend

        sampler2D _MainTex;
        fixed4 _TintColor;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _TintColor;
            o.Albedo = c;
            o.Alpha = c.a;
        }
        ENDCG
    }

    Fallback "Mobile/Diffuse"
}
