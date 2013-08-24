using UnityEngine;
using SGT_Internal;

public partial class SGT_Ring
{
	public void Awake()
	{
		if (ThisHasBeenDuplicated("ringMaterial", "ringMesh") == true)
		{
			ringMaterial  = SGT_Helper.CloneObject(ringMaterial);
			generatedMesh = SGT_Helper.CloneObject(generatedMesh);
		}
	}
	
	public void LateUpdate()
	{
		if (ringGameObject == null) ringGameObject = SGT_Helper.CreateGameObject("Ring Slices", gameObject);
		if (ringMesh       == null) ringMesh       = new SGT_MultiMesh();
		if (lightSource    == null) lightSource    = SGT_LightSource.Find();
		
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
		
		UpdateMaterial();
		UpdateShader();
		
		ringMesh.GameObject           = ringGameObject;
		ringMesh.HasMeshRenderers     = true;
		ringMesh.MeshRenderersEnabled = true;
		ringMesh.SharedMaterial       = ringMaterial;
		ringMesh.SharedMesh           = generatedMesh;
		UpdateRingRotations();
		
#if UNITY_EDITOR == true
		ringMesh.HideInEditor();
#endif
	}
	
	public void OnEnable()
	{
		if (ringMesh != null) ringMesh.OnEnable();
	}
	
	public void OnDisable()
	{
		if (ringMesh != null) ringMesh.OnDisable();
	}
	
	public new void OnDestroy()
	{
		base.OnDestroy();
		
		SGT_Helper.DestroyGameObject(ringGameObject);
		SGT_Helper.DestroyObject(generatedMesh);
		SGT_Helper.DestroyObject(ringMaterial);
	}
	
	public void FillRingDimensions(SGT_FillRingDimensions fill)
	{
		fill.Radius = ringRadius;
		fill.Width  = ringWidth;
	}
	
#if UNITY_EDITOR == true
	protected virtual void OnDrawGizmosSelected()
	{
		SGT_Handles.Colour = new Color(1.0f, 1.0f, 1.0f, 0.5f); SGT_Handles.DrawDisc(transform.position, transform.rotation, RingRadiusInner);
		SGT_Handles.Colour = new Color(1.0f, 1.0f, 1.0f, 0.5f); SGT_Handles.DrawDisc(transform.position, transform.rotation, RingRadiusOuter);
	}
#endif
}