Shader "Hidden/RadialBlur"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BlurStrength ("Blur Strength", Range(0, 1)) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float _BlurStrength;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 sum = col;

				float2 dir = i.uv - 0.5;
				float dist = length(dir);
				
				sum += tex2D(_MainTex, 0.5 + dir * 0.8);
				sum += tex2D(_MainTex, 0.5 + dir * 0.85);
				sum += tex2D(_MainTex, 0.5 + dir * 0.9);
				sum += tex2D(_MainTex, 0.5 + dir * 0.95);
				sum += tex2D(_MainTex, 0.5 + dir * 1.05);
				sum += tex2D(_MainTex, 0.5 + dir * 1.1);
				sum += tex2D(_MainTex, 0.5 + dir * 1.15);
				sum += tex2D(_MainTex, 0.5 + dir * 1.2);
				sum /= 9;

				float t = saturate(dist * 2 * _BlurStrength);

				return lerp(col, sum, t);// fixed4(lerp(col, sum, t).rgb, col.a);
			}
			ENDCG
		}
	}
}
