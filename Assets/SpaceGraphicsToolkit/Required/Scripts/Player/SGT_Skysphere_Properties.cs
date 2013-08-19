using ObjectList = System.Collections.Generic.List<UnityEngine.Object>;

using UnityEngine;

public partial class SGT_Skysphere
{
	[SerializeField]
	private GameObject skysphereGameObject;
	
	[SerializeField]
	private SGT_Mesh skysphereMesh;
	
	[SerializeField]
	private Material skysphereMaterial;
	
	[SerializeField]
	private string skysphereTechnique;
	
	[SerializeField]
	private Texture skysphereTexture;
	
	[SerializeField]
	private int skysphereRenderQueue = 1000;
	
	[SerializeField]
	private Camera skysphereObserver;
	
	public Mesh SkysphereMesh
	{
		set
		{
			if (skysphereMesh == null) skysphereMesh = new SGT_Mesh();
			
			skysphereMesh.SharedMesh = value;
		}
		
		get
		{
			return skysphereMesh != null ? skysphereMesh.SharedMesh : null;
		}
	}
	
	public int SkysphereRenderQueue
	{
		set
		{
			skysphereRenderQueue = value;
		}
		
		get
		{
			return skysphereRenderQueue;
		}
	}
	
	public Texture SkysphereTexture
	{
		set
		{
			skysphereTexture = value;
		}
		
		get
		{
			return skysphereTexture;
		}
	}
	
	public Camera SkysphereObserver
	{
		set
		{
			skysphereObserver = value;
		}
		
		get
		{
			return skysphereObserver;
		}
	}
	
	public override void BuildUndoTargets(ObjectList list)
	{
		base.BuildUndoTargets(list);
		
		if (skysphereMesh != null) skysphereMesh.BuildUndoTargets(list);
	}
}