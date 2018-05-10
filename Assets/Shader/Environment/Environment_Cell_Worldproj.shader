//2018-05-10 19:09:00
//By Chao


Shader "AO/Environment/Cell_Worldproj"
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
		_ShadowBrightness("Shadow Brightness", Range(0, 1)) = 0.5
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

			uniform float4 _LightColor0;//Uinty build-in variables
            uniform float4 _BaseColor;
            uniform sampler2D _BaseMap; uniform float4 _BaseMap_ST;
            uniform float _Outline_Width;
            uniform float _Farthest_Distance;
            uniform float _Nearest_Distance;
            uniform sampler2D _Outline_Sampler; uniform float4 _Outline_Sampler_ST;
            uniform float4 _Outline_Color;
            uniform fixed _Is_BlendOutlineWithBaseColor;
            uniform float _Offset_Depth;
			uniform float _Is_LightColor;

            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;

            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };

            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float4 _Outline_Sampler_var = tex2Dlod(_Outline_Sampler,float4(TRANSFORM_TEX(o.uv0, _Outline_Sampler),0.0,0));
                float Set_Outline_Width = (_Outline_Width*0.001*smoothstep( _Farthest_Distance, _Nearest_Distance, distance(objPos.rgb,_WorldSpaceCameraPos) )*_Outline_Sampler_var.rgb).r;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - o.pos.xyz);
                float4 viewDirectionVP = mul(UNITY_MATRIX_VP, float4(viewDirection.xyz, 1));
                _Offset_Depth = _Offset_Depth * -0.1;
                o.pos = UnityObjectToClipPos(float4(v.vertex.xyz + v.normal*Set_Outline_Width,1) );
                o.pos.z = o.pos.z + _Offset_Depth*viewDirectionVP.z;
                return o;
            }

            float4 frag(VertexOutput i, float facing : VFACE) : SV_Target{
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float4 _BaseMap_var = tex2D(_BaseMap,TRANSFORM_TEX(i.uv0, _BaseMap));
                float3 col = (_BaseColor.rgb*_BaseMap_var.rgb);
                float3 baseColor = lerp( col, (col*_LightColor0.rgb), _Is_LightColor);
                float3 outputColor = lerp( _Outline_Color.rgb, (_Outline_Color.rgb*baseColor*baseColor), _Is_BlendOutlineWithBaseColor );
                return fixed4(outputColor,0);
            }
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
			uniform float _ShadowBrightness;
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

				//Prj cood
				float2 coodX = 0;
				float2 coodY = 0;
				float2 coodZ = 0;
				coodX.x = i.posWorld.y;
				coodX.y = i.posWorld.z;
				coodY.x = i.posWorld.x;
				coodY.y = i.posWorld.z;
				coodZ.x = i.posWorld.x;
				coodZ.y = i.posWorld.y;

				//Unpack Normal Map
				float faceDir = (facing >= 0 ? 1 : -1);
				i.normalDir = normalize(i.normalDir);
				i.normalDir *= faceDir;
				float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
				fixed4 NormalSampleX = tex2D(_NormalMap, float2(coodX.x, 1-coodX.y));
				fixed4 NormalSampleY = tex2D(_NormalMap, float2(coodY.x, 1-coodY.y));
				fixed4 NormalSampleZ = tex2D(_NormalMap, float2(coodZ.x, 1-coodZ.y));
				float3 normalMapVar = saturate(NormalSampleX.rgb * i.normalDir.x*i.normalDir.x + NormalSampleY.rgb * i.normalDir.y*i.normalDir.y + NormalSampleZ.rgb * i.normalDir.z*i.normalDir.z);
				float3x3 tangentTransform = float3x3(i.tangentDir, i.bitangentDir, i.normalDir);
				float3 normalDirection = normalize(mul(normalMapVar, tangentTransform)); // Perturbed normals
				float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));  //_WorldSpaceLightPos0.w  0 paral light 1 spot light
				float3 halfDirection = normalize(viewDirection + lightDirection);

				//Base color
				float attenuation = LIGHT_ATTENUATION(i);
				float atten = saturate(attenuation+ _ShadowBrightness);
				fixed4 sampleX = tex2D(_BaseMap, float2(coodX.x, 1-coodX.y));
				fixed4 sampleY = tex2D(_BaseMap, float2(coodY.x, 1-coodY.y));
				fixed4 sampleZ = tex2D(_BaseMap, float2(coodZ.x, 1-coodZ.y));
				float3 baseColor = _BaseColor.rgb*saturate(sampleX.rgb * i.normalDir.x*i.normalDir.x + sampleY.rgb * i.normalDir.y*i.normalDir.y + sampleZ.rgb * i.normalDir.z*i.normalDir.z);
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
				finalColor = saturate(rimLightColor*atten*(1.0 - (DecodeLightProbe(normalDirection)*_GI_Intensity)));
				fixed4 finalRGBA = fixed4(finalColor,1.0);
				UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
				return finalRGBA;
			}
			ENDCG		
		}
		//Cast Shadow
		Pass{

			Name "ShadowCaster"
			Tags{
			"LightMode" = "ShadowCaster"
			}
			Offset 1, 1
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#define UNITY_PASS_SHADOWCASTER
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_shadowcaster
			#pragma multi_compile_fog
			#pragma only_renderers d3d9 d3d11 glcore gles gles3 metal xboxone ps4 switch
			#pragma target 3.0

			struct VertexInput {
				float4 vertex : POSITION;
			};

			struct VertexOutput {
				V2F_SHADOW_CASTER;
			};

			VertexOutput vert(VertexInput v) {
				VertexOutput o = (VertexOutput)0;
				o.pos = UnityObjectToClipPos(v.vertex);
				TRANSFER_SHADOW_CASTER(o)
				return o;
			}

			float4 frag(VertexOutput i, float facing : VFACE) : COLOR{
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
	}
	Fallback "VertexLit"
}
