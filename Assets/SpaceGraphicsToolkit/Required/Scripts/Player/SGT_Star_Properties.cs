using ObjectList = System.Collections.Generic.List<UnityEngine.Object>;

using UnityEngine;

public partial class SGT_Star
{
	[SerializeField]
	private string surfaceTechnique;
	
	[SerializeField]
	private string atmosphereTechnique;
	
	[SerializeField]
	private GameObject surfaceGameObject;
	
	[SerializeField]
	private SGT_SurfaceMultiMesh surfaceMultiMesh;
	
	[SerializeField]
	private GameObject atmosphereGameObject;
	
	[SerializeField]
	private GameObject oblatenessGameObject;
	
	[SerializeField]
	private SGT_Mesh atmosphereMesh;
	
	[SerializeField]
	private SGT_SurfaceTexture surfaceTexture;
	
	[SerializeField]
	private float surfaceRadius = 50.0f;
	
	[SerializeField]
	private float surfaceOblateness = 0.0f;
	
	[SerializeField]
	private float atmosphereHeight = 10.0f;
	
	[SerializeField]
	private bool atmosphereSurfacePerPixel;
	
	[SerializeField]
	private float atmosphereFalloff = 3.0f;
	
	[SerializeField]
	private float atmosphereSurfaceFalloff = 3.0f;
	
	[SerializeField]
	private float atmosphereSkyFalloff = 0.0f;
	
	[SerializeField]
	private float atmosphereSkyAltitude = 0.0f;
	
	[SerializeField]
	private float atmosphereFog = 0.0f;
	
	[SerializeField]
	private SGT_ColourGradient atmosphereDensityColour;
	
	[SerializeField]
	private SGT_SquareSize lutSize = SGT_SquareSize.Square32;
	
	[SerializeField]
	private Texture2D atmosphereTexture;
	
	[SerializeField]
	private Texture2D atmosphereSurfaceTexture;
	
	[SerializeField]
	private Material[] surfaceMaterials;
	
	[SerializeField]
	private Material atmosphereMaterial;
	
	[SerializeField]
	private int surfaceRenderQueue = 2000;
	
	[SerializeField]
	private int atmosphereRenderQueue = 3000;
	
	[SerializeField]
	private Camera starObserver;
	
	public SGT_SurfaceConfiguration SurfaceConfiguration
	{
		set
		{
			if (surfaceMultiMesh == null) surfaceMultiMesh = new SGT_SurfaceMultiMesh();
			
			surfaceMultiMesh.Configuration = value;
		}
		
		get
		{
			return surfaceMultiMesh != null ? surfaceMultiMesh.Configuration : SGT_SurfaceConfiguration.Sphere;
		}
	}
	
	public SGT_SurfaceMultiMesh SurfaceMesh
	{
		set
		{
		}
		
		get
		{
			if (surfaceMultiMesh == null) surfaceMultiMesh = new SGT_SurfaceMultiMesh();
			
			return surfaceMultiMesh;
		}
	}
	
	public SGT_SurfaceTexture SurfaceTexture
	{
		set
		{
		}
		
		get
		{
			if (surfaceTexture == null) surfaceTexture = new SGT_SurfaceTexture();
			
			surfaceTexture.Configuration = SurfaceConfiguration;
			
			return surfaceTexture;
		}
	}
	
	public float SurfaceRadius
	{
		set
		{
			surfaceRadius = value;
		}
		
		get
		{
			return surfaceRadius;
		}
	}
	
	public float SurfaceOblateness
	{
		set
		{
			if (value < 1.0f)
			{
				surfaceOblateness = Mathf.Clamp01(value);
			}
		}
		
		get
		{
			return surfaceOblateness;
		}
	}
	
	public bool AtmosphereFalloffPerPixel
	{
		set
		{
			atmosphereSurfacePerPixel = value;
		}
		
		get
		{
			return atmosphereSurfacePerPixel;
		}
	}
	
	public Mesh AtmosphereMesh
	{
		set
		{
			if (atmosphereMesh == null) atmosphereMesh = new SGT_Mesh();
			
			atmosphereMesh.SharedMesh = value;
		}
		
		get
		{
			return atmosphereMesh != null ? atmosphereMesh.SharedMesh : null;
		}
	}
	
	public float AtmosphereHeight
	{
		set
		{
			if (value > 0.0f)
			{
				atmosphereHeight = value;
			}
		}
		
		get
		{
			return atmosphereHeight;
		}
	}
	
	public float AtmosphereFalloffOutside
	{
		set
		{
			atmosphereFalloff = Mathf.Max(value, 0.0f);
		}
		
		get
		{
			return atmosphereFalloff;
		}
	}
	
	public float AtmosphereFalloffSurface
	{
		set
		{
			atmosphereSurfaceFalloff = Mathf.Max(value, 0.0f);
		}
		
		get
		{
			return atmosphereSurfaceFalloff;
		}
	}
	
	public float AtmosphereFalloffInside
	{
		set
		{
			atmosphereSkyFalloff = Mathf.Max(value, 0.0f);
		}
		
		get
		{
			return atmosphereSkyFalloff;
		}
	}
	
	public float AtmosphereSkyAltitude
	{
		set
		{
			atmosphereSkyAltitude = Mathf.Clamp01(value);
		}
		
		get
		{
			return atmosphereSkyAltitude;
		}
	}
	
	public float AtmosphereFog
	{
		set
		{
			atmosphereFog = Mathf.Clamp01(value);
		}
		
		get
		{
			return atmosphereFog;
		}
	}
	
	public SGT_ColourGradient AtmosphereDensityColour
	{
		set
		{
		}
		
		get
		{
			return atmosphereDensityColour;
		}
	}
	
	public SGT_SquareSize StarLutSize
	{
		set
		{
			if (value != lutSize)
			{
				lutSize = value;
				
				atmosphereDensityColour.Modified = true;
			}
		}
		
		get
		{
			return lutSize;
		}
	}
	
	public int SurfaceRenderQueue
	{
		set
		{
			surfaceRenderQueue = value;
		}
		
		get
		{
			return surfaceRenderQueue;
		}
	}
	
	public int AtmosphereRenderQueue
	{
		set
		{
			atmosphereRenderQueue = value;
		}
		
		get
		{
			return atmosphereRenderQueue;
		}
	}
	
	public Camera StarObserver
	{
		set
		{
			starObserver = value;
		}
		
		get
		{
			return starObserver;
		}
	}
	
	public bool SurfaceCollider
	{
		set
		{
			if (surfaceMultiMesh == null) surfaceMultiMesh = new SGT_SurfaceMultiMesh();
			
			surfaceMultiMesh.HasMeshColliders = value;
		}
		
		get
		{
			return surfaceMultiMesh != null ? surfaceMultiMesh.HasMeshColliders : false;
		}
	}
	
	public PhysicMaterial SurfaceColliderMaterial
	{
		set
		{
			if (surfaceMultiMesh == null) surfaceMultiMesh = new SGT_SurfaceMultiMesh();
			
			surfaceMultiMesh.SharedPhysicsMaterial = value;
		}
		
		get
		{
			return surfaceMultiMesh != null ? surfaceMultiMesh.SharedPhysicsMaterial : null;
		}
	}
	
	public float SurfacePolarRadius
	{
		get
		{
			return surfaceRadius * (1.0f - surfaceOblateness);
		}
	}
	
	public float SurfaceEquatorialRadius
	{
		get
		{
			return surfaceRadius;
		}
	}
	
	public float AtmospherePolarRadius
	{
		get
		{
			return (surfaceRadius + atmosphereHeight) * (1.0f - surfaceOblateness);
		}
	}
	
	public float AtmosphereEquatorialRadius
	{
		get
		{
			return surfaceRadius + atmosphereHeight;
		}
	}
	
	public override void BuildUndoTargets(ObjectList list)
	{
		base.BuildUndoTargets(list);
		
		if (surfaceMultiMesh    != null) surfaceMultiMesh.BuildUndoTargets(list);
		if (atmosphereMesh != null) atmosphereMesh.BuildUndoTargets(list);
	}
}