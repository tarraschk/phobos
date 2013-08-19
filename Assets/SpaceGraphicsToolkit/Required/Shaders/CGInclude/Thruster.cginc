#include "../CGInclude/Include.cginc"

sampler2D thrusterTexture;
float4    thrusterColour;
float     thrusterGlow;

#ifdef VARIANT_FALLOFF
float thrusterFalloff;
#endif

struct V2F
{
	float4 pos : SV_POSITION;
	float2 uv0 : TEXCOORD0;
	float4 col : COLOR0;
#ifdef VARIANT_FALLOFF
	float alpha : COLOR1;
#endif
};

void Vert(A2V i, out V2F o)
{
#ifdef VARIANT_FALLOFF
	float4 vertM        = mul(_Object2World, i.vertex);
	float3 cam2vertM    = _WorldSpaceCameraPos - vertM.xyz;
	float3 cam2vertMDir = normalize(cam2vertM);
	float3 normalM      = mul((float3x3)_Object2World, i.normal);
#endif
	
	o.pos = mul(UNITY_MATRIX_MVP, i.vertex);
	o.uv0 = i.texcoord.xy;
	o.col = i.color;
	
#ifdef VARIANT_FALLOFF
	o.alpha = 1.0f - pow(1.0f - abs(dot(cam2vertMDir, normalM * unity_Scale.w)), thrusterFalloff);
#endif
}

void Frag(V2F i, out half4 o : COLOR)
{
	float4 thruster = tex2D(thrusterTexture, i.uv0);
	
	o = thruster * thrusterColour * i.col + thruster * thrusterGlow;
#ifdef VARIANT_FALLOFF
	o *= i.alpha;
#endif
}