using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Skysphere")]
public partial class SGT_Skysphere : SGT_MonoBehaviourUnique<SGT_Skysphere>
{
	private void UpdateMaterial()
	{
		var targetSkysphereTechnique = "Variant";
		
		// Update surface?
		if (skysphereMaterial == null || skysphereTechnique != targetSkysphereTechnique)
		{
			SGT_Helper.DestroyObject(skysphereMaterial);
			
			skysphereTechnique = targetSkysphereTechnique;
			skysphereMaterial  = SGT_Helper.CreateMaterial("Hidden/SGT/Skysphere/" + skysphereTechnique, skysphereRenderQueue);
		}
		else
		{
			SGT_Helper.SetRenderQueue(skysphereMaterial, skysphereRenderQueue);
		}
	}
	
	private void UpdateShader()
	{
		skysphereMaterial.SetTexture("skysphereTexture", skysphereTexture);
	}
}