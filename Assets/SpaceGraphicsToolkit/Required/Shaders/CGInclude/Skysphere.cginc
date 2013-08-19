#include "../../CGInclude/Include.cginc"

sampler2D skysphereTexture;

struct V2F
{
	float4 pos : SV_POSITION;
	float2 uv0 : TEXCOORD0;
};

void Vert(A2V i, out V2F o)
{
	o.pos = mul(UNITY_MATRIX_MVP, i.vertex);
	o.uv0.x = 1.0f - i.texcoord.x;
	o.uv0.y = i.texcoord.y;
}

void Frag(V2F i, out half4 o : COLOR)
{
	o.xyz = tex2D(skysphereTexture, i.uv0);
	o.w   = 1.0f;
}