using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Planet")]
public partial class SGT_Planet : SGT_MonoBehaviourUnique<SGT_Planet>
{
	public bool InsideAtmosphere(Vector3 point)
	{
		if (atmosphereGameObject != null)
		{
			point = atmosphereGameObject.transform.InverseTransformPoint(point);
			
			return point.magnitude < 1.0f;
		}
		
		return false;
	}
	
	public float SurfaceRadiusAtPoint(Vector3 point)
	{
		return surfaceRadius;
	}
	
	public float DistanceToSurface(Vector3 point)
	{
		point = transform.InverseTransformPoint(point);
		
		return Mathf.Abs(point.magnitude - surfaceRadius);
	}
	
	private void UpdateCloudsOffset()
	{
		if (cloudsGameObject != null && planetObserver != null)
		{
			var observerDirection  = transform.InverseTransformPoint(planetObserver.transform.position).normalized;
			//var observerDirection = (observerPosition - transform.position).normalized;
			
			SGT_Helper.SetLocalPosition(cloudsGameObject.transform, observerDirection * cloudsOffset);
		}
	}
	
	private void UpdateGradient()
	{
		// See if any gradient has been modified
		if (planetLighting.Modified == true)
		{
			planetLighting.Modified = false;
			
			surfaceLightingTexture   = SGT_Helper.DestroyObject(surfaceLightingTexture);
			atmosphereTexture        = SGT_Helper.DestroyObject(atmosphereTexture);
			atmosphereSurfaceTexture = SGT_Helper.DestroyObject(atmosphereSurfaceTexture);
			cloudsLightingTexture    = SGT_Helper.DestroyObject(cloudsLightingTexture);
		}
		
		if (atmosphereTwilightColour.Modified == true)
		{
			atmosphereTwilightColour.Modified = false;
			
			atmosphereTexture        = SGT_Helper.DestroyObject(atmosphereTexture);
			atmosphereSurfaceTexture = SGT_Helper.DestroyObject(atmosphereSurfaceTexture);
			cloudsLightingTexture    = SGT_Helper.DestroyObject(cloudsLightingTexture);
		}
		
		if (atmosphereDensityColour.Modified == true)
		{
			atmosphereDensityColour.Modified = false;
			
			atmosphereTexture        = SGT_Helper.DestroyObject(atmosphereTexture);
			atmosphereSurfaceTexture = SGT_Helper.DestroyObject(atmosphereSurfaceTexture);
		}
		
		if (cloudsLimbColour.Modified == true)
		{
			cloudsLimbColour.Modified = false;
			
			cloudsLightingTexture = SGT_Helper.DestroyObject(cloudsLightingTexture);
		}
		
		// Build colour arrays
		var lighting       = surfaceLightingTexture   == null || atmosphereTexture == null || atmosphereSurfaceTexture == null || cloudsLightingTexture == null ? planetLighting.CalculateColours(0.0f, 1.0f, (int)planetLutSize) : null;
		var twilight       = atmosphereTexture        == null || atmosphereSurfaceTexture == null ? atmosphereTwilightColour.CalculateColours(0.0f, 1.0f, (int)planetLutSize) : null;
		var cloudsTwilight = cloudsLightingTexture    == null ? atmosphereTwilightColour.CalculateColours(cloudsTwilightOffset, cloudsTwilightOffset + 1.0f, (int)planetLutSize) : null;
		var density        = atmosphereTexture        == null ?  atmosphereDensityColour.CalculateColours(1.0f, 0.5f, (int)planetLutSize) : null;
		var surfaceDensity = atmosphereSurfaceTexture == null ?  atmosphereDensityColour.CalculateColours(0.0f, 0.5f, (int)planetLutSize) : null;
		var cloudsLimb     = cloudsLightingTexture    == null ?         cloudsLimbColour.CalculateColours(0.0f, 1.0f, (int)planetLutSize) : null;
		
		// Rebuild textures
		if (surfaceLightingTexture == null)
		{
			updateShader          |= ShaderFlags.SurfaceTextureLighting;
			surfaceLightingTexture = SGT_ColourGradient.AllocateTexture((int)planetLutSize);
			
			for (var x = 0; x < (int)planetLutSize; x++)
			{
				surfaceLightingTexture.SetPixel(x, 0, lighting[x]);
			}
			
			surfaceLightingTexture.anisoLevel = 8;
			surfaceLightingTexture.filterMode = FilterMode.Trilinear;
			surfaceLightingTexture.Apply();
		}
		
		if (atmosphereTexture == null)
		{
			updateShader     |= ShaderFlags.AtmosphereTexture;
			atmosphereTexture = SGT_ColourGradient.AllocateTexture((int)planetLutSize, (int)planetLutSize);
			
			for (var y = 0; y < (int)planetLutSize; y++)
			{
				var densityColour = density[y];
				
				for (var x = 0; x < (int)planetLutSize; x++)
				{
					var lightingColour   = lighting[x];
					//var twilightColour   = twilight[x];
					lightingColour.a = Mathf.Lerp(atmosphereNightOpacity, 1.0f, lightingColour.grayscale);
					//var colour           = SGT_Helper.AlphaBlend(densityColour, twilightColour) * lightingColour;
					
					var twilightColour = SGT_Helper.AlphaBlend(Color.white, twilight[x]);
					var colour         = SGT_Helper.BlendRGB(densityColour * lightingColour, twilightColour, SGT_Helper.ChannelBlendMode.Multiply);
					
					atmosphereTexture.SetPixel(x, y, SGT_Helper.PreventZeroRGB(colour));
					atmosphereTexture.filterMode = FilterMode.Trilinear;
				}
			}
			
			atmosphereTexture.Apply();
		}
		
		if (atmosphereSurfaceTexture == null)
		{
			updateShader            |= ShaderFlags.SurfaceTextureAtmosphere;
			atmosphereSurfaceTexture = SGT_ColourGradient.AllocateTexture((int)planetLutSize, (int)planetLutSize);
			
			for (var y = 0; y < (int)planetLutSize; y++)
			{
				var densityColour = surfaceDensity[y];
				
				for (var x = 0; x < (int)planetLutSize; x++)
				{
					var lightingColour   = lighting[x];
					var twilightColour   = twilight[x];
					lightingColour.a = Mathf.Lerp(atmosphereNightOpacity, 1.0f, lightingColour.grayscale);
					var colour           = SGT_Helper.AlphaBlend(densityColour, twilightColour) * lightingColour;
					
					atmosphereSurfaceTexture.SetPixel(x, y, SGT_Helper.PreventZeroRGB(colour));
					atmosphereSurfaceTexture.filterMode = FilterMode.Trilinear;
				}
			}
			
			atmosphereSurfaceTexture.Apply();
		}
		
		if (cloudsLightingTexture == null)
		{
			updateShader         |= ShaderFlags.CloudsTextureLighting;
			cloudsLightingTexture = SGT_ColourGradient.AllocateTexture((int)planetLutSize, (int)planetLutSize);
			
			for (var y = 0; y < (int)planetLutSize; y++)
			{
				var limb = cloudsLimb[y];
				
				for (var x = 0; x < (int)planetLutSize; x++)
				{
					var colour = lighting[x];
					
					if (atmosphere == true)
					{
						colour *= SGT_Helper.AlphaBlend(limb, cloudsTwilight[x]);
					}
					else
					{
						colour *= limb;
					}
					
					cloudsLightingTexture.SetPixel(x, y, colour);
					cloudsLightingTexture.filterMode = FilterMode.Trilinear;
				}
			}
			
			cloudsLightingTexture.Apply();
		}
	}
	
	private void UpdateTechnique()
	{
		var targetSurfaceTechnique    = "Variant";
		var targetAtmosphereTechnique = string.Empty;
		var tagetCloudsTechnique      = string.Empty;
		
		if (atmosphere == true)
		{
			targetSurfaceTechnique   += "Atmosphere";
			targetAtmosphereTechnique = "Variant";
			
			if (planetObserver != null)
			{
				if (InsideAtmosphere(planetObserver.transform.position) == false)
				{
					targetSurfaceTechnique    += "Outer";
					targetAtmosphereTechnique += "Outer";
				}
			}
			
			if (atmosphereScattering == true)
			{
				//targetSurfaceTechnique    += "Scattering";
				targetAtmosphereTechnique += "Scattering";
			}
		}
		
		if (clouds == true)
		{
			tagetCloudsTechnique = "Variant";
		}
		
		if (surfaceTextureNormal.ContainsSomething == true)
		{
			targetSurfaceTechnique += "Normal";
		}
		
		if (surfaceTextureSpecular.ContainsSomething == true)
		{
			targetSurfaceTechnique += "Specular";
		}
		
		if (shadow == true)
		{
			switch (shadowCasterType)
			{
				case SGT_ShadowOccluder.Ring:
				{
					targetSurfaceTechnique += "RingShadow";
					
					if (atmosphere == true)
					{
						targetAtmosphereTechnique += "RingShadow";
					}
				
					if (clouds == true)
					{
						tagetCloudsTechnique += "RingShadow";
					}
				}
				break;
				
				case SGT_ShadowOccluder.Planet:
				{
					targetSurfaceTechnique += "PlanetShadow";
					
					if (atmosphere == true)
					{
						targetAtmosphereTechnique += "PlanetShadow";
					}
					
					if (clouds == true)
					{
						tagetCloudsTechnique += "PlanetShadow";
					}
				}
				break;
			}
		}
		
		if (surfaceTextureDetail != null)
		{
			targetSurfaceTechnique += "Detail";
		}
		
		if (surfaceMaterials != null && (surfaceTechnique != targetSurfaceTechnique || SGT_ArrayHelper.ContainsSomething(surfaceMaterials) == false || surfaceMaterials.Length != SGT_SurfaceConfiguration_.SurfaceCount(SurfaceConfiguration)))
		{
			surfaceMaterials = SGT_Helper.DestroyObjects(surfaceMaterials);
		}
		
		if (atmosphereMaterial != null && (atmosphere == false || atmosphereTechnique != targetAtmosphereTechnique))
		{
			atmosphereMaterial = SGT_Helper.DestroyObject(atmosphereMaterial);
		}
		
		if (cloudsMaterials != null && (clouds == false || cloudsTechnique != tagetCloudsTechnique || SGT_ArrayHelper.ContainsSomething(cloudsMaterials) == false || cloudsMaterials.Length != SGT_SurfaceConfiguration_.SurfaceCount(CloudsConfiguration)))
		{
			cloudsMaterials = SGT_Helper.DestroyObjects(cloudsMaterials);
		}
		
		// Create planet surface shaders?
		if (surfaceMaterials == null)
		{
			updateShader    |= ShaderFlags.Surface;
			surfaceTechnique = targetSurfaceTechnique;
			surfaceMaterials = SGT_SurfaceConfiguration_.CreateMaterials(SurfaceConfiguration, "Hidden/SGT/PlanetSurface/" + surfaceTechnique, surfaceRenderQueue);
		}
		else
		{
			SGT_Helper.SetRenderQueues(surfaceMaterials, surfaceRenderQueue);
		}
		
		// Create planet atmosphere shaders?
		if (atmosphere == true)
		{
			if (atmosphereMaterial == null)
			{
				updateShader       |= ShaderFlags.Atmosphere;
				atmosphereTechnique = targetAtmosphereTechnique;
				atmosphereMaterial  = SGT_Helper.CreateMaterial("Hidden/SGT/PlanetAtmosphere/" + atmosphereTechnique, atmosphereRenderQueue);
			}
			else
			{
				SGT_Helper.SetRenderQueue(atmosphereMaterial, atmosphereRenderQueue);
			}
		}
		
		// Create planet cloud shaders?
		if (clouds == true)
		{
			if (cloudsMaterials == null)
			{
				updateShader   |= ShaderFlags.Clouds;
				cloudsTechnique = tagetCloudsTechnique;
				cloudsMaterials = SGT_SurfaceConfiguration_.CreateMaterials(CloudsConfiguration, "Hidden/SGT/PlanetClouds/" + cloudsTechnique, atmosphereRenderQueue);
			}
			else
			{
				SGT_Helper.SetRenderQueues(cloudsMaterials, atmosphereRenderQueue);
			}
		}
		else
		{
			if (cloudsMaterials != null) cloudsMaterials = SGT_Helper.DestroyObjects(cloudsMaterials);
		}
	}
	
	private void UpdateShader()
	{
		var starDirection       = planetLightSource != null ? (planetLightSource.transform.position - transform.position).normalized : Vector3.up;
		var lightSourcePosition = planetLightSource != null ? planetLightSource.transform.position : Vector3.zero;
		var transformPosition   = transform.position;
		var uniformScale        = UniformScale;
		var observerPosition    = planetObserver    != null ? planetObserver.transform.position : Vector3.zero;
		var distance            = (observerPosition - transformPosition).magnitude;
		
		float maxSurfaceDepth, maxAtmosphereDepth;
		
		SGT_Helper.CalculateHorizonAtmosphereDepth(surfaceRadius * uniformScale, AtmosphereRadius * uniformScale, distance, out maxSurfaceDepth, out maxAtmosphereDepth);
		
		// Common shader variables
		SGT_ShaderHelper.SetVector(surfaceMaterials, "centrePosition", transformPosition);
		SGT_ShaderHelper.SetVector(surfaceMaterials, "starDirection", starDirection);
		
		if (surfaceTextureNormal != null)
		{
			var starDirectionM = surfaceGameObject.transform.worldToLocalMatrix.MultiplyVector(starDirection).normalized;
			
			SGT_ShaderHelper.SetVector(surfaceMaterials, "starDirectionM", starDirectionM);
		}
		
		if (atmosphereMaterial != null)
		{
			var altitudeUnclamped = distance - surfaceRadius * uniformScale;
			var altitude          = Mathf.Clamp(altitudeUnclamped, 0.0f, atmosphereHeight * uniformScale);
			var atmoAlt           = SGT_Helper.RemapClamped(atmosphereHeight * uniformScale * atmosphereSkyAltitude, atmosphereHeight * uniformScale, altitude, 0.0f, 1.0f);
			
			SGT_ShaderHelper.SetFloat(surfaceMaterials, "maxDepth", maxSurfaceDepth * (1.0f - atmosphereFog)); // FOG
			
			atmosphereMaterial.SetFloat("maxDepth", maxAtmosphereDepth);
			atmosphereMaterial.SetVector("centrePosition", transformPosition);
			atmosphereMaterial.SetVector("starDirection", starDirection);
			atmosphereMaterial.SetFloat("atmosphereFalloff", Mathf.SmoothStep(atmosphereFalloffInside, atmosphereFalloffOutside, atmoAlt));
			
			if (atmosphereScattering == true)
			{
				SGT_ShaderHelper.SetVector(surfaceMaterials, "starPosition", lightSourcePosition);
				
				atmosphereMaterial.SetVector("starPosition", lightSourcePosition);
			}
		}
		
		if (cloudsMaterials != null)
		{
			SGT_ShaderHelper.SetVector(cloudsMaterials, "centrePosition", transformPosition);
			SGT_ShaderHelper.SetVector(cloudsMaterials, "starDirection", starDirection);
		}
		
		if (shadow == true)
		{
			var shadowMatrix = Matrix4x4.identity;
			
			switch (shadowCasterType)
			{
				case SGT_ShadowOccluder.Planet:
				{
					if (shadowCasterGameObject != null)
					{
						shadowMatrix = SGT_Helper.CalculateSphereShadowMatrix(ShadowCasterRadiusOuter, shadowCasterGameObject.transform.position, lightSourcePosition, transformPosition, uniformScale);
					}
				}
				break;
				
				case SGT_ShadowOccluder.Ring:
				{
					shadowMatrix = SGT_Helper.CalculateCircleShadowMatrix(ShadowCasterRadiusOuter, transform.position, transform.up, lightSourcePosition, transformPosition, uniformScale);
				}
				break;
			}
			
			SGT_ShaderHelper.SetMatrix(surfaceMaterials, "shadowMatrix", shadowMatrix);
			
			if (atmosphereMaterial != null) atmosphereMaterial.SetMatrix("shadowMatrix", shadowMatrix);
			if (cloudsMaterials    != null) SGT_ShaderHelper.SetMatrix(cloudsMaterials, "shadowMatrix", shadowMatrix);
		}
		
		// Uncommon shader variables
		if (SGT_Helper.FlagIsSet(updateShader, ShaderFlags.ShadowSettings) == true)
		{
			var cutoff       = ShadowCasterRadiusInner / ShadowCasterRadiusOuter;
			var invCutoff    = SGT_Helper.Reciprocal(1.0f - cutoff);
			var shadowValues = new Vector3(cutoff, invCutoff, 1.0f + ShadowCasterRadiusOuter);
			
			SGT_ShaderHelper.SetVector(surfaceMaterials, "shadowValues", shadowValues);
			
			if (atmosphereMaterial != null) atmosphereMaterial.SetVector("shadowValues", shadowValues);
			if (cloudsMaterials    != null) SGT_ShaderHelper.SetVector(cloudsMaterials, "shadowValues", shadowValues);
		}
		
		if (SGT_Helper.FlagIsSet(updateShader, ShaderFlags.SurfaceTextureDay) == true)
		{
			for (var i = 0; i < surfaceMaterials.Length; i++)
			{
				surfaceMaterials[i].SetTexture("dayTexture", surfaceTextureDay.GetTexture((CubemapFace)i));
			}
		}
		
		if (SGT_Helper.FlagIsSet(updateShader, ShaderFlags.SurfaceTextureNight) == true)
		{
			for (var i = 0; i < surfaceMaterials.Length; i++)
			{
				surfaceMaterials[i].SetTexture("nightTexture", surfaceTextureNight.GetTexture((CubemapFace)i));
			}
		}
		
		if (SGT_Helper.FlagIsSet(updateShader, ShaderFlags.SurfaceTextureNormal) == true)
		{
			for (var i = 0; i < surfaceMaterials.Length; i++)
			{
				surfaceMaterials[i].SetTexture("normalTexture", surfaceTextureNormal.GetTexture((CubemapFace)i));
			}
		}
		
		if (SGT_Helper.FlagIsSet(updateShader, ShaderFlags.SurfaceTextureSpecular) == true)
		{
			for (var i = 0; i < surfaceMaterials.Length; i++)
			{
				surfaceMaterials[i].SetTexture("specularTexture", surfaceTextureSpecular.GetTexture((CubemapFace)i));
			}
		}
		
		if (SGT_Helper.FlagIsSet(updateShader, ShaderFlags.SurfaceSpecularSettings) == true)
		{
			SGT_ShaderHelper.SetFloat(surfaceMaterials, "specularPower", surfaceSpecularPower * surfaceSpecularPower);
		}
		
		if (SGT_Helper.FlagIsSet(updateShader, ShaderFlags.SurfaceTextureDetail) == true)
		{
			SGT_ShaderHelper.SetTexture(surfaceMaterials, "detailTexture", surfaceTextureDetail);
			SGT_ShaderHelper.SetFloat(surfaceMaterials, "detailRepeat", surfaceDetailRepeat);
		}
		
		if (SGT_Helper.FlagIsSet(updateShader, ShaderFlags.SurfaceTextureLighting) == true)
		{
			SGT_ShaderHelper.SetTexture(surfaceMaterials, "lightingTexture", surfaceLightingTexture);
		}
		
		if (SGT_Helper.FlagIsSet(updateShader, ShaderFlags.SurfaceTextureShadow) == true)
		{
			SGT_ShaderHelper.SetTexture(surfaceMaterials, "shadowTexture", shadowTextureSurface);
		}
		
		if (atmosphereMaterial != null)
		{
			SGT_ShaderHelper.SetFloat(surfaceMaterials, "atmosphereRadius", AtmosphereRadius * uniformScale);
			SGT_ShaderHelper.SetFloat(surfaceMaterials, "atmosphereFalloff", atmosphereFalloffSurface);
			SGT_ShaderHelper.SetFloat(surfaceMaterials, "surfaceRadius", surfaceRadius * uniformScale);
			SGT_ShaderHelper.SetFloat(surfaceMaterials, "atmosphereHeight", atmosphereHeight * uniformScale);
			
			if (SGT_Helper.FlagIsSet(updateShader, ShaderFlags.SurfaceTextureAtmosphere) == true)
			{
				SGT_ShaderHelper.SetTexture(surfaceMaterials, "atmosphereTexture", atmosphereSurfaceTexture);
			}
			
			if (SGT_Helper.FlagIsSet(updateShader, ShaderFlags.AtmosphereTexture) == true)
			{
				atmosphereMaterial.SetTexture("atmosphereTexture", atmosphereTexture);
			}
			
			if (SGT_Helper.FlagIsSet(updateShader, ShaderFlags.AtmosphereTextureShadow) == true)
			{
				atmosphereMaterial.SetTexture("shadowTexture", shadowTextureAtmosphere);
			}
			
			if (SGT_Helper.FlagIsSet(updateShader, ShaderFlags.ScatteringSettings) == true)
			{
				var mie  = -(1.0f - 1.0f / Mathf.Pow(10.0f, atmosphereScatteringMie * 5.0f));
				var mie4 = new Vector4(mie * 2.0f, 1.0f - mie * mie, 1.0f + mie * mie, 1.5f);
				var ray2 = new Vector2(0.0f, atmosphereScatteringRayleigh);
				
				SGT_ShaderHelper.SetVector(surfaceMaterials, "rayleighValues", ray2);
				
				atmosphereMaterial.SetVector("mieValues", mie4);
				atmosphereMaterial.SetVector("rayleighValues", ray2);
			}
		}
		
		if (cloudsMaterials != null)
		{
			if (SGT_Helper.FlagIsSet(updateShader, ShaderFlags.CloudsTextureShadow) == true)
			{
				SGT_ShaderHelper.SetTexture(cloudsMaterials, "shadowTexture", shadowTextureClouds);
			}
			
			if (SGT_Helper.FlagIsSet(updateShader, ShaderFlags.CloudsTexture) == true)
			{
				for (var i = 0; i < cloudsMaterials.Length; i++)
				{
					cloudsMaterials[i].SetTexture("cloudsTexture", cloudsTexture.GetTexture((CubemapFace)i));
				}
			}
			
			if (SGT_Helper.FlagIsSet(updateShader, ShaderFlags.CloudsTextureLighting) == true)
			{
				SGT_ShaderHelper.SetTexture(cloudsMaterials, "lightingTexture", cloudsLightingTexture);
			}
			
			if (SGT_Helper.FlagIsSet(updateShader, ShaderFlags.CloudsFalloffSettings) == true)
			{
				SGT_ShaderHelper.SetFloat(cloudsMaterials, "falloff", cloudsFalloff * cloudsFalloff);
			}
		}
		
		updateShader = 0;
	}
}