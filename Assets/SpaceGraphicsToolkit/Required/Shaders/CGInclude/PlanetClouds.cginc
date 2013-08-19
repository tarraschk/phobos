#include "../../CGInclude/Include.cginc"

#if defined(VARIANT_RINGSHADOW) || defined(VARIANT_PLANETSHADOW)
 #define VARIANT_SHADOW
#endif

sampler2D cloudsTexture;
sampler2D lightingTexture;
float3    centrePosition;
float3    starDirection;
float     falloff;
#ifdef VARIANT_SHADOW
sampler2D shadowTexture;
float4x4  shadowMatrix;
float3    shadowValues;
#endif

struct V2F
{
	float4 vertex   : SV_POSITION;
	float4 texcoord : TEXCOORD0;
#ifdef VARIANT_SHADOW
	float4 texcoord1 : TEXCOORD1;
#endif
	fixed3 color     : COLOR;
};

void Vert(A2V i, out V2F o)
{
	float4 vertM        = mul(_Object2World, i.vertex);
	float3 cam2vertM    = _WorldSpaceCameraPos - vertM.xyz;
	float3 cam2vertMDir = normalize(cam2vertM);
	float3 localM       = vertM.xyz - centrePosition;
	float3 localMDir    = normalize(localM);
	
	float brightness = dot(starDirection, localMDir) * 0.5f + 0.5f;
	float limb       = 1.0f - dot(localMDir, cam2vertMDir);
	
	o.vertex      = mul(UNITY_MATRIX_MVP, i.vertex);
	o.texcoord.xy = i.texcoord;
	o.color.x     = brightness;
	o.color.y     = limb;
	o.color.z     = 1.0f - pow(limb, falloff);
	
#ifdef VARIANT_SHADOW
	float3 shadowM = mul(shadowMatrix, vertM).xyz;
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
	float2 cloudsUV     = i.texcoord.xy;
	float2 brightnessUV = i.color.xy;
#ifdef VARIANT_SHADOW
	float3 shadowM = i.texcoord1.xyz;
#endif
	
	float4 clouds   = tex2D(cloudsTexture, cloudsUV);
	float4 lighting = tex2D(lightingTexture, brightnessUV);
	
#ifdef VARIANT_SHADOW
	float2 shadowUV = (length(shadowM.xy) - shadowValues.x) * shadowValues.y;
 #ifdef VARIANT_PLANETSHADOW
	shadowUV += shadowM.z;
 #endif
	clouds.xyz *= tex2D(shadowTexture, shadowUV).xyz;
#endif
	
	clouds.w *= i.color.z;
	
	o = clouds * lighting;
}