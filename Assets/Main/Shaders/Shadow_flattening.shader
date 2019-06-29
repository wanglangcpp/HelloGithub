Shader "Sawyer Shader/ShadowFlattening"
{
	Properties
	{
		_Color("Color" , color) = (0.5,0.5,0.5,0.5)
		//_MainTex ("Texture", 2D) = "white" {}
		_High("High" , Range(-10,10)) = 0
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 100

	
		Pass
		{
			Stencil
            {
                Ref 0
                Comp equal
                Pass incrWrap
                Fail keep
                ZFail keep
            }
			Offset -1, -1
			
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
			};

			
			fixed4 _Color;
			float _High;

			v2f vert (appdata v)
			{
				v2f o;
				float4 wPos = mul(_Object2World , v.vertex);
				//float4 wPos = mul(unity_ObjectToWorld , v.vertex);
				wPos.y = _High;
				//float4 lPos = mul(unity_WorldToObject , wPos);
				float4 lPos = mul(_World2Object , wPos);
				o.pos = mul(UNITY_MATRIX_MVP, lPos );
				//o.pos =UnityObjectToClipPos( lPos);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{

				return _Color;
			}
			ENDCG
		}
	}
}
