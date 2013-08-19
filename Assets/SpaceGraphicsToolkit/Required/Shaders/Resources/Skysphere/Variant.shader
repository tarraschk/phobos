Shader "Hidden/SGT/Skysphere/Variant"
{
	Properties
	{
		skysphereTexture("skysphereTexture", 2D) = "black" {}
	}
	
	Category
	{
		Blend One Zero
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
				
				
				#include "../../CGInclude/Skysphere.cginc"
				
				ENDCG
			}
		}
	}
}