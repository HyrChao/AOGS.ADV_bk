//2018-04-27 14:05:40
//By Chao

Shader "AO/Environment_Pbr_Worldproj" {
	
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_ProjTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types

		#pragma vertex vert
		#pragma fragment frag
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		fixed4 _Color;
		sampler2D _ProjTex;
		half _Glossiness;
		half _Metallic;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)
		
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

		struct f2s 
		{
			float2 uv : TEXCOORD0;
			fixed4 col : COLOR;
		};

		sampler2D _ProjTex;
		//float4 _Color_ST;

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			float3 worldPosition = mul(unity_ObjectToWorld, v.vertex).xyz;
			o.normal = UnityObjectToWorldNormal(v.normal);
			o.coodX.x = worldPosition.y;
			o.coodX.y = worldPosition.z;
			o.coodY.x = worldPosition.x;
			o.coodY.y = worldPosition.z;
			o.coodZ.x = worldPosition.x;
			o.coodZ.y = worldPosition.y;
			return o;
		}

		f2s frag(v2f i) : SV_Target
		{
			// sample the texture
			f2s o;
			fixed4 sampleX = tex2D(_Color, float2(i.coodX.x, 1 - i.coodX.y));
			fixed4 sampleY = tex2D(_Color, float2(i.coodY.x, 1 - i.coodY.y));
			fixed4 sampleZ = tex2D(_Color, float2(i.coodZ.x, 1 - i.coodZ.y));

			o.col = saturate(sampleX * i.normal.x*i.normal.x + sampleY * i.normal.y*i.normal.y + sampleZ * i.normal.z*i.normal.z);
			o.uv = i.uv;
			return o;
		}

		void surf (f2s i, inout SurfaceOutputStandard o) 
		{
			// Albedo comes from a texture tinted by color
			o.Albedo = i.col;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG

	}
	FallBack "Diffuse"
}
