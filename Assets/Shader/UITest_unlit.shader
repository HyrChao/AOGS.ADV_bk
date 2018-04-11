Shader "AO/UITest_unlit"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex ("Noise", 2D) = "white" {}
		_DistortionStrenghth("Distortion Strenghth", float) = 0.2
		_DistortionSpread("Distortion Spread" , float) = 60
		_TimeDamper("Time Damper", float) = 50
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
			float _DistortionStrenghth;
			float _DistortionSpread;
			float _TimeDamper;
			
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
					tex2D(_NoiseTex , float2 (i.worldPosition.y/ _DistortionSpread , _Time[1]/_TimeDamper)).r,
					tex2D(_NoiseTex , float2 (_Time[1]/_TimeDamper, i.worldPosition.x/ _DistortionSpread)).r
					);

				offset -= 0.5;
				//offset /= 2;

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv + offset* _DistortionStrenghth);
				// apply fog

				UNITY_APPLY_FOG(i.fogCoord, col);
				//col.r = 0.7;
				return col;
			}
			ENDCG
		}
	}
}
