//2018-05-03 10:03:06
//By Chao

Shader "AO/Character/Cell_Generic"
{
	Properties
	{
		[Enum(OFF,0,FRONT,1,BACK,2)] _CullMode("Cull Mode", int) = 2  //OFF/FRONT/BACK
		_BaseMap("BaseMap", 2D) = "white" {}
		_BaseColor("BaseColor", Color) = (1,1,1,1)

		_Outline_Color("Outline_Color", Color) = (0.5,0.5,0.5,1)
		_Outline_Sampler("Outline_Sampler", 2D) = "white" {}
		_Outline_Width("Outline_Width", Float) = 1
		_Nearest_Distance("Nearest_Distance", Float) = 0.5
		_Farthest_Distance("Farthest_Distance", Float) = 10
		_Offset_Depth("Offset_Camera_Depth", Float) = 0
		[MaterialToggle] _Is_BlendOutlineWithBaseColor("_Is_BlendOutlineWithBaseColor", Float) = 0
		[MaterialToggle] _Is_OutlineReceiveLightColor("_Is_UseLightColor", Float) = 1

		[MaterialToggle] _Is_LightColor("Is_LightColor", Float) = 1
		[MaterialToggle] _Is_NormalMapLighting("_Is_NormalMapLighting", Float) = 0
        [MaterialToggle] _Is_EnableSystemShadow ("_Is_EnableSystemShadow", Float ) = 1
		_NormalMap("NormalMap", 2D) = "bump" {}
		_BaseColor_Step("BaseColor_Step", Range(0, 1)) = 0.6
		_BaseShade_Feather("Base/Shade_Feather", Range(0.0001, 1)) = 0.0001
		_Tweak_SystemShadowsLevel("Tweak_SystemShadowsLevel", Range(-0.5, 0.5)) = 0
		_ShadeColor_Step("ShadeColor_Step", Range(0, 1)) = 0.4




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
			uniform float _Is_EnableSystemShadow;
			uniform float _BaseColor_Step;
			uniform float _BaseShade_Feather;
			uniform float _Tweak_SystemShadowsLevel;
			uniform float _ShadeColor_Step;


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
				//Light setup:
				float attenuation = LIGHT_ATTENUATION(i);
				float4 baseMapColor = tex2D(_BaseMap,TRANSFORM_TEX(i.uv0, _BaseMap));
				float3 baseColor = (_BaseColor.rgb*baseMapColor.rgb);
				float3 baseColor = lerp(baseColor, (baseColor*_LightColor0.rgb), _Is_LightColor);//Blend basecolor with light color.

				float4 firstShadeMapVar = tex2D(_1st_ShadeMap,TRANSFORM_TEX(i.uv0, _1st_ShadeMap));//Control color in shadow part
				float3 firstShadeColor = (_1st_ShadeColor.rgb*firstShadeMapVar.rgb);
				float3 firstShadeColor = lerp(firstShadeColor, (firstShadeColor*_LightColor0.rgb), _Is_LightColor);

				float4 secondShadeMapVar = tex2D(_2nd_ShadeMap,TRANSFORM_TEX(i.uv0, _2nd_ShadeMap));
				float3 secondShadeColor = (_2nd_ShadeColor.rgb*secondShadeMapVar.rgb);
				float3 secondShadeColor = lerp(secondShadeColor, (secondShadeColor*_LightColor0.rgb), _Is_LightColor);

				float incidentAngle = 0.5*dot(lerp(i.normalDir, normalDirection, _Is_NormalMapLighting),lightDirection) + 0.5;  //Cos(incidentAngle), change value from [-1.1] to [0,1]
				float finalShadowSample = saturate(1.0 +(lerp(incidentAngle, (incidentAngle*saturate(attenuation*0.5 + 0.5 + _Tweak_SystemShadowsLevel), _Is_EnableSystemShadow) - _BaseColor_Step + _BaseShade_Feather) - 1.0) / _BaseShade_Feather);

				float3 node_1702 = lerp(baseColor,lerp(firstShadeColor, secondShadeColor,saturate(1.0 + incidentAngle / _ShadeColor_Step)),finalShadowSample); // Final Color
				float4 _Set_HighColorMask_var = tex2D(_Set_HighColorMask,TRANSFORM_TEX(i.uv0, _Set_HighColorMask)); // HighColorMask
				float node_1331 = 0.5*dot(halfDirection,lerp(i.normalDir, normalDirection, _Is_NormalMapToHighColor)) + 0.5; //  Specular
				float node_5489 = (saturate((_Set_HighColorMask_var.g + _Tweak_HighColorMaskLevel))*lerp((1.0 - step(node_1331,(1.0 - _HighColor_Power))), pow(node_1331,exp2(lerp(11,1,_HighColor_Power))), _Is_SpecularToHighColor));
				float3 node_5205 = (lerp(_HighColor.rgb, (_HighColor.rgb*_LightColor0.rgb), _Is_LightColor_HighColor)*node_5489);
				float node_2595 = finalShadowSample;
				float3 Set_HighColor = (lerp(saturate((node_1702 - node_5489)), node_1702, _Is_BlendAddToHiColor) + lerp(node_5205, (node_5205*((1.0 - node_2595) + (node_2595*_TweakHighColorOnShadow))), _Is_UseTweakHighColorOnShadow));
				float3 node_2379 = Set_HighColor;
				float4 _Set_RimLightMask_var = tex2D(_Set_RimLightMask,TRANSFORM_TEX(i.uv0, _Set_RimLightMask)); // RimLightMask
				float3 _Is_LightColor_RimLight_var = lerp(_RimLightColor.rgb, (_RimLightColor.rgb*_LightColor0.rgb), _Is_LightColor_RimLight);
				float node_2652 = (1.0 - dot(lerp(i.normalDir, normalDirection, _Is_NormalMapToRimLight),viewDirection));
				float node_7879 = pow(node_2652,exp2(lerp(3,0,_RimLight_Power)));
				float node_4535 = 1.0;
				float node_2699 = 0.0;
				float node_8305 = saturate(lerp((node_2699 + ((node_7879 - _RimLight_InsideMask) * (node_4535 - node_2699)) / (node_4535 - _RimLight_InsideMask)), step(_RimLight_InsideMask,node_7879), _RimLight_FeatherOff));
				float node_8429 = 0.5*dot(i.normalDir,lightDirection) + 0.5;
				float3 _LightDirection_MaskOn_var = lerp((_Is_LightColor_RimLight_var*node_8305), (_Is_LightColor_RimLight_var*saturate((node_8305 - ((1.0 - node_8429) + _Tweak_LightDirection_MaskLevel)))), _LightDirection_MaskOn);
				float node_8113 = pow(node_2652,exp2(lerp(3,0,_Ap_RimLight_Power)));
				float3 Set_RimLight = (saturate((_Set_RimLightMask_var.g + _Tweak_RimLightMaskLevel))*lerp(_LightDirection_MaskOn_var, (_LightDirection_MaskOn_var + (lerp(_Ap_RimLightColor.rgb, (_Ap_RimLightColor.rgb*_LightColor0.rgb), _Is_LightColor_Ap_RimLight)*saturate((lerp((node_2699 + ((node_8113 - _RimLight_InsideMask) * (node_4535 - node_2699)) / (node_4535 - _RimLight_InsideMask)), step(_RimLight_InsideMask,node_8113), _Ap_RimLight_FeatherOff) - (saturate(node_8429) + _Tweak_LightDirection_MaskLevel))))), _Add_Antipodean_RimLight));
				float3 _RimLight_var = lerp(node_2379, (node_2379 + Set_RimLight), _RimLight);
				float node_254_ang = (_Rotate_MatCapUV*3.141592654);
				float node_254_spd = 1.0;
				float node_254_cos = cos(node_254_spd*node_254_ang);
				float node_254_sin = sin(node_254_spd*node_254_ang);
				float2 node_254_piv = float2(0.5,0.5);
				float node_5552_ang = (_Rotate_NormalMapForMatCapUV*3.141592654);
				float node_5552_spd = 1.0;
				float node_5552_cos = cos(node_5552_spd*node_5552_ang);
				float node_5552_sin = sin(node_5552_spd*node_5552_ang);
				float2 node_5552_piv = float2(0.5,0.5);
				float2 node_5552 = (mul(i.uv0 - node_5552_piv,float2x2(node_5552_cos, -node_5552_sin, node_5552_sin, node_5552_cos)) + node_5552_piv);
				float3 _NormalMapForMatCap_var = UnpackNormal(tex2D(_NormalMapForMatCap,TRANSFORM_TEX(node_5552, _NormalMapForMatCap)));
				float node_1482 = 0.0;
				float node_7689 = (node_1482 + _Tweak_MatCapUV);
				float node_2941 = 1.0;
				float2 node_254 = (mul((node_1482 + (((mul(UNITY_MATRIX_V, float4(lerp(i.normalDir, mul(_NormalMapForMatCap_var.rgb, tangentTransform).xyz.rgb, _Is_NormalMapForMatCap),0)).xyz.rgb.rg*0.5 + 0.5) - node_7689) * (node_2941 - node_1482)) / ((node_2941 - _Tweak_MatCapUV) - node_7689)) - node_254_piv,float2x2(node_254_cos, -node_254_sin, node_254_sin, node_254_cos)) + node_254_piv);
				float4 _MatCap_Sampler_var = tex2D(_MatCap_Sampler,TRANSFORM_TEX(node_254, _MatCap_Sampler));
				float3 node_2280 = (_MatCap_Sampler_var.rgb*_MatCapColor.rgb);
				float3 _Is_LightColor_MatCap_var = lerp(node_2280, (node_2280*_LightColor0.rgb), _Is_LightColor_MatCap);
				float node_2829 = finalShadowSample;
				float3 Set_MatCap = lerp(_Is_LightColor_MatCap_var, (_Is_LightColor_MatCap_var*((1.0 - node_2829) + (node_2829*_TweakMatCapOnShadow))), _Is_UseTweakMatCapOnShadow);
				float3 node_4172 = Set_MatCap;
				float3 finalColor = saturate((1.0 - (1.0 - saturate(lerp(_RimLight_var, lerp((_RimLight_var*node_4172), (_RimLight_var + node_4172), _Is_BlendAddToMatCap), _MatCap)))*(1.0 - (DecodeLightProbe(normalDirection)*_GI_Intensity))));
				fixed4 finalRGBA = fixed4(finalColor * 1,0);
				UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
				return finalRGBA;
			}

			ENDCG
		
		}

	}
}
