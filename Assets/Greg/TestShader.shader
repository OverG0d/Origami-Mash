// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

/*
	POSITION is the vertex position, typically a float3 or float4.
	NORMAL is the vertex normal, typically a float3.
	TEXCOORD0 is the first UV coordinate, typically float2, float3 or float4.
	TEXCOORD1, TEXCOORD2 and TEXCOORD3 are the 2nd, 3rd and 4th UV coordinates, respectively.
	TANGENT is the tangent vector (used for normal mapping), typically a float4.
	COLOR is the per-vertex color, typically a float4.
*/

//need to map the distance of the point and the circle

Shader "Unlit/TestShader"
{
	Properties{
	_HoleColor("HoleColor", Color) = (1,1,1,1)
	_Color("Color", Color) = (1,1,1,1)
	_Radius("Radius", Float) = 1
	_Mid("Midpoint", Vector) = (0,0,0,0)
	_Width("Width", Range(0,1)) = 0.1
	_Amount("Amount", Range(0,20)) = 1
	}
		SubShader{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

			Pass {

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				#include "AutoLight.cginc"

				struct interp {
					float4 pos : SV_POSITION;
					float3 objPos : TEXCOORD0; //local position
				};

				float _Radius;
				float3 _Mid;
				float _Width;
				fixed4 _Color;
				fixed4 _HoleColor;
				float _Amount;
				float circle;
				
				interp vert(appdata_base v) {
					/*interp i;
					i.pos = UnityObjectToClipPos(v.vertex);
					i.objPos = v.vertex;
					circle = pow(i.objPos.x - _Mid.x, 2) + pow(i.objPos.z - _Mid.z, 2);
					if (circle <= pow(_Radius, 2))
					{
						i.pos.y *= _Amount;
					}*/

					interp i;
					circle = pow(v.vertex.x - _Mid.x, 2) + pow(v.vertex.z - _Mid.z, 2);
					float yOffset = 0;
					if (circle <= pow(_Radius, 2)) {
						yOffset = _Amount;
					}

					i.objPos = v.vertex + float3(0, yOffset, 0); // assuming you want this as the offset pos
					i.pos = UnityObjectToClipPos(i.objPos); // otherwise, do the offset here instead (i.objPos + float3(0, yOffset, 0))
					return i;
				}

				float4 frag(interp i) : SV_Target 
				{
					circle = pow(i.objPos.x - _Mid.x, 2) + pow(i.objPos.z - _Mid.z, 2);
					
					if (circle <= pow(_Radius, 2) && circle >= pow(_Radius - _Width, 2))
					{
						return _Color;
					}
					else
						return _HoleColor * float4(i.objPos + 0.5f , 1);
				}
				ENDCG
			}
	}
		FallBack "Diffuse"
}
