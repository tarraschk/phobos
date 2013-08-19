Shader "Hidden/SGT/Corona/VariantCullNear"
{
	Properties
	{
		coronaTexture("coronaTexture",  2D) = "white" {}
		coronaPosition("coronaPosition", Vector) = (0.0, 0.0, 0.0, 0.0)
		coronaColour("coronaColour", Color) = (0.0, 0.0, 0.0, 1.0)
		coronaFalloff("coronaFalloff", Float) = 0.0
		cullNearOffset("cullNearOffset", Float) = 0.0
		invCullNearLength("invCullNearLength", Float) = 0.0
	}
	
	Category
	{
		Blend One One
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
				
				#define VARIANT_CULLNEAR
				
				#include "../../CGInclude/Corona.cginc"
				
				ENDCG
			}
		}
	}
}