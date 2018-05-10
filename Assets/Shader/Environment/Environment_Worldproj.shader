//2018-04-27 14:05:19
//By Chao

Shader "AO/Environment_Worldproj"
{
	Properties
	{
		_ProjTex("Texture", 2D) = "white" {}
		_ShadowBrightness("Shadow Brightness",Range(0,1)) = 0.5
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			uniform sampler2D _ProjTex;
			uniform float _ShadowBrightness;

			struct appdata
			{
				float4 pos : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 normal : NORMAL;
				float2 coodX : TEXCOORDX;
				float2 coodY : TEXCOORDY;
				float2 coodZ : TEXCOORDZ;
				LIGHTING_COORDS(0, 1)
			};

			//float4 _ProjTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				float3 worldPosition = mul(unity_ObjectToWorld, v.pos).xyz;
				//Objectspace normal to worldspace
				//o.normal = mul(unity_ObjectToWorld, float4(v.normal, 0.0)).xyx;
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.coodX.x = worldPosition.y;
				o.coodX.y = worldPosition.z;
				o.coodY.x = worldPosition.x;
				o.coodY.y = worldPosition.z;
				o.coodZ.x = worldPosition.x;
				o.coodZ.y = worldPosition.y;
				o.pos = UnityObjectToClipPos(v.pos);
				TRANSFER_VERTEX_TO_FRAGMENT(o)

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{

				fixed atten = LIGHT_ATTENUATION(i);
				atten = saturate(atten+ _ShadowBrightness);

				// sample the texture
				fixed4 sampleX = tex2D(_ProjTex, float2(i.coodX.x, 1-i.coodX.y));
				fixed4 sampleY = tex2D(_ProjTex, float2(i.coodY.x, 1-i.coodY.y));
				fixed4 sampleZ = tex2D(_ProjTex, float2(i.coodZ.x, 1-i.coodZ.y));

				//fixed4 col = saturate(sampleX * abs(i.normal.x) + sampleY * abs(i.normal.y) + sampleZ * abs(i.normal.z));
				//fixed4 col = saturate(sampleX * i.normal.x + sampleY * i.normal.y + sampleZ * i.normal.z);
				fixed4 col = saturate(sampleX * i.normal.x*i.normal.x + sampleY * i.normal.y*i.normal.y + sampleZ * i.normal.z*i.normal.z);
				col = atten*col;
				//fixed4 col = (sampleX * i.normal.x*i.normal.x + sampleY * i.normal.y*i.normal.y + sampleZ * i.normal.z*i.normal.z);
				//fixed4 col = sampleX * i.normal.x + sampleY * i.normal.y + sampleZ * i.normal.z;
				//fixed4 col = sampleX  + sampleY  + sampleZ;

				return col;
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
