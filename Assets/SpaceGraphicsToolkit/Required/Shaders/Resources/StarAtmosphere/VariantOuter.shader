Shader "Hidden/SGT/StarAtmosphere/VariantOuter"
{
	Properties
	{
		atmosphereTexture("atmosphereTexture",  2D) = "white" {}
		centrePosition("centrePosition", Vector) = (0.0, 0.0, 0.0, 0.0)
		atmosphereFalloff("atmosphereFalloff", Float) = 0.0
		maxDepth("maxDepth", Float) = 0.0
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
				
				#define VARIANT_OUTER
				
				#include "../../CGInclude/StarAtmosphere.cginc"
				
				ENDCG
			}
		}
	}
}