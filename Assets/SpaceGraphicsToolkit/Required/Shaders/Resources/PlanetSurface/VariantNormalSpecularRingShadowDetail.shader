Shader "Hidden/SGT/PlanetSurface/VariantNormalSpecularRingShadowDetail"
{
	Properties
	{
		dayTexture("dayTexture", 2D) = "gray" {}
		nightTexture("nightTexture", 2D) = "black" {}
		lightingTexture("lightingTexture",  2D) = "white" {}
		centrePosition("centrePosition", Vector) = (0.0, 0.0, 0.0, 0.0)
		starDirection("starDirection", Vector) = (0.0, 0.0, 0.0, 0.0)
		normalTexture("normalTexture", 2D) = "bump" {}
		starDirectionM("starDirectionM", Vector) = (0.0, 0.0, 0.0, 0.0)
		specularTexture("specularTexture", 2D) = "black" {}
		specularPower("specularPower", Float) = 0.0
		shadowTexture("shadowTexture",  2D) = "white" {}
		shadowValues("shadowValues", Vector) = (0.0, 0.0, 0.0, 0.0)
		detailTexture("detailTexture",  2D) = "white" {}
		detailRepeat("detailRepeat", Float) = 0.0
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
				
				#define VARIANT_NORMAL
				#define VARIANT_SPECULAR
				#define VARIANT_RINGSHADOW
				#define VARIANT_DETAIL
				
				#include "../../CGInclude/PlanetSurface.cginc"
				
				ENDCG
			}
		}
	}
}