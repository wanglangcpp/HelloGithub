Shader "Dodjoy/Environment/BRDF_Around" {
Properties {
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "grey" {}
	_BRDFTex ("NdotL NdotH (RGBA)", 2D) = "white" {}
	_SpecMap ("SpecMap", 2D) = "white" {}
	_EmissionScale("Emission scale", float) = 1.0
}	 

SubShader { 
	Tags { "RenderType"="Opaque" "LightMode"="ForwardBase" }
	LOD 400
	
CGPROGRAM
#pragma surface surf PseudoBRDF exclude_path:prepass nolightmap noforwardadd approxview

struct MySurfaceOutput {
	fixed3 Albedo;
	fixed3 Normal;
	fixed3 Emission;
	fixed Specular;
	fixed Gloss;
	fixed Alpha;
};

sampler2D _BRDFTex;

inline fixed4 LightingPseudoBRDF (MySurfaceOutput s, fixed3 lightDir, fixed3 viewDir, fixed atten)
{
	// Half vector
	fixed3 halfDir = normalize (lightDir + viewDir);
	
	// N.L
	fixed NdotL = dot (s.Normal, lightDir);
	// N.H
	fixed NdotH = dot (s.Normal, halfDir);
	
	// remap N.L from [-1..1] to [0..1]
	// this way we can shade pixels facing away from the light - helps to simulate bounce lights
	fixed biasNdotL = NdotL * 0.5 + 0.5;
	
	// lookup light texture
	//  rgb = diffuse term
	//    a = specular term
	fixed4 l = tex2D (_BRDFTex, fixed2(biasNdotL, NdotH));

	fixed4 c;
	// mask specular term by Gloss factor
	// modulate specular with Albedo to allow metalic-ish look
	c.rgb = s.Albedo * (l.rgb + s.Gloss * l.a) * 2;
	c.a = 0;
	
	return c;
}


sampler2D _MainTex;
sampler2D _SpecMap;
fixed _EmissionScale;

struct Input {
	float2 uv_MainTex;
	float2 uv_SpecMap;
};

void surf (Input IN, inout MySurfaceOutput o) {
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	fixed4 spec = tex2D(_SpecMap, IN.uv_SpecMap);
	
	o.Emission = tex.rgb * spec.g * _EmissionScale;
	o.Albedo = tex.rgb;
	o.Gloss = spec.r;
	o.Alpha = tex.a;
}
ENDCG

	}

FallBack "Mobile/Diffuse"
}
