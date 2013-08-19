#include "../../CGInclude/Include.cginc"

sampler2D starTexture;
float     starPulseRateMax;
#ifdef VARIANT_MINSIZE
float starSizeMin;
#endif

struct V2F
{
	float4 pos   : SV_POSITION;
	float2 uv0   : TEXCOORD0;
#ifdef VARIANT_MINSIZE
	float  alpha : COLOR0;
#endif
};

void Vert(A2V i, out V2F o)
{
	float  midRadius   = i.texcoord1.x;
	float  pulseRadius = i.texcoord1.y;
	float  pulseRate   = i.color.x * starPulseRateMax;
	float  pulseOffset = i.color.y + _Time.y;
	float  radius      = midRadius + sin(pulseOffset * pulseRate) * pulseRadius;
	float4 vertexMV    = mul(UNITY_MATRIX_MV, i.vertex);
	float3 offsetMV    = i.normal * radius / unity_Scale.w;
	
#ifdef VARIANT_MINSIZE
	float scale = (length(offsetMV) / starSizeMin) / length(vertexMV);
	
	offsetMV /= saturate(scale);
	
	o.alpha = scale;
#endif
	
	float4 cornerMV  = vertexMV; cornerMV.xyz += offsetMV;
	float4 cornerMVP = mul(UNITY_MATRIX_P, cornerMV);
	
	o.pos = cornerMVP;
	o.uv0 = i.texcoord.xy;
}

void Frag(V2F i, out half4 o : COLOR)
{
	float3 star = tex2D(starTexture, i.uv0);
	
#ifdef VARIANT_MINSIZE
	star *= i.alpha;
#endif
	
	o.xyz = star;
	o.w   = 1.0f;
}