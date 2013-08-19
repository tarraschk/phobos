using UnityEngine;

public partial class SGT_Corona
{
	public void Awake()
	{
		if (ThisHasBeenDuplicated("generatedMesh", "coronaMaterial") == true)
		{
			generatedMesh  = SGT_Helper.CloneObject(generatedMesh);
			coronaMaterial = SGT_Helper.CloneObject(coronaMaterial);
		}
	}
	
	public void LateUpdate()
	{
		if (coronaGameObject == null) coronaGameObject = SGT_Helper.CreateGameObject("Corona", gameObject);
		if (coronaMesh       == null) coronaMesh       = new SGT_Mesh();
		if (coronaObserver   == null) coronaObserver   = SGT_Helper.FindCamera();
		
		SGT_Helper.SetParent(coronaGameObject, gameObject);
		SGT_Helper.SetLayer(coronaGameObject, gameObject.layer);
		SGT_Helper.SetTag(coronaGameObject, gameObject.tag);
		
		if (coronaAutoRegen == true)
		{
			Regenerate();
		}
		
		UpdateMaterial();
		UpdateShader();
		UpdateCoronaOffset();
		
		coronaMesh.GameObject          = coronaGameObject;
		coronaMesh.HasMeshRenderer     = true;
		coronaMesh.MeshRendererEnabled = true;
		coronaMesh.SharedMaterial      = coronaMaterial;
		coronaMesh.SharedMesh          = generatedMesh;
		coronaMesh.Update();
		
#if UNITY_EDITOR == true
		coronaMesh.HideInEditor();
#endif
	}
	
	public void OnEnable()
	{
		if (coronaMesh != null) coronaMesh.OnEnable();
	}
	
	public void OnDisable()
	{
		if (coronaMesh != null) coronaMesh.OnDisable();
	}
	
	public new void OnDestroy()
	{
		base.OnDestroy();
		
		SGT_Helper.DestroyGameObject(coronaGameObject);
		SGT_Helper.DestroyObject(generatedMesh);
		SGT_Helper.DestroyObject(coronaMaterial);
	}
	
#if UNITY_EDITOR == true
	protected virtual void OnDrawGizmosSelected()
	{
		SGT_Handles.Colour = new Color(1.0f, 1.0f, 1.0f, 0.5f); SGT_Handles.DrawSphere(transform.position, meshRadius);
		SGT_Handles.Colour = new Color(1.0f, 1.0f, 1.0f, 0.5f); SGT_Handles.DrawSphere(transform.position, meshRadius + meshHeight);
	}
#endif
}