Shader "Hidden/SGT/PlanetSurface/VariantAtmosphereOuterRingShadow"
{
	Properties
	{
		dayTexture("dayTexture", 2D) = "gray" {}
		nightTexture("nightTexture", 2D) = "black" {}
		lightingTexture("lightingTexture",  2D) = "white" {}
		centrePosition("centrePosition", Vector) = (0.0, 0.0, 0.0, 0.0)
		starDirection("starDirection", Vector) = (0.0, 0.0, 0.0, 0.0)
		atmosphereTexture("atmosphereTexture",  2D) = "white" {}
		atmosphereRadius("atmosphereRadius", Float) = 0.0
		atmosphereFalloff("atmosphereFalloff", Float) = 0.0
		maxDepth("maxDepth", Float) = 0.0
		surfaceRadius("surfaceRadius", Float) = 0.0
		atmosphereHeight("atmosphereHeight", Float) = 0.0
		shadowTexture("shadowTexture",  2D) = "white" {}
		shadowValues("shadowValues", Vector) = (0.0, 0.0, 0.0, 0.0)
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
				
				#define VARIANT_ATMOSPHERE
				#define VARIANT_OUTER
				#define VARIANT_RINGSHADOW
				
				#include "../../CGInclude/PlanetSurface.cginc"
				
				ENDCG
			}
		}
	}
}