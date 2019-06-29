// Shader created with Shader Forge v1.27 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.27;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:0,lgpr:1,limd:0,spmd:0,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:True,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:9361,x:34719,y:32860,varname:node_9361,prsc:2|normal-5412-RGB,emission-1922-OUT,custl-8462-OUT;n:type:ShaderForge.SFN_NormalVector,id:5235,x:31327,y:33596,prsc:2,pt:False;n:type:ShaderForge.SFN_Transform,id:3679,x:31515,y:33596,varname:node_3679,prsc:2,tffrom:0,tfto:3|IN-5235-OUT;n:type:ShaderForge.SFN_ComponentMask,id:2940,x:31698,y:33596,varname:node_2940,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-3679-XYZ;n:type:ShaderForge.SFN_RemapRange,id:3395,x:31882,y:33596,varname:node_3395,prsc:1,frmn:-1,frmx:1,tomn:0,tomx:1|IN-2940-OUT;n:type:ShaderForge.SFN_Tex2d,id:370,x:32054,y:33597,ptovrint:False,ptlb:Matcap,ptin:_Matcap,varname:_Matcap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:4bf13b20228d0324cb884aa4d0f614f9,ntxv:2,isnm:False|UVIN-3395-OUT;n:type:ShaderForge.SFN_Tex2d,id:5412,x:32999,y:32859,ptovrint:False,ptlb:NormalMap,ptin:_NormalMap,varname:_NormalMap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:81fe724606114b2408c9785d973b1d8c,ntxv:3,isnm:False|UVIN-871-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:3034,x:32374,y:32938,ptovrint:False,ptlb:MainTexture,ptin:_MainTexture,varname:_MainTexture,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:cf4dcf7a7bac741499e9fff435a9b591,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:116,x:32585,y:33029,varname:node_116,prsc:2|A-3034-RGB,B-7151-OUT;n:type:ShaderForge.SFN_Slider,id:7151,x:32228,y:33157,ptovrint:False,ptlb:MetalEmission,ptin:_MetalEmission,varname:_MetalEmission,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Tex2d,id:4147,x:29878,y:33153,ptovrint:False,ptlb:MaskTexture,ptin:_MaskTexture,varname:_MaskTexture,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:57a75d4c30a26bd45b5931c5c189b4a4,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:1939,x:32975,y:33407,varname:node_1939,prsc:2|A-116-OUT,B-6422-OUT,C-2396-OUT;n:type:ShaderForge.SFN_Slider,id:5752,x:32228,y:33341,ptovrint:False,ptlb:MatcapStonge,ptin:_MatcapStonge,varname:_MatcapStonge,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:5,max:7;n:type:ShaderForge.SFN_Multiply,id:6422,x:32561,y:33577,varname:node_6422,prsc:2|A-5752-OUT,B-9211-OUT,C-5312-RGB;n:type:ShaderForge.SFN_Multiply,id:1922,x:32764,y:32876,varname:node_1922,prsc:2|A-9566-OUT,B-3034-RGB;n:type:ShaderForge.SFN_Slider,id:9566,x:32296,y:32798,ptovrint:False,ptlb:MainTexLightness,ptin:_MainTexLightness,varname:_MainTexLightness,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:2;n:type:ShaderForge.SFN_Vector4Property,id:9828,x:31697,y:34377,ptovrint:False,ptlb:FakeLightAngle,ptin:_FakeLightAngle,varname:_FakeLightAngle,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1,v2:-0.5,v3:0.5,v4:0;n:type:ShaderForge.SFN_Dot,id:1077,x:31938,y:34260,varname:node_1077,prsc:2,dt:1|A-6513-OUT,B-9828-XYZ;n:type:ShaderForge.SFN_NormalVector,id:6513,x:31697,y:34189,prsc:2,pt:False;n:type:ShaderForge.SFN_ViewVector,id:7179,x:31859,y:33834,varname:node_7179,prsc:2;n:type:ShaderForge.SFN_OneMinus,id:1255,x:32136,y:34260,varname:node_1255,prsc:2|IN-1077-OUT;n:type:ShaderForge.SFN_TexCoord,id:871,x:32745,y:32715,varname:node_871,prsc:2,uv:0;n:type:ShaderForge.SFN_ComponentMask,id:173,x:30350,y:32792,varname:node_173,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-4147-RGB;n:type:ShaderForge.SFN_ComponentMask,id:5520,x:30355,y:33112,varname:node_5520,prsc:2,cc1:1,cc2:-1,cc3:-1,cc4:-1|IN-4147-RGB;n:type:ShaderForge.SFN_ComponentMask,id:3513,x:30359,y:33417,varname:node_3513,prsc:2,cc1:2,cc2:-1,cc3:-1,cc4:-1|IN-4147-RGB;n:type:ShaderForge.SFN_Slider,id:5889,x:30202,y:32952,ptovrint:False,ptlb:Mask_R_Ctrl,ptin:_Mask_R_Ctrl,varname:_Mask_R_Ctrl,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Multiply,id:6160,x:30572,y:32792,varname:node_6160,prsc:2|A-5889-OUT,B-173-OUT;n:type:ShaderForge.SFN_Slider,id:1779,x:30202,y:33271,ptovrint:False,ptlb:Mask_G_Ctrl,ptin:_Mask_G_Ctrl,varname:_Mask_G_Ctrl,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Slider,id:6476,x:30202,y:33590,ptovrint:False,ptlb:Mask_B_Ctrl,ptin:_Mask_B_Ctrl,varname:_Mask_B_Ctrl,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Multiply,id:409,x:30572,y:33112,varname:node_409,prsc:2|A-5520-OUT,B-1779-OUT;n:type:ShaderForge.SFN_Multiply,id:6926,x:30572,y:33427,varname:node_6926,prsc:2|A-3513-OUT,B-6476-OUT;n:type:ShaderForge.SFN_ToggleProperty,id:483,x:30572,y:32974,ptovrint:False,ptlb:R,ptin:_R,varname:_R,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True;n:type:ShaderForge.SFN_Multiply,id:2115,x:30762,y:32955,varname:node_2115,prsc:2|A-6160-OUT,B-483-OUT;n:type:ShaderForge.SFN_Multiply,id:3140,x:30762,y:33196,varname:node_3140,prsc:0|A-409-OUT,B-6573-OUT;n:type:ShaderForge.SFN_ToggleProperty,id:6573,x:30572,y:33270,ptovrint:False,ptlb:G,ptin:_G,varname:_G,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True;n:type:ShaderForge.SFN_ToggleProperty,id:3090,x:30572,y:33585,ptovrint:False,ptlb:B,ptin:_B,varname:_B,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True;n:type:ShaderForge.SFN_Multiply,id:6581,x:30762,y:33427,varname:node_6581,prsc:0|A-6926-OUT,B-3090-OUT;n:type:ShaderForge.SFN_Add,id:6222,x:31009,y:33196,varname:node_6222,prsc:2|A-2115-OUT,B-3140-OUT,C-6581-OUT;n:type:ShaderForge.SFN_Multiply,id:2396,x:31254,y:33263,varname:node_2396,prsc:2|A-6222-OUT,B-8959-OUT;n:type:ShaderForge.SFN_Vector1,id:8959,x:31009,y:33363,varname:node_8959,prsc:2,v1:1;n:type:ShaderForge.SFN_Fresnel,id:5444,x:32320,y:33878,varname:node_5444,prsc:2|EXP-8621-OUT;n:type:ShaderForge.SFN_Dot,id:8621,x:32072,y:33878,varname:node_8621,prsc:2,dt:1|A-7179-OUT,B-2265-OUT;n:type:ShaderForge.SFN_NormalVector,id:2265,x:31859,y:33972,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:757,x:32611,y:33890,varname:node_757,prsc:2|A-5444-OUT,B-9400-OUT;n:type:ShaderForge.SFN_Slider,id:9400,x:32139,y:34048,ptovrint:False,ptlb:Fake_Light_Expose,ptin:_Fake_Light_Expose,varname:_Fake_Light_Expose,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:5;n:type:ShaderForge.SFN_Max,id:2592,x:33164,y:33407,varname:node_2592,prsc:2|A-1939-OUT,B-9645-OUT;n:type:ShaderForge.SFN_Color,id:8025,x:32620,y:34218,ptovrint:False,ptlb:FakeLightColor,ptin:_FakeLightColor,varname:_FakeLightColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.145888,c3:0.01599997,c4:1;n:type:ShaderForge.SFN_Multiply,id:9645,x:32688,y:33406,varname:node_9645,prsc:2|A-8189-OUT,B-8025-RGB;n:type:ShaderForge.SFN_Multiply,id:8189,x:32620,y:34053,varname:node_8189,prsc:2|A-1255-OUT,B-1255-OUT,C-1255-OUT,D-1255-OUT,E-9400-OUT;n:type:ShaderForge.SFN_TexCoord,id:2301,x:30185,y:33852,varname:node_2301,prsc:2,uv:0;n:type:ShaderForge.SFN_Time,id:3159,x:29950,y:34016,varname:node_3159,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:7722,x:29950,y:34375,ptovrint:False,ptlb:MotionTextureV,ptin:_MotionTextureV,varname:_MotionTextureV,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Append,id:265,x:30185,y:34202,varname:node_265,prsc:2|A-2768-OUT,B-7722-OUT;n:type:ShaderForge.SFN_Multiply,id:1042,x:30293,y:34051,varname:node_1042,prsc:2|A-3159-T,B-265-OUT;n:type:ShaderForge.SFN_Add,id:2864,x:30438,y:33900,varname:node_2864,prsc:2|A-2301-UVOUT,B-1042-OUT;n:type:ShaderForge.SFN_Tex2d,id:6504,x:30618,y:33900,ptovrint:False,ptlb:UVMotionTexture,ptin:_UVMotionTexture,varname:_UVMotionTexture,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:6de44f2400e43574f859eda127990174,ntxv:0,isnm:False|UVIN-2864-OUT;n:type:ShaderForge.SFN_Multiply,id:8530,x:33280,y:33050,varname:node_8530,prsc:2|A-116-OUT,B-6085-OUT,C-8177-OUT,D-83-OUT;n:type:ShaderForge.SFN_Slider,id:83,x:32897,y:33183,ptovrint:False,ptlb:MotionTextureAlpha,ptin:_MotionTextureAlpha,varname:_MotionTextureAlpha,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1451447,max:3;n:type:ShaderForge.SFN_Add,id:8462,x:33575,y:33091,varname:node_8462,prsc:2|A-8530-OUT,B-2592-OUT,C-1783-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:8177,x:31254,y:33085,ptovrint:False,ptlb:G/B_UVMotionTextureSwitch,ptin:_GB_UVMotionTextureSwitch,varname:_GB_UVMotionTextureSwitch,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True|A-3140-OUT,B-6581-OUT;n:type:ShaderForge.SFN_Desaturate,id:9211,x:32306,y:33505,varname:node_9211,prsc:2|COL-370-RGB;n:type:ShaderForge.SFN_Color,id:5312,x:32306,y:33673,ptovrint:False,ptlb:MacapColor,ptin:_MacapColor,varname:_MacapColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:6085,x:30897,y:34022,varname:node_6085,prsc:2|A-6504-RGB,B-8110-OUT;n:type:ShaderForge.SFN_Color,id:1705,x:31678,y:32420,ptovrint:False,ptlb:LgihtSweepColor,ptin:_LgihtSweepColor,varname:_LgihtSweepColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.7743199,c3:0.272,c4:1;n:type:ShaderForge.SFN_TexCoord,id:2799,x:30419,y:32177,varname:node_2799,prsc:2,uv:0;n:type:ShaderForge.SFN_Sin,id:7696,x:31351,y:32213,varname:node_7696,prsc:2|IN-15-OUT;n:type:ShaderForge.SFN_RemapRange,id:7613,x:30862,y:32177,varname:node_7613,prsc:2,frmn:0,frmx:1,tomn:0,tomx:3.14|IN-4462-OUT;n:type:ShaderForge.SFN_Power,id:5701,x:31678,y:32222,varname:node_5701,prsc:2|VAL-7696-OUT,EXP-5036-OUT;n:type:ShaderForge.SFN_Exp,id:5036,x:31351,y:32409,varname:node_5036,prsc:2,et:0|IN-7507-OUT;n:type:ShaderForge.SFN_Slider,id:9807,x:30784,y:32608,ptovrint:False,ptlb:LightSweepRange,ptin:_LightSweepRange,varname:_LightSweepRange,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:7.93841,max:10;n:type:ShaderForge.SFN_RemapRange,id:7507,x:31148,y:32409,varname:node_7507,prsc:2,frmn:0,frmx:10,tomn:10,tomx:1|IN-9807-OUT;n:type:ShaderForge.SFN_Multiply,id:8053,x:32045,y:32381,varname:node_8053,prsc:2|A-5701-OUT,B-1705-RGB;n:type:ShaderForge.SFN_Add,id:15,x:31136,y:32213,varname:node_15,prsc:2|A-7613-OUT,B-7986-OUT;n:type:ShaderForge.SFN_ConstantClamp,id:4482,x:32236,y:32381,varname:node_4482,prsc:2,min:0.01,max:1|IN-8053-OUT;n:type:ShaderForge.SFN_Lerp,id:4462,x:30624,y:32177,varname:node_4462,prsc:2|A-2799-U,B-2799-V,T-1004-OUT;n:type:ShaderForge.SFN_Slider,id:2945,x:30080,y:32346,ptovrint:False,ptlb:LightSweepAngle,ptin:_LightSweepAngle,varname:_LightSweepAngle,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:90,max:90;n:type:ShaderForge.SFN_RemapRange,id:1004,x:30419,y:32347,varname:node_1004,prsc:2,frmn:0,frmx:90,tomn:0,tomx:1|IN-2945-OUT;n:type:ShaderForge.SFN_Time,id:7090,x:30616,y:32334,varname:node_7090,prsc:2;n:type:ShaderForge.SFN_Multiply,id:7986,x:30862,y:32334,varname:node_7986,prsc:2|A-7090-T,B-7223-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7223,x:30616,y:32537,ptovrint:False,ptlb:LightSweepOffsetValue,ptin:_LightSweepOffsetValue,varname:_LightSweepOffsetValue,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_ToggleProperty,id:9543,x:32236,y:32586,ptovrint:False,ptlb:LightSweepToggle,ptin:_LightSweepToggle,varname:_LightSweepToggle,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True;n:type:ShaderForge.SFN_Multiply,id:1352,x:32471,y:32474,varname:node_1352,prsc:2|A-4482-OUT,B-9543-OUT,C-6581-OUT;n:type:ShaderForge.SFN_Multiply,id:1783,x:33262,y:32674,varname:node_1783,prsc:2|A-1352-OUT,B-116-OUT,C-353-OUT;n:type:ShaderForge.SFN_ValueProperty,id:353,x:32713,y:32632,ptovrint:False,ptlb:LightSweepExpose,ptin:_LightSweepExpose,varname:_LightSweepExpose,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_ValueProperty,id:8110,x:30618,y:34161,ptovrint:False,ptlb:MotionTextureExpose,ptin:_MotionTextureExpose,varname:_MotionTextureExpose,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:2768,x:29903,y:34251,ptovrint:False,ptlb:MotionTextureU,ptin:_MotionTextureU,varname:_MotionTextureU,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;proporder:3034-9566-5412-370-5312-5752-7151-4147-8025-9828-9400-483-6573-3090-5889-1779-6476-6504-8177-8110-2768-7722-83-1705-353-9807-2945-7223-9543;pass:END;sub:END;*/

Shader "Sawyer Shader/NewCharacterShow 1.3" {
    Properties {
        _MainTexture ("MainTexture", 2D) = "white" {}
        _MainTexLightness ("MainTexLightness", Range(0, 2)) = 0
        _NormalMap ("NormalMap", 2D) = "bump" {}
        _Matcap ("Matcap", 2D) = "black" {}
        _MacapColor ("MacapColor", Color) = (1,1,1,1)
        _MatcapStonge ("MatcapStonge", Range(0, 7)) = 5
        _MetalEmission ("MetalEmission", Range(0, 1)) = 1
        _MaskTexture ("MaskTexture", 2D) = "white" {}
        _FakeLightColor ("FakeLightColor", Color) = (1,0.145888,0.01599997,1)
        _FakeLightAngle ("FakeLightAngle", Vector) = (1,-0.5,0.5,0)
        _Fake_Light_Expose ("Fake_Light_Expose", Range(0, 5)) = 0
        [MaterialToggle] _R ("R", Float ) = 1
        [MaterialToggle] _G ("G", Float ) = 1
        [MaterialToggle] _B ("B", Float ) = 1
        _Mask_R_Ctrl ("Mask_R_Ctrl", Range(0, 1)) = 1
        _Mask_G_Ctrl ("Mask_G_Ctrl", Range(0, 1)) = 1
        _Mask_B_Ctrl ("Mask_B_Ctrl", Range(0, 1)) = 1
        _UVMotionTexture ("UVMotionTexture", 2D) = "white" {}
        [MaterialToggle] _GB_UVMotionTextureSwitch ("G/B_UVMotionTextureSwitch", Float ) = 0
        _MotionTextureExpose ("MotionTextureExpose", Float ) = 1
        _MotionTextureU ("MotionTextureU", Float ) = 0
        _MotionTextureV ("MotionTextureV", Float ) = 0
        _MotionTextureAlpha ("MotionTextureAlpha", Range(0, 3)) = 0.1451447
        _LgihtSweepColor ("LgihtSweepColor", Color) = (1,0.7743199,0.272,1)
        _LightSweepExpose ("LightSweepExpose", Float ) = 2
        _LightSweepRange ("LightSweepRange", Range(0, 10)) = 7.93841
        _LightSweepAngle ("LightSweepAngle", Range(0, 90)) = 90
        _LightSweepOffsetValue ("LightSweepOffsetValue", Float ) = 2
        [MaterialToggle] _LightSweepToggle ("LightSweepToggle", Float ) = 1
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
            #pragma multi_compile_fog
            #pragma exclude_renderers d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 2.0
            uniform float4 _TimeEditor;
            uniform sampler2D _Matcap; uniform float4 _Matcap_ST;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform sampler2D _MainTexture; uniform float4 _MainTexture_ST;
            uniform fixed _MetalEmission;
            uniform sampler2D _MaskTexture; uniform float4 _MaskTexture_ST;
            uniform fixed _MatcapStonge;
            uniform fixed _MainTexLightness;
            uniform float4 _FakeLightAngle;
            uniform fixed _Mask_R_Ctrl;
            uniform fixed _Mask_G_Ctrl;
            uniform fixed _Mask_B_Ctrl;
            uniform fixed _R;
            uniform fixed _G;
            uniform fixed _B;
            uniform fixed _Fake_Light_Expose;
            uniform float4 _FakeLightColor;
            uniform float _MotionTextureV;
            uniform sampler2D _UVMotionTexture; uniform float4 _UVMotionTexture_ST;
            uniform fixed _MotionTextureAlpha;
            uniform fixed _GB_UVMotionTextureSwitch;
            uniform float4 _MacapColor;
            uniform float4 _LgihtSweepColor;
            uniform fixed _LightSweepRange;
            uniform fixed _LightSweepAngle;
            uniform fixed _LightSweepOffsetValue;
            uniform fixed _LightSweepToggle;
            uniform fixed _LightSweepExpose;
            uniform float _MotionTextureExpose;
            uniform float _MotionTextureU;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float3 tangentDir : TEXCOORD2;
                float3 bitangentDir : TEXCOORD3;
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float4 _NormalMap_var = tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap));
                float3 normalLocal = _NormalMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
////// Lighting:
////// Emissive:
                float4 _MainTexture_var = tex2D(_MainTexture,TRANSFORM_TEX(i.uv0, _MainTexture));
                float3 emissive = (_MainTexLightness*_MainTexture_var.rgb);
                float3 node_116 = (_MainTexture_var.rgb*_MetalEmission);
                float4 node_3159 = _Time + _TimeEditor;
                float2 node_2864 = (i.uv0+(node_3159.g*float2(_MotionTextureU,_MotionTextureV)));
                float4 _UVMotionTexture_var = tex2D(_UVMotionTexture,TRANSFORM_TEX(node_2864, _UVMotionTexture));
                float4 _MaskTexture_var = tex2D(_MaskTexture,TRANSFORM_TEX(i.uv0, _MaskTexture));
                fixed node_3140 = ((_MaskTexture_var.rgb.g*_Mask_G_Ctrl)*_G);
                fixed node_6581 = ((_MaskTexture_var.rgb.b*_Mask_B_Ctrl)*_B);
                half2 node_3395 = (mul( UNITY_MATRIX_V, float4(i.normalDir,0) ).xyz.rgb.rg*0.5+0.5);
                float4 _Matcap_var = tex2D(_Matcap,TRANSFORM_TEX(node_3395, _Matcap));
                float node_1255 = (1.0 - max(0,dot(i.normalDir,_FakeLightAngle.rgb)));
                float4 node_7090 = _Time + _TimeEditor;
                float3 finalColor = emissive + ((node_116*(_UVMotionTexture_var.rgb*_MotionTextureExpose)*lerp( node_3140, node_6581, _GB_UVMotionTextureSwitch )*_MotionTextureAlpha)+max((node_116*(_MatcapStonge*dot(_Matcap_var.rgb,float3(0.3,0.59,0.11))*_MacapColor.rgb)*((((_Mask_R_Ctrl*_MaskTexture_var.rgb.r)*_R)+node_3140+node_6581)*1.0)),((node_1255*node_1255*node_1255*node_1255*_Fake_Light_Expose)*_FakeLightColor.rgb))+((clamp((pow(sin(((lerp(i.uv0.r,i.uv0.g,(_LightSweepAngle*0.01111111+0.0))*3.14+0.0)+(node_7090.g*_LightSweepOffsetValue))),exp((_LightSweepRange*-0.9+10.0)))*_LgihtSweepColor.rgb),0.01,1)*_LightSweepToggle*node_6581)*node_116*_LightSweepExpose));
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    //CustomEditor "ShaderForgeMaterialInspector"
}
