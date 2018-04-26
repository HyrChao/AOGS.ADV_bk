

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
		float3 worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;
		//Objectspace normal to worldspace
		//o.normal = mul(unity_ObjectToWorld, float4(v.normal, 0.0)).xyx;
		o.normal = v.normal;
		o.normal = UnityObjectToWorldNormal(v.normal);
		o.coodX.x = worldPosition.y;
		o.coodX.y = worldPosition.z;
		o.coodY.x = worldPosition.x;
		o.coodY.y = worldPosition.z;
		o.coodZ.x = worldPosition.x;
		o.coodZ.y = worldPosition.y;

		o.vertex = UnityObjectToClipPos(v.vertex);

		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{

	// sample the texture
	fixed4 sampleX = tex2D(_ProjTex, float2(i.coodX.x, 1-i.coodX.y));
	fixed4 sampleY = tex2D(_ProjTex, float2(i.coodY.x, 1-i.coodY.y));
	fixed4 sampleZ = tex2D(_ProjTex, float2(i.coodZ.x, 1-i.coodZ.y));

	//fixed4 col = saturate(sampleX * abs(i.normal.x) + sampleY * abs(i.normal.y) + sampleZ * abs(i.normal.z));
	//fixed4 col = saturate(sampleX * abs(i.normal.x) + sampleY * abs(i.normal.y) + sampleZ * abs(i.normal.z));
	//fixed4 col = saturate(sampleX * i.normal.x + sampleY * i.normal.y + sampleZ * i.normal.z);
	fixed4 col = (sampleX * i.normal.x*i.normal.x + sampleY * i.normal.y*i.normal.y + sampleZ * i.normal.z*i.normal.z);
	//fixed4 col = sampleX * i.normal.x + sampleY * i.normal.y + sampleZ * i.normal.z;
	//fixed4 col = sampleX  + sampleY  + sampleZ;
	//fixed4 col;
	//col.rgb = i.normal;
	//col.rgb= i.normal * 0.5 + 0.5;
	//col.rgb = abs(i.normal);
	// apply fog
	return col;
	}
		ENDCG
	}
	}
}
