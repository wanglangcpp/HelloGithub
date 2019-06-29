Shader "Game/Highlight Projector"
{
    Properties
    {
        _ShadowTex("Projected Image", 2D) = "white" {}
        _HighlightFactor("Factor", Range(0.0, 3.0)) = 1.0
        _MainColor("Main Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Pass
        {
            Blend DstColor One
            ZWrite Off
            Offset -1, -1

            CGPROGRAM

            #pragma vertex vert  
            #pragma fragment frag 

            uniform sampler2D _ShadowTex;
            uniform fixed _HighlightFactor;
            uniform fixed4 _MainColor;

            // Projector-specific uniforms
            uniform float4x4 _Projector; // transformation matrix from object space to projector space 

            struct vertexInput
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct vertexOutput
            {
                float4 pos : SV_POSITION;
                float4 posProj : TEXCOORD0; // position in projector space
            };

            vertexOutput vert(vertexInput input)
            {
                vertexOutput output;

                output.posProj = mul(_Projector, input.vertex);
                output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
                return output;
            }

            fixed4 frag(vertexOutput input) : COLOR
            {
                // In front of projector and within the boundary of the texture.
                if (input.posProj.w > 0.0 && input.posProj.x <= 1.0 && input.posProj.y <= 1.0 && input.posProj.x >= 0.0 && input.posProj.y >= 0.0)
                {
                    return tex2Dproj(_ShadowTex, input.posProj) * _MainColor * _HighlightFactor;
                }

                return fixed4(0.0, 0.0, 0.0, 0.0);
            }

            ENDCG
        }
    }

	Fallback "Mobile/Diffuse"
}