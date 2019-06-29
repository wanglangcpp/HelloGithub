// Shader created with Shader Forge v1.27 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.27;sub:START;pass:START;ps:flbk:Sawyer Shader/MobileAdditive 1.0,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:False;n:type:ShaderForge.SFN_Final,id:4795,x:32922,y:32668,varname:node_4795,prsc:2|emission-6291-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:31876,y:32150,ptovrint:False,ptlb:TexTure1,ptin:_TexTure1,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c381121bbdd7d78478c4bc440387c6d7,ntxv:0,isnm:False|UVIN-6819-OUT;n:type:ShaderForge.SFN_Multiply,id:2393,x:32306,y:32526,varname:node_2393,prsc:2|A-6074-RGB,B-2053-RGB,C-797-RGB,D-5047-OUT;n:type:ShaderForge.SFN_VertexColor,id:2053,x:31655,y:32638,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Color,id:797,x:31955,y:32494,ptovrint:True,ptlb: Texture1Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Slider,id:5047,x:31847,y:32662,ptovrint:False,ptlb:Texture1 Light Stanger,ptin:_Texture1LightStanger,varname:node_5047,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1.675218,max:4;n:type:ShaderForge.SFN_Append,id:4417,x:31229,y:32645,varname:node_4417,prsc:2|A-3008-OUT,B-385-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3008,x:31027,y:32664,ptovrint:False,ptlb:Texture1 U,ptin:_Texture1U,varname:node_3008,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:385,x:31027,y:32771,ptovrint:False,ptlb:Texture1 V,ptin:_Texture1V,varname:node_385,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_Multiply,id:1729,x:31406,y:32464,varname:node_1729,prsc:2|A-5531-T,B-4417-OUT;n:type:ShaderForge.SFN_Add,id:6819,x:31681,y:32288,varname:node_6819,prsc:2|A-3898-UVOUT,B-1729-OUT;n:type:ShaderForge.SFN_Tex2d,id:2741,x:31925,y:32790,ptovrint:False,ptlb:Texture2,ptin:_Texture2,varname:_MainTex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-4950-OUT;n:type:ShaderForge.SFN_TexCoord,id:3898,x:31443,y:32708,varname:node_3898,prsc:2,uv:0;n:type:ShaderForge.SFN_Time,id:5531,x:31229,y:32817,varname:node_5531,prsc:2;n:type:ShaderForge.SFN_Append,id:5058,x:31200,y:33040,varname:node_5058,prsc:2|A-3228-OUT,B-9700-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3228,x:30998,y:33059,ptovrint:False,ptlb:Texture2 U,ptin:_Texture2U,varname:_U_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:9700,x:30998,y:33166,ptovrint:False,ptlb:Texture2 V,ptin:_Texture2V,varname:_V_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:1154,x:31477,y:33072,varname:node_1154,prsc:2|A-5531-T,B-5058-OUT;n:type:ShaderForge.SFN_Add,id:4950,x:31655,y:32892,varname:node_4950,prsc:2|A-3898-UVOUT,B-1154-OUT;n:type:ShaderForge.SFN_Multiply,id:4268,x:32277,y:32803,varname:node_4268,prsc:2|A-2741-RGB,B-2053-RGB,C-7830-RGB,D-5624-OUT;n:type:ShaderForge.SFN_Color,id:7830,x:31925,y:32982,ptovrint:False,ptlb:Texture2Color,ptin:_Texture2Color,varname:node_7830,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.003999949,c3:1,c4:1;n:type:ShaderForge.SFN_Slider,id:5624,x:31860,y:33192,ptovrint:False,ptlb:Texture2 Lighting Stanger,ptin:_Texture2LightingStanger,varname:node_5624,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.7597823,max:4;n:type:ShaderForge.SFN_Add,id:6291,x:32509,y:32686,varname:node_6291,prsc:2|A-2393-OUT,B-4268-OUT;proporder:6074-797-5047-3008-385-2741-3228-9700-7830-5624;pass:END;sub:END;*/

Shader "Sawyer Shader/UV Addtive 1.0" {
    Properties {
        _TexTure1 ("TexTure1", 2D) = "white" {}
        _TintColor (" Texture1Color", Color) = (1,0,0,1)
        _Texture1LightStanger ("Texture1 Light Stanger", Range(0, 4)) = 1.675218
        _Texture1U ("Texture1 U", Float ) = 0
        _Texture1V ("Texture1 V", Float ) = 0.2
        _Texture2 ("Texture2", 2D) = "white" {}
        _Texture2U ("Texture2 U", Float ) = 0
        _Texture2V ("Texture2 V", Float ) = 1
        _Texture2Color ("Texture2Color", Color) = (0,0.003999949,1,1)
        _Texture2LightingStanger ("Texture2 Lighting Stanger", Range(0, 4)) = 0.7597823
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha One 
            Cull Off Lighting Off ZWrite Off 
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _TexTure1; uniform float4 _TexTure1_ST;
            uniform float4 _TintColor;
            uniform float _Texture1LightStanger;
            uniform float _Texture1U;
            uniform float _Texture1V;
            uniform sampler2D _Texture2; uniform float4 _Texture2_ST;
            uniform float _Texture2U;
            uniform float _Texture2V;
            uniform float4 _Texture2Color;
            uniform float _Texture2LightingStanger;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_5531 = _Time + _TimeEditor;
                float2 node_6819 = (i.uv0+(node_5531.g*float2(_Texture1U,_Texture1V)));
                float4 _TexTure1_var = tex2D(_TexTure1,TRANSFORM_TEX(node_6819, _TexTure1));
                float2 node_4950 = (i.uv0+(node_5531.g*float2(_Texture2U,_Texture2V)));
                float4 _Texture2_var = tex2D(_Texture2,TRANSFORM_TEX(node_4950, _Texture2));
                float3 emissive = ((_TexTure1_var.rgb*i.vertexColor.rgb*_TintColor.rgb*_Texture1LightStanger)+(_Texture2_var.rgb*i.vertexColor.rgb*_Texture2Color.rgb*_Texture2LightingStanger));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0,0,0,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Sawyer Shader/MobileAdditive 1.0"
    CustomEditor "ShaderForgeMaterialInspector"
}
