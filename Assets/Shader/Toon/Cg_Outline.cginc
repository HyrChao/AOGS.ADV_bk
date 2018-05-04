//2018-05-03 11:34:29
//Based on UnityChain
//Modified by Chao

//--------------------------------------------------------------------------------
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// UCTS_Outline.cginc
// 2017/03/08 N.Kobayashi (Unity Technologies Japan)
// カメラオフセット付きアウトライン（BaseColorライトカラー反映修正版）
// 2017/06/05 PS4対応版
//-----------------------------------------------------------------------------------


            uniform float4 _LightColor0;//Uinty build-in variables
            uniform float4 _BaseColor;
            uniform sampler2D _BaseMap; uniform float4 _BaseMap_ST;
            uniform float _Outline_Width;
            uniform float _Farthest_Distance;
            uniform float _Nearest_Distance;
            uniform sampler2D _Outline_Sampler; uniform float4 _Outline_Sampler_ST;
            uniform float4 _Outline_Color;
            uniform fixed _Is_BlendOutlineWithBaseColor;
            uniform fixed _Is_OutlineReceiveLightColor;
            uniform float _Offset_Depth;

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
                float3 baseColor = lerp( col, (col*_LightColor0.rgb), _Is_OutlineReceiveLightColor);
                float3 outputColor = lerp( _Outline_Color.rgb, (_Outline_Color.rgb*baseColor*baseColor), _Is_BlendOutlineWithBaseColor );
                return fixed4(outputColor,0);
            }
// UCTS_Outline.cginc ここまで.
