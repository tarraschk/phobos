#include "../../CGInclude/Include.cginc"

#if defined(VARIANT_RINGSHADOW) || defined(VARIANT_PLANETSHADOW)
 #define VARIANT_SHADOW
#endif

sampler2D atmosphereTexture;
float3    centrePosition;
float3    starDirection;
float     atmosphereFalloff;
float     maxDepth;
#ifdef VARIANT_SCATTERING
float3 starPosition;
float4 mieValues;
float2 rayleighValues;
#endif
#ifdef VARIANT_SHADOW
sampler2D shadowTexture;
float4x4  shadowMatrix;
float3    shadowValues;
#endif

struct V2F
{
	float4 vertex   : SV_POSITION;
	float2 texcoord : TEXCOORD0;
#ifdef VARIANT_SCATTERING
	float4 texcoord1 : TEXCOORD1;
	float4 texcoord2 : TEXCOORD2;
#endif
#ifdef VARIANT_SHADOW
	float4 texcoord3 : TEXCOORD3;
#endif
	fixed color : COLOR;
};

void Vert(A2V i, out V2F o)
{
	float4 vertM        = mul(_Object2World, i.vertex);
	float3 cam2vertM    = _WorldSpaceCameraPos - vertM.xyz;
	float3 cam2vertMDir = normalize(cam2vertM);
	float3 localM       = vertM.xyz - centrePosition;
	float3 localMDir    = normalize(localM);
	
#ifdef VARIANT_OUTER
	float3 nearM = reflect(localM, cam2vertMDir);
#else
	float3 nearM = _WorldSpaceCameraPos - centrePosition;
#endif
	
	float3 nearMDir    = normalize(nearM);
	float  depth       = length(localM - nearM);
	float  scaledDepth = depth / maxDepth;
	float  brightnessL = dot(starDirection, localMDir);
	float  brightnessN = dot(starDirection, nearMDir);
	
	o.vertex     = mul(UNITY_MATRIX_MVP, i.vertex);
	o.texcoord.x = scaledDepth;
	o.texcoord.y = pow(scaledDepth, atmosphereFalloff);
	o.color.x    = brightnessN * 0.5f + 0.5f;
	
#ifdef VARIANT_SCATTERING
	float3 star2vertM    = starPosition - vertM.xyz;
	float3 star2vertMDir = normalize(star2vertM);
	
	o.texcoord1.xyz = cam2vertM;
	o.texcoord2.xyz = star2vertMDir;
#endif
	
#ifdef VARIANT_SHADOW
	float3 shadowM = mul(shadowMatrix, vertM).xyz;
 #ifdef VARIANT_RINGSHADOW
	shadowM.xy *= (shadowM.y > 0.0f);
 #endif
 #ifdef VARIANT_PLANETSHADOW
	shadowM.z = (shadowM.z > 0.0f) * shadowValues.z;
 #endif
	o.texcoord3.xyz = shadowM.xyz;
#endif
}

float Grayscale(float3 colour)
{
	return colour.x * 0.3f + colour.y * 0.59f + colour.z * 0.11f;
}

void Frag(V2F i, out half4 o : COLOR)
{
	float  scaledDepth  = i.texcoord.x;
	float  opticalDepth = saturate(i.texcoord.y);
	float  brightness   = i.color.x;
#ifdef VARIANT_SCATTERING
	float3 cam2vertMDir  = normalize(i.texcoord1.xyz); // This interpolates poorly in the vertex shader when close to the edge
	float3 star2vertMDir = i.texcoord2.xyz;
#endif
#ifdef VARIANT_SHADOW
	float3 shadowM = i.texcoord3.xyz;
#endif
	
	float4 atmosphereColour = tex2D(atmosphereTexture, float2(brightness, scaledDepth));
	
	atmosphereColour.w *= opticalDepth;
	
#ifdef VARIANT_SCATTERING
	float starAngle  = dot(cam2vertMDir, star2vertMDir);
	float mie        = MiePhase(starAngle, mieValues);
	float rayleigh   = RayleighPhase(starAngle * starAngle, rayleighValues);
	float scattering = rayleigh + mie;
	
	float4 contrib = Expose(atmosphereColour.w * atmosphereColour * scattering);
	
	atmosphereColour += contrib * (1.0f - atmosphereColour);
#endif

#ifdef VARIANT_SHADOW
	float2 shadowUV = (length(shadowM.xy) - shadowValues.x) * shadowValues.y;
 #ifdef VARIANT_PLANETSHADOW
	shadowUV += shadowM.z;
 #endif
	atmosphereColour.xyz *= tex2D(shadowTexture, shadowUV).xyz;
#endif
	
	o = atmosphereColour;
}