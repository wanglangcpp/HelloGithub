Shader "Sawyer Shader/UV_BG"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MainColor("MainColor", COLOR) = (1,1,1,1)
		_Main_USpeed("Main_USpeed", float) = 1
		_Main_VSpeed("Main_VSpeed",float) = 1
		_SinSpeed("TexUVSpeed", float) =1
		_USpeed1("SinUVSpeed", Range(-5, 5)) = 1
		_USpeed2("SinUVSpeed2", Range(-5, 5)) = 1
		_USpeed3("SinUVSpeed3", Range(-5, 5)) = 1
		_colorEmission("color1Emission",Range(-2 , 2)) = 1
		//_color2Emission("color2Emission",Range(-2 , 2)) = 1
		//_color3Emission("color3Emission",Range(-2 , 2)) = 1

	}
	SubShader
	{
		Tags { 
				"IgnoreProjector"="True"
            	"Queue"="Transparent"
            	"RenderType"="Transparent"
		
		}
		LOD 100

		
		Blend SrcAlpha One
		Cull Off Lighting Off ZWrite Off 
 
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _MainColor;
			fixed _Main_USpeed;
			fixed _Main_VSpeed;
			fixed _SinSpeed;
			fixed _USpeed1;
			fixed _USpeed2;
			fixed _USpeed3;

			fixed _colorEmission;
			
			v2f vert (appdata_base v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
				//o.uv = v.tecoord.xy;
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				//fixed4 col = tex2D(_MainTex, i.uv);
				fixed2 uv = i.uv;
				fixed offset_uv =_USpeed2 * sin(i.uv * _SinSpeed + _Time.x *_USpeed3);
				uv += offset_uv;

				uv.x += _Time.x * _Main_USpeed;
				uv.y += _Time.y * _Main_VSpeed;
				fixed4 color_0 = tex2D(_MainTex, uv);
				// color_1. b /= 10 * _color1Emission;

				fixed4 color_1 =tex2D(_MainTex, uv);
				uv -=offset_uv  ;
				// color_2.g /= 10 * _color2Emission;


				fixed4 color_2 =tex2D(_MainTex, uv);
				fixed offset_uv2 =_USpeed2 * cos(i.uv * _SinSpeed + _Time.x *_USpeed3);
				// uv.x += _Time.x * _USpeed3;
				 uv -=offset_uv2;
				// color_3.r /= 10 * _color2Emission;

				fixed4 finalcol =_MainColor * _colorEmission *(color_0  + color_1+ color_2) ;

				//col = finalcol;

				return finalcol;
			}
			ENDCG
		}
	}
}
