Shader "AO/UITest_unlit"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex ("Noise", 2D) = "white" {}
		_Strenghth("Strenghth", float) = 0.2
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
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
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 worldPosition : VERTEXWORDPOSITION;
			};

			sampler2D _MainTex;
			sampler2D _NoiseTex;
			float4 _MainTex_ST;
			float _Strenghth;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.worldPosition = v.vertex;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 offset = float2 (
					tex2D(_NoiseTex , float2 (i.worldPosition.y/60 , 0)).r,
					tex2D(_NoiseTex , float2 (0, i.worldPosition.x/60)).r
					);

				offset -= 0.5;
				//offset /= 2;

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv + offset* _Strenghth);
				// apply fog

				UNITY_APPLY_FOG(i.fogCoord, col);
				//col.r = 0.7;
				return col;
			}
			ENDCG
		}
	}
}
