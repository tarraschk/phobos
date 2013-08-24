using UnityEngine;
using SGT_Internal;

public partial class SGT_Star
{
	public void Awake()
	{
		if (ThisHasBeenDuplicated("atmosphereMaterial", "surfaceMaterials") == true)
		{
			surfaceMaterials         = SGT_Helper.CloneObjects(surfaceMaterials);
			atmosphereMaterial       = SGT_Helper.CloneObject(atmosphereMaterial);
			atmosphereTexture        = SGT_Helper.CloneObject(atmosphereTexture);
			atmosphereSurfaceTexture = SGT_Helper.CloneObject(atmosphereSurfaceTexture);
		}
	}
	
	public void LateUpdate()
	{
		if (oblatenessGameObject == null) oblatenessGameObject = SGT_Helper.CreateGameObject("Oblateness", gameObject);
		if (surfaceGameObject    == null) surfaceGameObject    = SGT_Helper.CreateGameObject("Surface", oblatenessGameObject);
		if (atmosphereGameObject == null) atmosphereGameObject = SGT_Helper.CreateGameObject("Atmosphere", oblatenessGameObject);
		if (atmosphereMesh       == null) atmosphereMesh       = new SGT_Mesh();
		if (starObserver         == null) starObserver         = SGT_Helper.FindCamera();
		if (surfaceTexture       == null) surfaceTexture       = new SGT_SurfaceTexture();
		if (surfaceMultiMesh     == null) surfaceMultiMesh     = new SGT_SurfaceMultiMesh();
		
		SGT_Helper.SetParent(oblatenessGameObject, gameObject);
		SGT_Helper.SetLayer(oblatenessGameObject, gameObject.layer);
		SGT_Helper.SetTag(oblatenessGameObject, gameObject.tag);
		
		SGT_Helper.SetParent(surfaceGameObject, oblatenessGameObject);
		SGT_Helper.SetLayer(surfaceGameObject, oblatenessGameObject.layer);
		SGT_Helper.SetTag(surfaceGameObject, oblatenessGameObject.tag);
		
		SGT_Helper.SetParent(atmosphereGameObject, oblatenessGameObject);
		SGT_Helper.SetLayer(atmosphereGameObject, oblatenessGameObject.layer);
		SGT_Helper.SetTag(atmosphereGameObject, oblatenessGameObject.tag);
		
		if (atmosphereDensityColour == null)
		{
			atmosphereDensityColour = new SGT_ColourGradient(false, true);
			atmosphereDensityColour.AddColourNode(new Color(1.0f, 1.0f, 0.0f, 1.0f), 0.0f);
			atmosphereDensityColour.AddColourNode(new Color(1.0f, 0.5f, 0.0f, 1.0f), 0.5f).Locked = true;
			atmosphereDensityColour.AddColourNode(new Color(1.0f, 0.0f, 0.0f, 1.0f), 1.0f);
		}
		
		UpdateTransform();
		UpdateGradient();
		UpdateMaterial();
		UpdateShader();
		
		surfaceMultiMesh.GameObject           = surfaceGameObject;
		surfaceMultiMesh.HasMeshRenderers     = true;
		surfaceMultiMesh.MeshRenderersEnabled = true;
		surfaceMultiMesh.MeshCollidersEnabled = true;
		surfaceMultiMesh.SetSharedMaterials(surfaceMaterials);
		surfaceMultiMesh.Update(gameObject.layer, gameObject.tag);
		
		atmosphereMesh.GameObject          = atmosphereGameObject;
		atmosphereMesh.HasMeshRenderer     = true;
		atmosphereMesh.MeshRendererEnabled = true;
		atmosphereMesh.SharedMaterial      = atmosphereMaterial;
		atmosphereMesh.Update();
		
#if UNITY_EDITOR == true
		SGT_Helper.HideGameObject(oblatenessGameObject);
		
		atmosphereMesh.HideInEditor();
		surfaceMultiMesh.HideInEditor();
#endif
	}
	
	public void OnEnable()
	{
		if (surfaceMultiMesh != null) surfaceMultiMesh.OnEnable();
		if (atmosphereMesh   != null) atmosphereMesh.OnEnable();
	}
	
	public void OnDisable()
	{
		if (surfaceMultiMesh != null) surfaceMultiMesh.OnDisable();
		if (atmosphereMesh   != null) atmosphereMesh.OnDisable();
	}
	
	public new void OnDestroy()
	{
		base.OnDestroy();
		
		SGT_Helper.DestroyGameObject(surfaceGameObject);
		SGT_Helper.DestroyGameObject(oblatenessGameObject);
		SGT_Helper.DestroyGameObject(atmosphereGameObject);
		SGT_Helper.DestroyObjects(surfaceMaterials);
		SGT_Helper.DestroyObject(atmosphereMaterial);
		SGT_Helper.DestroyObject(atmosphereTexture);
		SGT_Helper.DestroyObject(atmosphereSurfaceTexture);
	}
	
	public void CopySurfaceMultiMeshInto(SGT_SurfaceMultiMesh target)
	{
		if (target != null && surfaceMultiMesh != null)
		{
			surfaceMultiMesh.CopyMeshesInto(target);
		}
	}
	
	public void SetSurfaceMultiMesh(SGT_SurfaceMultiMesh newSurfaceMultiMesh)
	{
		if (newSurfaceMultiMesh != null)
		{
			if (surfaceMultiMesh == null) surfaceMultiMesh = new SGT_SurfaceMultiMesh();
			
			newSurfaceMultiMesh.CopyMeshesInto(surfaceMultiMesh);
		}
	}
	
	public void FillSurfaceObserverPosition(SGT_FillVector3 fill)
	{
		if (fill != null && surfaceGameObject != null && starObserver != null)
		{
			fill.Vector3 = surfaceGameObject.transform.InverseTransformPoint(starObserver.transform.position);
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