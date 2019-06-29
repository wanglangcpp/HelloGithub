Shader "Game/Unlit/Overlayed Texture (No fog)"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _OverlayColor ("Overlaying Color", Color) = (.5, .5, .5, 1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        
        Pass
        {
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_fog
                
                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    half2 texcoord : TEXCOORD0;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                fixed4 _OverlayColor;
                
                v2f vert (appdata_t v)
                {
                    v2f o;
                    o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                    return o;
                }
                
                fixed4 frag (v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.texcoord);
                    fixed luminance = dot(col, fixed4(0.2126, 0.7152, 0.0722, 0));
                    fixed oldAlpha = col.a;
                    
                    if (luminance < 0.5)
                    {
                        col = 2 * _OverlayColor * col;
                    }
                    else
                    {
                        col = 1 - 2 * (1 - col) * (1 - _OverlayColor);
                    }
                   
                    col.a  = oldAlpha * _OverlayColor.a;
                    return col;
                }
            ENDCG
        }
    }

}
