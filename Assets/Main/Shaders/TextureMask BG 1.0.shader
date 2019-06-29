// Shader created with Shader Forge v1.27 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.27;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:True,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33448,y:32181,varname:node_3138,prsc:2|diff-2098-RGB,emission-429-OUT,alpha-2098-A,clip-2098-A;n:type:ShaderForge.SFN_Color,id:7241,x:32383,y:31739,ptovrint:False,ptlb:Textuer Color,ptin:_TextuerColor,varname:_TextuerColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.868853,c3:0.3308824,c4:1;n:type:ShaderForge.SFN_Tex2d,id:2098,x:32167,y:32133,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:ad713efbabeef7b4094e066bf66ee9b0,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:3802,x:30943,y:32022,ptovrint:False,ptlb:RGB Mask,ptin:_RGBMask,varname:_RGBMask,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:707d920aadbe7d247a304979532736a8,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ComponentMask,id:7755,x:31201,y:31949,varname:node_7755,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-3802-RGB;n:type:ShaderForge.SFN_ComponentMask,id:4208,x:31201,y:32105,varname:node_4208,prsc:2,cc1:1,cc2:-1,cc3:-1,cc4:-1|IN-3802-RGB;n:type:ShaderForge.SFN_Tex2d,id:3116,x:32016,y:31411,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:_Texture,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:bafe90a56be68ca4c85c28f7706485bb,ntxv:0,isnm:False|UVIN-9336-OUT;n:type:ShaderForge.SFN_Add,id:838,x:32756,y:31980,varname:node_838,prsc:2|A-6580-OUT,B-9291-OUT;n:type:ShaderForge.SFN_Slider,id:9291,x:32368,y:31917,ptovrint:False,ptlb:BG Expose,ptin:_BGExpose,varname:_BGExpose,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-2,cur:-0.9432514,max:1;n:type:ShaderForge.SFN_Slider,id:9057,x:31973,y:31774,ptovrint:False,ptlb:Textuer Screen Ctrl,ptin:_TextuerScreenCtrl,varname:_TextuerScreenCtrl,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0.313125,max:1;n:type:ShaderForge.SFN_Blend,id:1239,x:32383,y:31555,varname:node_1239,prsc:2,blmd:6,clmp:True|SRC-3116-RGB,DST-9057-OUT;n:type:ShaderForge.SFN_TexCoord,id:3139,x:31338,y:31421,varname:node_3139,prsc:2,uv:0;n:type:ShaderForge.SFN_Time,id:5835,x:31033,y:31373,varname:node_5835,prsc:2;n:type:ShaderForge.SFN_Append,id:7552,x:31185,y:31625,varname:node_7552,prsc:2|A-2835-OUT,B-3947-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2835,x:31014,y:31562,ptovrint:False,ptlb:Texture U,ptin:_TextureU,varname:_TextureU,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_ValueProperty,id:3947,x:31001,y:31696,ptovrint:False,ptlb:Texture V,ptin:_TextureV,varname:_TextureV,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:4441,x:31338,y:31570,varname:node_4441,prsc:2|A-5835-T,B-7552-OUT;n:type:ShaderForge.SFN_Add,id:9336,x:31798,y:31536,varname:node_9336,prsc:2|A-3139-UVOUT,B-4441-OUT;n:type:ShaderForge.SFN_ComponentMask,id:4638,x:31201,y:32266,varname:node_4638,prsc:2,cc1:2,cc2:-1,cc3:-1,cc4:-1|IN-3802-RGB;n:type:ShaderForge.SFN_ToggleProperty,id:6071,x:31346,y:31835,ptovrint:False,ptlb:Mask R Channel,ptin:_MaskRChannel,varname:_MaskRChannel,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True;n:type:ShaderForge.SFN_Multiply,id:5457,x:31543,y:31835,varname:node_5457,prsc:2|A-6071-OUT,B-7755-OUT;n:type:ShaderForge.SFN_Multiply,id:1109,x:31548,y:32028,varname:node_1109,prsc:2|A-5253-OUT,B-4208-OUT;n:type:ShaderForge.SFN_ToggleProperty,id:5253,x:31346,y:32049,ptovrint:False,ptlb:Mask G Channel,ptin:_MaskGChannel,varname:_MaskGChannel,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False;n:type:ShaderForge.SFN_ToggleProperty,id:4351,x:31346,y:32208,ptovrint:False,ptlb:Mask B Channel,ptin:_MaskBChannel,varname:_MaskBChannel,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False;n:type:ShaderForge.SFN_Multiply,id:1789,x:31548,y:32208,varname:node_1789,prsc:2|A-4351-OUT,B-4638-OUT;n:type:ShaderForge.SFN_Add,id:5433,x:31926,y:32030,varname:node_5433,prsc:2|A-5457-OUT,B-1109-OUT,C-1789-OUT;n:type:ShaderForge.SFN_Multiply,id:2677,x:32567,y:31677,varname:node_2677,prsc:2|A-1239-OUT,B-5433-OUT,C-7241-RGB;n:type:ShaderForge.SFN_Add,id:429,x:32980,y:31958,varname:node_429,prsc:2|A-2677-OUT,B-838-OUT;n:type:ShaderForge.SFN_ToggleProperty,id:9188,x:32167,y:32030,ptovrint:False,ptlb:node_9188,ptin:_node_9188,varname:_node_9188,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True;n:type:ShaderForge.SFN_Multiply,id:6580,x:32525,y:31992,varname:node_6580,prsc:2|A-9188-OUT,B-2098-RGB;proporder:2098-9291-3802-3116-7241-9057-2835-3947-6071-5253-4351-9188;pass:END;sub:END;*/

Shader "Sawyer Shader/TextureMask BG 1.1" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _BGExpose ("BG Expose", Range(-2, 1)) = -0.9432514
        _RGBMask ("RGB Mask", 2D) = "white" {}
        _Texture ("Texture", 2D) = "white" {}
        _TextuerColor ("Textuer Color", Color) = (1,0.868853,0.3308824,1)
        _TextuerScreenCtrl ("Textuer Screen Ctrl", Range(-1, 1)) = 0.313125
        _TextureU ("Texture U", Float ) = 0.2
        _TextureV ("Texture V", Float ) = 0
        [MaterialToggle] _MaskRChannel ("Mask R Channel", Float ) = 1
        [MaterialToggle] _MaskGChannel ("Mask G Channel", Float ) = 0
        [MaterialToggle] _MaskBChannel ("Mask B Channel", Float ) = 0
        [MaterialToggle] _node_9188 ("node_9188", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
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
            uniform float4 _TimeEditor;
            uniform float4 _TextuerColor;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _RGBMask; uniform float4 _RGBMask_ST;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float _BGExpose;
            uniform float _TextuerScreenCtrl;
            uniform fixed _TextureU;
            uniform fixed _TextureV;
            uniform fixed _MaskRChannel;
            uniform fixed _MaskGChannel;
            uniform fixed _MaskBChannel;
            uniform fixed _node_9188;
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
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                clip(_MainTex_var.a - 0.5);
////// Lighting:
////// Emissive:
                float4 node_5835 = _Time + _TimeEditor;
                float2 node_9336 = (i.uv0+(node_5835.g*float2(_TextureU,_TextureV)));
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(node_9336, _Texture));
                float4 _RGBMask_var = tex2D(_RGBMask,TRANSFORM_TEX(i.uv0, _RGBMask));
                float3 emissive = ((saturate((1.0-(1.0-_Texture_var.rgb)*(1.0-_TextuerScreenCtrl)))*((_MaskRChannel*_RGBMask_var.rgb.r)+(_MaskGChannel*_RGBMask_var.rgb.g)+(_MaskBChannel*_RGBMask_var.rgb.b))*_TextuerColor.rgb)+((_node_9188*_MainTex_var.rgb)+_BGExpose));
                float3 finalColor = emissive;
                return fixed4(finalColor,_MainTex_var.a);
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
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                clip(_MainTex_var.a - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
   // CustomEditor "ShaderForgeMaterialInspector"
}
