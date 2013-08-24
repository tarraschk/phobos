#include "../../CGInclude/Include.cginc"

#define VARIANT_CHECKELEVATION

// Flash has less temp registers than other platforms, so I had to disable some features to get this to compile without warnings. You can change which features are disabled here.
#if SHADER_API_FLASH
 #ifdef VARIANT_NORMAL
  #undef VARIANT_NORMAL
 #endif
 #ifdef VARIANT_SPECULAR
  #undef VARIANT_SPECULAR
 #endif
 #ifdef VARIANT_RINGSHADOW
  #undef VARIANT_RINGSHADOW
 #endif
 #ifdef VARIANT_PLANETSHADOW
  #undef VARIANT_PLANETSHADOW
 #endif
#endif

#if defined(VARIANT_RINGSHADOW) || defined(VARIANT_PLANETSHADOW)
 #define VARIANT_SHADOW
#endif

sampler2D dayTexture;
sampler2D nightTexture;
sampler2D lightingTexture;
float3    centrePosition;
float3    starDirection;

#ifdef VARIANT_ATMOSPHERE
sampler2D atmosphereTexture;
float     atmosphereRadius;
float     atmosphereFalloff;
float     maxDepth;
 #ifdef VARIANT_CHECKELEVATION
float surfaceRadius;
float atmosphereHeight;
 #endif
 #ifdef VARIANT_SCATTERING
float3 starPosition;
float2 rayleighValues;
 #endif
#endif
#ifdef VARIANT_NORMAL
sampler2D normalTexture;
float3    starDirectionM;
#endif
#ifdef VARIANT_SPECULAR
sampler2D specularTexture;
float     specularPower;
#endif
#ifdef VARIANT_SHADOW
sampler2D shadowTexture;
float4x4  shadowMatrix;
float3    shadowValues;
#endif
#ifdef VARIANT_DETAIL
sampler2D detailTexture;
float     detailRepeat;
#endif

struct V2F
{
	float4 vertex    : SV_POSITION;
	float4 texcoord  : TEXCOORD0;
	float3 texcoord2 : TEXCOORD2;
#ifdef VARIANT_ATMOSPHERE
 #ifdef VARIANT_SCATTERING
	float3 texcoord3 : TEXCOORD3;
 #endif
#endif
#ifdef VARIANT_NORMAL
	float3 texcoord4 : TEXCOORD4;
#endif
#ifdef VARIANT_SHADOW
	float3 texcoord5 : TEXCOORD5;
#endif
#ifdef VARIANT_SPECULAR
	float4 texcoord6 : TEXCOORD6;
#endif
#ifdef VARIANT_ATMOSPHERE
 #ifdef VARIANT_CHECKELEVATION
	fixed4 color : COLOR;
 #else
	fixed3 color : COLOR;
 #endif
#else
	fixed2 color : COLOR;
#endif
};

void Vert(A2V i, out V2F o)
{
	float4 vertM        = mul(_Object2World, i.vertex);
	float3 cam2vertM    = _WorldSpaceCameraPos - vertM.xyz;
	float3 cam2vertMDir = normalize(cam2vertM);
	float3 localM       = vertM.xyz - centrePosition;
	float3 localMDir    = normalize(localM);
	float  localMLen    = length(localM);
	float3 normalM      = mul((float3x3)_Object2World, i.normal);
	float  brightnessL  = dot(starDirection, normalM * unity_Scale.w);
	
	o.vertex      = mul(UNITY_MATRIX_MVP, i.vertex);
	o.texcoord.xy = i.texcoord;
	
#ifdef VARIANT_ATMOSPHERE
 #ifdef VARIANT_CHECKELEVATION
	float elevation = 1.0f - (localMLen - surfaceRadius) / atmosphereHeight;
	
	o.color.w = elevation;
 #endif
	
 #ifdef VARIANT_OUTER
	float3 nearM = IntersectUnitSphere(localM / atmosphereRadius, -cam2vertMDir) * atmosphereRadius;
 #else
	float3 nearM = _WorldSpaceCameraPos - centrePosition;
 #endif
	
	float3 ray      = localM - nearM;
	float3 nearMDir = normalize(nearM);
	
	float scaledDepth  = length(ray) / maxDepth;
	float opticalDepth = pow(scaledDepth, atmosphereFalloff);
	
	o.color.z = opticalDepth;
	
	float  brightnessN = dot(starDirection, nearMDir);
	float2 brightness  = float2(brightnessL, brightnessN);
	
 #ifdef VARIANT_SCATTERING
	float3 star2vertM    = starPosition - vertM.xyz;
	float3 star2vertMDir = normalize(star2vertM);
	
	o.texcoord3.xyz = star2vertMDir;
 #endif
#else
	float2 brightness = brightnessL;
#endif
	o.texcoord2.xyz = cam2vertMDir;
	o.color.xy = brightness * 0.5f + 0.5f;
	
#ifdef VARIANT_NORMAL
	float3   binormal = cross(i.normal, i.tangent.xyz) * i.tangent.w;
	float3x3 rotation = float3x3(i.tangent.xyz, binormal, i.normal);
	
	o.texcoord4.xyz = mul(rotation, starDirectionM);
#endif
	
#ifdef VARIANT_SPECULAR
	float3 reflectedStarDir = reflect(-starDirection, localMDir);
	
	o.texcoord6.xyz = reflectedStarDir;
#endif
	
#ifdef VARIANT_SHADOW
	float3 shadowM = mul(shadowMatrix, vertM).xyz;
 #ifdef VARIANT_RINGSHADOW
		shadowM.xy *= (shadowM.y > 0.0f);
 #endif
 #ifdef VARIANT_PLANETSHADOW
		shadowM.z = (shadowM.z > 0.0f) * shadowValues.z;
 #endif
	o.texcoord5.xyz = shadowM.xyz;
#endif

#ifdef VARIANT_DETAIL
	o.texcoord.zw = i.texcoord1.xy * detailRepeat;
#endif
}

inline float3 UnpackNormal2(float4 packednormal)
{
#if defined(SHADER_API_GLES) && defined(SHADER_API_MOBILE)
	return packednormal.xyz * 2 - 1;
#else
	float3 normal;
	normal.xy = packednormal.wy * 2 - 1;
	normal.z = sqrt(1 - normal.x*normal.x - normal.y * normal.y);
	return normal;
#endif
}

void Frag(V2F i, out half4 o : COLOR)
{
	float2 surfaceUV    = i.texcoord.xy;
	float2 brightnessUV = i.color.xy; // Surface brightness in x. Atmosphere brightness in y.
	float3 cam2vertMDir  = normalize(i.texcoord2.xyz);
#ifdef VARIANT_ATMOSPHERE
 #ifdef VARIANT_CHECKELEVATION
	float elevation = i.color.w;
 #endif
	float opticalDepth = i.color.z;
 #ifdef VARIANT_SCATTERING
	float3 star2vertMDir = i.texcoord3.xyz;
 #endif
#endif
#ifdef VARIANT_NORMAL
	float3 starDirTS = i.texcoord4.xyz;
#endif
#ifdef VARIANT_SPECULAR
	float3 reflectedStarDir = normalize(i.texcoord6.xyz);
	float  specular = saturate(dot(reflectedStarDir, cam2vertMDir));
	
	specular = pow(specular, specularPower);
	
	//float specular = i.color1.x;
#endif
#ifdef VARIANT_SHADOW
	float3 shadowM = i.texcoord5.xyz;
#endif
#ifdef VARIANT_DETAIL
	float2 detailUV = i.texcoord.zw;
#endif
	
	float4 day   = tex2D(dayTexture, surfaceUV);
	float4 night = tex2D(nightTexture, surfaceUV);
	
#ifdef VARIANT_NORMAL
	float3 normal     = UnpackNormal2(tex2D(normalTexture, surfaceUV));
	float  brightness = dot(normal, starDirTS) * 0.5f + 0.5f;
	//day.xyz *= tex2D(lightingTexture, brightness.xx).xyz;
	brightnessUV.x = brightness;
#endif
	
	float4 lighting = tex2D(lightingTexture, brightnessUV);
	
#ifdef VARIANT_SHADOW
	float2 shadowUV = (length(shadowM.xy) - shadowValues.x) * shadowValues.y;
 #ifdef VARIANT_PLANETSHADOW
	shadowUV += shadowM.z;
 #endif
	float shadow = tex2D(shadowTexture, shadowUV).xyz;
	lighting.xyz *= shadow;
#endif
	
#ifdef VARIANT_SPECULAR
	float3 specularMask = tex2D(specularTexture, surfaceUV).xyz - 0.5f;
	specularMask = saturate(specularMask * 10.0f + 0.5f);
	
	day.xyz += specular * specularMask;
#endif
	
	float4 finalColour = lerp(night, day, lighting);
	
#ifdef VARIANT_DETAIL
	float3 detail = tex2D(detailTexture, detailUV).xyz;
	
 #ifdef VARIANT_SPECULAR
	detail = saturate(detail + specularMask);
 #endif
	
	finalColour.xyz *= detail;
#endif
	
#ifdef VARIANT_ATMOSPHERE
 #ifdef VARIANT_CHECKELEVATION
	//opticalDepth = saturate(opticalDepth * elevation);
	opticalDepth = opticalDepth * elevation;
 #endif
	
	float2 atmosphereUV     = float2(brightnessUV.y, opticalDepth);
	float4 atmosphereColour = tex2D(atmosphereTexture, atmosphereUV);
	
	atmosphereColour.w *= opticalDepth;
	
 #ifdef VARIANT_SHADOW
	atmosphereColour.w *= shadow;
 #endif
	
 #ifdef VARIANT_SCATTERING
	float starAngle  = dot(cam2vertMDir, star2vertMDir);
	float rayleigh   = RayleighPhase(starAngle * starAngle, rayleighValues);
	float scattering = rayleigh;
	
	float4 contrib = atmosphereColour.w * atmosphereColour * scattering;
	
	atmosphereColour += contrib * (1.0f - atmosphereColour);
 #endif
	
	finalColour.xyz = lerp(finalColour.xyz, atmosphereColour.xyz, atmosphereColour.w).xyz;
#endif
	
	o = (half4)finalColour;
}