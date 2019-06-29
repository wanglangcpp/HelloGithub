Shader "Game/Scene Darkening (Post Effect)"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (.2, .2, .2, .2)
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    
    SubShader
    {
        Pass
        {
            Stencil
            {
                Ref 1
                ReadMask 1
                Comp NotEqual
            }

            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform fixed4 _MainColor;

            fixed4 frag(v2f_img i) : SV_Target
            {
                return tex2D(_MainTex, i.uv) * _MainColor;
            }

            ENDCG
        }
    }

    FallBack "Mobile/Diffuse"
}
