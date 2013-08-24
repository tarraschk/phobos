Shader "Hidden/SGT/PlanetAtmosphere/VariantScattering"
{
	Properties
	{
		atmosphereTexture("atmosphereTexture",  2D) = "white" {}
		centrePosition("centrePosition", Vector) = (0.0, 0.0, 0.0, 0.0)
		starDirection("starDirection", Vector) = (0.0, 0.0, 0.0, 0.0)
		atmosphereFalloff("atmosphereFalloff", Float) = 0.0
		maxDepth("maxDepth", Float) = 0.0
		camToStarDirection("camToStarDirection", Vector) = (0.0, 0.0, 0.0, 0.0)
		mieValues("mieValues", Vector) = (0.0, 0.0, 0.0, 0.0)
		rayleighValues("rayleighValues", Vector) = (0.0, 0.0, 0.0, 0.0)
	}
	
	Category
	{
		Blend SrcAlpha OneMinusSrcAlpha
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
				
				#define VARIANT_SCATTERING
				
				#include "../../CGInclude/PlanetAtmosphere.cginc"
				
				ENDCG
			}
		}
	}
}