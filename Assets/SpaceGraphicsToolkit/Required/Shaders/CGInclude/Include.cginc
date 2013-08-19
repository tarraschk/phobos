#include "UnityCG.cginc"

struct A2V
{
	float4 vertex    : POSITION;
	float4 tangent   : TANGENT;
	float3 normal    : NORMAL;
	float2 texcoord  : TEXCOORD0;
	float2 texcoord1 : TEXCOORD1;
	fixed4 color     : COLOR;
};

// struct V2F
// {
// 	float4 vertex    : SV_POSITION;
// 	float4 texcoord  : TEXCOORD0;
// 	float4 texcoord1 : TEXCOORD1;
// 	float4 texcoord2 : TEXCOORD2;
// 	float4 texcoord3 : TEXCOORD3;
// 	float4 texcoord4 : TEXCOORD4;
// 	float4 texcoord5 : TEXCOORD5;
// 	float4 texcoord6 : TEXCOORD6;
// 	float4 texcoord7 : TEXCOORD7;
// 	fixed4 color     : COLOR;
// 	fixed4 color1    : COLOR1;
// };

float4 Expose(float4 from)
{
	return 1.0f - exp(-from);
}

float3 Expose(float3 from)
{
	return 1.0f - exp(-from);
}

float2 Expose(float2 from)
{
	return 1.0f - exp(-from);
}

float Expose(float from)
{
	return 1.0f - exp(-from);
}

float3 IntersectUnitSphere(float3 ray, float3 rayD)
{
	float B = dot(ray, rayD);
	float C = dot(ray, ray) - 1.0f;
	float D = B * B - C;
	return ray + (-B - sqrt(D)) * rayD;
}

float RayleighPhase(float angleSq, float2 rayleigh)
{
	//return rayleigh.x + rayleigh.y * angleSq;
	return rayleigh.y * angleSq;
}

// x = g * 2
// y = 1 - g * g
// z = 1 + g * g
// w = 1.5
float MiePhase(float angle, float4 mie)
{
    return mie.y / pow(mie.z - mie.x * angle, mie.w);
}