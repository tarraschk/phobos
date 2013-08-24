#include "../../CGInclude/Include.cginc"

sampler2D ringTexture;
float3    position;
float     ringRadius;
float     ringThickness;

#ifdef VARIANT_SHADOW
float4x4 shadowMatrix;
float3   umbraColour;
float3   penumbraColour;
float    shadowRatio;
float    shadowScale;
#endif

#ifdef VARIANT_LIT
float3 starDirection;
float  ringBrightness;
float  ringBrightnessRange;
#endif

#ifdef VARIANT_SCATTERING
float3 starPosition;
float4 mieValues;
float  occlusion;
#endif

struct V2F
{
	float4 vertex   : SV_POSITION;
	float4 texcoord : TEXCOORD0;
#ifdef VARIANT_SHADOW
	float3 texcoord1 : TEXCOORD1;
#endif
#if defined(VARIANT_LIT) || defined(VARIANT_SCATTERING)
	float3 texcoord2 : TEXCOORD2;
#endif
#ifdef VARIANT_SCATTERING
	float4 texcoord3 : TEXCOORD3;
#endif
};

void Vert(A2V i, out V2F o)
{
	float4 vertM     = mul(_Object2World, i.vertex);
	float3 localM    = vertM.xyz - position;
	float3 cam2vertM = _WorldSpaceCameraPos - vertM;
	
	o.vertex       = mul(UNITY_MATRIX_MVP, i.vertex);
	o.texcoord.xyz = localM;
	
#ifdef VARIANT_STRETCHED
	o.texcoord.w = 0.0f;
#else
	o.texcoord.w = i.texcoord.y;
#endif
	
#ifdef VARIANT_SHADOW
	o.texcoord1.xyz = mul(shadowMatrix, vertM).xyz;
#endif
	
#if defined(VARIANT_LIT) || defined(VARIANT_SCATTERING)
	o.texcoord2.xyz = cam2vertM;
#endif
	
#ifdef VARIANT_SCATTERING
	float3 star2vertM    = starPosition - vertM.xyz;
	//float3 star2vertMDir = normalize(star2vertM);
	
	o.texcoord3.xyz = star2vertM;
#endif
}

void Frag(V2F i, out half4 o : COLOR)
{
	float3 localM = i.texcoord.xyz;
	float  v      = i.texcoord.w;
#ifdef VARIANT_SHADOW
	float3 shadowPoint = i.texcoord1;
#endif
#if defined(VARIANT_LIT) || defined(VARIANT_SCATTERING)
	float3 cam2vertM    = i.texcoord2.xyz;
	float3 cam2vertMDir = normalize(cam2vertM);
#endif
#ifdef VARIANT_SCATTERING
	float3 star2vertMDir = normalize(i.texcoord3.xyz);
#endif
	
	float distance = length(localM);
	float u        = saturate((distance - ringRadius) / ringThickness);
	
	float4 colour = tex2D(ringTexture, float2(u, v));
	
#ifdef VARIANT_LIT
	float brightness = dot(cam2vertMDir, starDirection);
	
	colour.xyz *= ringBrightness + ringBrightnessRange * brightness;
#endif
	
#ifdef VARIANT_SCATTERING
	float starAngle = dot(cam2vertMDir, star2vertMDir);
	float mie       = MiePhase(starAngle, mieValues);
	float scattering = mie;
	
	float4 contrib = Expose(colour.w * scattering * (1.0f - pow(colour.w, occlusion)));
	
	colour += contrib;
#endif
	
#ifdef VARIANT_SHADOW
	float shadowRadius  = length(shadowPoint.xy);
	float shadow        = saturate(saturate(shadowPoint.z) * (1.0f - (shadowRadius - shadowRatio) * shadowScale));
	float3 shadowColour = lerp(penumbraColour, umbraColour, shadow);
	
	colour.xyz = lerp(colour.xyz, shadowColour, shadow);
#endif
	
	o = colour;
}