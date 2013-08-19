using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Surface Displacement")]
public class SGT_SurfaceDisplacement : SGT_MonoBehaviourUnique<SGT_SurfaceDisplacement>
{
	[SerializeField]
	private bool modified;
	
	[SerializeField]
	private bool displacementAutoRegen = true;
	
	[SerializeField]
	private SGT_SurfaceMultiMesh sourceSurfaceMesh;
	
	[SerializeField]
	private GameObject surface;
	
	[SerializeField]
	private SGT_SurfaceTexture displacementTexture;
	
	[SerializeField]
	private bool useUV = false;
	
	[SerializeField]
	private bool clamp = true;
	
	[SerializeField]
	private float scaleMin = 1.0f;
	
	[SerializeField]
	private float scaleMax = 1.1f;
	
	[SerializeField]
	private SGT_SurfaceMultiMesh generatedSurfaceMesh;
	
	[SerializeField]
	private Mesh lazyDupeCheck;
	
	public bool DisplacementAutoRegen
	{
		set
		{
			displacementAutoRegen = value;
		}
		
		get
		{
			return displacementAutoRegen;
		}
	}
	
	public bool Modified
	{
		get
		{
			if (modified == false) CheckForModifications();
			
			return modified;
		}
	}
	
	public SGT_SurfaceConfiguration SourceConfiguration
	{
		set
		{
			if (sourceSurfaceMesh == null) sourceSurfaceMesh = new SGT_SurfaceMultiMesh();
			
			if (value != sourceSurfaceMesh.Configuration)
			{
				sourceSurfaceMesh.Configuration = value;
				
				SendMessage("SetSurfaceConfiguration", value);
			}
		}
		
		get
		{
			return sourceSurfaceMesh != null ? sourceSurfaceMesh.Configuration : SGT_SurfaceConfiguration.Sphere;
		}
	}
	
	public SGT_SurfaceMultiMesh SourceSurfaceMesh
	{
		set
		{
		}
		
		get
		{
			if (sourceSurfaceMesh == null) sourceSurfaceMesh = new SGT_SurfaceMultiMesh();
			
			return sourceSurfaceMesh;
		}
	}
	
	public SGT_SurfaceConfiguration DisplacementConfiguration
	{
		set
		{
			if (displacementTexture == null) displacementTexture = new SGT_SurfaceTexture();
			
			if (value != displacementTexture.Configuration)
			{
				displacementTexture.Configuration = value;
				
				modified = true;
			}
		}
		
		get
		{
			return displacementTexture != null ? displacementTexture.Configuration : SGT_SurfaceConfiguration.Sphere;
		}
	}
	
	public SGT_SurfaceTexture DisplacementTexture
	{
		set
		{
		}
		
		get
		{
			if (displacementTexture == null) displacementTexture = new SGT_SurfaceTexture();
			
			return displacementTexture;
		}
	}
	
	public bool DisplacementUseUV
	{
		set
		{
			if (value != useUV)
			{
				useUV    = value;
				modified = true;
			}
		}
		
		get
		{
			return useUV;
		}
	}
	
	public bool DisplacementClamp
	{
		set
		{
			if (value != clamp)
			{
				clamp    = value;
				modified = true;
			}
		}
		
		get
		{
			return clamp;
		}
	}
	
	public float DisplacementScaleMin
	{
		set
		{
			if (value != scaleMin)
			{
				scaleMin = value;
				modified = true;
			}
		}
		
		get
		{
			return scaleMin;
		}
	}
	
	public float DisplacementScaleMax
	{
		set
		{
			if (value != scaleMax)
			{
				scaleMax = value;
				modified = true;
			}
		}
		
		get
		{
			return scaleMax;
		}
	}
	
	public void Awake()
	{
		switch (FindAwakeState("lazyDupeCheck"))
		{
			case AwakeState.AwakeOriginal:
			{
				if (sourceSurfaceMesh == null)
				{
					sourceSurfaceMesh = new SGT_SurfaceMultiMesh();
					
					SendMessage("CopySurfaceMultiMeshInto", sourceSurfaceMesh, SendMessageOptions.DontRequireReceiver);
					
					modified = true;
				}
			}
			break;
			case AwakeState.AwakeDuplicate:
			{
				lazyDupeCheck        = null;
				generatedSurfaceMesh = null;
				modified             = true;
			}
			break;
			case AwakeState.AwakeAgain:
			{
			}
			break;
		}
		
		if (lazyDupeCheck == null) lazyDupeCheck = new Mesh();
	}
	
	public void Update()
	{
		if (sourceSurfaceMesh == null) sourceSurfaceMesh = new SGT_SurfaceMultiMesh();
		
		if (displacementAutoRegen == true)
		{
			Regenerate();
		}
	}
	
	public void OnEnable()
	{
		if (generatedSurfaceMesh != null && generatedSurfaceMesh.ContainsSomething == true)
		{
			SendMessage("SetSurfaceMultiMesh", generatedSurfaceMesh, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	public void OnDisable()
	{
		if (sourceSurfaceMesh != null) SendMessage("SetSurfaceMultiMesh", sourceSurfaceMesh, SendMessageOptions.DontRequireReceiver);
	}
	
	public new void OnDestroy()
	{
		base.OnDestroy();
		
		if (generatedSurfaceMesh != null) generatedSurfaceMesh.DestroyAllMeshes();
		if (sourceSurfaceMesh    != null) SendMessage("SetSurfaceMultiMesh", sourceSurfaceMesh, SendMessageOptions.DontRequireReceiver);
	}
	
	public float RadiusScaleAtPoint(Vector3 xyz)
	{
		if (surface == null)
		{
			var fill = new SGT_Internal.SGT_FillGameObject();
			
			SendMessage("FillSurfaceGameObject", fill, SendMessageOptions.DontRequireReceiver);
			
			surface = fill.GameObject;
		}
		
		if (surface != null && displacementTexture != null)
		{
			switch (DisplacementConfiguration)
			{
				case SGT_SurfaceConfiguration.Sphere:
				{
					var texture = displacementTexture.GetTexture2D(0);
					
					if (texture != null)
					{
						xyz = surface.transform.InverseTransformPoint(xyz);
						
						var pixelUV      = SGT_Helper.PixelUV(texture);
						var uv           = SGT_Helper.CartesianToPolarUV(xyz); uv.y = SGT_Helper.ClampUV(uv.y, pixelUV.y);
						var displacement = texture.GetPixelBilinear(uv.x, uv.y).r;
						var scale        = Mathf.Lerp(scaleMin, scaleMax, displacement);
						
						return scale;
					}
				}
				break;
				case SGT_SurfaceConfiguration.Cube:
				{
					xyz = surface.transform.InverseTransformPoint(xyz);
					
					var face    = SGT_Helper.CubeFace(xyz);
					var texture = displacementTexture.GetTexture2D(face);
					
					if (texture != null)
					{
						var uv           = SGT_Helper.CubeUV(face, xyz, true);
						var displacement = texture.GetPixelBilinear(uv.x, uv.y).r;
						var scale        = Mathf.Lerp(scaleMin, scaleMax, displacement);
						
						return scale;
					}
				}
				break;
			}
		}
		
		return 1.0f;
	}
	
	public void Regenerate()
	{
		if (modified == false)
		{
			CheckForModifications();
		}
		
		if (modified == true)
		{
			if (displacementTexture != null && sourceSurfaceMesh != null)
			{
				if (generatedSurfaceMesh == null) generatedSurfaceMesh = new SGT_SurfaceMultiMesh();
				
				// Destroy old meshes
				generatedSurfaceMesh.DestroyAllMeshes();
				
				sourceSurfaceMesh.CopyMeshesInto(generatedSurfaceMesh);
				
				var surfaceCount = generatedSurfaceMesh.Count;
				
				for (var i = 0; i < surfaceCount; i++)
				{
					var multiMesh = generatedSurfaceMesh.GetMultiMesh(i);
					
					if (multiMesh != null)
					{
						for (var j = 0; j < multiMesh.Count; j++)
						{
							var mesh = multiMesh.GetSharedMesh(j);
							
							if (mesh != null)
							{
								mesh = SGT_Helper.CloneObject(mesh);
								
								var positions      = mesh.vertices;
								var uv0s           = mesh.uv;
								var newPositions   = new Vector3[positions.Length];
								var displacementUV = Vector2.zero;
								
								for (var k = 0; k < positions.Length; k++)
								{
									var texture  = (Texture2D)null;
									var position = positions[k];
									
									switch (displacementTexture.Configuration)
									{
										case SGT_SurfaceConfiguration.Sphere:
										{
											texture = displacementTexture.GetTexture2D(0);
											
											if (texture != null)
											{
												displacementUV = useUV == true ? uv0s[k] : SGT_Helper.CartesianToPolarUV(position);
												
												if (clamp == true) displacementUV = SGT_Helper.ClampCollapseV(displacementUV, SGT_Helper.PixelUV(texture));
											}
										}
										break;
										case SGT_SurfaceConfiguration.Cube:
										{
											texture = displacementTexture.GetTexture2D(position);
											
											if (texture != null)
											{
												displacementUV = useUV == true ? uv0s[k] : SGT_Helper.CubeUV(position, true);
												
												if (clamp == true) displacementUV = SGT_Helper.ClampUV(displacementUV, SGT_Helper.PixelUV(texture));
											}
										}
										break;
									}
									
									var displacement = texture != null ? texture.GetPixelBilinear(displacementUV.x, displacementUV.y).r : 0.0f;
									
									newPositions[k] = position * Mathf.Lerp(scaleMin, scaleMax, displacement);
								}
								
								mesh.name     += "(Displaced)";
								mesh.vertices = newPositions;
								mesh.bounds   = new Bounds(mesh.bounds.center, mesh.bounds.size * scaleMax);
								mesh.RecalculateNormals();
								
								multiMesh.SetSharedMesh(mesh, j);
							}
						}
					}
				}
				
				MarkAsUnmodified();
				
				SendMessage("SetSurfaceMultiMesh", generatedSurfaceMesh, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	public void SetSurfaceConfiguration(SGT_SurfaceConfiguration newConfiguration)
	{
		if (sourceSurfaceMesh == null) sourceSurfaceMesh = new SGT_SurfaceMultiMesh();
		
		sourceSurfaceMesh.Configuration = newConfiguration;
	}
	
	private void CheckForModifications()
	{
		if (modified == false)
		{
			if (sourceSurfaceMesh != null && sourceSurfaceMesh.MeshesModified == true)
			{
				modified = true;
			}
			else if (displacementTexture != null && displacementTexture.Modified == true)
			{
				modified = true;
			}
		}
	}
	
	private void MarkAsUnmodified()
	{
		modified = false;
		
		if (sourceSurfaceMesh   != null) sourceSurfaceMesh.MeshesModified = false;
		if (displacementTexture != null) displacementTexture.Modified     = false;
	}
}