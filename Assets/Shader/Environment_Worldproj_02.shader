Shader "AO/Environment_Worldproj_02"
{
	Properties
	{
		_ProjTex("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members coodX,coodY,coodZ)
#pragma exclude_renderers d3d11
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				//float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				//float3 normal : NORMAL;
				//float2 coodX;
				//float2 coodY;
				//float2 coodZ;
			};

			sampler2D _ProjTex;
			float4 _ProjTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _ProjTex);
				//o.normal = i.normal;
				//o.coodX.x = v.vertex.y;
				//o.coodX.y = -v.vertex.z;
				//o.coodY.x = v.vertex.x;
				//o.coodY.y = -v.vertex.z;
				//o.coodZ.x = v.vertex.x;
				//o.coodZ.y = v.vertex.y;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				//fixed4 sampleY = tex2D(_ProjTex, i.coodY);
				//fixed4 sampleZ = tex2D(_ProjTex, i.coodZ);
				//fixed4 sampleX = tex2D(_ProjTex, i.coodX);

				//fixed4 col = sampleX * abs(i.normal.x) + sampleY * abs(i.normal.y) + sampleZ * abs(i.normal.z);
				fixed4 col = tex2D(_ProjTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
