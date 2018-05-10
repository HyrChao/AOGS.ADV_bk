//2018-05-09 15:48:50
//By Chao

Shader "AO/Cell/Cell_Generic_Simplified"
{
	Properties
	{
		//Base properties
		[Enum(OFF,0,FRONT,1,BACK,2)] _CullMode("Cull Mode", int) = 2  //OFF/FRONT/BACK
		_BaseColor("BaseColor", Color) = (1,1,1,1)
		_BaseMap("BaseMap", 2D) = "white" {}
		_BaseColor_Step("BaseColor Step", Range(0, 1)) = 0.6
		_NormalMap("NormalMap", 2D) = "bump" {}	

		//Shadow & Light related
		[MaterialToggle] _Is_NormalMapLighting("Use NormalMap", Float) = 0
		[MaterialToggle] _Is_LightColor("Use LightColor", Float) = 1
		_BaseShade_Feather("BaseShade Feather", Range(0.0001, 1)) = 0.0001
		_Contrast("Contrast",Range(0.0001,0.9999)) = 0.0001

		_ShadeColor_Step("ShadeColor Step", Range(0, 1)) = 0.4
		_ShadeColor("ShadeColor", Color) = (1,1,1,1)
		[MaterialToggle] _Is_EnableSystemShadow("Enable SystemShadow", Float) = 0
		_Tweak_SystemShadowsLevel("Tweak SystemShadowsLevel", Range(-0.5, 0.5)) = 0

		//Outline related		
		_Outline_Color("Outline Color", Color) = (0.5,0.5,0.5,1)
		_Outline_Sampler("Outline Sampler", 2D) = "white" {}
		_Outline_Width("Outline Width", Float) = 1
		_Nearest_Distance("Nearest Distance", Float) = 0.5
		_Farthest_Distance("Farthest Distance", Float) = 10
		_Offset_Depth("Offset Camera Depth", Float) = 0
		[MaterialToggle] _Is_BlendOutlineWithBaseColor("Outline Blend BaseColor", Float) = 0

		//Highlight related
		[MaterialToggle] _AddHighlight("Add Highlight", Float) = 0
		_HighLightColor("Highlight Color", Color) = (1,1,1,1)
		_HighLightPower("Highlight Power", Range(0, 1)) = 0
		[MaterialToggle] _UseSpecularAsHighlight("Use Specular As Highlight", Float) = 0

		//Rimlight related
		[MaterialToggle] _UseRimLight("Use RimLight", Float) = 0
		_RimLightColor("RimLight Color", Color) = (1,1,1,1)
		_RimLightPower("RimLight Power", Range(0, 1)) = 0.1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		
		//Render outline
		Pass{
			Name "Outline"
			Tags{
				}
			Cull FRONT

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#pragma only_renderers d3d9 d3d11 glcore gles gles3 metal xboxone ps4 switch
			#pragma target 3.0	
			//target 3.0 https://docs.unity3d.com/Manual/SL-ShaderCompileTargets.html
			//DX9 shader model 3.0: derivative instructions, texture LOD sampling, 10 interpolators, more math/texture instructions allowed.
			//Not supported on DX11 feature level 9.x GPUs(e.g.most Windows Phone devices).
			//Might not be fully supported by some OpenGL ES 2.0 devices, depending on driver extensions present and features used.
			#include "Cg_Outline.cginc"
			ENDCG
		}

		
		//Core pass
		pass {
			Name "Forward"
			Tags{
				"LightMode" = "ForwardBase"
				}
			Cull [_CullMode]

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#define UNITY_PASS_FORWARDBASE
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "Lighting.cginc"
			#pragma multi_compile_fwdbase_fullshadows	
			//https://docs.unity3d.com/Manual/SL-MultipleProgramVariants.html
			//There are several “shortcut” notations for compiling multiple shader variants; 
			//they are mostly to deal with different light, shadow and lightmap types in Unity. See rendering pipeline for details.
			#pragma multi_compile_fog
			#pragma only_renderers d3d9 d3d11 glcore gles gles3 metal xboxone ps4 switch
			#pragma target 3.0

			struct VertexInput {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float2 texcoord0 : TEXCOORD0;
			};
			struct VertexOutput {
				float4 pos : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				float4 posWorld : TEXCOORD1;
				float3 normalDir : TEXCOORD2;
				float3 tangentDir : TEXCOORD3;
				float3 bitangentDir : TEXCOORD4;
				LIGHTING_COORDS(5, 6)
				UNITY_FOG_COORDS(7)
			};

			uniform float4 _BaseColor;
			uniform sampler2D _BaseMap; uniform float4 _BaseMap_ST;
			uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
			uniform float _Is_LightColor;
			uniform float _Is_NormalMapLighting;
			uniform float _BaseColor_Step;
			uniform float _BaseShade_Feather;

			uniform float _Contrast;
			uniform float4 _ShadeColor;
			uniform float _Is_UseShadeMap;
			uniform float _Tweak_SystemShadowsLevel;
			uniform float _Is_EnableSystemShadow;

			uniform float4 _HighLightColor;
			uniform float _HighLightPower;
			uniform float _AddHighlight;
			uniform float _UseSpecularAsHighlight;
			uniform float _UseRimLight;
			uniform float4 _RimLightColor;
			uniform float _RimLightPower;
			uniform float _GI_Intensity;

			fixed3 DecodeLightProbe(fixed3 N) {
				return ShadeSH9(float4(N, 1));
			}

			VertexOutput vert(VertexInput v) {
				VertexOutput o = (VertexOutput)0;
				o.uv0 = v.texcoord0;
				o.normalDir = UnityObjectToWorldNormal(v.normal);
				o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
				o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.pos = UnityObjectToClipPos(v.vertex);
				UNITY_TRANSFER_FOG(o, o.pos);
				TRANSFER_VERTEX_TO_FRAGMENT(o)
				return o;
			}
			float4 frag(VertexOutput i, float facing : VFACE) : COLOR{
				float faceDir = (facing >= 0 ? 1 : -1);
				i.normalDir = normalize(i.normalDir);
				i.normalDir *= faceDir;
				float3x3 tangentTransform = float3x3(i.tangentDir, i.bitangentDir, i.normalDir);
				float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
				//Load normal
				float3 normalMapVar = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(i.uv0, _NormalMap))); //TRANSFORM_TEX calc tiling and offset, return uv coord.
				float3 normalDirection = normalize(mul(normalMapVar, tangentTransform)); // Perturbed normals
				float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));  //_WorldSpaceLightPos0.w  0 paral light 1 spot light
				float3 halfDirection = normalize(viewDirection + lightDirection);

				//------------------------------Light setup-----------------------------------------------------

				//Base color
				float attenuation = LIGHT_ATTENUATION(i);
				float4 baseMapColor = tex2D(_BaseMap,TRANSFORM_TEX(i.uv0, _BaseMap));
				float3 baseColor = (_BaseColor.rgb*baseMapColor.rgb);
				//-Toggle-  Use light color
				baseColor = lerp(baseColor, (baseColor*_LightColor0.rgb), _Is_LightColor);//Blend basecolor with light color.

				//Shadow

				//Setup first ShadeColor
				float3 firstShadeColor = (1- _Contrast)*_ShadeColor.rgb*baseColor.rgb;
				firstShadeColor = lerp(firstShadeColor, (firstShadeColor*_LightColor0.rgb), _Is_LightColor);
				float incidentAngle = 0.5*dot(lerp(i.normalDir, normalDirection, _Is_NormalMapLighting),lightDirection) + 0.5;  //Cos(incidentAngle), change value from [-1.1] to [0,1]
				float finalShadowSample = saturate(1-(lerp(incidentAngle, incidentAngle*saturate(attenuation*0.5 + 0.5 + _Tweak_SystemShadowsLevel), _Is_EnableSystemShadow) - _BaseColor_Step + _BaseShade_Feather) / _BaseShade_Feather);				
				float3 finalColor = lerp(baseColor/(1-_Contrast),firstShadeColor,finalShadowSample); //Aply shaadow
			
				//High light
				float specularAngle = 0.5*dot(halfDirection,lerp(i.normalDir, normalDirection, _Is_NormalMapLighting)) + 0.5; //  Specular cosine angle (0,1)
				float3 highlightColor = saturate(lerp(1.0 - step(specularAngle,(1.0 - _HighLightPower)), pow(specularAngle,exp2(lerp(11,1,_HighLightPower))), _UseSpecularAsHighlight));
				highlightColor = (lerp(_HighLightColor.rgb, (_HighLightColor.rgb*_LightColor0.rgb), _Is_LightColor)*highlightColor);
				highlightColor = lerp(saturate(finalColor - highlightColor), finalColor, _AddHighlight) + highlightColor;

				//Rimlight 
				float3 rimLightColor = lerp(_RimLightColor.rgb, (_RimLightColor.rgb*_LightColor0.rgb), _Is_LightColor);
				float viewNormalAngel = (1.0 - dot(lerp(i.normalDir, normalDirection, _Is_NormalMapLighting),viewDirection));
				viewNormalAngel = pow(viewNormalAngel,exp2(lerp(3,0,_RimLightPower)));
				rimLightColor = rimLightColor * viewNormalAngel;
				rimLightColor = lerp(highlightColor, (highlightColor + rimLightColor), _UseRimLight);

				// Final Color
				finalColor = saturate(rimLightColor*(1.0 - (DecodeLightProbe(normalDirection)*_GI_Intensity)));
				fixed4 finalRGBA = fixed4(finalColor,1.0);
				UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
				return finalRGBA;
			}

			ENDCG
		
		}

	}
}
