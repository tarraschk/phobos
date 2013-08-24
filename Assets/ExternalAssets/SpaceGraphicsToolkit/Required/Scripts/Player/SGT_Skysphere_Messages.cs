using UnityEngine;

public partial class SGT_Skysphere
{
	public void Awake()
	{
		if (ThisHasBeenDuplicated("skysphereMaterial") == true)
		{
			skysphereMaterial = SGT_Helper.CloneObject(skysphereMaterial);
		}
	}
	
	public void OnEnable()
	{
		if (skysphereMesh != null) skysphereMesh.OnEnable();
	}
	
	public void OnDisable()
	{
		if (skysphereMesh != null) skysphereMesh.OnDisable();
	}
	
	public void LateUpdate() // TODO: Check mesh size
	{
		if (skysphereGameObject == null) skysphereGameObject = SGT_Helper.CreateGameObject("Skysphere", gameObject);
		if (skysphereMesh       == null) skysphereMesh       = new SGT_Mesh();
		if (skysphereObserver   == null) skysphereObserver   = SGT_Helper.FindCamera();
		
		SGT_Helper.SetParent(skysphereGameObject, gameObject);
		SGT_Helper.SetLayer(skysphereGameObject, gameObject.layer);
		SGT_Helper.SetTag(skysphereGameObject, gameObject.tag);
		
		UpdateMaterial();
		UpdateShader();
		
		if (skysphereObserver != null)
		{
			// Stretch to camera's far view frustum
			SGT_Helper.SetLocalScale(skysphereGameObject.transform, skysphereObserver.far * 0.9f);
			
			// Centre to main camera
			SGT_Helper.SetPosition(skysphereGameObject.transform, skysphereObserver.transform.position);
		}
		
		skysphereMesh.GameObject          = skysphereGameObject;
		skysphereMesh.HasMeshRenderer     = true;
		skysphereMesh.MeshRendererEnabled = true;
		skysphereMesh.SharedMaterial      = skysphereMaterial;
		skysphereMesh.Update();
		
#if UNITY_EDITOR == true
		skysphereMesh.HideInEditor();
#endif
	}
	
	public new void OnDestroy()
	{
		base.OnDestroy();
		
		SGT_Helper.DestroyGameObject(skysphereGameObject);
		SGT_Helper.DestroyObject(skysphereMaterial);
	}
}