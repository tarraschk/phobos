using ObjectList = System.Collections.Generic.List<UnityEngine.Object>;

using UnityEngine;

public partial class SGT_Planet
{
	[System.Flags]
	private enum ShaderFlags
	{
		ShadowSettings     = 1 << 0,
		ScatteringSettings = 1 << 2,
		
		SurfaceTextureDay         = 1 << 3,
		SurfaceTextureNight       = 1 << 4,
		SurfaceTextureNormal      = 1 << 5,
		SurfaceTextureSpecular    = 1 << 6,
		SurfaceTextureDetail      = 1 << 7,
		SurfaceTextureLighting    = 1 << 8,
		SurfaceTextureShadow      = 1 << 9,
		SurfaceTextureAtmosphere  = 1 << 10,
		SurfaceSpecularSettings   = 1 << 11,
		
		AtmosphereTexture       = 1 << 12,
		AtmosphereTextureShadow = 1 << 13,
		
		CloudsTexture         = 1 << 14,
		CloudsTextureShadow   = 1 << 15,
		CloudsTextureLighting = 1 << 16,
		CloudsFalloffSettings = 1 << 17,
		
		Surface =
			ShadowSettings           |
			ScatteringSettings       |
			SurfaceTextureDay        |
			SurfaceTextureNight      |
			SurfaceTextureNormal     |
			SurfaceTextureSpecular   |
			SurfaceTextureDetail     |
			SurfaceTextureLighting   |
			SurfaceTextureShadow     |
			SurfaceTextureAtmosphere |
			SurfaceSpecularSettings  ,
		
		Atmosphere =
			ShadowSettings           |
			ScatteringSettings       |
			AtmosphereTexture        |
			AtmosphereTextureShadow  |
			SurfaceTextureAtmosphere ,
		
		Clouds = 
			ShadowSettings        |
			ScatteringSettings    |
			CloudsTexture         |
			CloudsTextureShadow   |
			CloudsTextureLighting |
			CloudsFalloffSettings ,
	}
	
	[SerializeField]
	private ShaderFlags updateShader;
	
	[SerializeField]
	private SGT_ColourGradient planetLighting;
	
	[SerializeField]
	private SGT_LightSource planetLightSource;
	
	[SerializeField]
	private Camera planetObserver;
	
	[SerializeField]
	private SGT_SquareSize planetLutSize = SGT_SquareSize.Square32;
	
	[SerializeField]
	private float surfaceRadius = 50.0f;
	
	[SerializeField]
	private float surfaceSpecularPower = 3.0f;
	
	[SerializeField]
	private float surfaceDetailRepeat = 1.0f;
	
	[SerializeField]
	private SGT_SurfaceMultiMesh surfaceMesh;
	
	[SerializeField]
	private int surfaceRenderQueue = 2000;
	
	[SerializeField]
	private SGT_SurfaceTexture surfaceTextureDay;
	
	[SerializeField]
	private SGT_SurfaceTexture surfaceTextureNight;
	
	[SerializeField]
	private SGT_SurfaceTexture surfaceTextureNormal;
	
	[SerializeField]
	private SGT_SurfaceTexture surfaceTextureSpecular;
	
	[SerializeField]
	private Texture surfaceTextureDetail;
	
	[SerializeField]
	private GameObject surfaceGameObject;
	
	[SerializeField]
	private string surfaceTechnique;
	
	[SerializeField]
	private Material[] surfaceMaterials;
	
	[SerializeField]
	private Texture2D surfaceLightingTexture;
	
	
	
	[SerializeField]
	private bool atmosphere;
	
	[SerializeField]
	private SGT_Mesh atmosphereMesh;
	
	[SerializeField]
	private int atmosphereRenderQueue = 3000;
	
	[SerializeField]
	private float atmosphereHeight = 2.0f;
	
	[SerializeField]
	private SGT_ColourGradient atmosphereDensityColour;
	
	[SerializeField]
	private SGT_ColourGradient atmosphereTwilightColour;
	
	[SerializeField]
	private float atmosphereNightOpacity;
	
	[SerializeField]
	private float atmosphereSkyAltitude = 0.25f;
	
	[SerializeField]
	private float atmosphereFog;
	
	[SerializeField]
	private float atmosphereFalloffSurface = 1.5f;
	
	[SerializeField]
	private float atmosphereFalloffInside = 0.08f;
	
	[SerializeField]
	private float atmosphereFalloffOutside = 3.0f;
	
	[SerializeField]
	private bool atmosphereScattering;
	
	[SerializeField]
	private float atmosphereScatteringMie = 0.5f;
	
	[SerializeField]
	private float atmosphereScatteringRayleigh = 4.0f;
	
	[SerializeField]
	private GameObject atmosphereGameObject;
	
	[SerializeField]
	private string atmosphereTechnique;
	
	[SerializeField]
	private Material atmosphereMaterial;
	
	[SerializeField]
	private Texture2D atmosphereSurfaceTexture;
	
	[SerializeField]
	private Texture2D atmosphereTexture;
	
	
	
	[SerializeField]
	private bool clouds;
	
	[SerializeField]
	private SGT_SurfaceMultiMesh cloudsMesh;
	
	[SerializeField]
	private float cloudsHeight = 1.0f;
	
	[SerializeField]
	private SGT_SurfaceTexture cloudsTexture;
	
	[SerializeField]
	private SGT_ColourGradient cloudsLimbColour;
	
	[SerializeField]
	private float cloudsFalloff = 5.0f;
	
	[SerializeField]
	private float cloudsRotationPeriod;
	
	[SerializeField]
	private float cloudsOffset = 0.1f;
	
	[SerializeField]
	private float cloudsTwilightOffset;
	
	[SerializeField]
	private GameObject cloudsGameObject;
	
	[SerializeField]
	private string cloudsTechnique;
	
	[SerializeField]
	private Material[] cloudsMaterials;
	
	[SerializeField]
	private Texture2D cloudsLightingTexture;
	
	
	
	[SerializeField]
	private bool shadow;
	
	[SerializeField]
	private SGT_ShadowOccluder shadowCasterType;
	
	[SerializeField]
	private GameObject shadowCasterGameObject;
	
	[SerializeField]
	private bool shadowCasterAutoUpdate = true;
	
	[SerializeField]
	private float shadowCasterRadius = 10.0f;
	
	[SerializeField]
	private float shadowCasterWidth = 10.0f;
	
	[SerializeField]
	private Texture2D shadowTextureSurface;
	
	[SerializeField]
	private Texture2D shadowTextureAtmosphere;
	
	[SerializeField]
	private Texture2D shadowTextureClouds;
	
	public SGT_ColourGradient PlanetLighting
	{
		set
		{
		}
		
		get
		{
			if (planetLighting == null) planetLighting = new SGT_ColourGradient(false, false);
			
			return planetLighting;
		}
	}
	
	public SGT_LightSource PlanetLightSource
	{
		set
		{
			planetLightSource = value;
		}
		
		get
		{
			return planetLightSource;
		}
	}
	
	public Camera PlanetObserver
	{
		set
		{
			planetObserver = value;
		}
		
		get
		{
			return planetObserver;
		}
	}
	
	public SGT_SquareSize PlanetLutSize
	{
		set
		{
			if (value != planetLutSize)
			{
				planetLutSize = value;
				
				planetLighting.Modified = true; // This will cause them all to be rebuilt
			}
		}
		
		get
		{
			return planetLutSize;
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
	
	public float SurfaceSpecularPower
	{
		set
		{
			if (value != surfaceSpecularPower)
			{
				surfaceSpecularPower = value;
				updateShader        |= ShaderFlags.SurfaceSpecularSettings;
			}
		}
		
		get
		{
			return surfaceSpecularPower;
		}
	}
	
	public float SurfaceDetailRepeat
	{
		set
		{
			surfaceDetailRepeat = Mathf.Max(0.0f, value);
		}
		
		get
		{
			return surfaceDetailRepeat;
		}
	}
	
	public SGT_SurfaceConfiguration SurfaceConfiguration
	{
		set
		{
			if (surfaceMesh == null) surfaceMesh = new SGT_SurfaceMultiMesh();
			
			if (value != surfaceMesh.Configuration)
			{
				surfaceMesh.Configuration = value;
				
				SendMessage("SetSurfaceConfiguration", value);
			}
		}
		
		get
		{
			return surfaceMesh != null ? surfaceMesh.Configuration : SGT_SurfaceConfiguration.Sphere;
		}
	}
	
	public SGT_SurfaceMultiMesh SurfaceMesh
	{
		set
		{
		}
		
		get
		{
			if (surfaceMesh == null) surfaceMesh = new SGT_SurfaceMultiMesh();
			
			return surfaceMesh;
		}
	}
	
	public int SurfaceRenderQueue
	{
		set
		{
			if (value != surfaceRenderQueue)
			{
				surfaceRenderQueue = value;
				
				SGT_Helper.SetRenderQueues(surfaceMaterials, surfaceRenderQueue);
			}
		}
		
		get
		{
			return surfaceRenderQueue;
		}
	}
	
	public bool SurfaceCollider
	{
		set
		{
			if (surfaceMesh == null) surfaceMesh = new SGT_SurfaceMultiMesh();
			
			surfaceMesh.HasMeshColliders = value;
		}
		
		get
		{
			return surfaceMesh != null ? surfaceMesh.HasMeshColliders : false;
		}
	}
	
	public PhysicMaterial SurfaceColliderMaterial
	{
		set
		{
			if (surfaceMesh == null) surfaceMesh = new SGT_SurfaceMultiMesh();
			
			surfaceMesh.SharedPhysicsMaterial = value;
		}
		
		get
		{
			return surfaceMesh != null ? surfaceMesh.SharedPhysicsMaterial : null;
		}
	}
	
	public SGT_SurfaceTexture SurfaceTextureDay
	{
		set
		{
		}
		
		get
		{
			if (surfaceTextureDay == null) surfaceTextureDay = new SGT_SurfaceTexture();
			
			surfaceTextureDay.Configuration = SurfaceConfiguration;
			
			return surfaceTextureDay;
		}
	}
	
	public SGT_SurfaceTexture SurfaceTextureNight
	{
		set
		{
		}
		
		get
		{
			if (surfaceTextureNight == null) surfaceTextureNight = new SGT_SurfaceTexture();
			
			surfaceTextureNight.Configuration = SurfaceConfiguration;
			
			return surfaceTextureNight;
		}
	}
	
	public SGT_SurfaceTexture SurfaceTextureNormal
	{
		set
		{
		}
		
		get
		{
			if (surfaceTextureNormal == null) surfaceTextureNormal = new SGT_SurfaceTexture();
			
			surfaceTextureNormal.Configuration = SurfaceConfiguration;
			
			return surfaceTextureNormal;
		}
	}
	
	public SGT_SurfaceTexture SurfaceTextureSpecular
	{
		set
		{
		}
		
		get
		{
			if (surfaceTextureSpecular == null) surfaceTextureSpecular = new SGT_SurfaceTexture();
			
			surfaceTextureSpecular.Configuration = SurfaceConfiguration;
			
			return surfaceTextureSpecular;
		}
	}
	
	public Texture SurfaceTextureDetail
	{
		set
		{
			if (value != surfaceTextureDetail)
			{
				surfaceTextureDetail = value;
				updateShader        |= ShaderFlags.SurfaceTextureDetail;
			}
		}
		
		get
		{
			return surfaceTextureDetail;
		}
	}
	
	public bool Atmosphere
	{
		set
		{
			if (value != atmosphere)
			{
				atmosphere    = value;
				updateShader |= ShaderFlags.Surface | ShaderFlags.Atmosphere | ShaderFlags.Clouds;
				
				if (cloudsLimbColour != null) cloudsLimbColour.Modified = true;
			}
		}
		
		get
		{
			return atmosphere;
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
	
	public int AtmosphereRenderQueue
	{
		set
		{
			if (value != atmosphereRenderQueue)
			{
				atmosphereRenderQueue = value;
				
				SGT_Helper.SetRenderQueue(atmosphereMaterial, atmosphereRenderQueue);
				SGT_Helper.SetRenderQueues(cloudsMaterials, atmosphereRenderQueue);
			}
		}
		
		get
		{
			return atmosphereRenderQueue;
		}
	}
	
	public float AtmosphereHeight
	{
		set
		{
			if (value != atmosphereHeight && value > 0.0f)
			{
				atmosphereHeight = value;
			}
		}
		
		get
		{
			return atmosphereHeight;
		}
	}
	
	public float AtmosphereRadius
	{
		get
		{
			return surfaceRadius + atmosphereHeight;
		}
	}
	
	public SGT_ColourGradient AtmosphereDensityColour
	{
		set
		{
		}
		
		get
		{
			if (atmosphereDensityColour == null) atmosphereDensityColour = new SGT_ColourGradient(false, true);
			
			return atmosphereDensityColour;
		}
	}
	
	public SGT_ColourGradient AtmosphereTwilightColour
	{
		set
		{
		}
		
		get
		{
			if (atmosphereTwilightColour == null) atmosphereTwilightColour = new SGT_ColourGradient(false, true);
			
			return atmosphereTwilightColour;
		}
	}
	
	public float AtmosphereNightOpacity
	{
		set
		{
			if (value != atmosphereNightOpacity)
			{
				atmosphereNightOpacity = value;
				
				if (atmosphereTwilightColour != null) atmosphereTwilightColour.Modified = true;
			}
		}
		
		get
		{
			return atmosphereNightOpacity;
		}
	}
	
	public float AtmosphereSkyAltitude
	{
		set
		{
			atmosphereSkyAltitude = Mathf.Max(value, 0.0f);
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
	
	public float AtmosphereFalloffSurface
	{
		set
		{
			atmosphereFalloffSurface = Mathf.Max(value, 0.0f);
		}
		
		get
		{
			return atmosphereFalloffSurface;
		}
	}
	
	public float AtmosphereFalloffOutside
	{
		set
		{
			atmosphereFalloffOutside = Mathf.Max(value, 0.0f);
		}
		
		get
		{
			return atmosphereFalloffOutside;
		}
	}
	
	public float AtmosphereFalloffInside
	{
		set
		{
			atmosphereFalloffInside = Mathf.Max(value, 0.0f);
		}
		
		get
		{
			return atmosphereFalloffInside;
		}
	}
	
	public bool AtmosphereScattering
	{
		set
		{
			if (value != atmosphereScattering)
			{
				atmosphereScattering = value;
				updateShader        |= ShaderFlags.ScatteringSettings;
			}
		}
		
		get
		{
			return atmosphereScattering;
		}
	}
	
	public float AtmosphereScatteringMie
	{
		set
		{
			if (value != atmosphereScatteringMie)
			{
				atmosphereScatteringMie = value;
				updateShader           |= ShaderFlags.ScatteringSettings;
			}
		}
		
		get
		{
			return atmosphereScatteringMie;
		}
	}
	
	public float AtmosphereScatteringRayleigh
	{
		set
		{
			if (value != atmosphereScatteringRayleigh)
			{
				atmosphereScatteringRayleigh = value;
				updateShader                |= ShaderFlags.ScatteringSettings;
			}
		}
		
		get
		{
			return atmosphereScatteringRayleigh;
		}
	}
	
	public bool Clouds
	{
		set
		{
			if (value != clouds)
			{
				clouds        = value;
				updateShader |= ShaderFlags.Clouds;
			}
		}
		
		get
		{
			return clouds;
		}
	}
	
	public SGT_SurfaceConfiguration CloudsConfiguration
	{
		set
		{
			if (cloudsMesh == null) cloudsMesh = new SGT_SurfaceMultiMesh();
			
			cloudsMesh.Configuration = value;
		}
		
		get
		{
			if (cloudsMesh == null) cloudsMesh = new SGT_SurfaceMultiMesh();
			
			return cloudsMesh.Configuration;
		}
	}
	
	public SGT_SurfaceMultiMesh CloudsMesh
	{
		set
		{
		}
		
		get
		{
			if (cloudsMesh == null) cloudsMesh = new SGT_SurfaceMultiMesh();
			
			return cloudsMesh;
		}
	}
	
	public int CloudsRenderQueue
	{
		set
		{
			AtmosphereRenderQueue = value;
		}
		
		get
		{
			return AtmosphereRenderQueue;
		}
	}
	
	public float CloudsHeight
	{
		set
		{
			if (value != cloudsHeight)
			{
				cloudsHeight  = value;
				updateShader |= ShaderFlags.Clouds;
			}
		}
		
		get
		{
			return cloudsHeight;
		}
	}
	
	public float CloudsRadius
	{
		get
		{
			return surfaceRadius + cloudsHeight;
		}
	}
	
	public SGT_SurfaceTexture CloudsTexture
	{
		set
		{
		}
		
		get
		{
			if (cloudsTexture == null) cloudsTexture = new SGT_SurfaceTexture();
			
			cloudsTexture.Configuration = CloudsConfiguration;
			
			return cloudsTexture;
		}
	}
	
	public SGT_ColourGradient CloudsLimbColour
	{
		set
		{
		}
		
		get
		{
			if (cloudsLimbColour == null) cloudsLimbColour = new SGT_ColourGradient(false, true);
			
			return cloudsLimbColour;
		}
	}
	
	public float CloudsFalloff
	{
		set
		{
			value = Mathf.Max(value, 0.0f);
			
			if (value != cloudsFalloff)
			{
				cloudsFalloff = value;
				updateShader |= ShaderFlags.CloudsFalloffSettings;
			}
		}
		
		get
		{
			return cloudsFalloff;
		}
	}
	
	public float CloudsRotationPeriod
	{
		set
		{
			cloudsRotationPeriod = value;
		}
		
		get
		{
			return cloudsRotationPeriod;
		}
	}
	
	public float CloudsOffset
	{
		set
		{
			if (value != cloudsOffset)
			{
				cloudsOffset = value;
				
				UpdateCloudsOffset();
			}
		}
		
		get
		{
			return cloudsOffset;
		}
	}
	
	public float CloudsTwilightOffset
	{
		set
		{
			if (value != cloudsTwilightOffset)
			{
				cloudsTwilightOffset = value;
				
				if (cloudsLimbColour != null) cloudsLimbColour.Modified = true;
			}
		}
		
		get
		{
			return cloudsTwilightOffset;
		}
	}
	
	public bool Shadow
	{
		set
		{
			if (value != shadow)
			{
				shadow        = value;
				updateShader |= ShaderFlags.ShadowSettings;
			}
		}
		
		get
		{
			return shadow;
		}
	}
	
	public SGT_ShadowOccluder ShadowCasterType
	{
		set
		{
			if (value != shadowCasterType)
			{
				shadowCasterType = value;
				updateShader    |= ShaderFlags.ShadowSettings;
			}
		}
		
		get
		{
			return shadowCasterType;
		}
	}
	
	public GameObject ShadowCasterGameObject
	{
		set
		{
			shadowCasterGameObject = value;
		}
		
		get
		{
			return shadowCasterGameObject;
		}
	}
	
	public bool ShadowCasterAutoUpdate
	{
		set
		{
			shadowCasterAutoUpdate = value;
		}
		
		get
		{
			return shadowCasterAutoUpdate;
		}
	}
	
	public float ShadowCasterRadius
	{
		set
		{
			value = Mathf.Max(value, 0.0f);
			
			if (value != shadowCasterRadius)
			{
				shadowCasterRadius = value;
				updateShader      |= ShaderFlags.ShadowSettings;
			}
		}
		
		get
		{
			return shadowCasterRadius;
		}
	}
	
	public float ShadowCasterWidth
	{
		set
		{
			value = Mathf.Max(value, float.Epsilon);
			
			if (value != shadowCasterWidth)
			{
				shadowCasterWidth = value;
				updateShader     |= ShaderFlags.ShadowSettings;
			}
		}
		
		get
		{
			return shadowCasterWidth;
		}
	}
	
	public float ShadowCasterRadiusInner
	{
		get
		{
			return shadowCasterRadius - shadowCasterWidth * 0.5f;
		}
	}
	
	public float ShadowCasterRadiusOuter
	{
		get
		{
			return shadowCasterRadius + shadowCasterWidth * 0.5f;
		}
	}
	
	public Texture2D ShadowTextureSurface
	{
		set
		{
			if (value != shadowTextureSurface)
			{
				shadowTextureSurface = value;
				updateShader        |= ShaderFlags.SurfaceTextureShadow;
			}
		}
		
		get
		{
			return shadowTextureSurface;
		}
	}
	
	public Texture2D ShadowTextureAtmosphere
	{
		set
		{
			if (value != shadowTextureAtmosphere)
			{
				shadowTextureAtmosphere = value;
				updateShader           |= ShaderFlags.AtmosphereTextureShadow;
			}
		}
		
		get
		{
			return shadowTextureAtmosphere;
		}
	}
	
	public Texture2D ShadowTextureClouds
	{
		set
		{
			if (value != shadowTextureClouds)
			{
				shadowTextureClouds = value;
				updateShader       |= ShaderFlags.CloudsTextureShadow;
			}
		}
		
		get
		{
			return shadowTextureClouds;
		}
	}
	
	public override void BuildUndoTargets(ObjectList list)
	{
		base.BuildUndoTargets(list);
		
		if (atmosphereMesh != null) atmosphereMesh.BuildUndoTargets(list);
	}
}