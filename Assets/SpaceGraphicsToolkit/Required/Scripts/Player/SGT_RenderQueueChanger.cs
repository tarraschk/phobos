using MaterialList = System.Collections.Generic.List<UnityEngine.Material>;
using IntList      = System.Collections.Generic.List<int>;

using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/RenderQueue Changer")]
public class SGT_RenderQueueChanger : SGT_MonoBehaviour
{
	[SerializeField]
	private MaterialList materials;
	
	[SerializeField]
	private IntList renderQueues;
	
	public int Count
	{
		get
		{
			return materials != null ? materials.Count : 0;
		}
	}
	
	public void Awake()
	{
		UpdateRenderQueues();
	}
	
	public void LateUpdate()
	{
		UpdateRenderQueues();
	}
	
	public Material GetMaterial(int index)
	{
		return SGT_ArrayHelper.Index(materials, index);
	}
	
	public int GetRenderQueue(int index)
	{
		return SGT_ArrayHelper.Index(renderQueues, index);
	}
	
	public void SetMaterial(Material material, int index)
	{
		SGT_ArrayHelper.Set(materials, material, index);
	}
	
	public void SetRenderQueue(int renderQueue, int index)
	{
		SGT_ArrayHelper.Set(renderQueues, renderQueue, index);
	}
	
	public void Add(Material material, int renderQueue)
	{
		if (materials    == null) materials    = new MaterialList();
		if (renderQueues == null) renderQueues = new IntList();
		
		materials.Add(material);
		renderQueues.Add(renderQueue);
	}
	
	public void Remove(int index)
	{
		SGT_ArrayHelper.Remove(materials, index);
		SGT_ArrayHelper.Remove(renderQueues, index);
	}
	
	private void UpdateRenderQueues()
	{
		if (materials    == null) materials    = new MaterialList();
		if (renderQueues == null) renderQueues = new IntList();
		
		if (materials.Count != renderQueues.Count)
		{
			materials.Clear();
			renderQueues.Clear();
		}
		
		for (var i = 0; i < materials.Count; i++)
		{
			SGT_Helper.SetRenderQueue(materials[i], renderQueues[i]);
		}
	}
}