// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Game/Simple Water" {

	Properties {
		_Normal("Normal", 2D) = "bump" {}
		_MainColor ("Base Ocean Color", Color) = (0.1, 0.4, 0.3, 1)
		_MainAlpha("Base Alpha",Range(0,1)) = 0.8
	
		_ReflectionTex("Reflection Texture",2D) = "white"{}
		_ReflectionBias("Reflection Bias",float) = 0.01
		_ReflectionAlpha("Reflection Alpha", Range(0,1)) = 1.0
	
		_Shininess ("Specular Shininess", Range (2.0, 500.0)) = 200.0
	
		_FogColor ("Ocean Fog color", COLOR)  = ( 0.11, .11, .32, 1)
		_FogStrength ("Fog Strength", Range(0,1))  = 0.3
		_FogShininess ("Fog Shininess", Range(0,10)) = 2
	
		_InvFadeParemeter ("Edge Blend", float) = 0.04
	
		_DirectionUv("Wet scroll direction (2 samples)", Vector) = (2.0,2.0, 0,-1.0)
		_TexAtlasTiling("Tex atlas tiling", Vector) = (8.0,8.0, 20.0,20.0)
	
		_SunPos("Sun light position)",Vector) = (0,0,0,0)
	}
	
	CGINCLUDE	
	
	#include "UnityCG.cginc"
	
	fixed4 _MainColor;
	half _MainAlpha;
	
	half _ReflectionBias;
	half _ReflectionAlpha;
	
	fixed4 _FogColor;
	half _FogStrength;
	half4 _DirectionUv;
	half4 _TexAtlasTiling;
	
	half _Shininess;
	half _FogShininess;
	
	half4 _SunPos;
	
	sampler2D _Normal;
	sampler2D _ReflectionTex;
	
	sampler2D _CameraDepthTexture;
	half4 _CameraDepthNormalsTexture_ST;
	half _InvFadeParemeter;
	
	struct v2f_full {
		half4 pos : SV_POSITION;
		half4 normalScrollUv : TEXCOORD0;
		half4 screen : TEXCOORD1;
		half4 viewInterpolator : TEXCOORD2;
		half4 testUV : TEXCOORD3;
	};
	
	ENDCG
	
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent-549" }
	
		LOD 200
	
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			ZTest LEqual
			ZWrite Off
			Cull Off
	
			CGPROGRAM
			
			v2f_full vert (appdata_full v) {
				v2f_full o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.viewInterpolator = mul (_Object2World, v.vertex);
				o.viewInterpolator.xyz -= _WorldSpaceCameraPos;
				o.viewInterpolator.w = length(o.viewInterpolator);
				o.normalScrollUv.xyzw = v.texcoord.xyxy * _TexAtlasTiling + frac((_Time.xxxx) * _DirectionUv.xyxy * _DirectionUv.w);
				o.testUV = v.texcoord;
				o.screen = ComputeScreenPos(o.pos);
				return o;
			}
	
			fixed4 frag (v2f_full i) : COLOR0 {
				fixed4 normalMap_xy = tex2D(_Normal, i.normalScrollUv.xy);
				fixed4 normalMap_zw = tex2D(_Normal, i.normalScrollUv.zw);
	
				half4 edgeBlendFactors = half4(1.0, 0.0, 0.0, 0.0);
	
				fixed4 bump = normalMap_xy + normalMap_zw;
				bump.xy = bump.wy - fixed2(1.0, 1.0);
				fixed3 worldNormal = normalize(fixed3(0,1,0) + bump.xxy * fixed3(1,0,1));
				fixed3 viewVector = normalize(i.viewInterpolator.xyz);
				fixed3 reflectVector = normalize(reflect(viewVector, worldNormal));
	
				fixed3 h = normalize (_SunPos.xyz + viewVector.xyz);
				fixed nh = max (0, dot (reflectVector, h));
				fixed spec = max(0,pow (nh, _Shininess));
	
				fixed4 rtReflNorm = (normalMap_zw - 0.5) * _ReflectionBias;
				//fixed4 rtRefl = tex2D(_ReflectionTex , (i.screen.xy / i.screen.w) + rtReflNorm.xy);
				fixed4 rtRefl = tex2D(_ReflectionTex ,i.testUV.xy+ rtReflNorm.xy);
				fixed t_ShadowAlpha = rtRefl.a * _ReflectionAlpha;
				rtRefl.a = 1;
	
				fixed t_fogScale = pow(1 + viewVector.y,_FogShininess);
				fixed t_fogRange = min(max(-0.2,t_fogScale),1);
	
				fixed4 baseColor = _MainColor * (1 ) + (rtRefl * t_ShadowAlpha) + (t_fogRange * (_FogColor + (spec + nh) * _FogStrength));
				baseColor.a = _MainAlpha;
	
				return baseColor;
	
			}
	
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
	
			ENDCG
		}
	
	}
	
	FallBack "Diffuse"
}
