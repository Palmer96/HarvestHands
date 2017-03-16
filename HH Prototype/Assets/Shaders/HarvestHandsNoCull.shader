// Shader created with Shader Forge v1.34 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.34;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:1,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:False,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:14,ufog:True,aust:False,igpj:False,qofs:600,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5958045,fgcg:0.6475212,fgcb:0.6985294,fgca:1,fgde:0.006,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:32719,y:32712,varname:node_4013,prsc:2|diff-8339-OUT,diffpow-6742-OUT;n:type:ShaderForge.SFN_VertexColor,id:6905,x:31675,y:32569,varname:node_6905,prsc:2;n:type:ShaderForge.SFN_HsvToRgb,id:8339,x:32517,y:33030,varname:node_8339,prsc:2|H-7919-OUT,S-591-OUT,V-310-OUT;n:type:ShaderForge.SFN_RgbToHsv,id:1229,x:32093,y:32842,varname:node_1229,prsc:2|IN-1758-OUT;n:type:ShaderForge.SFN_Add,id:7919,x:32304,y:32682,varname:node_7919,prsc:2|A-7289-OUT,B-1229-HOUT;n:type:ShaderForge.SFN_Slider,id:1452,x:31936,y:32600,ptovrint:False,ptlb:Hue,ptin:_Hue,varname:node_1452,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:3744,x:31936,y:32678,ptovrint:False,ptlb:Sat,ptin:_Sat,varname:node_3744,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0,max:10;n:type:ShaderForge.SFN_Add,id:591,x:32304,y:32794,varname:node_591,prsc:2|A-3744-OUT,B-1229-SOUT;n:type:ShaderForge.SFN_Slider,id:4386,x:31936,y:32760,ptovrint:False,ptlb:Val,ptin:_Val,varname:node_4386,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0,max:1;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:3051,x:32093,y:32955,varname:node_3051,prsc:2|IN-1229-VOUT,IMIN-4425-OUT,IMAX-3457-OUT,OMIN-2623-OUT,OMAX-7258-OUT;n:type:ShaderForge.SFN_Vector1,id:3457,x:31829,y:32991,varname:node_3457,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:4425,x:31829,y:32945,varname:node_4425,prsc:2,v1:0;n:type:ShaderForge.SFN_SwitchProperty,id:1758,x:31672,y:32911,ptovrint:False,ptlb:Vertex Tex Switch,ptin:_VertexTexSwitch,cmnt:Chooses between Textures and Vertex Colours,varname:node_1758,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-6905-RGB,B-8672-RGB;n:type:ShaderForge.SFN_Tex2d,id:8672,x:31675,y:32707,ptovrint:False,ptlb:Diffuse Map,ptin:_DiffuseMap,varname:node_8672,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:7258,x:31672,y:33131,ptovrint:False,ptlb:Contrast Lights,ptin:_ContrastLights,varname:node_7258,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:1,max:1;n:type:ShaderForge.SFN_Slider,id:2623,x:31672,y:33054,ptovrint:False,ptlb:Contrast Darks,ptin:_ContrastDarks,varname:node_2623,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:0,max:1;n:type:ShaderForge.SFN_Add,id:761,x:32304,y:32914,varname:node_761,prsc:2|A-4386-OUT,B-3051-OUT;n:type:ShaderForge.SFN_Vector1,id:869,x:32093,y:33072,varname:node_869,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:6772,x:32093,y:33118,varname:node_6772,prsc:2,v1:1;n:type:ShaderForge.SFN_Clamp,id:310,x:32304,y:33032,varname:node_310,prsc:2|IN-761-OUT,MIN-869-OUT,MAX-6772-OUT;n:type:ShaderForge.SFN_Slider,id:6742,x:32631,y:32648,ptovrint:False,ptlb:Diffuse Power,ptin:_DiffusePower,varname:node_6742,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:10;n:type:ShaderForge.SFN_SwitchProperty,id:7289,x:32304,y:32558,ptovrint:False,ptlb:Tree Colour Randomization,ptin:_TreeColourRandomization,varname:node_7289,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-1452-OUT,B-2728-OUT;n:type:ShaderForge.SFN_Sin,id:8745,x:32093,y:32332,varname:node_8745,prsc:2|IN-8018-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5134,x:32093,y:32254,ptovrint:False,ptlb:ColourRandomization,ptin:ColourRandomization,varname:node_5134,prsc:2,glob:False,taghide:True,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Relay,id:2728,x:32363,y:32445,cmnt:Colour Randomization,varname:node_2728,prsc:2|IN-8461-OUT;n:type:ShaderForge.SFN_RemapRange,id:8461,x:32293,y:32243,varname:node_8461,prsc:2,frmn:0.1,frmx:1,tomn:-0.125,tomx:0.35|IN-8745-OUT;n:type:ShaderForge.SFN_ConstantClamp,id:8018,x:31923,y:32332,varname:node_8018,prsc:2,min:0.1,max:1|IN-5134-OUT;proporder:8672-1758-6742-1452-3744-4386-7258-2623-7289-5134;pass:END;sub:END;*/

Shader "GoblinHammerGames/HarvestHandsNoCullSF" {
    Properties {
        _DiffuseMap ("Diffuse Map", 2D) = "white" {}
        [MaterialToggle] _VertexTexSwitch ("Vertex Tex Switch", Float ) = 0
        _DiffusePower ("Diffuse Power", Range(0, 10)) = 1
        _Hue ("Hue", Range(0, 1)) = 0
        _Sat ("Sat", Range(-1, 10)) = 0
        _Val ("Val", Range(-1, 1)) = 0
        _ContrastLights ("Contrast Lights", Range(-1, 1)) = 1
        _ContrastDarks ("Contrast Darks", Range(-1, 1)) = 0
        [MaterialToggle] _TreeColourRandomization ("Tree Colour Randomization", Float ) = 0
        [HideInInspector]ColourRandomization ("ColourRandomization", Float ) = 1
    }
    SubShader {
        Tags {
            "Queue"="Geometry+600"
            "RenderType"="Opaque"
            "DisableBatching"="True"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            ColorMask RGB
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 xboxone ps4 wiiu 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float _Hue;
            uniform float _Sat;
            uniform float _Val;
            uniform fixed _VertexTexSwitch;
            uniform sampler2D _DiffuseMap; uniform float4 _DiffuseMap_ST;
            uniform float _ContrastLights;
            uniform float _ContrastDarks;
            uniform float _DiffusePower;
            uniform fixed _TreeColourRandomization;
            uniform float ColourRandomization;
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
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = pow(max( 0.0, NdotL), _DiffusePower) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _DiffuseMap_var = tex2D(_DiffuseMap,TRANSFORM_TEX(i.uv0, _DiffuseMap));
                float3 _VertexTexSwitch_var = lerp( i.vertexColor.rgb, _DiffuseMap_var.rgb, _VertexTexSwitch ); // Chooses between Textures and Vertex Colours
                float4 node_1229_k = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 node_1229_p = lerp(float4(float4(_VertexTexSwitch_var,0.0).zy, node_1229_k.wz), float4(float4(_VertexTexSwitch_var,0.0).yz, node_1229_k.xy), step(float4(_VertexTexSwitch_var,0.0).z, float4(_VertexTexSwitch_var,0.0).y));
                float4 node_1229_q = lerp(float4(node_1229_p.xyw, float4(_VertexTexSwitch_var,0.0).x), float4(float4(_VertexTexSwitch_var,0.0).x, node_1229_p.yzx), step(node_1229_p.x, float4(_VertexTexSwitch_var,0.0).x));
                float node_1229_d = node_1229_q.x - min(node_1229_q.w, node_1229_q.y);
                float node_1229_e = 1.0e-10;
                float3 node_1229 = float3(abs(node_1229_q.z + (node_1229_q.w - node_1229_q.y) / (6.0 * node_1229_d + node_1229_e)), node_1229_d / (node_1229_q.x + node_1229_e), node_1229_q.x);;
                float node_4425 = 0.0;
                float3 diffuseColor = (lerp(float3(1,1,1),saturate(3.0*abs(1.0-2.0*frac((lerp( _Hue, (sin(clamp(ColourRandomization,0.1,1))*0.5277778+-0.1777778), _TreeColourRandomization )+node_1229.r)+float3(0.0,-1.0/3.0,1.0/3.0)))-1),(_Sat+node_1229.g))*clamp((_Val+(_ContrastDarks + ( (node_1229.b - node_4425) * (_ContrastLights - _ContrastDarks) ) / (1.0 - node_4425))),0.0,1.0));
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            Cull Off
            
            ColorMask RGB
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 xboxone ps4 wiiu 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float _Hue;
            uniform float _Sat;
            uniform float _Val;
            uniform fixed _VertexTexSwitch;
            uniform sampler2D _DiffuseMap; uniform float4 _DiffuseMap_ST;
            uniform float _ContrastLights;
            uniform float _ContrastDarks;
            uniform float _DiffusePower;
            uniform fixed _TreeColourRandomization;
            uniform float ColourRandomization;
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
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = pow(max( 0.0, NdotL), _DiffusePower) * attenColor;
                float4 _DiffuseMap_var = tex2D(_DiffuseMap,TRANSFORM_TEX(i.uv0, _DiffuseMap));
                float3 _VertexTexSwitch_var = lerp( i.vertexColor.rgb, _DiffuseMap_var.rgb, _VertexTexSwitch ); // Chooses between Textures and Vertex Colours
                float4 node_1229_k = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 node_1229_p = lerp(float4(float4(_VertexTexSwitch_var,0.0).zy, node_1229_k.wz), float4(float4(_VertexTexSwitch_var,0.0).yz, node_1229_k.xy), step(float4(_VertexTexSwitch_var,0.0).z, float4(_VertexTexSwitch_var,0.0).y));
                float4 node_1229_q = lerp(float4(node_1229_p.xyw, float4(_VertexTexSwitch_var,0.0).x), float4(float4(_VertexTexSwitch_var,0.0).x, node_1229_p.yzx), step(node_1229_p.x, float4(_VertexTexSwitch_var,0.0).x));
                float node_1229_d = node_1229_q.x - min(node_1229_q.w, node_1229_q.y);
                float node_1229_e = 1.0e-10;
                float3 node_1229 = float3(abs(node_1229_q.z + (node_1229_q.w - node_1229_q.y) / (6.0 * node_1229_d + node_1229_e)), node_1229_d / (node_1229_q.x + node_1229_e), node_1229_q.x);;
                float node_4425 = 0.0;
                float3 diffuseColor = (lerp(float3(1,1,1),saturate(3.0*abs(1.0-2.0*frac((lerp( _Hue, (sin(clamp(ColourRandomization,0.1,1))*0.5277778+-0.1777778), _TreeColourRandomization )+node_1229.r)+float3(0.0,-1.0/3.0,1.0/3.0)))-1),(_Sat+node_1229.g))*clamp((_Val+(_ContrastDarks + ( (node_1229.b - node_4425) * (_ContrastLights - _ContrastDarks) ) / (1.0 - node_4425))),0.0,1.0));
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
