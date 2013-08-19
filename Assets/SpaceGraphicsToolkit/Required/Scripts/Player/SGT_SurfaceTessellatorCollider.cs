using MeshList = System.Collections.Generic.List<UnityEngine.Mesh>;

using UnityEngine;
using SGT_Internal;

[ExecuteInEditMode]
[RequireComponent(typeof(SGT_SurfaceTessellator))]
[AddComponentMenu("Space Graphics Toolkit/Surface Tessellator Collider")]
public class SGT_SurfaceTessellatorCollider : SGT_MonoBehaviourUnique<SGT_SurfaceTessellatorCollider>
{
	/*[SerializeField]*/
	private Vector3[] currentVertices;
	
	/*[SerializeField]*/
	private Vector3[] nextVertices;
	
	/*[SerializeField]*/
	private SGT_MultiMesh activeCollider;
	
	/*[SerializeField]*/
	private SGT_MultiMesh nextCollider;
	
	[SerializeField]
	private GameObject activeGameObject;
	
	[SerializeField]
	private GameObject nextGameObject;
	
	[SerializeField]
	private GameObject surfaceGameObject;
	
	[SerializeField]
	private PhysicMaterial physicsMaterial;
	
	[SerializeField]
	private int highestLOD = 3;
	
	[SerializeField]
	private int verticesPerMesh = 600;
	
	[SerializeField]
	private Mesh lazyDupeCheck;
	
	public PhysicMaterial PhysicsMaterial
	{
		set
		{
			physicsMaterial = value;
		}
		
		get
		{
			return physicsMaterial;
		}
	}
	
	public int HighestLOD
	{
		set
		{
			highestLOD = value;
		}
		
		get
		{
			return highestLOD;
		}
	}
	
	public int VerticesPerMesh
	{
		set
		{
			verticesPerMesh = Mathf.Max(value - (value % 3), 30);
		}
		
		get
		{
			return verticesPerMesh;
		}
	}
	
	public void Awake()
	{
		switch (FindAwakeState("lazyDupeCheck"))
		{
			case AwakeState.AwakeOriginal:
			{
			}
			break;
			case AwakeState.AwakeDuplicate:
			{
			}
			break;
			case AwakeState.AwakeAgain:
			{
			}
			break;
		}
		
		if (lazyDupeCheck == null) lazyDupeCheck = new Mesh();
	}
	
	public void OnEnable()
	{
		StopAllCoroutines();
		
		if (Application.isPlaying == true)
		{
			StartCoroutine(Update_Coroutine());
		}
		
		if (activeCollider != null)
		{
			activeCollider.MeshCollidersEnabled = true;
			activeCollider.Update();
		}
	}
	
	public void OnDisable()
	{
		StopAllCoroutines();
		
		if (activeCollider != null)
		{
			activeCollider.MeshCollidersEnabled = false;
			activeCollider.Update();
		}
	}
	
	public new void OnDestroy()
	{
		base.OnDestroy();
		
		if (nextCollider   != null) nextCollider.DestroyAllMeshes();
		if (activeCollider != null) activeCollider.DestroyAllMeshes();
	}
	
	public void Update()
	{
		if (surfaceGameObject == null)
		{
			var fillSurfaceGameObject = new SGT_FillGameObject();
			
			SendMessage("FillSurfaceGameObject", fillSurfaceGameObject, SendMessageOptions.DontRequireReceiver);
			
			surfaceGameObject = fillSurfaceGameObject.GameObject;
		}
		
		if (activeCollider != null)
		{
			if (activeGameObject == null) activeGameObject = SGT_Helper.CreateGameObject("Collider (Active)", gameObject);
			
			activeCollider.GameObject = activeGameObject;
			activeCollider.Update();
		}
		else
		{
			if (activeGameObject != null) SGT_Helper.DestroyGameObject(activeGameObject);
		}
		
		if (nextCollider != null)
		{
			if (nextGameObject == null) nextGameObject = SGT_Helper.CreateGameObject("Collider (Currently Building)", gameObject);
			
			nextCollider.GameObject = nextGameObject;
			nextCollider.Update();
		}
		else
		{
			if (nextGameObject != null) SGT_Helper.DestroyGameObject(nextGameObject);
		}
		
		if (activeGameObject != null) SGT_Helper.SetLocalScale(activeGameObject.transform, surfaceGameObject.transform.localScale);
		if (nextGameObject   != null) SGT_Helper.SetLocalScale(nextGameObject.transform  , surfaceGameObject.transform.localScale);
		
#if UNITY_EDITOR == true
		if (Application.isEditor == true)
		{
			SGT_Helper.HideGameObject(activeGameObject);
			SGT_Helper.HideGameObject(nextGameObject);
		}
#endif
	}
	
	public void TessellationFinished(SGT_SurfaceTessellator tessellator)
	{
		if (tessellator != null)
		{
			nextVertices = tessellator.BuildVertices(highestLOD);
		}
	}
	
	private System.Collections.IEnumerator Update_Coroutine()
	{
		for (;;)
		{
			// Swap
			currentVertices = nextVertices;
			nextVertices    = null;
			
			if (currentVertices != null)
			{
				nextGameObject = SGT_Helper.CreateGameObject("Collider (Currently Building)", gameObject);
				nextGameObject.layer = gameObject.layer;
				nextGameObject.tag   = gameObject.tag;
				
				nextCollider   = new SGT_MultiMesh();
				nextCollider.GameObject            = nextGameObject;
				nextCollider.HasMeshColliders      = true;
				nextCollider.MeshCollidersEnabled  = true;
				nextCollider.SharedPhysicsMaterial = physicsMaterial;
				
				var vertexCount = currentVertices.Length;
				var copyFrom    = 0;
				
				while (copyFrom < vertexCount)
				{
					var copyTo    = Mathf.Min(copyFrom + verticesPerMesh, vertexCount);
					var copyCount = copyTo - copyFrom;
					var vertices  = new Vector3[copyCount];
					var triangles = new int[copyCount];
					
					for (var i = 0; i < copyCount; i++)
					{
						vertices[i]  = currentVertices[copyFrom + i];
						triangles[i] = i;
					}
					
					var mesh = new Mesh();
					
					mesh.name      = "Tessellator Collider Generated";
					mesh.vertices  = vertices;
					mesh.triangles = triangles;
					mesh.RecalculateBounds();
					
					nextCollider.Add(mesh);
					nextCollider.Update();
					
					copyFrom += verticesPerMesh;
					
					yield return new WaitForEndOfFrame();
				}
				
				// Remove current ones
				if (activeCollider != null) activeCollider.DestroyAllMeshes();
				
				SGT_Helper.DestroyGameObject(activeGameObject);
				
				// Swap
				activeCollider   = nextCollider;
				activeGameObject = nextGameObject;
				activeGameObject.name = "Collider (Active)";
				
				// Remove old
				nextGameObject = null;
				nextCollider   = null;
			}
			
			yield return new WaitForEndOfFrame();
		}
	}
}