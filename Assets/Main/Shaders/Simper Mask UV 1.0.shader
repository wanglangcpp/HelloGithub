// Shader created with Shader Forge v1.27 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.27;sub:START;pass:START;ps:flbk:Mobile/Particles/Additive,iptp:0,cusa:False,bamd:0,lico:0,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:True,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4795,x:32724,y:32693,varname:node_4795,prsc:2|emission-2393-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:31775,y:32827,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:f4f048b445a95ed46a793a2ea66ab877,ntxv:0,isnm:False|UVIN-9766-OUT;n:type:ShaderForge.SFN_Multiply,id:2393,x:32495,y:32793,varname:node_2393,prsc:2|A-3881-OUT,B-2053-RGB,C-797-RGB,D-9978-OUT;n:type:ShaderForge.SFN_VertexColor,id:2053,x:32258,y:32451,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Color,id:797,x:32258,y:33071,ptovrint:True,ptlb:Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Tex2d,id:4353,x:31557,y:32435,ptovrint:False,ptlb:Mask,ptin:_Mask,varname:_Mask,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:d2505e1fdde7de94c874d9661b0a4020,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:3881,x:32042,y:32762,varname:node_3881,prsc:2|A-4677-OUT,B-6074-RGB;n:type:ShaderForge.SFN_Slider,id:5011,x:31478,y:32675,ptovrint:False,ptlb:Mask Alpha,ptin:_MaskAlpha,varname:_MaskAlpha,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Blend,id:4677,x:31777,y:32467,varname:node_4677,prsc:2,blmd:1,clmp:True|SRC-4353-RGB,DST-5011-OUT;n:type:ShaderForge.SFN_Slider,id:9978,x:32101,y:33271,ptovrint:False,ptlb:Texture Expose,ptin:_TextureExpose,varname:_TextureExpose,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:2,max:4;n:type:ShaderForge.SFN_TexCoord,id:9658,x:31055,y:32640,varname:node_9658,prsc:2,uv:0;n:type:ShaderForge.SFN_Time,id:8207,x:31086,y:32920,varname:node_8207,prsc:2;n:type:ShaderForge.SFN_Append,id:7725,x:31140,y:33188,varname:node_7725,prsc:2|A-5651-OUT,B-8658-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5651,x:30810,y:33090,ptovrint:False,ptlb:U,ptin:_U,varname:_U,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_ValueProperty,id:8658,x:30798,y:33220,ptovrint:False,ptlb:V,ptin:_V,varname:_V,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:3075,x:31340,y:32999,varname:node_3075,prsc:2|A-8207-T,B-7725-OUT;n:type:ShaderForge.SFN_Add,id:9766,x:31557,y:32807,varname:node_9766,prsc:2|A-9658-UVOUT,B-3075-OUT;proporder:6074-797-9978-5651-8658-4353-5011;pass:END;sub:END;*/

Shader "Sawyer Shader/Simple Mask UV 1.3" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _TintColor ("Color", Color) = (0.5,0.5,0.5,1)
        _TextureExpose ("Texture Expose", Range(0, 4)) = 2
        _U ("U", Float ) = 0.1
        _V ("V", Float ) = 0
        _Mask ("Mask", 2D) = "white" {}
        _MaskAlpha ("Mask Alpha", Range(0, 1)) = 1
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
            Blend One One
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 2.0
            uniform float4 _TimeEditor;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _TintColor;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform float _MaskAlpha;
            uniform float _TextureExpose;
            uniform float _U;
            uniform float _V;
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
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                float4 node_8207 = _Time + _TimeEditor;
                float2 node_9766 = (i.uv0+(node_8207.g*float2(_U,_V)));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_9766, _MainTex));
                float3 emissive = ((saturate((_Mask_var.rgb*_MaskAlpha))*_MainTex_var.rgb)*i.vertexColor.rgb*_TintColor.rgb*_TextureExpose);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0,0,0,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Mobile/Particles/Additive"
   // CustomEditor "ShaderForgeMaterialInspector"
}
