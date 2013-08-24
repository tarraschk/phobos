using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Star")]
public partial class SGT_Star : SGT_MonoBehaviourUnique<SGT_Star>
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
	
	public float AtmosphereHeightAtPoint(Vector3 point)
	{
		if (atmosphereGameObject != null)
		{
			point = atmosphereGameObject.transform.InverseTransformPoint(point);
			
			return SGT_Helper.RemapClamped(SurfaceEquatorialRadius / AtmosphereEquatorialRadius, 1.0f, point.magnitude, 0.0f, 1.0f);
		}
		
		return 0.0f;
	}
	
	public float SurfaceRadiusAtPoint(Vector3 point)
	{
		point = transform.InverseTransformPoint(point);
		
		return SGT_Helper.EllipseRadius(new Vector2(SurfaceEquatorialRadius, SurfacePolarRadius), point);
	}
	
	private void UpdateTransform()
	{
		var oblatenessScale       = new Vector3(1.0f, 1.0f - surfaceOblateness, 1.0f);
		var surfaceScale          = SGT_Helper.NewVector3(SurfaceEquatorialRadius);
		var atmosphereScale       = SGT_Helper.NewVector3(AtmosphereEquatorialRadius);
		var starObserverPosition  = SGT_Helper.GetPosition(starObserver);
		var starObserverDirection = (starObserverPosition - atmosphereGameObject.transform.position).normalized;
		
		SGT_Helper.SetLocalScale(oblatenessGameObject.transform, oblatenessScale);
		SGT_Helper.SetLocalScale(surfaceGameObject.transform, surfaceScale);
		SGT_Helper.SetLocalScale(atmosphereGameObject.transform, atmosphereScale);
		
		if (atmosphereGameObject.transform.up != starObserverDirection)
		{
			atmosphereGameObject.transform.up = starObserverDirection;
			
			SGT_Helper.UpdateNonOrthogonalTransform(atmosphereGameObject.transform);
		}
	}
	
	private void UpdateGradient()
	{
		if (atmosphereDensityColour.Modified == true)
		{
			atmosphereTexture = SGT_Helper.DestroyObject(atmosphereTexture);
		}
		
		atmosphereDensityColour.Modified = false;
		
		if (atmosphereTexture == null)
		{
			atmosphereDensityColour.Modified = false;
			
			SGT_Helper.DestroyObject(atmosphereTexture);
			SGT_Helper.DestroyObject(atmosphereSurfaceTexture);
			
			var density        = atmosphereDensityColour.CalculateColours(1.0f, 0.5f, (int)lutSize);
			var surfaceDensity = atmosphereDensityColour.CalculateColours(0.0f, 0.5f, (int)lutSize);
			
			atmosphereTexture        = SGT_ColourGradient.AllocateTexture((int)lutSize);
			atmosphereSurfaceTexture = SGT_ColourGradient.AllocateTexture((int)lutSize);
			
			for (var x = 0; x < (int)lutSize; x++)
			{
				atmosphereTexture.SetPixel(x, 0, density[x]);
			}
			
			for (var x = 0; x < (int)lutSize; x++)
			{
				atmosphereSurfaceTexture.SetPixel(x, 0, surfaceDensity[x]);
			}
			
			atmosphereTexture.Apply();
			atmosphereSurfaceTexture.Apply();
		}
	}
	
	private void UpdateMaterial()
	{
		var targetSurfaceTechnique    = "Variant";
		var targetAtmosphereTechnique = "Variant";
		
		if (starObserver != null)
		{
			if (InsideAtmosphere(starObserver.transform.position) == false)
			{
				targetSurfaceTechnique    += "Outer";
				targetAtmosphereTechnique += "Outer";
			}
		}
		
		if (atmosphereSurfacePerPixel == true)
		{
			targetSurfaceTechnique += "PerPixel";
		}
		
		// Update surface?
		if (surfaceMaterials == null || (surfaceTechnique != targetSurfaceTechnique || SGT_ArrayHelper.ContainsSomething(surfaceMaterials) == false || surfaceMaterials.Length != SGT_SurfaceConfiguration_.SurfaceCount(SurfaceConfiguration)))
		{
			SGT_Helper.DestroyObjects(surfaceMaterials);
			
			surfaceTechnique = targetSurfaceTechnique;
			surfaceMaterials = SGT_SurfaceConfiguration_.CreateMaterials(surfaceMultiMesh.Configuration, "Hidden/SGT/StarSurface/" + surfaceTechnique, surfaceRenderQueue);
		}
		else
		{
			SGT_Helper.SetRenderQueues(surfaceMaterials, surfaceRenderQueue);
		}
		
		// Update atmosphere?
		if (atmosphereTechnique != targetAtmosphereTechnique || atmosphereMaterial == null)
		{
			SGT_Helper.DestroyObject(atmosphereMaterial);
			
			atmosphereTechnique = targetAtmosphereTechnique;
			atmosphereMaterial  = SGT_Helper.CreateMaterial("Hidden/SGT/StarAtmosphere/" + atmosphereTechnique, atmosphereRenderQueue);
		}
		else
		{
			SGT_Helper.SetRenderQueue(atmosphereMaterial, atmosphereRenderQueue);
		}
	}
	
	private void UpdateShader()
	{
		var uniformScale     = UniformScale;
		var centrePosition   = transform.position;
		var observerPosition = SGT_Helper.GetPosition(starObserver);
		
		var distance   = AtmosphereHeightAtPoint(observerPosition);
		var altitude01 = SGT_Helper.Remap(0.0f, 1.0f, distance, atmosphereSkyAltitude, 1.0f);
		
		var invScale         = SGT_MatrixHelper.Scaling(SGT_Helper.Reciprocal(new Vector3(1.0f, 1.0f - surfaceOblateness, 1.0f)));
		var rot              = SGT_MatrixHelper.Rotation(transform.rotation);
		var invRot           = SGT_MatrixHelper.Rotation(Quaternion.Inverse(transform.rotation));
		var ellipsoid2sphere = rot * invScale * invRot;
		
		var rel  = ellipsoid2sphere.MultiplyPoint(observerPosition - centrePosition);
		var dist = rel.magnitude;
		
		float maxSurfaceDepth, maxAtmosphereDepth;
		
		SGT_Helper.CalculateHorizonAtmosphereDepth(surfaceRadius * uniformScale, AtmosphereEquatorialRadius * uniformScale, dist, out maxSurfaceDepth, out maxAtmosphereDepth);
		
		// Update shader variables
		for (var i = 0; i < surfaceMaterials.Length; i++)
		{
			surfaceMaterials[i].SetTexture("surfaceTexture", surfaceTexture.GetTexture((CubemapFace)i));
		}
		
		SGT_ShaderHelper.SetTexture(surfaceMaterials, "atmosphereTexture", atmosphereSurfaceTexture);
		SGT_ShaderHelper.SetVector(surfaceMaterials, "centrePosition", centrePosition);
		SGT_ShaderHelper.SetFloat(surfaceMaterials, "atmosphereRadius", AtmosphereEquatorialRadius * uniformScale);
		SGT_ShaderHelper.SetFloat(surfaceMaterials, "atmosphereFalloff", atmosphereSurfaceFalloff);
		SGT_ShaderHelper.SetFloat(surfaceMaterials, "maxDepth", maxSurfaceDepth * (1.0f - atmosphereFog)); // FOG
		SGT_ShaderHelper.SetFloat(surfaceMaterials, "surfaceRadius", surfaceRadius * uniformScale);
		SGT_ShaderHelper.SetFloat(surfaceMaterials, "atmosphereHeight", atmosphereHeight * uniformScale);
		SGT_ShaderHelper.SetMatrix(surfaceMaterials, "ellipsoid2sphere", ellipsoid2sphere);
		
		atmosphereMaterial.SetTexture("atmosphereTexture", atmosphereTexture);
		atmosphereMaterial.SetVector("centrePosition", centrePosition);
		atmosphereMaterial.SetFloat("atmosphereFalloff", Mathf.Lerp(atmosphereSkyFalloff, atmosphereFalloff, altitude01));
		atmosphereMaterial.SetFloat("maxDepth", maxAtmosphereDepth);
		atmosphereMaterial.SetMatrix("ellipsoid2sphere", ellipsoid2sphere);
	}
}