// Shader created with Shader Forge v1.27 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.27;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:32856,y:32712,varname:node_3138,prsc:2|emission-395-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32260,y:32770,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2d,id:8228,x:32260,y:32972,ptovrint:False,ptlb:Diffuse Texturemm,ptin:_DiffuseTexturemm,varname:node_8228,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:047e3c8c386c13b449fe6ee3ff4d4bd0,ntxv:0,isnm:False|UVIN-3727-OUT;n:type:ShaderForge.SFN_Multiply,id:395,x:32578,y:32886,varname:node_395,prsc:2|A-7241-RGB,B-8228-RGB,C-8647-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8647,x:32421,y:33083,ptovrint:False,ptlb:Expose,ptin:_Expose,varname:node_8647,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_TexCoord,id:4916,x:31702,y:32848,varname:node_4916,prsc:2,uv:0;n:type:ShaderForge.SFN_Slider,id:4955,x:31172,y:32544,ptovrint:False,ptlb:node_834,ptin:_node_834,varname:node_834,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0,max:1;n:type:ShaderForge.SFN_Time,id:7920,x:31537,y:33045,varname:node_7920,prsc:2;n:type:ShaderForge.SFN_Append,id:9781,x:31705,y:33160,varname:node_9781,prsc:2|A-5106-OUT,B-5945-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5106,x:31537,y:33234,ptovrint:False,ptlb:U,ptin:_U,varname:node_5106,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:5945,x:31537,y:33326,ptovrint:False,ptlb:V,ptin:_V,varname:node_5945,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Add,id:3727,x:32072,y:33032,varname:node_3727,prsc:2|A-4916-UVOUT,B-5074-OUT;n:type:ShaderForge.SFN_Multiply,id:5074,x:31896,y:33100,varname:node_5074,prsc:2|A-7920-TSL,B-9781-OUT;proporder:7241-8228-8647-5106-5945;pass:END;sub:END;*/

Shader "Sawyer Shader/SimpleDiffuseUV " {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _DiffuseTexturemm ("Diffuse Texturemm", 2D) = "white" {}
        _Expose ("Expose", Float ) = 1
        _U ("U", Float ) = 0
        _V ("V", Float ) = 1
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform sampler2D _DiffuseTexturemm; uniform float4 _DiffuseTexturemm_ST;
            uniform float _Expose;
            uniform float _U;
            uniform float _V;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_7920 = _Time + _TimeEditor;
                float2 node_3727 = (i.uv0+(node_7920.r*float2(_U,_V)));
                float4 _DiffuseTexturemm_var = tex2D(_DiffuseTexturemm,TRANSFORM_TEX(node_3727, _DiffuseTexturemm));
                float3 emissive = (_Color.rgb*_DiffuseTexturemm_var.rgb*_Expose);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
