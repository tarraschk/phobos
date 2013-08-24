Shader "Hidden/SGT/GasGiant/VariantOuter"
{
	Properties
	{
		lightingTexture("lightingTexture",  2D) = "white" {}
		centrePosition("centrePosition", Vector) = (0.0, 0.0, 0.0, 0.0)
		starDirection("starDirection", Vector) = (0.0, 0.0, 0.0, 0.0)
		maxDepth("maxDepth", Float) = 0.0
		falloff("falloff", Float) = 0.0
		dayTexture("dayTexture", 2D) = "gray" {}
		nightTexture("nightTexture", 2D) = "black" {}
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
				
				#include "../../CGInclude/GasGiant.cginc"
				
				ENDCG
			}
		}
	}
}