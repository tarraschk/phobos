using UnityEngine;
using SGT_Internal;

public partial class SGT_Planet
{
	public void Awake()
	{
		if (ThisHasBeenDuplicated("surfaceMaterials", "atmosphereMaterial", "cloudsMaterials", "surfaceLightingTexture", "atmosphereTexture", "atmosphereSurfaceTexture", "cloudsLightingTexture") == true)
		{
			surfaceLightingTexture   = SGT_Helper.CloneObject(surfaceLightingTexture);
			atmosphereTexture        = SGT_Helper.CloneObject(atmosphereTexture);
			atmosphereSurfaceTexture = SGT_Helper.CloneObject(atmosphereSurfaceTexture);
			cloudsLightingTexture    = SGT_Helper.CloneObject(cloudsLightingTexture);
			surfaceMaterials         = SGT_Helper.CloneObjects(surfaceMaterials);
			atmosphereMaterial       = SGT_Helper.CloneObject(atmosphereMaterial);
			cloudsMaterials          = SGT_Helper.CloneObjects(cloudsMaterials);
		}
	}
	
	public new void Start()
	{
		base.Start();
		
		SendMessage("SetSurfaceConfiguration", SurfaceConfiguration);
	}
	
	public void LateUpdate()
	{
		if (surfaceGameObject      == null) surfaceGameObject      = SGT_Helper.CreateGameObject("Surface", gameObject);
		if (atmosphereGameObject   == null) atmosphereGameObject   = SGT_Helper.CreateGameObject("Atmosphere", gameObject);
		if (cloudsGameObject       == null) cloudsGameObject       = SGT_Helper.CreateGameObject("Clouds", gameObject);
		if (surfaceMesh            == null) surfaceMesh            = new SGT_SurfaceMultiMesh();
		if (surfaceTextureDay      == null) surfaceTextureDay      = new SGT_SurfaceTexture();
		if (surfaceTextureNight    == null) surfaceTextureNight    = new SGT_SurfaceTexture();
		if (surfaceTextureNormal   == null) surfaceTextureNormal   = new SGT_SurfaceTexture();
		if (surfaceTextureSpecular == null) surfaceTextureSpecular = new SGT_SurfaceTexture();
		if (planetObserver         == null) planetObserver         = SGT_Helper.FindCamera();
		if (planetLightSource      == null) planetLightSource      = SGT_LightSource.Find();
		
		SGT_Helper.SetParent(surfaceGameObject, gameObject);
		SGT_Helper.SetLayer(surfaceGameObject, gameObject.layer);
		SGT_Helper.SetTag(surfaceGameObject, gameObject.tag);
		
		SGT_Helper.SetParent(atmosphereGameObject, gameObject);
		SGT_Helper.SetLayer(atmosphereGameObject, gameObject.layer);
		SGT_Helper.SetTag(atmosphereGameObject, gameObject.tag);
		
		SGT_Helper.SetParent(cloudsGameObject, gameObject);
		SGT_Helper.SetLayer(cloudsGameObject, gameObject.layer);
		SGT_Helper.SetTag(cloudsGameObject, gameObject.tag);
		
		surfaceTextureDay.Configuration      = surfaceMesh.Configuration;
		surfaceTextureNight.Configuration    = surfaceMesh.Configuration;
		surfaceTextureNormal.Configuration   = surfaceMesh.Configuration;
		surfaceTextureSpecular.Configuration = surfaceMesh.Configuration;
		
		if (surfaceTextureDay.Modified      == true) { updateShader |= ShaderFlags.SurfaceTextureDay;      surfaceTextureDay.Modified      = false; }
		if (surfaceTextureNight.Modified    == true) { updateShader |= ShaderFlags.SurfaceTextureNight;    surfaceTextureNight.Modified    = false; }
		if (surfaceTextureNormal.Modified   == true) { updateShader |= ShaderFlags.SurfaceTextureNormal;   surfaceTextureNormal.Modified   = false; }
		if (surfaceTextureSpecular.Modified == true) { updateShader |= ShaderFlags.SurfaceTextureSpecular; surfaceTextureSpecular.Modified = false; }
		
		if (atmosphere == true)
		{
			if (atmosphereMesh == null) atmosphereMesh = new SGT_Mesh();
		}
		else
		{
			if (atmosphereMesh != null) atmosphereMesh.Clear();
		}
		
		if (clouds == true)
		{
			if (cloudsMesh    == null) cloudsMesh    = new SGT_SurfaceMultiMesh();
			if (cloudsTexture == null) cloudsTexture = new SGT_SurfaceTexture();
			
			if (cloudsTexture.Modified == true) { updateShader |= ShaderFlags.CloudsTexture; cloudsTexture.Modified = false; }
		}
		else
		{
			if (cloudsTexture != null) cloudsTexture = null;
			if (cloudsMesh    != null) cloudsMesh.Clear();
		}
		
		if (planetLighting == null)
		{
			planetLighting = new SGT_ColourGradient(false, false);
			planetLighting.AddColourNode(Color.black, 0.45f);
			planetLighting.AddColourNode(Color.white, 0.55f);
		}
		
		if (atmosphereTwilightColour == null)
		{
			atmosphereTwilightColour = new SGT_ColourGradient(false, true);
			atmosphereTwilightColour.AddAlphaNode(1.0f, 0.45f);
			atmosphereTwilightColour.AddAlphaNode(0.0f, 0.65f);
			atmosphereTwilightColour.AddColourNode(new Color(1.0f, 0.19f, 0.0f, 1.0f), 0.5f);
		}
		
		if (atmosphereDensityColour == null)
		{
			atmosphereDensityColour = new SGT_ColourGradient(false, true);
			atmosphereDensityColour.AddColourNode(new Color(0.17f, 0.53f, 0.85f), 0.2f);
			atmosphereDensityColour.AddColourNode(Color.white, 0.5f).Locked = true;
			atmosphereDensityColour.AddColourNode(new Color(0.17f, 0.51f, 1.0f), 1.0f);
		}
		
		if (cloudsLimbColour == null)
		{
			cloudsLimbColour = new SGT_ColourGradient(false, true);
			cloudsLimbColour.AddColourNode(Color.white, 0.5f);
		}
		
		// Rotate?
		if (Application.isPlaying == true)
		{
			if (cloudsRotationPeriod != 0.0f)
			{
				cloudsGameObject.transform.Rotate(0.0f, SGT_Helper.DegreesPerSecond(cloudsRotationPeriod) * Time.smoothDeltaTime, 0.0f);
			}
		}
		
		// Update scales
		SGT_Helper.SetLocalScale(surfaceGameObject.transform, surfaceRadius);
		
		if (atmosphere == true)
		{
			SGT_Helper.SetLocalScale(atmosphereGameObject.transform, AtmosphereRadius);
			
			// Point atmosphere at camera
			if (planetObserver != null)
			{
				var observerPosition  = planetObserver.transform.position;
				var observerDirection = (observerPosition - gameObject.transform.position).normalized;
				
				SGT_Helper.SetUp(atmosphereGameObject.transform, observerDirection);
			}
		}
		
		if (clouds == true)
		{
			SGT_Helper.SetLocalScale(cloudsGameObject.transform, CloudsRadius);
			
			UpdateCloudsOffset();
		}
		
		UpdateGradient();
		UpdateTechnique();
		UpdateShader();
		
		surfaceMesh.GameObject           = surfaceGameObject;
		surfaceMesh.HasMeshRenderers     = true;
		surfaceMesh.MeshRenderersEnabled = true;
		surfaceMesh.MeshCollidersEnabled = true;
		surfaceMesh.SetSharedMaterials(surfaceMaterials);
		surfaceMesh.Update(gameObject.layer, gameObject.tag);
		
		if (atmosphere == true)
		{
			atmosphereMesh.GameObject          = atmosphereGameObject;
			atmosphereMesh.HasMeshRenderer     = true;
			atmosphereMesh.MeshRendererEnabled = true;
			atmosphereMesh.SharedMaterial      = atmosphereMaterial;
			atmosphereMesh.Update();
		}
		
		if (clouds == true)
		{
			cloudsMesh.GameObject           = cloudsGameObject;
			cloudsMesh.HasMeshRenderers     = true;
			cloudsMesh.MeshRenderersEnabled = true;
			cloudsMesh.SetSharedMaterials(cloudsMaterials);
			cloudsMesh.Update(gameObject.layer, gameObject.tag);
		}
		
#if UNITY_EDITOR == true
		surfaceMesh.HideInEditor();
		if (atmosphereMesh != null) atmosphereMesh.HideInEditor();
		if (cloudsMesh     != null) cloudsMesh.HideInEditor();
		
		SGT_Helper.HideGameObject(atmosphereGameObject);
		SGT_Helper.HideGameObject(cloudsGameObject);
#endif
	}
	
	public void OnEnable()
	{
		if (surfaceMesh    != null) surfaceMesh.OnEnable();
		if (atmosphereMesh != null) atmosphereMesh.OnEnable();
		if (cloudsMesh     != null) cloudsMesh.OnEnable();
	}
	
	public void OnDisable()
	{
		if (surfaceMesh    != null) surfaceMesh.OnDisable();
		if (atmosphereMesh != null) atmosphereMesh.OnDisable();
		if (cloudsMesh     != null) cloudsMesh.OnDisable();
	}
	
	public new void OnDestroy()
	{
		base.OnDestroy();
		
		SGT_Helper.DestroyGameObject(surfaceGameObject);
		SGT_Helper.DestroyGameObject(atmosphereGameObject);
		SGT_Helper.DestroyGameObject(cloudsGameObject);
		
		SGT_Helper.DestroyObjects(surfaceMaterials);
		SGT_Helper.DestroyObject(atmosphereMaterial);
		SGT_Helper.DestroyObjects(cloudsMaterials);
		
		SGT_Helper.DestroyObject(surfaceLightingTexture);
		SGT_Helper.DestroyObject(atmosphereTexture);
		SGT_Helper.DestroyObject(atmosphereSurfaceTexture);
		SGT_Helper.DestroyObject(cloudsLightingTexture);
	}
	
	public void CopySurfaceMultiMeshInto(SGT_SurfaceMultiMesh target)
	{
		if (target != null && surfaceMesh != null)
		{
			surfaceMesh.CopyMeshesInto(target);
		}
	}
	
	public void SetSurfaceMultiMesh(SGT_SurfaceMultiMesh newSurfaceMultiMesh)
	{
		if (newSurfaceMultiMesh != null)
		{
			if (surfaceMesh == null) surfaceMesh = new SGT_SurfaceMultiMesh();
			
			newSurfaceMultiMesh.CopyMeshesInto(surfaceMesh);
		}
	}
	
	public void SetSurfaceConfiguration(SGT_SurfaceConfiguration newConfiguration)
	{
		if (surfaceMesh == null) surfaceMesh = new SGT_SurfaceMultiMesh();
		
		if (newConfiguration != surfaceMesh.Configuration)
		{
			surfaceMesh.Configuration = newConfiguration;
		}
	}
	
	public void FillSurfaceObserverPosition(SGT_FillVector3 fill)
	{
		if (fill != null && surfaceGameObject != null && planetObserver != null)
		{
			fill.Vector3 = surfaceGameObject.transform.InverseTransformPoint(planetObserver.transform.position);
		}
	}
	
	public void FillSurfaceGameObject(SGT_FillGameObject fill)
	{
		if (surfaceGameObject != null)
		{
			fill.GameObject = surfaceGameObject;
		}
	}
	
	public void FillShadowRadius(SGT_FillFloat fill)
	{
		fill.Float = surfaceRadius;
	}
}