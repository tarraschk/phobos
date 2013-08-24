using UnityEngine;
using VariantList = System.Collections.Generic.List<SGT_StarfieldStarVariant>;

public partial class SGT_Starfield
{
	public void Awake()
	{
		if (ThisHasBeenDuplicated("starfieldMaterial", "meshes") == true)
		{
			starfieldMaterial = SGT_Helper.CloneObject(starfieldMaterial);
			meshes            = SGT_Helper.CloneObjects(meshes);
			
			if (packer != null) packer.Duplicated();
		}
	}
	
	public void LateUpdate()
	{
		if (starfieldGameObject == null) starfieldGameObject = SGT_Helper.CreateGameObject("Starfield", gameObject);
		if (starfieldMultiMesh  == null) starfieldMultiMesh  = new SGT_MultiMesh();
		if (starfieldObserver   == null) starfieldObserver   = SGT_Helper.FindCamera();
		if (packer              == null) packer              = new SGT_Packer();
		if (starVariants        == null) starVariants        = new VariantList();
		
		SGT_Helper.SetParent(starfieldGameObject, gameObject);
		SGT_Helper.SetLayer(starfieldGameObject, gameObject.layer);
		SGT_Helper.SetTag(starfieldGameObject, gameObject.tag);
		
		packer.AtlasFormat     = TextureFormat.RGB24;
		packer.AtlasMaxSize    = SGT_SquareSize.Square2048;
		packer.AtlasFilterMode = FilterMode.Trilinear;
		packer.AtlasCountMax   = 1;
		
		if (starfieldAutoRegen == true)
		{
			Regenerate();
		}
		
		UpdateMaterial();
		UpdateShader();
		UpdateBackground();
		
		starfieldMultiMesh.GameObject           = starfieldGameObject;
		starfieldMultiMesh.HasMeshRenderers     = true;
		starfieldMultiMesh.MeshRenderersEnabled = true;
		starfieldMultiMesh.SharedMaterial       = starfieldMaterial;
		starfieldMultiMesh.ReplaceAll(meshes);
		starfieldMultiMesh.Update();
		
#if UNITY_EDITOR == true
		starfieldMultiMesh.HideInEditor();
#endif
	}
	
	public void OnEnable()
	{
		if (starfieldMultiMesh != null) starfieldMultiMesh.OnEnable();
	}
	
	public void OnDisable()
	{
		if (starfieldMultiMesh != null) starfieldMultiMesh.OnDisable();
	}
	
	public new void OnDestroy()
	{
		base.OnDestroy();
		
		SGT_Helper.DestroyGameObject(starfieldGameObject);
		SGT_Helper.DestroyObject(starfieldMaterial);
		SGT_Helper.DestroyObjects(meshes);
		
		if (packer != null) packer.OnDestroy();
	}
}