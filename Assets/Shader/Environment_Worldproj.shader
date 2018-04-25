// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

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
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
		// make fog work
#pragma multi_compile_fog

#include "UnityCG.cginc"

		struct appdata
	{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		UNITY_FOG_COORDS(1)
			float4 vertex : SV_POSITION;
		float3 normal : NORMAL;
		float2 coodX : COODX;
		float2 coodY : COODY;
		float2 coodZ : COODZ;
	};

	sampler2D _ProjTex;
	//float4 _ProjTex_ST;

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		UNITY_TRANSFER_FOG(o,o.vertex);
		float3 worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;
		o.normal = v.normal;
		o.coodX.x = worldPosition.y;
		o.coodX.y = worldPosition.z;
		o.coodY.x = worldPosition.x;
		o.coodY.y = worldPosition.z;
		o.coodZ.x = worldPosition.x;
		o.coodZ.y = worldPosition.y;
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		// sample the texture
		fixed4 sampleX = tex2D(_ProjTex, float2(i.coodX.x, 1 - i.coodX.y));
	fixed4 sampleY = tex2D(_ProjTex, float2(i.coodY.x, 1 - i.coodY.y));
	fixed4 sampleZ = tex2D(_ProjTex, float2(i.coodZ.x, 1 - i.coodZ.y));

	fixed4 col = saturate(sampleX * abs(i.normal.x) + sampleY * abs(i.normal.y) + sampleZ * abs(i.normal.z));
	//fixed4 col = sampleX * abs(i.normal.x) + sampleY * abs(i.normal.y) + sampleZ * abs(i.normal.z);
	//fixed4 col = sampleX * i.normal.x + sampleY * i.normal.y + sampleZ * i.normal.z;
	//fixed4 col = sampleX  + sampleY  + sampleZ;
	//fixed4 col;
	//col.rgb= i.normal * 0.5 + 0.5;
	// apply fog
	UNITY_APPLY_FOG(i.fogCoord, col);
	return col;
	}
		ENDCG
	}
	}
}
