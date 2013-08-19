Shader "Hidden/SGT/AsteroidRing/VariantSpin"
{
	Properties
	{
		dayTexture("dayTexture", 2D) = "gray" {}
		nightTexture("nightTexture", 2D) = "black" {}
		bumpTexture("bumpTexture",  2D) = "white" {}
		starPositionRaw("starPositionRaw", Vector) = (0.0, 0.0, 0.0, 0.0)
		centrePosition("centrePosition", Vector) = (0.0, 0.0, 0.0, 0.0)
		ringHeight("ringHeight", Float) = 0.0
		spinRateMax("spinRateMax", Float) = 0.0
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
				
				#define VARIANT_SPIN
				
				#include "../../CGInclude/AsteroidRing.cginc"
				
				ENDCG
			}
		}
	}
}