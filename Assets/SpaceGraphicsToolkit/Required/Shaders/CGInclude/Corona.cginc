#include "../../CGInclude/Include.cginc"

sampler2D coronaTexture;
float3    coronaPosition;
float3    coronaColour;
float     coronaFalloff;
#ifdef VARIANT_CULLNEAR
float cullNearOffset;
float invCullNearLength;
#endif
#ifdef VARIANT_RING
float coronaRadius;
float coronaHeight;
#endif

struct V2F
{
	float4 vertex : SV_POSITION;
#ifdef VARIANT_RING
	float4 texcoord : TEXCOORD0;
#else
	float2 texcoord : TEXCOORD0;
#endif
#ifdef VARIANT_CULLNEAR
	float4 texcoord2 : TEXCOORD2;
#endif
#ifdef VARIANT_PERPIXEL
	float3 texcoord3 : TEXCOORD3;
	float3 texcoord4 : TEXCOORD4;
#else
	fixed color : COLOR;
#endif
};

void Vert(A2V i, out V2F o)
{
	float4 vertM        = mul(_Object2World, i.vertex);
	float3 cam2vertM    = _WorldSpaceCameraPos - vertM.xyz;
	float3 cam2vertMDir = normalize(cam2vertM);
	float3 localM       = vertM.xyz - coronaPosition;
	float3 normalM      = mul((float3x3)_Object2World, i.normal);
	
	o.vertex = mul(UNITY_MATRIX_MVP, i.vertex);
	
#ifdef VARIANT_CULLNEAR
	o.texcoord2.xyz = cam2vertM;
	o.texcoord2.w   = length(_WorldSpaceCameraPos - coronaPosition);
#endif
	
#ifdef VARIANT_PERPIXEL
	o.texcoord3.xyz = cam2vertMDir;
	o.texcoord4.xyz = normalM;
#else
	o.color.x = pow(abs(dot(cam2vertMDir, normalize(normalM))), coronaFalloff);
#endif
	
#ifdef VARIANT_RING
	o.texcoord.xyz = localM;
	o.texcoord.w   = i.texcoord.x;
#else
	o.texcoord.xy = i.texcoord.xy;
#endif
}

void Frag(V2F i, out half4 o : COLOR)
{
#ifdef VARIANT_PERPIXEL
	float3 cam2vertMDir = normalize(i.texcoord3.xyz);
	float3 normalM      = normalize(i.texcoord4.xyz);
#else
	float opacity = i.color.x;
#endif
#ifdef VARIANT_CULLNEAR
	float3 cam2vertM     = i.texcoord2.xyz;
	float  cam2vertMLen  = length(cam2vertM);
	float  cam2coronaLen = i.texcoord2.w;
#endif
#ifdef VARIANT_RING
	float3 localM   = i.texcoord.xyz;
	float  distance = length(localM);
	float2 coronaUV;
	coronaUV.x = i.texcoord.w;
	coronaUV.y = saturate((distance - coronaRadius) / coronaHeight);
#else
	float2 coronaUV = i.texcoord.xy;
#endif
	
#ifdef VARIANT_PERPIXEL
	float opacity = pow(abs(dot(cam2vertMDir, normalM)), coronaFalloff);
#endif
	
#ifdef VARIANT_CULLNEAR
	opacity *= saturate((cam2vertMLen - cam2coronaLen + cullNearOffset) * invCullNearLength);
#endif
	
	float4 corona = tex2D(coronaTexture, coronaUV);
	
	corona.xyz *= coronaColour * opacity;
	
	o = corona;
}