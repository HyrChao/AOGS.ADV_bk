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
