// Shader created with Shader Forge v1.27 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.27;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:True,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-5193-OUT,alpha-1005-OUT,clip-8099-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32138,y:32556,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843138,c2:0.3921569,c3:0.7843137,c4:1;n:type:ShaderForge.SFN_Tex2d,id:3506,x:32111,y:32793,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:f55fd16b8b0e5ad4eaeec06970efb5f4,ntxv:0,isnm:False|UVIN-7132-UVOUT,MIP-2339-OUT;n:type:ShaderForge.SFN_TexCoord,id:5599,x:31404,y:32680,varname:node_5599,prsc:2,uv:0;n:type:ShaderForge.SFN_Append,id:41,x:31404,y:32830,varname:node_41,prsc:2|A-4906-OUT,B-1511-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4906,x:31203,y:32792,ptovrint:False,ptlb:U,ptin:_U,varname:_U,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:1511,x:31203,y:32936,ptovrint:False,ptlb:V,ptin:_V,varname:_V,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:5193,x:32437,y:32570,varname:node_5193,prsc:2|A-7241-RGB,B-3506-RGB,C-6191-RGB;n:type:ShaderForge.SFN_Add,id:4788,x:31640,y:32793,varname:node_4788,prsc:2|A-5599-UVOUT,B-41-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2339,x:31895,y:33009,ptovrint:False,ptlb:Blur,ptin:_Blur,varname:_Blur,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Rotator,id:7132,x:31895,y:32793,varname:node_7132,prsc:2|UVIN-4788-OUT,ANG-4921-OUT;n:type:ShaderForge.SFN_Slider,id:4921,x:31468,y:33018,ptovrint:False,ptlb:Rotation,ptin:_Rotation,varname:_Rotation,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:3.272308,max:6;n:type:ShaderForge.SFN_Vector1,id:8099,x:32474,y:33042,varname:node_8099,prsc:2,v1:1;n:type:ShaderForge.SFN_Slider,id:2121,x:31880,y:33171,ptovrint:False,ptlb:TextureAlpha,ptin:_TextureAlpha,varname:_TextureAlpha,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:1005,x:32365,y:32908,varname:node_1005,prsc:2|A-3506-A,B-2121-OUT;n:type:ShaderForge.SFN_VertexColor,id:6191,x:32138,y:32416,varname:node_6191,prsc:2;proporder:7241-3506-4906-1511-2339-4921-2121;pass:END;sub:END;*/

Shader "Sawyer Shader/SimpleSlash 1.4" {
    Properties {
        _Color ("Color", Color) = (0.07843138,0.3921569,0.7843137,1)
        _MainTex ("MainTex", 2D) = "white" {}
        _U ("U", Float ) = 0
        _V ("V", Float ) = 0
        _Blur ("Blur", Float ) = 2
        _Rotation ("Rotation", Range(0, 6)) = 3.272308
        _TextureAlpha ("TextureAlpha", Range(0, 1)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 2.0
            #pragma glsl
            uniform float4 _Color;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform fixed _U;
            uniform fixed _V;
            uniform float _Blur;
            uniform fixed _Rotation;
            uniform float _TextureAlpha;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                clip(1.0 - 0.5);
////// Lighting:
////// Emissive:
                float node_7132_ang = _Rotation;
                float node_7132_spd = 1.0;
                float node_7132_cos = cos(node_7132_spd*node_7132_ang);
                float node_7132_sin = sin(node_7132_spd*node_7132_ang);
                float2 node_7132_piv = float2(0.5,0.5);
                float2 node_7132 = (mul((i.uv0+float2(_U,_V))-node_7132_piv,float2x2( node_7132_cos, -node_7132_sin, node_7132_sin, node_7132_cos))+node_7132_piv);
                float4 _MainTex_var = tex2Dlod(_MainTex,float4(TRANSFORM_TEX(node_7132, _MainTex),0.0,_Blur));
                float3 emissive = (_Color.rgb*_MainTex_var.rgb*i.vertexColor.rgb);
                float3 finalColor = emissive;
                return fixed4(finalColor,(_MainTex_var.a*_TextureAlpha));
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma exclude_renderers d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 2.0
            #pragma glsl
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                clip(1.0 - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    //CustomEditor "ShaderForgeMaterialInspector"
}
