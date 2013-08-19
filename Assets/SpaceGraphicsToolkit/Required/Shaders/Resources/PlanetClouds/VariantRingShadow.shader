Shader "Hidden/SGT/PlanetClouds/VariantRingShadow"
{
	Properties
	{
		cloudsTexture("cloudsTexture",  2D) = "white" {}
		lightingTexture("lightingTexture",  2D) = "white" {}
		centrePosition("centrePosition", Vector) = (0.0, 0.0, 0.0, 0.0)
		starDirection("starDirection", Vector) = (0.0, 0.0, 0.0, 0.0)
		falloff("falloff", Float) = 0.0
		shadowTexture("shadowTexture",  2D) = "white" {}
		shadowValues("shadowValues", Vector) = (0.0, 0.0, 0.0, 0.0)
	}
	
	Category
	{
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Back
		ZWrite Off
		ZTest LEqual
		Offset -1, -1
		
		SubShader
		{
			Pass
			{
				CGPROGRAM
				
				#pragma vertex   Vert
				#pragma fragment Frag
				
				#define VARIANT_RINGSHADOW
				
				#include "../../CGInclude/PlanetClouds.cginc"
				
				ENDCG
			}
		}
	}
}