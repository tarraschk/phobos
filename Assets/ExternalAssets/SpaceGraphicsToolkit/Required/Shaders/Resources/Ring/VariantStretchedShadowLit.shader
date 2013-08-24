Shader "Hidden/SGT/Ring/VariantStretchedShadowLit"
{
	Properties
	{
		ringTexture("ringTexture",  2D) = "white" {}
		position("position", Vector) = (0.0, 0.0, 0.0, 0.0)
		ringRadius("ringRadius", Float) = 0.0
		ringThickness("ringThickness", Float) = 0.0
		umbraColour("umbraColour", Color) = (0.0, 0.0, 0.0, 1.0)
		penumbraColour("penumbraColour", Color) = (0.0, 0.0, 0.0, 1.0)
		shadowRatio("shadowRatio", Float) = 0.0
		shadowScale("shadowScale", Float) = 0.0
		starDirection("starDirection", Vector) = (0.0, 0.0, 0.0, 0.0)
		ringBrightness("ringBrightness", Float) = 0.0
		ringBrightnessRange("ringBrightnessRange", Float) = 0.0
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
				
				#define VARIANT_STRETCHED
				#define VARIANT_SHADOW
				#define VARIANT_LIT
				
				#include "../../CGInclude/Ring.cginc"
				
				ENDCG
			}
		}
	}
}