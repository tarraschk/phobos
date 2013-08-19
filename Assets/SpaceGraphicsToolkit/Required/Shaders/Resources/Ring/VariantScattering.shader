Shader "Hidden/SGT/Ring/VariantScattering"
{
	Properties
	{
		ringTexture("ringTexture",  2D) = "white" {}
		position("position", Vector) = (0.0, 0.0, 0.0, 0.0)
		ringRadius("ringRadius", Float) = 0.0
		ringThickness("ringThickness", Float) = 0.0
		starPosition("starPosition", Vector) = (0.0, 0.0, 0.0, 0.0)
		mieValues("mieValues", Vector) = (0.0, 0.0, 0.0, 0.0)
		occlusion("occlusion", Float) = 0.0
	}
	
	Category
	{
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		ZWrite Off
		ZTest LEqual
		
		SubShader
		{
			Pass
			{
				CGPROGRAM
				
				#pragma vertex   Vert
				#pragma fragment Frag
				
				#define VARIANT_SCATTERING
				
				#include "../../CGInclude/Ring.cginc"
				
				ENDCG
			}
		}
	}
}