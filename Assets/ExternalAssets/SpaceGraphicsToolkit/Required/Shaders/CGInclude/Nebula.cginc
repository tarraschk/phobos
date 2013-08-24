#include "../../CGInclude/Include.cginc"
#include "UnityCG.cginc"

sampler2D particleTexture;
float4    particleColour;
float     particleFadeInDistance;

struct a2v
{
	float4 vertex    : POSITION;
	float2 texcoord  : TEXCOORD0;
	float2 texcoord1 : TEXCOORD1;
	float3 normal    : NORMAL;
	float3 color     : COLOR0;
};
struct v2f
{
	float4 pos  : SV_POSITION;
	float2 uv0  : TEXCOORD0;
	float3 rgb  : COLOR0;
	float3 wpos : TEXCOORD1;
};

void Vert(a2v i, out v2f o)
{
	float  radius   = i.texcoord1.x;
	float4 vertexMV = mul(UNITY_MATRIX_MV, i.vertex);
	float3 offsetMV = i.normal * radius / unity_Scale.w;
	float4 cornerMV  = vertexMV; cornerMV.xyz += offsetMV;
	float4 cornerMVP = mul(UNITY_MATRIX_P, cornerMV);
	
	o.uv0  = i.texcoord;
	o.pos  = cornerMVP;
	o.rgb  = i.color * particleColour.rgb;
	o.wpos = cornerMV;
}

void Frag(v2f i, out half4 o : COLOR)
{
	float wd = (length(i.wpos) - _ProjectionParams.y) / particleFadeInDistance;
	float a  = saturate(wd);
	
	o.rgb = tex2D(particleTexture, i.uv0).rgb * i.rgb * a;
	o.a   = 1.0f;
}