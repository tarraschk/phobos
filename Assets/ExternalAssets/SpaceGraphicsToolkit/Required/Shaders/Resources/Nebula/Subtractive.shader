Shader "Hidden/SGT/Nebula/Subtractive"
{
	Properties
	{
		particleTexture ("particleTexture", 2D) = "white" {}
		particleColour ("particleColour", Color) = (1.0, 1.0, 1.0, 1.0)
		particleFadeInDistance ("particleFadeInDistance", Float) = 0.0
	}
	
	Category
	{
		Blend    One One
		BlendOp  RevSub
		ZWrite   Off
		Lighting Off
		
		Tags
		{
			"Queue"           = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType"      = "Transparent"
		}
		
		SubShader
		{
			Pass
			{
				CGPROGRAM
				
				#pragma vertex   Vert
				#pragma fragment Frag
				
				
				#include "../../CGInclude/Nebula.cginc"
				
				ENDCG
			}
		}
	}
}