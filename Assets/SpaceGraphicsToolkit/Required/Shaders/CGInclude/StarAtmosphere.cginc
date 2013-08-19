#include "../../CGInclude/Include.cginc"

sampler2D atmosphereTexture;
float4x4  ellipsoid2sphere;
float3    centrePosition;
float     atmosphereFalloff;
float     maxDepth;

struct V2F
{
	float4 vertex    : SV_POSITION;
	float3 texcoord  : TEXCOORD0;
};

float3 RemoveOblateness(float3 pos)
{
	return mul(ellipsoid2sphere, float4(pos, 1.0f));
}

void Vert(A2V i, out V2F o)
{
	float3 oblateCameraM = _WorldSpaceCameraPos;
	float4 oblateVertM   = mul(_Object2World, i.vertex);
	float3 localM        = RemoveOblateness(oblateVertM.xyz - centrePosition);
	float3 vertM         = localM + centrePosition;
	
#ifdef VARIANT_OUTER
	float3 cam2centreM = RemoveOblateness(oblateCameraM - centrePosition);
	float3 nearM       = reflect(localM, normalize(localM - cam2centreM));
#else
	float3 nearM = RemoveOblateness(oblateCameraM - centrePosition);
#endif
	
	float3 ray = localM - nearM;
	
	float scaledDepth  = length(ray) / maxDepth;
	float opticalDepth = pow(scaledDepth, atmosphereFalloff);
	
	o.vertex      = mul(UNITY_MATRIX_MVP, i.vertex);
	o.texcoord.xy = scaledDepth;
	o.texcoord.z  = opticalDepth;
}

void Frag(V2F i, out half4 o : COLOR)
{
	float2 atmosphereUV = i.texcoord.xy;
	float  opticalDepth = saturate(i.texcoord.z);
	
	float4 atmosphereColour = tex2D(atmosphereTexture, atmosphereUV);
	
	o.xyz = atmosphereColour.xyz;
	o.w   = atmosphereColour.w * opticalDepth;
}