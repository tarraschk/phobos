#include "../../CGInclude/Include.cginc"

#define VARIANT_CHECKELEVATION

sampler2D surfaceTexture;
sampler2D atmosphereTexture;
float4x4  ellipsoid2sphere;
float3    centrePosition;
float     atmosphereRadius;
float     atmosphereFalloff;
float     maxDepth;
#ifdef VARIANT_CHECKELEVATION
float surfaceRadius;
float atmosphereHeight;
#endif

struct V2F
{
	float4 vertex   : SV_POSITION;
	float2 texcoord : TEXCOORD0;
#ifdef VARIANT_PERPIXEL
	float3 texcoord1 : TEXCOORD1;
#endif
#ifdef VARIANT_PERPIXEL
#else
	fixed2 color : COLOR;
#endif
#ifdef VARIANT_CHECKELEVATION
	fixed color1 : COLOR1;
#endif
};

float3 RemoveOblateness(float3 pos)
{
	return mul(ellipsoid2sphere, float4(pos, 1.0f));
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
	float3 nearM = IntersectUnitSphere(localM / atmosphereRadius, cam2vertMDir) * atmosphereRadius;
#else
	float3 nearM = RemoveOblateness(oblateCameraM - centrePosition);
#endif
	
	o.vertex      = mul(UNITY_MATRIX_MVP, i.vertex);
	o.texcoord.xy = i.texcoord;
	
	float3 ray = localM - nearM;
	
#ifdef VARIANT_CHECKELEVATION
	float elevation = 1.0f - (localMLen - surfaceRadius) / atmosphereHeight;
	
	o.color1.x = elevation;
#endif
	
#ifdef VARIANT_PERPIXEL
	o.texcoord1.xyz = ray;
#else
	float scaledDepth  = length(ray) / maxDepth;
	float opticalDepth = pow(scaledDepth, atmosphereFalloff);
	
	o.color.x = opticalDepth;
	o.color.y = scaledDepth;
#endif
}

void Frag(V2F i, out half4 o : COLOR)
{
	float2 surfaceUV = i.texcoord.xy;
#ifdef VARIANT_CHECKELEVATION
	float elevation = i.color1.x;
#endif
#ifdef VARIANT_PERPIXEL
	float3 ray          = i.texcoord1.xyz;
	float  scaledDepth  = saturate(length(ray) / maxDepth);
	float  opticalDepth = pow(scaledDepth, atmosphereFalloff);
#else
	float opticalDepth = i.color.x;
	float scaledDepth  = i.color.y;
#endif
	
#ifdef VARIANT_CHECKELEVATION
	opticalDepth *= elevation;
#endif
	
	float2 atmosphereUV     = float2(opticalDepth, opticalDepth);
	float3 surface          = tex2D(surfaceTexture, surfaceUV);
	float4 atmosphereColour = tex2D(atmosphereTexture, atmosphereUV);
	
	o.xyz = lerp(surface, atmosphereColour.xyz, atmosphereColour.w * opticalDepth);
	o.w   = 1.0f;
}