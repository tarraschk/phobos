#include "../../CGInclude/Include.cginc"

#if defined(VARIANT_RINGSHADOW) || defined(VARIANT_PLANETSHADOW)
 #define VARIANT_SHADOW
#endif

sampler2D dayTexture;
sampler2D nightTexture;
sampler2D lightingTexture;
float4x4  ellipsoid2sphere;
float3    centrePosition;
float3    starDirection;
float     maxDepth;
float     falloff;
#ifdef VARIANT_SHADOW
sampler2D shadowTexture;
float4x4  shadowMatrix;
float3    shadowValues;
#endif

// 3.141592654f;
// 6.283185307f;

struct V2F
{
	float4 vertex   : SV_POSITION;
	float4 texcoord : TEXCOORD0;
#ifdef VARIANT_SHADOW
	float4 texcoord1 : TEXCOORD1;
#endif
	fixed2 color : COLOR;
};

float2 SphericalUV(float3 surfaceDir)
{
	float rawU = atan2(surfaceDir.x, surfaceDir.z);
	float rawV = asin(surfaceDir.y);
	return float2(0.5f - rawU / 6.283185307f, 0.5f + rawV / 3.141592654f);
}

float3 RemoveOblateness(float3 pos)
{
	return mul(ellipsoid2sphere, float4(pos, 1.0f));
}

float Grayscale(float3 colour)
{
	return dot(colour, float3(0.3f, 0.59f, 0.11f));
}

void Vert(A2V i, out V2F o)
{
	float3 oblateCameraM = _WorldSpaceCameraPos;
	float4 oblateVertM   = mul(_Object2World, i.vertex);
	float3 cam2vertM     = RemoveOblateness(oblateVertM - oblateCameraM);
	float3 cam2vertMDir  = normalize(cam2vertM);
	float3 localM        = RemoveOblateness(oblateVertM - centrePosition);
	float3 localMDir     = normalize(localM);
	float  localMLen     = length(localM);
	float3 vertM         = localM + centrePosition;
	
#ifdef VARIANT_OUTER
	float3 nearM = reflect(localM, cam2vertMDir);
#else
	float3 nearM = RemoveOblateness(oblateCameraM - centrePosition);
#endif
	
	float3 nearMDir = normalize(nearM);
	float3 nearO    = mul(_World2Object, float4(nearM + centrePosition, 1.0f));
	
	float3 ray = localM - nearM;
	
	nearO = normalize(nearO);
	
	o.vertex      = mul(UNITY_MATRIX_MVP, i.vertex);
	o.texcoord.xz = nearO.xz;
	o.texcoord.y  = nearO.y;
	o.texcoord.w  = (length(ray) / maxDepth);
	o.color.x     = dot(starDirection, nearMDir) * 0.5f + 0.5f;
	o.color.y     = 1.0f - dot(localMDir, cam2vertMDir);
	
#ifdef VARIANT_SHADOW
	float3 shadowM = mul(shadowMatrix, float4(nearM, 1.0f)).xyz;
 #ifdef VARIANT_RINGSHADOW
	shadowM.xy *= (shadowM.y > 0.0f);
 #endif
 #ifdef VARIANT_PLANETSHADOW
	shadowM.z = (shadowM.z > 0.0f) * shadowValues.z;
 #endif
	o.texcoord1.xyz = shadowM.xyz;
#endif
}

void Frag(V2F i, out half4 o : COLOR)
{
	float2 surfaceUV  = SphericalUV(i.texcoord.xyz);
	float2 lightingUV = i.color.xy;
#ifdef VARIANT_SHADOW
	float3 shadowM = i.texcoord1.xyz;
#endif
	
	float3 day      = tex2D(dayTexture, surfaceUV).xyz;
	float3 night    = tex2D(nightTexture, surfaceUV).xyz;
	float3 lighting = tex2D(lightingTexture, lightingUV).xyz;
	
	float4 surfaceColour;
	surfaceColour.xyz = lerp(night, day, lighting);
	surfaceColour.w   = 1.0f;
	
#ifdef VARIANT_SHADOW
	float2 shadowUV = (length(shadowM.xy) - shadowValues.x) * shadowValues.y;
 #ifdef VARIANT_PLANETSHADOW
	shadowUV += shadowM.z;
 #endif
	surfaceColour.xyz *= tex2D(shadowTexture, shadowUV).xyz;
#endif
	
	float e = saturate(Expose(i.texcoord.w));
	surfaceColour.w = pow(e, falloff);
	
	o = surfaceColour;
}