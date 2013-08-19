using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Gas Giant")]
public partial class SGT_GasGiant : SGT_MonoBehaviourUnique<SGT_GasGiant>
{
	public bool InsideGasGiant(Vector3 point)
	{
		point = transform.InverseTransformPoint(point);
		
		return SGT_Helper.PointInsideEllipse(new Vector2(gasGiantEquatorialRadius, GasGiantPolarRadius), point);
	}
	
	public float DistanceToSurface(Vector3 point)
	{
		point = transform.InverseTransformPoint(point);
		
		return SGT_Helper.DistanceToEllipse(new Vector2(gasGiantEquatorialRadius, GasGiantPolarRadius), point);
	}
	
	private Color SampleAtmosphereDay(Vector2 uv)
	{
		var tex    = atmosphereDayTexture as Texture2D;
		var colour = Color.gray;
		
		if (tex != null)
		{
			colour = tex.GetPixelBilinear(uv.x, uv.y);
		}
		
		return colour;
	}
	
	private Color SampleAtmosphereNight(Vector2 uv)
	{
		var tex    = atmosphereNightTexture as Texture2D;
		var colour = Color.black;
		
		if (tex != null)
		{
			colour = tex.GetPixelBilinear(uv.x, uv.y);
		}
		
		return colour;
	}
	
	private Color SampleLighting(Vector2 uv)
	{
		var tex    = lightingTexture as Texture2D;
		var colour = Color.white;
		
		if (tex != null)
		{
			colour = tex.GetPixelBilinear(uv.x, uv.y);
		}
		
		return colour;
	}
	
	private void UpdateTransform()
	{
		//var starObserverPosition  = SGT_Helper.GetPosition(gasGiantObserver);
		//var starObserverDirection = (starObserverPosition - atmosphereGameObject.transform.position).normalized;
		var atmosphereScale       = new Vector3(gasGiantEquatorialRadius, GasGiantPolarRadius, gasGiantEquatorialRadius);
		
		SGT_Helper.SetLocalScale(oblatenessGameObject.transform, atmosphereScale);
		
		// TODO: Make this work
		/*
		if (atmosphereGameObject.transform.up != starObserverDirection)
		{
			atmosphereGameObject.transform.up = starObserverDirection;
			
			SGT_Helper.UpdateNonOrthogonalTransform(atmosphereGameObject.transform);
		}
		*/
	}
	
	// TODO: Support multiple intersection
	public static bool ColourToPoint(Vector3 observer, Vector3 point, float searchDelay, bool ignoreCase1, bool ignoreCase2, out Color finalColour)
	{
		finalColour = Color.clear;
		
		var gasGiants = SGT_CachedFind<SGT_GasGiant>.All(searchDelay);
		
		foreach (var gasGiant in gasGiants)
		{
			if (gasGiant != null && gasGiant.atmosphereGameObject != null)
			{
				var oInside = gasGiant.InsideGasGiant(observer);
				var pInside = gasGiant.InsideGasGiant(point);
				
				// The gas giant itself will provide the colour in these cases
				if (ignoreCase1 == true)
				{
					if (oInside == true && pInside == false)
					{
						continue;
					}
				}
				
				if (ignoreCase2 == true)
				{
					if (oInside == false && pInside == false)
					{
						continue;
					}
				}
				
				//var oblateFix = 1.0f / (1.0f - gasGiant.gasGiantOblateness);
				var near      = gasGiant.atmosphereGameObject.transform.InverseTransformPoint(observer);
				var far       = gasGiant.atmosphereGameObject.transform.InverseTransformPoint(point);
				var far2      = far; // This is the actual far point on the gas giant's surface
				var ray       = (far - near).normalized;
				
				if (oInside == false)
				{
					var dist = 0.0f;
					
					if (SGT_Helper.IntersectRayToSphereA(near, ray, Vector3.zero, 1.0f, out dist) == false)
					{
						continue;
					}
					
					near = (near + ray * dist);
				}
				
				if (pInside == false)
				{
					var dist = 0.0f;
					
					if (SGT_Helper.IntersectRayToSphereA(far, -ray, Vector3.zero, 1.0f, out dist) == false)
					{
						continue;
					}
					
					far  = (far - ray * dist);
					far2 = far;
				}
				else
				{
					var dist = 0.0f;
					
					if (SGT_Helper.IntersectRayToSphereB(far, -ray, Vector3.zero, 1.0f, out dist) == false)
					{
						continue;
					}
					
					far2 = (far2 + ray * dist).normalized;
				}
				
				var polar    = SGT_Helper.CartesianToPolarUV(near);
				var nearDir  = near.normalized;
				var lightDir = gasGiant.atmosphereGameObject.transform.InverseTransformDirection(gasGiant.GasGiantLightSourceDirection);
				var lightU   = Vector3.Dot(nearDir, lightDir) * 0.5f + 0.5f;
				var lightV   = 1.0f - Vector3.Dot(ray, far2);
				
				var day      = gasGiant.SampleAtmosphereDay(polar);
				var night    = gasGiant.SampleAtmosphereNight(polar);
				var lighting = gasGiant.SampleLighting(new Vector2(lightU, lightV));
				
				var falloff      = gasGiant.atmosphereDensityFalloff * gasGiant.atmosphereDensityFalloff;
				var depthRatio   = ((near - far).magnitude * gasGiant.gasGiantEquatorialRadius) / gasGiant.maxDepth;
				var opticalDepth = Mathf.Pow(Mathf.Clamp01(SGT_Helper.Expose(depthRatio)), falloff);
				
				finalColour = SGT_Helper.Lerp(night, day, lighting);
				finalColour.a = opticalDepth;
				
				return true;
			}
		}
		
		return false;
	}
	
	private void UpdateGradient()
	{
		if (atmosphereLightingColour.Modified == true || atmosphereTwilightColour.Modified == true || atmosphereLimbColour.Modified == true)
		{
			lightingTexture = SGT_Helper.DestroyObject(lightingTexture);
		}
		
		atmosphereLightingColour.Modified = false;
		atmosphereTwilightColour.Modified = false;
		atmosphereLimbColour.Modified     = false;
		
		if (lightingTexture == null)
		{
			var lightingColours = atmosphereLightingColour.CalculateColours(0.0f, 1.0f, (int)lutSize);
			var twilightColours = atmosphereTwilightColour.CalculateColours(0.0f, 1.0f, (int)lutSize);
			var limbColours     = atmosphereLimbColour.CalculateColours(0.0f, 1.0f, (int)lutSize);
			
			lightingTexture = SGT_ColourGradient.AllocateTexture((int)lutSize, (int)lutSize);
			
			for (var y = 0; y < (int)lutSize; y++)
			{
				var limbPixel = limbColours[y];
				
				for (var x = 0; x < (int)lutSize; x++)
				{
					var lightingPixel = lightingColours[x];
					var twilightPixel = twilightColours[x];
					var colour        = Color.white;
					
					colour = SGT_Helper.AlphaBlend(colour, twilightPixel);
					colour = SGT_Helper.AlphaBlend(colour, limbPixel);
					colour = colour * lightingPixel;
					
					lightingTexture.SetPixel(x, y, colour);
				}
			}
			
			lightingTexture.Apply();
		}
	}
	
	private void UpdateMaterial()
	{
		var targetAtmosphereTechnique = "Variant";
		
		if (gasGiantObserver != null)
		{
			if (InsideGasGiant(gasGiantObserver.transform.position) == false)
			{
				targetAtmosphereTechnique += "Outer";
			}
		}
		
		if (shadow == true)
		{
			switch (shadowType)
			{
				case SGT_ShadowOccluder.Ring:   targetAtmosphereTechnique += "RingShadow";   break;
				case SGT_ShadowOccluder.Planet: targetAtmosphereTechnique += "PlanetShadow"; break;
			}
		}
		
		// Update surface?
		if (atmosphereMaterial == null || atmosphereTechnique != targetAtmosphereTechnique)
		{
			SGT_Helper.DestroyObject(atmosphereMaterial);
			
			atmosphereTechnique = targetAtmosphereTechnique;
			atmosphereMaterial  = SGT_Helper.CreateMaterial("Hidden/SGT/GasGiant/" + atmosphereTechnique, atmosphereRenderQueue);
		}
		else
		{
			SGT_Helper.SetRenderQueue(atmosphereMaterial, atmosphereRenderQueue);
		}
	}
	
	private void UpdateShader()
	{
		var uniformScale        = UniformScale;
		var invScale            = SGT_MatrixHelper.Scaling(SGT_Helper.Reciprocal(new Vector3(1.0f, 1.0f - gasGiantOblateness, 1.0f)));
		var rot                 = SGT_MatrixHelper.Rotation(transform.rotation);
		var invRot              = SGT_MatrixHelper.Rotation(Quaternion.Inverse(transform.rotation));
		var ellipsoid2sphere    = rot * invScale * invRot;
		var sphere2ellipsoid    = ellipsoid2sphere.inverse;
		var lightSourcePosition = SGT_Helper.GetPosition(gasGiantLightSource);
		
		maxDepth = (gasGiantEquatorialRadius * uniformScale) / (atmosphereDensity * atmosphereDensity);
		
		// Update shader variables
		atmosphereMaterial.SetTexture("dayTexture", atmosphereDayTexture);
		atmosphereMaterial.SetTexture("nightTexture", atmosphereNightTexture);
		atmosphereMaterial.SetTexture("lightingTexture", lightingTexture);
		atmosphereMaterial.SetMatrix("ellipsoid2sphere", ellipsoid2sphere);
		atmosphereMaterial.SetVector("centrePosition", transform.position);
		atmosphereMaterial.SetVector("starDirection", GasGiantLightSourceDirection);
		atmosphereMaterial.SetFloat("falloff", atmosphereDensityFalloff * atmosphereDensityFalloff);
		atmosphereMaterial.SetFloat("maxDepth", maxDepth);
		
		if (shadow == true)
		{
			var cutoff       = ShadowCasterRadiusInner / ShadowCasterRadiusOuter;
			var invCutoff    = SGT_Helper.Reciprocal(1.0f - cutoff);
			var shadowValues = new Vector3(cutoff, invCutoff, 1.0f + ShadowCasterRadiusOuter);
			var shadowMatrix = Matrix4x4.identity;
			
			switch (shadowType)
			{
				case SGT_ShadowOccluder.Ring:
				{
					shadowMatrix = SGT_Helper.CalculateCircleShadowMatrix(ShadowCasterRadiusOuter, transform.position, transform.up, lightSourcePosition, transform.position, uniformScale);
					
					shadowMatrix *= SGT_MatrixHelper.Translation(transform.position); // This is to remove one instruction in the VS
					shadowMatrix *= sphere2ellipsoid;
				}
				break;
				
				case SGT_ShadowOccluder.Planet:
				{
					if (shadowGameObject != null)
					{
						shadowMatrix = SGT_Helper.CalculateSphereShadowMatrix(ShadowCasterRadiusOuter, shadowGameObject.transform.position, lightSourcePosition, transform.position, uniformScale);
						
						shadowMatrix *= SGT_MatrixHelper.Translation(transform.position); // This is to remove one instruction in the VS
					}
				}
				break;
			}
			
			atmosphereMaterial.SetTexture("shadowTexture", shadowTexture);
			atmosphereMaterial.SetMatrix("shadowMatrix", shadowMatrix);
			atmosphereMaterial.SetVector("shadowValues", shadowValues);
		}
	}
}