#include "../../CGInclude/Include.cginc"

sampler2D dayTexture;
sampler2D nightTexture;
sampler2D bumpTexture;
float4    starPositionRaw;
float3    centrePosition;
float     ringHeight;
#ifdef VARIANT_SHADOW
float4x4 shadowMatrix;
float3   umbraColour;
float3   penumbraColour;
float    shadowRatio;
float    shadowScale;
#endif
#ifdef VARIANT_SPIN
float spinRateMax;
#endif

struct V2F
{
	float4 vertex    : SV_POSITION;
	float2 texcoord  : TEXCOORD0;
	float3 texcoord1 : TEXCOORD1;
#ifdef VARIANT_SHADOW
	float3 texcoord2 : TEXCOORD2;
#endif
#ifdef VARIANT_NORMALMAP
#else
	float3 texcoord3 : TEXCOORD3;
#endif
};

float2 Rotate(float2 v, float a)
{
	float s = sin(a);
	float c = cos(a);
	
	float2 o;
	
	o.x = c * v.x - s * v.y;
	o.y = s * v.x + c * v.y;
	
	return o;
}

void Vert(A2V i, out V2F o)
{
#ifdef VARIANT_SPIN
	i.normal.xy = Rotate(i.normal.xy, (i.color.y - 0.5f) * _Time.y * spinRateMax);
#endif
	float  orbitAngle    = i.vertex.x;
	float  orbitDistance = i.vertex.y;
	float  orbitSpeed    = i.vertex.z;
	float3 offsetMV      = i.normal * i.texcoord1.y / unity_Scale.w;
	float  radius        = i.texcoord1.x / unity_Scale.w;
	float  height        = (i.color.x - 0.5f) * ringHeight;
	
	orbitAngle = orbitAngle + _Time.y * orbitSpeed;
	
	// Reconstruct vertex position
	float4 i_vertex;
	i_vertex.x = sin(orbitAngle) * orbitDistance;
	i_vertex.y = height;
	i_vertex.z = cos(orbitAngle) * orbitDistance;
	i_vertex.w = 1.0f;
	
	float4 vertM     = mul(_Object2World, i_vertex);
	float3 localM    = vertM.xyz - centrePosition;
	float4 vertexMV  = mul(UNITY_MATRIX_MV, i_vertex);
	float4 cornerMV  = vertexMV; cornerMV.xyz += offsetMV;
	float4 cornerMVP = mul(UNITY_MATRIX_P, cornerMV);
	
	o.vertex      = cornerMVP;
	o.texcoord.xy = i.texcoord.xy;
	
#ifdef VARIANT_SHADOW
	float3 shadowPoint = mul(shadowMatrix, vertM).xyz;
	o.texcoord2.xy = shadowPoint.xy;
	o.texcoord2.z  = saturate(shadowPoint.z);
#endif
	
	float3 starPosMV  = mul(UNITY_MATRIX_MV, starPositionRaw).xyz;
	float3 lightVecMV = starPosMV - vertexMV.xyz;
	float3 lightDirMV = normalize(lightVecMV);
	
#ifdef VARIANT_NORMALMAP
	// TODO: Fix rotation matrix
	float3   normal   = float3(0, 0, 1);
	float3   tangent  = float3(1, 0, 0);
	float3   binormal = float3(0, 1, 0);
	float3x3 rotation = float3x3(tangent, binormal, normal);
	
	lightDirMV = mul(rotation, lightDirMV);
#else
	float3 offset = (cornerMV - vertexMV) / radius;
	o.texcoord3.xyz = offset;
#endif
	
	o.texcoord1.xyz = lightDirMV;
}

void Frag(V2F i, out half4 o : COLOR)
{
	float2 surfaceUV  = i.texcoord.xy;
	float3 lightDirMV = i.texcoord1.xyz;
#ifdef VARIANT_SHADOW
	float3 shadowPoint = i.texcoord2.xyz;
#endif
#ifndef VARIANT_NORMALMAP
	float3 offset = i.texcoord3.xyz;
#endif
	
	float4 day      = tex2D(dayTexture, surfaceUV); clip(day.a - 0.5f);
	float3 night    = tex2D(nightTexture, surfaceUV).xyz;
	float3 lighting = float3(1.0f, 1.0f, 1.0f);
	
#ifdef VARIANT_SHADOW
	float  shadowRadius = length(shadowPoint.xy);
	float  shadow       = saturate(shadowPoint.z * (1.0f - (shadowRadius - shadowRatio) * shadowScale));
	float3 shadowColour = lerp(penumbraColour, umbraColour, shadow);
	
	lighting = lerp(lighting, shadowColour, shadow);
#endif
	
#ifdef VARIANT_NORMALMAP
	float3 dir = UnpackNormal(tex2D(bumpTexture, surfaceUV));
#else
	offset.z += tex2D(bumpTexture, surfaceUV);
	float3 dir = normalize(offset);
#endif
	
	lighting *= saturate(dot(dir, lightDirMV));
	
	o.xyz = lerp(night, day.xyz, lighting);
	o.w   = day.w;
}