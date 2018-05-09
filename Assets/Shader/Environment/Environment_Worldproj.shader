//2018-04-27 14:05:19
//By Chao

Shader "AO/Environment_Worldproj"
{
	Properties
	{
		_ProjTex("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			Lighting On
			Tags{ "LightMode" = "ForwardBase" }
			CGPROGRAM

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "Lighting.cginc"
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase

			uniform sampler2D _ProjTex;

			struct appdata
			{
				float4 pos : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float2 coodX : COODX;
				float2 coodY : COODY;
				float2 coodZ : COODZ;
				//LIGHTING_COORDS(4, 5)
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
				//TRANSFER_VERTEX_TO_FRAGMENT(o)

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{

				//fixed atten = LIGHT_ATTENUATION(i);

				// sample the texture
				fixed4 sampleX = tex2D(_ProjTex, float2(i.coodX.x, 1-i.coodX.y));
				fixed4 sampleY = tex2D(_ProjTex, float2(i.coodY.x, 1-i.coodY.y));
				fixed4 sampleZ = tex2D(_ProjTex, float2(i.coodZ.x, 1-i.coodZ.y));

				//fixed4 col = saturate(sampleX * abs(i.normal.x) + sampleY * abs(i.normal.y) + sampleZ * abs(i.normal.z));
				//fixed4 col = saturate(sampleX * i.normal.x + sampleY * i.normal.y + sampleZ * i.normal.z);
				fixed4 col = saturate(sampleX * i.normal.x*i.normal.x + sampleY * i.normal.y*i.normal.y + sampleZ * i.normal.z*i.normal.z);
				//col *= atten;
				//fixed4 col = (sampleX * i.normal.x*i.normal.x + sampleY * i.normal.y*i.normal.y + sampleZ * i.normal.z*i.normal.z);
				//fixed4 col = sampleX * i.normal.x + sampleY * i.normal.y + sampleZ * i.normal.z;
				//fixed4 col = sampleX  + sampleY  + sampleZ;

				return col;
			}
			ENDCG
		}

		// Pass to render object as a shadow collector
		Pass
		{
			Name "ShadowCollector"
			Tags{ "LightMode" = "ShadowCollector" }

			Fog{ Mode Off }
			ZWrite On ZTest Less

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_shadowcollector

			#define SHADOW_COLLECTOR_PASS
			#include "UnityCG.cginc"

			uniform float _Scale;

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				V2F_SHADOW_COLLECTOR;
			};

			v2f vert(appdata v)
			{
				v2f o;
				v.vertex.xyz *= _Scale;
				TRANSFER_SHADOW_COLLECTOR(o)
					return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				SHADOW_COLLECTOR_FRAGMENT(i)
			}
			ENDCG

		}
	}
	Fallback Off
}
