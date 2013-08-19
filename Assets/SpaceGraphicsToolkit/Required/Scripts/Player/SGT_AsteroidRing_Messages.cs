using UnityEngine;
using SGT_Internal;

public partial class SGT_AsteroidRing
{
	public void Awake()
	{
		if (ThisHasBeenDuplicated("ringMaterial", "generatedMeshes") == true)
		{
			ringMaterial    = SGT_Helper.CloneObject(ringMaterial);
			generatedMeshes = SGT_Helper.CloneObjects(generatedMeshes);
		}
	}
	
	public void LateUpdate()
	{
		if (ringGameObject  == null) ringGameObject  = SGT_Helper.CreateGameObject("Ring", gameObject);
		if (ringMultiMesh   == null) ringMultiMesh   = new SGT_MultiMesh();
		if (ringLightSource == null) ringLightSource = SGT_LightSource.Find();
		
		SGT_Helper.SetParent(ringGameObject, gameObject);
		SGT_Helper.SetLayer(ringGameObject, gameObject.layer);
		SGT_Helper.SetTag(ringGameObject, gameObject.tag);
		
		if (ringAutoRegen == true)
		{
			Regenerate();
		}
		
		if (shadowAutoUpdate == true)
		{
			var fill = new SGT_FillFloat();
			
			SendMessage("FillShadowRadius", fill, SendMessageOptions.DontRequireReceiver);
			
			shadowRadius = fill.Float;
		}
		
		UpdateTechnique();
		UpdateShader();
		
		ringMultiMesh.GameObject           = ringGameObject;
		ringMultiMesh.HasMeshRenderers     = true;
		ringMultiMesh.MeshRenderersEnabled = true;
		ringMultiMesh.SharedMaterial       = ringMaterial;
		ringMultiMesh.ReplaceAll(generatedMeshes);
		ringMultiMesh.Update();
		
#if UNITY_EDITOR == true
		ringMultiMesh.HideInEditor();
#endif
	}
	
	public void OnEnable()
	{
		if (ringMultiMesh != null) ringMultiMesh.OnEnable();
	}
	
	public void OnDisable()
	{
		if (ringMultiMesh != null) ringMultiMesh.OnDisable();
	}
	
	public new void OnDestroy()
	{
		base.OnDestroy();
		
		SGT_Helper.DestroyObject(ringGameObject);
		SGT_Helper.DestroyObjects(generatedMeshes);
		SGT_Helper.DestroyObject(ringMaterial);
	}
	
	public void ListenLightSource(SGT_LightSource listen)
	{
		ringLightSource = listen;
	}
	
#if UNITY_EDITOR == true
	protected virtual void OnDrawGizmosSelected()
	{
		SGT_Handles.Colour = new Color(1.0f, 1.0f, 1.0f, 0.5f); SGT_Handles.DrawDisc(transform.position + transform.up * ringHeight * 0.5f, transform.rotation, RingRadiusInner, RingRadiusOuter);
		SGT_Handles.Colour = new Color(1.0f, 1.0f, 1.0f, 0.5f); SGT_Handles.DrawDisc(transform.position - transform.up * ringHeight * 0.5f, transform.rotation, RingRadiusInner, RingRadiusOuter);
	}
#endif
}