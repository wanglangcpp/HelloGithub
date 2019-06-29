// Shader created with Shader Forge v1.27 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.27;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:True,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4795,x:32724,y:32693,varname:node_4795,prsc:2|emission-297-OUT,voffset-4515-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:31463,y:32574,ptovrint:False,ptlb:Main Texture,ptin:_MainTexture,varname:_MainTexture,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c9d9e230b69372f4e9a798e5ab826143,ntxv:0,isnm:False|UVIN-5295-OUT;n:type:ShaderForge.SFN_Multiply,id:2393,x:31733,y:32750,varname:node_2393,prsc:2|A-6074-RGB,B-2053-RGB,C-797-RGB,D-9248-OUT,E-8330-OUT;n:type:ShaderForge.SFN_VertexColor,id:2053,x:31463,y:32746,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Color,id:797,x:31463,y:32904,ptovrint:True,ptlb:Main Texture Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Vector1,id:9248,x:31463,y:33055,varname:node_9248,prsc:2,v1:2;n:type:ShaderForge.SFN_NormalVector,id:8269,x:30406,y:32443,prsc:2,pt:False;n:type:ShaderForge.SFN_ViewVector,id:6428,x:30435,y:32217,varname:node_6428,prsc:2;n:type:ShaderForge.SFN_Dot,id:1846,x:30662,y:32304,varname:node_1846,prsc:2,dt:1|A-6428-OUT,B-8269-OUT;n:type:ShaderForge.SFN_Fresnel,id:742,x:31321,y:32411,varname:node_742,prsc:2|EXP-5921-OUT;n:type:ShaderForge.SFN_Slider,id:4894,x:30374,y:32628,ptovrint:False,ptlb:OutLine Fresnel ,ptin:_OutLineFresnel,varname:_OutLineFresnel,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5409656,max:1;n:type:ShaderForge.SFN_Multiply,id:4758,x:30856,y:32403,varname:node_4758,prsc:2|A-1846-OUT,B-2847-OUT;n:type:ShaderForge.SFN_Add,id:9201,x:32061,y:32405,varname:node_9201,prsc:2|A-9981-OUT,B-7167-OUT;n:type:ShaderForge.SFN_Multiply,id:297,x:32145,y:32613,varname:node_297,prsc:2|A-9201-OUT,B-2393-OUT;n:type:ShaderForge.SFN_Slider,id:8330,x:31170,y:32799,ptovrint:False,ptlb:Main Texture Alpha,ptin:_MainTextureAlpha,varname:_MainTextureAlpha,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.8694004,max:2;n:type:ShaderForge.SFN_Tex2d,id:9010,x:31058,y:31902,varname:node_9010,prsc:2,tex:9cfcafa71217ce3459aa8a458ef5d10b,ntxv:0,isnm:False|UVIN-2712-OUT,TEX-9118-TEX;n:type:ShaderForge.SFN_Tex2d,id:9191,x:30234,y:31911,varname:node_9191,prsc:2,tex:9cfcafa71217ce3459aa8a458ef5d10b,ntxv:0,isnm:False|UVIN-5639-OUT,TEX-9118-TEX;n:type:ShaderForge.SFN_TexCoord,id:9798,x:30246,y:31733,varname:node_9798,prsc:2,uv:0;n:type:ShaderForge.SFN_Power,id:7107,x:31420,y:31884,varname:node_7107,prsc:2|VAL-9010-G,EXP-8552-OUT;n:type:ShaderForge.SFN_Multiply,id:7167,x:31805,y:31994,varname:node_7167,prsc:2|A-7107-OUT,B-9191-G;n:type:ShaderForge.SFN_Tex2dAsset,id:9118,x:30068,y:32031,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:_Texture,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:9cfcafa71217ce3459aa8a458ef5d10b,ntxv:0,isnm:False;n:type:ShaderForge.SFN_RemapRange,id:5921,x:31055,y:32403,varname:node_5921,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:3|IN-4758-OUT;n:type:ShaderForge.SFN_Multiply,id:9981,x:31674,y:32402,varname:node_9981,prsc:2|A-742-OUT,B-6074-RGB,C-6873-OUT;n:type:ShaderForge.SFN_Multiply,id:2326,x:30461,y:31911,varname:node_2326,prsc:2|A-9798-UVOUT,B-9191-R;n:type:ShaderForge.SFN_Slider,id:8552,x:31058,y:32137,ptovrint:False,ptlb:Mask Stronger,ptin:_MaskStronger,varname:_MaskStronger,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1324359,max:5;n:type:ShaderForge.SFN_SwitchProperty,id:2712,x:30637,y:31820,ptovrint:False,ptlb:Mask UV Switch,ptin:_MaskUVSwitch,varname:_MaskUVSwitch,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True|A-9798-UVOUT,B-2326-OUT;n:type:ShaderForge.SFN_TexCoord,id:8366,x:29694,y:31785,varname:node_8366,prsc:2,uv:0;n:type:ShaderForge.SFN_Time,id:1696,x:29337,y:31908,varname:node_1696,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:8306,x:29337,y:32096,ptovrint:False,ptlb:Texture U,ptin:_TextureU,varname:_TextureU,prsc:1,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Multiply,id:4384,x:29716,y:31928,varname:node_4384,prsc:2|A-1696-T,B-6281-OUT;n:type:ShaderForge.SFN_Append,id:6281,x:29551,y:32064,varname:node_6281,prsc:2|A-8306-OUT,B-1136-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1136,x:29337,y:32190,ptovrint:False,ptlb:Texture V,ptin:_TextureV,varname:_TextureV,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Add,id:5639,x:29913,y:31959,varname:node_5639,prsc:2|A-8366-UVOUT,B-4384-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6873,x:31478,y:32467,ptovrint:False,ptlb:OutLine Expose,ptin:_OutLineExpose,varname:_OutLineExpose,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_TexCoord,id:2082,x:30608,y:32749,varname:node_2082,prsc:2,uv:0;n:type:ShaderForge.SFN_Time,id:1554,x:30239,y:32788,varname:node_1554,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:325,x:30239,y:32976,ptovrint:False,ptlb:Main Texture U,ptin:_MainTextureU,varname:_MainTextureU,prsc:1,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.05;n:type:ShaderForge.SFN_Multiply,id:2681,x:30608,y:32901,varname:node_2681,prsc:2|A-1554-T,B-3433-OUT;n:type:ShaderForge.SFN_Append,id:3433,x:30457,y:32990,varname:node_3433,prsc:2|A-325-OUT,B-4519-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4519,x:30239,y:33070,ptovrint:False,ptlb:Main Texture V,ptin:_MainTextureV,varname:_MainTextureV,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Add,id:5295,x:30815,y:32839,varname:node_5295,prsc:2|A-2082-UVOUT,B-2681-OUT;n:type:ShaderForge.SFN_RemapRange,id:2847,x:30764,y:32564,varname:node_2847,prsc:2,frmn:0,frmx:1,tomn:1,tomx:0|IN-4894-OUT;n:type:ShaderForge.SFN_Multiply,id:4515,x:32224,y:33084,varname:node_4515,prsc:2|A-6074-R,B-4982-OUT,C-8029-OUT;n:type:ShaderForge.SFN_ToggleProperty,id:8029,x:32027,y:33205,ptovrint:False,ptlb:Vertex Offset,ptin:_VertexOffset,varname:_VertexOffset,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False;n:type:ShaderForge.SFN_Slider,id:4982,x:31870,y:33104,ptovrint:False,ptlb:Vertex Offset Stronger,ptin:_VertexOffsetStronger,varname:_VertexOffsetStronger,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;proporder:6074-797-325-4519-8330-9118-8306-1136-6873-4894-8552-2712-8029-4982;pass:END;sub:END;*/

Shader "Sawyer Shader/Ice&Water 1.35" {
    Properties {
        _MainTexture ("Main Texture", 2D) = "white" {}
        _TintColor ("Main Texture Color", Color) = (0.5,0.5,0.5,1)
        _MainTextureU ("Main Texture U", Float ) = 0.05
        _MainTextureV ("Main Texture V", Float ) = 0
        _MainTextureAlpha ("Main Texture Alpha", Range(0, 2)) = 0.8694004
        _Texture ("Texture", 2D) = "white" {}
        _TextureU ("Texture U", Float ) = 0.1
        _TextureV ("Texture V", Float ) = 0
        _OutLineExpose ("OutLine Expose", Float ) = 1
        _OutLineFresnel ("OutLine Fresnel ", Range(0, 1)) = 0.5409656
        _MaskStronger ("Mask Stronger", Range(0, 5)) = 0.1324359
        [MaterialToggle] _MaskUVSwitch ("Mask UV Switch", Float ) = 0
        [MaterialToggle] _VertexOffset ("Vertex Offset", Float ) = 0
        _VertexOffsetStronger ("Vertex Offset Stronger", Range(0, 1)) = 1
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
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform sampler2D _MainTexture; uniform float4 _MainTexture_ST;
            uniform float4 _TintColor;
            uniform float _OutLineFresnel;
            uniform float _MainTextureAlpha;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float _MaskStronger;
            uniform fixed _MaskUVSwitch;
            uniform half _TextureU;
            uniform float _TextureV;
            uniform float _OutLineExpose;
            uniform half _MainTextureU;
            uniform float _MainTextureV;
            uniform fixed _VertexOffset;
            uniform float _VertexOffsetStronger;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(3)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_1554 = _Time + _TimeEditor;
                float2 node_5295 = (o.uv0+(node_1554.g*float2(_MainTextureU,_MainTextureV)));
                float4 _MainTexture_var = tex2Dlod(_MainTexture,float4(TRANSFORM_TEX(node_5295, _MainTexture),0.0,0));
                float node_4515 = (_MainTexture_var.r*_VertexOffsetStronger*_VertexOffset);
                v.vertex.xyz += float3(node_4515,node_4515,node_4515);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float4 node_1554 = _Time + _TimeEditor;
                float2 node_5295 = (i.uv0+(node_1554.g*float2(_MainTextureU,_MainTextureV)));
                float4 _MainTexture_var = tex2D(_MainTexture,TRANSFORM_TEX(node_5295, _MainTexture));
                float4 node_1696 = _Time + _TimeEditor;
                float2 node_5639 = (i.uv0+(node_1696.g*float2(_TextureU,_TextureV)));
                float4 node_9191 = tex2D(_Texture,TRANSFORM_TEX(node_5639, _Texture));
                float2 _MaskUVSwitch_var = lerp( i.uv0, (i.uv0*node_9191.r), _MaskUVSwitch );
                float4 node_9010 = tex2D(_Texture,TRANSFORM_TEX(_MaskUVSwitch_var, _Texture));
                float3 emissive = (((pow(1.0-max(0,dot(normalDirection, viewDirection)),((max(0,dot(viewDirection,i.normalDir))*(_OutLineFresnel*-1.0+1.0))*4.0+-1.0))*_MainTexture_var.rgb*_OutLineExpose)+(pow(node_9010.g,_MaskStronger)*node_9191.g))*(_MainTexture_var.rgb*i.vertexColor.rgb*_TintColor.rgb*2.0*_MainTextureAlpha));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0.5,0.5,0.5,1));
                return finalRGBA;
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
            #pragma multi_compile_fog
            #pragma exclude_renderers d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 2.0
            #pragma glsl
            uniform float4 _TimeEditor;
            uniform sampler2D _MainTexture; uniform float4 _MainTexture_ST;
            uniform half _MainTextureU;
            uniform float _MainTextureV;
            uniform fixed _VertexOffset;
            uniform float _VertexOffsetStronger;
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
                float4 node_1554 = _Time + _TimeEditor;
                float2 node_5295 = (o.uv0+(node_1554.g*float2(_MainTextureU,_MainTextureV)));
                float4 _MainTexture_var = tex2Dlod(_MainTexture,float4(TRANSFORM_TEX(node_5295, _MainTexture),0.0,0));
                float node_4515 = (_MainTexture_var.r*_VertexOffsetStronger*_VertexOffset);
                v.vertex.xyz += float3(node_4515,node_4515,node_4515);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Sawyer Shader/Simple Double UV 1.0"
    //CustomEditor "ShaderForgeMaterialInspector"
}
