// Shader created with Shader Forge v1.27 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.27;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:True,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33545,y:32949,varname:node_3138,prsc:2|emission-6984-OUT,alpha-9187-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32307,y:34778,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.7743199,c3:0.272,c4:1;n:type:ShaderForge.SFN_TexCoord,id:7651,x:30813,y:34147,varname:node_7651,prsc:2,uv:0;n:type:ShaderForge.SFN_Sin,id:9260,x:32121,y:34489,varname:node_9260,prsc:2|IN-6950-OUT;n:type:ShaderForge.SFN_RemapRange,id:3981,x:31667,y:34273,varname:node_3981,prsc:2,frmn:0,frmx:1,tomn:0,tomx:3.14|IN-76-OUT;n:type:ShaderForge.SFN_Power,id:9187,x:32589,y:34492,varname:node_9187,prsc:2|VAL-9260-OUT,EXP-9026-OUT;n:type:ShaderForge.SFN_Exp,id:9026,x:32079,y:34763,varname:node_9026,prsc:2,et:1|IN-1825-OUT;n:type:ShaderForge.SFN_Slider,id:8850,x:31521,y:34860,ptovrint:False,ptlb:Light Range,ptin:_LightRange,varname:_LightRange,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:7.425829,max:10;n:type:ShaderForge.SFN_RemapRange,id:1825,x:31876,y:34763,varname:node_1825,prsc:2,frmn:0,frmx:10,tomn:10,tomx:1|IN-8850-OUT;n:type:ShaderForge.SFN_Multiply,id:6940,x:32582,y:34775,varname:node_6940,prsc:2|A-7241-RGB,B-9187-OUT;n:type:ShaderForge.SFN_Vector1,id:864,x:32307,y:34934,varname:node_864,prsc:2,v1:3;n:type:ShaderForge.SFN_Add,id:6950,x:31938,y:34489,varname:node_6950,prsc:2|A-3981-OUT,B-3753-OUT;n:type:ShaderForge.SFN_ConstantClamp,id:6984,x:32821,y:34707,varname:node_6984,prsc:2,min:0.01,max:1|IN-6940-OUT;n:type:ShaderForge.SFN_Lerp,id:76,x:31271,y:34263,varname:node_76,prsc:2|A-7651-U,B-7651-V,T-5489-OUT;n:type:ShaderForge.SFN_Slider,id:7288,x:30608,y:34505,ptovrint:False,ptlb:Angle,ptin:_Angle,varname:_Angle,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:90,max:90;n:type:ShaderForge.SFN_RemapRange,id:5489,x:30948,y:34542,varname:node_5489,prsc:2,frmn:0,frmx:90,tomn:0,tomx:1|IN-7288-OUT;n:type:ShaderForge.SFN_Time,id:1501,x:31375,y:34423,varname:node_1501,prsc:2;n:type:ShaderForge.SFN_Multiply,id:3753,x:31542,y:34596,varname:node_3753,prsc:2|A-1501-T,B-3151-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3151,x:31226,y:34623,ptovrint:False,ptlb:Offset Value,ptin:_OffsetValue,varname:_OffsetValue,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;proporder:7241-8850-7288-3151;pass:END;sub:END;*/

Shader "Sawyer Shader/Simple Light Sweep" {
    Properties {
        _Color ("Color", Color) = (1,0.7743199,0.272,1)
        _LightRange ("Light Range", Range(0, 10)) = 7.425829
        _Angle ("Angle", Range(0, 90)) = 90
        _OffsetValue ("Offset Value", Float ) = 2
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
            Blend One One
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
            uniform float4 _Color;
            uniform fixed _LightRange;
            uniform fixed _Angle;
            uniform fixed _OffsetValue;
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
                float4 node_1501 = _Time + _TimeEditor;
                float node_9187 = pow(sin(((lerp(i.uv0.r,i.uv0.g,(_Angle*0.01111111+0.0))*3.14+0.0)+(node_1501.g*_OffsetValue))),exp2((_LightRange*-0.9+10.0)));
                float3 emissive = clamp((_Color.rgb*node_9187),0.01,1);
                float3 finalColor = emissive;
                return fixed4(finalColor,node_9187);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    //CustomEditor "ShaderForgeMaterialInspector"
}
