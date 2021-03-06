// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/PSX_Effect Emissive"
{
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_AOTexture("AO Texture", 2D) = "white" {}
		_AOColor("AO Color", Color) = (1, 1, 1, 1)
		_AOPower("AO Texture Power", Range(0, 3)) = 0
		_EmissiveMap("Emissive Map", 2D) = "black" {}
		[HDR] _EmissiveColor("Emissive Color", Color) = (0,0,0)
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			Pass {
			Lighting On
				CGPROGRAM

					#pragma vertex vert
					#pragma fragment frag
					#include "UnityCG.cginc"

					struct v2f
					{
						fixed4 pos : SV_POSITION;
						half4 color : COLOR0;
						half4 colorFog : COLOR1;
						float2 uv_MainTex : TEXCOORD0;
						half3 normal : TEXCOORD1;
						float2 aouv : TEXCOORD4;
					};

					float4 _MainTex_ST;
					uniform half4 unity_FogStart;
					uniform half4 unity_FogEnd;
					uniform sampler2D _AOTexture;
					uniform fixed4 _AOTexture_ST;
					uniform half3 _AOColor;
					uniform half _AOPower;

					static const half3 whiteColor = half3(1, 1, 1);

					v2f vert(appdata_full v)
					{
						v2f o;

						//Vertex snapping
						float4 snapToPixel = UnityObjectToClipPos(v.vertex);
						float4 vertex = snapToPixel;
						vertex.xyz = snapToPixel.xyz / snapToPixel.w;
						vertex.x = floor(160 * vertex.x) / 160;
						vertex.y = floor(120 * vertex.y) / 120;
						vertex.xyz *= snapToPixel.w;
						o.pos = vertex;

						//Vertex lighting 
					//	o.color =  float4(ShadeVertexLights(v.vertex, v.normal), 1.0);
						o.color = float4(ShadeVertexLightsFull(v.vertex, v.normal, 4, true), 1.0);
						o.color *= v.color;

						float distance = length(mul(UNITY_MATRIX_MV,v.vertex));

						//Affine Texture Mapping
						float4 affinePos = vertex; //vertex;				
						o.uv_MainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
						o.uv_MainTex *= distance + (vertex.w*(UNITY_LIGHTMODEL_AMBIENT.a * 8)) / distance / 2;
						o.normal = distance + (vertex.w*(UNITY_LIGHTMODEL_AMBIENT.a * 8)) / distance / 2;

						//Fog
						float4 fogColor = unity_FogColor;

						float fogDensity = (unity_FogEnd - distance) / (unity_FogEnd - unity_FogStart);
						o.normal.g = fogDensity;
						o.normal.b = 1;

						o.colorFog = fogColor;
						o.colorFog.a = clamp(fogDensity,0,1);

						//Cut out polygons
						if (distance > unity_FogStart.z + unity_FogColor.a * 255)
						{
							o.pos.w = 0;
						}
						o.aouv = TRANSFORM_TEX(o.uv_MainTex, _AOTexture);
						return o;
					}

					sampler2D _MainTex;
					sampler2D _EmissiveMap;

					float4 frag(v2f IN) : COLOR
					{
						half4 c = tex2D(_MainTex, IN.uv_MainTex / IN.normal.r)*IN.color;
						half4 AOTexVar = lerp(half4(_AOColor, 1), half4(whiteColor, 1), lerp(half4(1, 1, 1, 1), tex2D(_AOTexture, IN.aouv), _AOPower));
						half4 emission = tex2D(_EmissiveMap, IN.uv) * _EmissiveColor;
						half4 color = c*(IN.colorFog.a) * AOTexVar;
						color.rgb += IN.colorFog.rgb * (1 - IN.colorFog.a) + emission.rgb;
						return color;
					}
				ENDCG
			}
	}
}
