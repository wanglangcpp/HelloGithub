Shader "Sawyer Shader/MobileAdditive 1.3" {
//1.1去除精简了软粒子显示；
//1.2增加高亮颜色调整；
//1.3增加uv位移；

Properties {
	_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_MainTex ("Texture", 2D) = "white" {}
	_SpeedX("SpeedX(U)",float) = 0
	_SpeedY("SpeedY(V)",float) = 0
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha One
	ColorMask RGB
	Cull Off Lighting Off ZWrite Off
	
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _TintColor;
			fixed4 _MainTex_ST;
			fixed _SpeedX;
			fixed _SpeedY;

				struct appdata_t 
			{
				fixed4 vertex : POSITION;
				fixed4 color : COLOR;
				fixed2 uv : TEXCOORD0;
			};

			struct v2f 
			{
				fixed4 vertex : POSITION;
				fixed4 color : COLOR;
				fixed2 uv : TEXCOORD0;		
			};
			
			v2f vert (v2f v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
				o.uv = TRANSFORM_TEX(v.uv.xy, _MainTex) + frac(fixed2(_SpeedX,_SpeedY) * _Time);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = 4.0f * i.color * _TintColor * tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG 
		}
	}	
}
}
