// Reference: Particles/Alpha Blended

Shader "Game/Particles/Alpha Blended (Culling, Z-Test)"
{
    Properties
    {
        _Illum ("Illum", Range(0.1, 10)) = 1.0
        _Alpha ("Alpha", Range(0, 1)) = 0.5
        _MainTex ("Particle Texture", 2D) = "white" {}
    }

    Category
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask RGB
        Lighting Off
        ZTest LEqual
        Cull Back

        SubShader
        {
            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_particles
                #pragma multi_compile_fog

                #include "UnityCG.cginc"

                sampler2D _MainTex;
                fixed4 _TintColor;

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    fixed4 color : COLOR;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    fixed4 color : COLOR;
                    float2 texcoord : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                };

                float4 _MainTex_ST;
                fixed _Illum;
                fixed _Alpha;

                v2f vert (appdata_t v)
                {
                    v2f o;
                    o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                    o.color = v.color;
                    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                    UNITY_TRANSFER_FOG(o, o.vertex);
                    return o;
                }

                
                fixed4 frag (v2f i) : SV_Target
                {                    
                    fixed4 col = i.color * tex2D(_MainTex, i.texcoord);
                    col.rgb = 1.0 - pow(1.0 - col.rgb, _Illum);
                    col.a *= _Alpha;
                    UNITY_APPLY_FOG(i.fogCoord, col);
                    return col;
                }
                ENDCG 
            }
        }    
    }

    Fallback "Particles/Alpha Blended"
}
