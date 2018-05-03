//2018-05-03 10:03:06
//By Chao

Shader "AO/Character/Cell_Generic"
{
	Properties
	{
		[Enum(OFF,0,FRONT,1,BACK,2)] _CullMode("Cull Mode", int) = 2  //OFF/FRONT/BACK
		_BaseMap("BaseMap", 2D) = "white" {}
		_BaseColor("BaseColor", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Outline_Color("Outline_Color", Color) = (0.5,0.5,0.5,1)
		_Outline_Sampler("Outline_Sampler", 2D) = "white" {}
		_Outline_Width("Outline_Width", Float) = 1
		_Nearest_Distance("Nearest_Distance", Float) = 0.5
		_Farthest_Distance("Farthest_Distance", Float) = 10
		_Offset_Depth("Offset_Camera_Depth", Float) = 0

		[MaterialToggle] _Is_BlendOutlineWithBaseColor("_Is_BlendOutlineWithBaseColor", Float) = 0
		[MaterialToggle] _Is_OutlineReceiveLightColor("_Is_UseLightColor", Float) = 1
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
				float2 Set_UV0 = i.uv0;
				float2 node_6830 = Set_UV0;
				float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(node_6830, _NormalMap)));
				float3 normalLocal = _NormalMap_var.rgb;
				float3 normalDirection = normalize(mul(normalLocal, tangentTransform)); // Perturbed normals
				float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
				float3 lightColor = _LightColor0.rgb;
				float3 halfDirection = normalize(viewDirection + lightDirection);
			//Light setup:
				float attenuation = LIGHT_ATTENUATION(i);
				float2 node_6858 = Set_UV0;
				float4 _BaseMap_var = tex2D(_BaseMap,TRANSFORM_TEX(node_6858, _BaseMap));
				float3 node_9970 = (_BaseColor.rgb*_BaseMap_var.rgb);
				float3 Set_LightColor = _LightColor0.rgb;
				float3 Set_BaseColor = lerp(node_9970, (node_9970*Set_LightColor), _Is_LightColor_Base);
				float2 node_8098 = Set_UV0;
				float4 _1st_ShadeMap_var = tex2D(_1st_ShadeMap,TRANSFORM_TEX(node_8098, _1st_ShadeMap));
				float3 node_6918 = (_1st_ShadeColor.rgb*_1st_ShadeMap_var.rgb);
				float3 Set_1st_ShadeColor = lerp(node_6918, (node_6918*Set_LightColor), _Is_LightColor_1st_Shade);
				float2 node_5380 = Set_UV0;
				float4 _2nd_ShadeMap_var = tex2D(_2nd_ShadeMap,TRANSFORM_TEX(node_5380, _2nd_ShadeMap));
				float3 node_8559 = (_2nd_ShadeColor.rgb*_2nd_ShadeMap_var.rgb);
				float3 Set_2nd_ShadeColor = lerp(node_8559, (node_8559*Set_LightColor), _Is_LightColor_2nd_Shade);
				float node_4315 = 0.5*dot(lerp(i.normalDir, normalDirection, _Is_NormalMapToBase),lightDirection) + 0.5;
				float node_2294 = (_ShadeColor_Step - _1st2nd_Shades_Feather);
				float node_9309 = 1.0;
				float2 node_1071 = Set_UV0;
				float4 _Set_2nd_ShadePosition_var = tex2D(_Set_2nd_ShadePosition,TRANSFORM_TEX(node_1071, _Set_2nd_ShadePosition));
				float node_3494 = 0.5;
				float node_583 = (_BaseColor_Step - _BaseShade_Feather);
				float node_8323 = 1.0;
				float2 node_1237 = Set_UV0;
				float4 _Set_1st_ShadePosition_var = tex2D(_Set_1st_ShadePosition,TRANSFORM_TEX(node_1237, _Set_1st_ShadePosition));
				float Set_FinalShadowSample = saturate((node_8323 + ((lerp(node_4315, (node_4315*saturate(((attenuation*node_3494) + node_3494 + _Tweak_SystemShadowsLevel))), _Set_SystemShadowsToBase) - node_583) * ((1.0 - _Set_1st_ShadePosition_var.rgb).r - node_8323)) / (_BaseColor_Step - node_583)));
				float3 node_1702 = lerp(Set_BaseColor,lerp(Set_1st_ShadeColor,Set_2nd_ShadeColor,saturate((node_9309 + ((node_4315 - node_2294) * ((1.0 - _Set_2nd_ShadePosition_var.rgb).r - node_9309)) / (_ShadeColor_Step - node_2294)))),Set_FinalShadowSample); // Final Color
				float2 node_5701 = Set_UV0;
				float4 _Set_HighColorMask_var = tex2D(_Set_HighColorMask,TRANSFORM_TEX(node_5701, _Set_HighColorMask)); // HighColorMask
				float node_1331 = 0.5*dot(halfDirection,lerp(i.normalDir, normalDirection, _Is_NormalMapToHighColor)) + 0.5; //  Specular
				float node_5489 = (saturate((_Set_HighColorMask_var.g + _Tweak_HighColorMaskLevel))*lerp((1.0 - step(node_1331,(1.0 - _HighColor_Power))), pow(node_1331,exp2(lerp(11,1,_HighColor_Power))), _Is_SpecularToHighColor));
				float3 node_5205 = (lerp(_HighColor.rgb, (_HighColor.rgb*Set_LightColor), _Is_LightColor_HighColor)*node_5489);
				float node_2595 = Set_FinalShadowSample;
				float3 Set_HighColor = (lerp(saturate((node_1702 - node_5489)), node_1702, _Is_BlendAddToHiColor) + lerp(node_5205, (node_5205*((1.0 - node_2595) + (node_2595*_TweakHighColorOnShadow))), _Is_UseTweakHighColorOnShadow));
				float3 node_2379 = Set_HighColor;
				float2 node_4781 = Set_UV0;
				float4 _Set_RimLightMask_var = tex2D(_Set_RimLightMask,TRANSFORM_TEX(node_4781, _Set_RimLightMask)); // RimLightMask
				float3 _Is_LightColor_RimLight_var = lerp(_RimLightColor.rgb, (_RimLightColor.rgb*Set_LightColor), _Is_LightColor_RimLight);
				float node_2652 = (1.0 - dot(lerp(i.normalDir, normalDirection, _Is_NormalMapToRimLight),viewDirection));
				float node_7879 = pow(node_2652,exp2(lerp(3,0,_RimLight_Power)));
				float node_4535 = 1.0;
				float node_2699 = 0.0;
				float node_8305 = saturate(lerp((node_2699 + ((node_7879 - _RimLight_InsideMask) * (node_4535 - node_2699)) / (node_4535 - _RimLight_InsideMask)), step(_RimLight_InsideMask,node_7879), _RimLight_FeatherOff));
				float node_8429 = 0.5*dot(i.normalDir,lightDirection) + 0.5;
				float3 _LightDirection_MaskOn_var = lerp((_Is_LightColor_RimLight_var*node_8305), (_Is_LightColor_RimLight_var*saturate((node_8305 - ((1.0 - node_8429) + _Tweak_LightDirection_MaskLevel)))), _LightDirection_MaskOn);
				float node_8113 = pow(node_2652,exp2(lerp(3,0,_Ap_RimLight_Power)));
				float3 Set_RimLight = (saturate((_Set_RimLightMask_var.g + _Tweak_RimLightMaskLevel))*lerp(_LightDirection_MaskOn_var, (_LightDirection_MaskOn_var + (lerp(_Ap_RimLightColor.rgb, (_Ap_RimLightColor.rgb*Set_LightColor), _Is_LightColor_Ap_RimLight)*saturate((lerp((node_2699 + ((node_8113 - _RimLight_InsideMask) * (node_4535 - node_2699)) / (node_4535 - _RimLight_InsideMask)), step(_RimLight_InsideMask,node_8113), _Ap_RimLight_FeatherOff) - (saturate(node_8429) + _Tweak_LightDirection_MaskLevel))))), _Add_Antipodean_RimLight));
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
				float2 node_5552 = (mul(Set_UV0 - node_5552_piv,float2x2(node_5552_cos, -node_5552_sin, node_5552_sin, node_5552_cos)) + node_5552_piv);
				float3 _NormalMapForMatCap_var = UnpackNormal(tex2D(_NormalMapForMatCap,TRANSFORM_TEX(node_5552, _NormalMapForMatCap)));
				float node_1482 = 0.0;
				float node_7689 = (node_1482 + _Tweak_MatCapUV);
				float node_2941 = 1.0;
				float2 node_254 = (mul((node_1482 + (((mul(UNITY_MATRIX_V, float4(lerp(i.normalDir, mul(_NormalMapForMatCap_var.rgb, tangentTransform).xyz.rgb, _Is_NormalMapForMatCap),0)).xyz.rgb.rg*0.5 + 0.5) - node_7689) * (node_2941 - node_1482)) / ((node_2941 - _Tweak_MatCapUV) - node_7689)) - node_254_piv,float2x2(node_254_cos, -node_254_sin, node_254_sin, node_254_cos)) + node_254_piv);
				float4 _MatCap_Sampler_var = tex2D(_MatCap_Sampler,TRANSFORM_TEX(node_254, _MatCap_Sampler));
				float3 node_2280 = (_MatCap_Sampler_var.rgb*_MatCapColor.rgb);
				float3 _Is_LightColor_MatCap_var = lerp(node_2280, (node_2280*Set_LightColor), _Is_LightColor_MatCap);
				float node_2829 = Set_FinalShadowSample;
				float3 Set_MatCap = lerp(_Is_LightColor_MatCap_var, (_Is_LightColor_MatCap_var*((1.0 - node_2829) + (node_2829*_TweakMatCapOnShadow))), _Is_UseTweakMatCapOnShadow);
				float3 node_4172 = Set_MatCap;
				float3 finalColor = saturate((1.0 - (1.0 - saturate(lerp(_RimLight_var, lerp((_RimLight_var*node_4172), (_RimLight_var + node_4172), _Is_BlendAddToMatCap), _MatCap)))*(1.0 - (DecodeLightProbe(normalDirection)*_GI_Intensity))));
				fixed4 finalRGBA = fixed4(finalColor * 1,0);
				UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
				return finalRGBA;
			}

			ENDCG
		
		}

		//Pass
		//{
		//	CGPROGRAM
		//	#pragma vertex vert
		//	#pragma fragment frag
		//	// make fog work
		//	#pragma multi_compile_fog
		//	
		//	#include "UnityCG.cginc"

		//	struct appdata
		//	{
		//		float4 vertex : POSITION;
		//		float2 uv : TEXCOORD0;
		//	};

		//	struct v2f
		//	{
		//		float2 uv : TEXCOORD0;
		//		UNITY_FOG_COORDS(1)
		//		float4 vertex : SV_POSITION;
		//	};

		//	sampler2D _MainTex;
		//	float4 _MainTex_ST;
		//	
		//	v2f vert (appdata v)
		//	{
		//		v2f o;
		//		o.vertex = UnityObjectToClipPos(v.vertex);
		//		o.uv = TRANSFORM_TEX(v.uv, _MainTex);
		//		UNITY_TRANSFER_FOG(o,o.vertex);
		//		return o;
		//	}
		//	
		//	fixed4 frag (v2f i) : SV_Target
		//	{
		//		// sample the texture
		//		fixed4 col = tex2D(_MainTex, i.uv);
		//		// apply fog
		//		UNITY_APPLY_FOG(i.fogCoord, col);
		//		return col;
		//	}
		//	ENDCG
		//}
	}
}
