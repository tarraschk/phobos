using ObjectList = System.Collections.Generic.List<UnityEngine.Object>;

using UnityEngine;

public partial class SGT_GasGiant
{
	[SerializeField]
	private SGT_LightSource gasGiantLightSource;
	
	[SerializeField]
	private Camera gasGiantObserver;
	
	[SerializeField]
	private float gasGiantEquatorialRadius = 50.0f;
	
	[SerializeField]
	private float gasGiantOblateness = 0.0f;
	
	[SerializeField]
	private int atmosphereRenderQueue = 3000;
	
	[SerializeField]
	private float atmosphereDensity = 4.0f;
	
	[SerializeField]
	private float atmosphereDensityFalloff = 5.0f;
	
	[SerializeField]
	private Texture atmosphereDayTexture;
	
	[SerializeField]
	private Texture atmosphereNightTexture;
	
	[SerializeField]
	private SGT_ColourGradient atmosphereLightingColour;
	
	[SerializeField]
	private SGT_ColourGradient atmosphereTwilightColour;
	
	[SerializeField]
	private SGT_ColourGradient atmosphereLimbColour;
	
	[SerializeField]
	private SGT_SquareSize lutSize = SGT_SquareSize.Square32;
	
	[SerializeField]
	private GameObject atmosphereGameObject;
	
	[SerializeField]
	private GameObject oblatenessGameObject;
	
	[SerializeField]
	private SGT_Mesh atmosphereMesh;
	
	[SerializeField]
	private string atmosphereTechnique;
	
	[SerializeField]
	private Material atmosphereMaterial;
	
	[SerializeField]
	private Texture2D lightingTexture;
	
	[SerializeField]
	private bool shadow;
	
	[SerializeField]
	private Texture2D shadowTexture;
	
	[SerializeField]
	private bool shadowAutoUpdate = true;
	
	[SerializeField]
	private SGT_ShadowOccluder shadowType;
	
	[SerializeField]
	private GameObject shadowGameObject;
	
	[SerializeField]
	private float shadowRadius = 10.0f;
	
	[SerializeField]
	private float shadowWidth = 10.0f;
	
	[SerializeField]
	private float maxDepth;
	
	public float AtmosphereRadius
	{
		set
		{
			gasGiantEquatorialRadius = value;
		}
		
		get
		{
			return gasGiantEquatorialRadius;
		}
	}
	
	public float AtmosphereOblateness
	{
		set
		{
			gasGiantOblateness = value;
		}
		
		get
		{
			return gasGiantOblateness;
		}
	}
	
	public SGT_LightSource GasGiantLightSource
	{
		set
		{
			gasGiantLightSource = value;
		}
		
		get
		{
			return gasGiantLightSource;
		}
	}
	
	public Camera GasGiantObserver
	{
		set
		{
			gasGiantObserver = value;
		}
		
		get
		{
			return gasGiantObserver;
		}
	}
	
	public Mesh GasGiantMesh
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
	
	public int GasGiantRenderQueue
	{
		set
		{
			atmosphereRenderQueue = value;
			
			SGT_Helper.SetRenderQueue(atmosphereMaterial, atmosphereRenderQueue);
		}
		
		get
		{
			return atmosphereRenderQueue;
		}
	}
	
	public float AtmosphereDensity
	{
		set
		{
			atmosphereDensity = Mathf.Max(value, 0.0f);
		}
		
		get
		{
			return atmosphereDensity;
		}
	}
	
	public float AtmosphereDensityFalloff
	{
		set
		{
			atmosphereDensityFalloff = Mathf.Max(value, 0.0f);
		}
		
		get
		{
			return atmosphereDensityFalloff;
		}
	}
	
	public Texture AtmosphereTextureDay
	{
		set
		{
			atmosphereDayTexture = value;
		}
		
		get
		{
			return atmosphereDayTexture;
		}
	}
	
	public Texture AtmosphereTextureNight
	{
		set
		{
			atmosphereNightTexture = value;
		}
		
		get
		{
			return atmosphereNightTexture;
		}
	}
	
	public SGT_ColourGradient AtmosphereLighting
	{
		set
		{
		}
		
		get
		{
			return atmosphereLightingColour;
		}
	}
	
	public SGT_ColourGradient AtmosphereTwilightColour
	{
		set
		{
		}
		
		get
		{
			return atmosphereTwilightColour;
		}
	}
	
	public SGT_ColourGradient AtmosphereLimbColour
	{
		set
		{
		}
		
		get
		{
			return atmosphereLimbColour;
		}
	}
	
	public SGT_SquareSize GasGiantLutSize
	{
		set
		{
			if (value != lutSize)
			{
				lutSize         = value;
				lightingTexture = SGT_Helper.DestroyObject(lightingTexture);
			}
		}
		
		get
		{
			return lutSize;
		}
	}
	
	public bool Shadow
	{
		set
		{
			shadow = value;
		}
		
		get
		{
			return shadow;
		}
	}
	
	public Texture2D ShadowTexture
	{
		set
		{
			shadowTexture = value;
		}
		
		get
		{
			return shadowTexture;
		}
	}
	
	public bool ShadowCasterAutoUpdate
	{
		set
		{
			shadowAutoUpdate = value;
		}
		
		get
		{
			return shadowAutoUpdate;
		}
	}
	
	public SGT_ShadowOccluder ShadowCasterType
	{
		set
		{
			shadowType = value;
		}
		
		get
		{
			return shadowType;
		}
	}
	
	public GameObject ShadowCasterGameObject
	{
		set
		{
			shadowGameObject = value;
		}
		
		get
		{
			return shadowGameObject;
		}
	}
	
	public float ShadowCasterRadius
	{
		set
		{
			shadowRadius = Mathf.Max(value, 0.0f);
		}
		
		get
		{
			return shadowRadius;
		}
	}
	
	public float ShadowCasterWidth
	{
		set
		{
			if (value > 0.0f)
			{
				shadowWidth = value;
			}
		}
		
		get
		{
			return shadowWidth;
		}
	}
	
	public float ShadowCasterRadiusInner
	{
		get
		{
			return shadowRadius - shadowWidth * 0.5f;
		}
	}
	
	public float ShadowCasterRadiusOuter
	{
		get
		{
			return shadowRadius + shadowWidth * 0.5f;
		}
	}
	
	public float GasGiantPolarRadius
	{
		get
		{
			return gasGiantEquatorialRadius * (1.0f - gasGiantOblateness);
		}
	}
	
	public Vector3 GasGiantLightSourceDirection
	{
		get
		{
			return (SGT_Helper.GetPosition(gasGiantLightSource) - transform.position).normalized;
		}
	}
	
	public override void BuildUndoTargets(ObjectList list)
	{
		base.BuildUndoTargets(list);
		
		if (atmosphereMesh != null)
		{
			atmosphereMesh.BuildUndoTargets(list);
		}
	}
}