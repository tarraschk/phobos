Shader "Hidden/SGT/Starfield/VariantMinSize"
{
	Properties
	{
		starTexture("starTexture",  2D) = "white" {}
		starPulseRateMax("starPulseRateMax", Float) = 0.0
		starSizeMin("starSizeMin", Float) = 0.0
		starSizeMin("starSizeMin", Float) = 0.0
	}
	
	Category
	{
		Blend One One
		Cull Back
		ZWrite Off
		ZTest LEqual
		
		SubShader
		{
			Pass
			{
				CGPROGRAM
				
				#pragma vertex   Vert
				#pragma fragment Frag
				
				#define VARIANT_MINSIZE
				
				#include "../../CGInclude/Starfield.cginc"
				
				ENDCG
			}
		}
	}
}