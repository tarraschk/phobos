Shader "Hidden/SGT/Fog/Variant"
{
	Properties
	{
		fogColour("fogColour", Color) = (0.0, 0.0, 0.0, 1.0)
	}
	
	Category
	{
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
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
				
				
				#include "../../CGInclude/Fog.cginc"
				
				ENDCG
			}
		}
	}
}