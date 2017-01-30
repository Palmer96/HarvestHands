// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:0,lgpr:1,limd:1,spmd:1,trmd:1,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:True,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.1280277,fgcg:0.1953466,fgcb:0.2352941,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:1021,x:32719,y:32712,varname:node_1021,prsc:2|diff-5002-OUT,spec-249-OUT,alpha-4920-OUT;n:type:ShaderForge.SFN_Append,id:8822,x:31909,y:32398,varname:node_8822,prsc:2|A-5073-OUT,B-5073-OUT;n:type:ShaderForge.SFN_Vector1,id:249,x:32558,y:32738,varname:node_249,prsc:2,v1:0;n:type:ShaderForge.SFN_DepthBlend,id:5073,x:31450,y:32380,varname:node_5073,prsc:2|DIST-9724-OUT;n:type:ShaderForge.SFN_Slider,id:9724,x:31104,y:32373,ptovrint:False,ptlb:Gradient Depth,ptin:_GradientDepth,varname:node_9724,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:10;n:type:ShaderForge.SFN_DepthBlend,id:3364,x:32155,y:32601,varname:node_3364,prsc:2|DIST-5597-OUT;n:type:ShaderForge.SFN_Slider,id:5597,x:31764,y:32595,ptovrint:False,ptlb:Stickiness,ptin:_Stickiness,varname:node_5597,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:3155,x:32367,y:32601,varname:node_3155,prsc:2|A-1198-RGB,B-3364-OUT;n:type:ShaderForge.SFN_Add,id:5002,x:32472,y:32372,varname:node_5002,prsc:2|A-5966-RGB,B-3155-OUT;n:type:ShaderForge.SFN_Color,id:5966,x:32273,y:32191,ptovrint:False,ptlb:Water Colour,ptin:_WaterColour,varname:node_5966,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:1,c3:0.9172413,c4:1;n:type:ShaderForge.SFN_Slider,id:4920,x:32038,y:32919,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:node_4920,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.2,max:1;n:type:ShaderForge.SFN_Panner,id:5460,x:32014,y:32159,varname:node_5460,prsc:2,spu:0,spv:-1|UVIN-8822-OUT,DIST-7909-OUT;n:type:ShaderForge.SFN_Tex2d,id:1198,x:32225,y:32408,ptovrint:False,ptlb:Animated Ramp,ptin:_AnimatedRamp,varname:node_1198,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-5460-UVOUT;n:type:ShaderForge.SFN_If,id:9455,x:31651,y:32222,varname:node_9455,prsc:2|A-5073-OUT,B-7-OUT,GT-1517-OUT,EQ-1517-OUT,LT-5527-OUT;n:type:ShaderForge.SFN_Slider,id:7,x:31381,y:32065,ptovrint:False,ptlb:Depth Limit,ptin:_DepthLimit,varname:node_7,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_ValueProperty,id:1517,x:31341,y:32177,ptovrint:False,ptlb:Min Pan Speed,ptin:_MinPanSpeed,varname:node_1517,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:5527,x:31341,y:32263,ptovrint:False,ptlb:Max Pan Speed,ptin:_MaxPanSpeed,varname:node_5527,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Time,id:7727,x:31702,y:32038,varname:node_7727,prsc:2;n:type:ShaderForge.SFN_Multiply,id:7909,x:31808,y:32190,varname:node_7909,prsc:2|A-7727-TSL,B-9455-OUT;proporder:5966-4920-9724-5597-1198-7-1517-5527;pass:END;sub:END;*/

Shader "waterShader" {
    Properties {
        _WaterColour ("Water Colour", Color) = (0,1,0.9172413,1)
        _Opacity ("Opacity", Range(0, 1)) = 0.2
        _GradientDepth ("Gradient Depth", Range(0, 10)) = 0
        _Stickiness ("Stickiness", Range(0, 1)) = 0
        _AnimatedRamp ("Animated Ramp", 2D) = "white" {}
        _DepthLimit ("Depth Limit", Range(0, 1)) = 0.5
        _MinPanSpeed ("Min Pan Speed", Float ) = 0
        _MaxPanSpeed ("Max Pan Speed", Float ) = 1
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
            Blend One OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _TimeEditor;
            uniform float _GradientDepth;
            uniform float _Stickiness;
            uniform float4 _WaterColour;
            uniform float _Opacity;
            uniform sampler2D _AnimatedRamp; uniform float4 _AnimatedRamp_ST;
            uniform float _DepthLimit;
            uniform float _MinPanSpeed;
            uniform float _MaxPanSpeed;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float4 projPos : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = 0.5;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float node_249 = 0.0;
                float3 specularColor = float3(node_249,node_249,node_249);
                float specularMonochrome = max( max(specularColor.r, specularColor.g), specularColor.b);
                float normTerm = (specPow + 8.0 ) / (8.0 * Pi);
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow)*normTerm*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 node_7727 = _Time + _TimeEditor;
                float node_5073 = saturate((sceneZ-partZ)/_GradientDepth);
                float node_9455_if_leA = step(node_5073,_DepthLimit);
                float node_9455_if_leB = step(_DepthLimit,node_5073);
                float2 node_5460 = (float2(node_5073,node_5073)+(node_7727.r*lerp((node_9455_if_leA*_MaxPanSpeed)+(node_9455_if_leB*_MinPanSpeed),_MinPanSpeed,node_9455_if_leA*node_9455_if_leB))*float2(0,-1));
                float4 _AnimatedRamp_var = tex2D(_AnimatedRamp,TRANSFORM_TEX(node_5460, _AnimatedRamp));
                float3 diffuseColor = (_WaterColour.rgb+(_AnimatedRamp_var.rgb*saturate((sceneZ-partZ)/_Stickiness)));
                diffuseColor *= 1-specularMonochrome;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse * _Opacity + specular;
                return fixed4(finalColor,_Opacity);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
