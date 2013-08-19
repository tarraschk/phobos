using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Volumetric Probe")]
public class SGT_VolumetricProbe : SGT_MonoBehaviourUnique<SGT_VolumetricProbe>
{
	[SerializeField]
	private Material probeMaterial;
	
	[SerializeField]
	private int probeRenderQueue = 3000;
	
	[SerializeField]
	private bool probeRecursive = true;
	
	public int ProbeRenderQueue
	{
		set
		{
			probeRenderQueue = value;
		}
		
		get
		{
			return probeRenderQueue;
		}
	}
	
	public bool ProbeRecursive
	{
		set
		{
			probeRecursive = value;
		}
		
		get
		{
			return probeRecursive;
		}
	}
	
	public void Awake()
	{
		if (ThisHasBeenDuplicated("probeMaterial") == true)
		{
			SGT_Helper.RemoveSharedMaterial(renderer, probeMaterial, probeRecursive);
			
			probeMaterial = SGT_Helper.CloneObject(probeMaterial);
			
			SGT_Helper.SetRenderQueue(probeMaterial, probeRenderQueue);
		}
	}
	
	public void LateUpdate()
	{
		Color volumetricColour;
		
		if (SGT_GasGiant.ColourToPoint(Camera.main.transform.position, transform.position, 1.0f, false, false, out volumetricColour) == true)
		{
			if (probeMaterial == null) probeMaterial = SGT_Helper.CreateMaterial("Hidden/SGT/Fog/Variant", probeRenderQueue);
			
			probeMaterial.SetColor("fogColour", volumetricColour);
			
			SGT_Helper.InsertSharedMaterial(renderer, probeMaterial, probeRecursive);
			
			SGT_Helper.SetRenderQueue(probeMaterial, probeRenderQueue);
		}
		else
		{
			if (probeMaterial != null)
			{
				SGT_Helper.RemoveSharedMaterial(renderer, probeMaterial, probeRecursive);
				
				probeMaterial = SGT_Helper.DestroyObject(probeMaterial);
			}
		}
	}
	
	public new void OnDestroy()
	{
		base.OnDestroy();
		
		SGT_Helper.DestroyObject(probeMaterial);
	}
}
