Shader "Hidden/SGT/StarSurface/VariantPerPixel"
{
	Properties
	{
		surfaceTexture("surfaceTexture", 2D) = "gray" {}
		atmosphereTexture("atmosphereTexture",  2D) = "white" {}
		centrePosition("centrePosition", Vector) = (0.0, 0.0, 0.0, 0.0)
		atmosphereRadius("atmosphereRadius", Float) = 0.0
		atmosphereFalloff("atmosphereFalloff", Float) = 0.0
		maxDepth("maxDepth", Float) = 0.0
		surfaceRadius("surfaceRadius", Float) = 0.0
		atmosphereHeight("atmosphereHeight", Float) = 0.0
	}
	
	Category
	{
		Blend One Zero
		Cull Back
		ZWrite On
		ZTest LEqual
		
		SubShader
		{
			Pass
			{
				CGPROGRAM
				
				#pragma vertex   Vert
				#pragma fragment Frag
				
				#define VARIANT_PERPIXEL
				
				#include "../../CGInclude/StarSurface.cginc"
				
				ENDCG
			}
		}
	}
}