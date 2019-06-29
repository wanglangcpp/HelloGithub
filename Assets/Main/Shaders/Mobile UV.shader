Shader "Sawyer Shader/Mobile UV"
{
	Properties
	{
		_MainTex ("MainTex", 2D) = "white" {}
		_MainTexAplha("MainTexAplha",float) = 1
		_Color("Color",COLOR) = (0.5,0.5,0.5,1)
		_SpeedX("SpeedX",float) = 0
		_SpeedY("SpeedY",float) = 0

	}
	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 100
		cull Off
		ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			fixed4 _MainTex_ST;
			fixed _MainTexAplha;
			fixed4 _Color;
			fixed _SpeedX;
			fixed _SpeedY;
		
			
			v2f vert (appdata_base v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex) + frac(float2(_SpeedX,_SpeedY) * _Time);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv) * _MainTexAplha;
				return col*_Color;
			}
			ENDCG
		}
	}
}
