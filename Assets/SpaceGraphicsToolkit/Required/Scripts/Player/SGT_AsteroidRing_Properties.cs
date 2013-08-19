using ObjectList = System.Collections.Generic.List<UnityEngine.Object>;

using UnityEngine;

public partial class SGT_AsteroidRing
{
	public enum Distribution
	{
		Uniform,
		Exponential2,
		Bates2
	}
	
	[SerializeField]
	private bool ringAutoRegen = true;
	
	[SerializeField]
	private string technique;
	
	[SerializeField]
	private bool modified = true;
	
	[SerializeField]
	protected GameObject ringGameObject;
	
	[SerializeField]
	protected SGT_MultiMesh ringMultiMesh;
	
	[SerializeField]
	protected Material ringMaterial;
	
	[SerializeField]
	private Mesh[] generatedMeshes;
	
	[SerializeField]
	private int ringSeed;
	
	[SerializeField]
	private float ringRadius = 30.0f;
	
	[SerializeField]
	private float ringWidth = 30.0f;
	
	[SerializeField]
	private float ringHeight = 5.0f;
	
	[SerializeField]
	private int ringRenderQueue = 2450;
	
	[SerializeField]
	private SGT_LightSource ringLightSource;
	
	[SerializeField]
	private Texture2D asteroidTextureDay;
	
	[SerializeField]
	private Texture2D asteroidTextureNight;
	
	[SerializeField]
	private Texture2D asteroidTextureHeight;
	
	[SerializeField]
	private Distribution ringDistribution;
	
	[SerializeField]
	private int asteroidTextureTilesX = 1;
	
	[SerializeField]
	private int asteroidTextureTilesY = 1;
	
	[SerializeField]
	private int ringAsteroidCount = 1000;
	
	[SerializeField]
	private float asteroidRadiusMin = 1.0f;
	
	[SerializeField]
	private float asteroidRadiusMax = 3.0f;
	
	[SerializeField]
	private float orbitRateInner = 0.2f;
	
	[SerializeField]
	private float orbitRateOuter = 0.1f;
	
	[SerializeField]
	private float orbitRateDeviation = 0.0f;
	
	[SerializeField]
	private bool spin;
	
	[SerializeField]
	private float spinRateMax;
	
	[SerializeField]
	private bool shadow;
	
	[SerializeField]
	private bool shadowAutoUpdate = true;
	
	[SerializeField]
	private float shadowRadius = 10.0f;
	
	[SerializeField]
	private float shadowWidth = 10.0f;
	
	[SerializeField]
	private Color shadowUmbraColour;
	
	[SerializeField]
	private Color shadowPenumbraColour;
	
	public bool RingAutoRegen
	{
		set
		{
			ringAutoRegen = value;
		}
		
		get
		{
			return ringAutoRegen;
		}
	}
	
	public bool Modified
	{
		set
		{
			modified = value;
		}
		
		get
		{
			if (modified == false) CheckForModifications();
			
			return modified;
		}
	}
	
	public Texture2D AsteroidTextureDay
	{
		set
		{
			asteroidTextureDay = value;
		}
		
		get
		{
			return asteroidTextureDay;
		}
	}
	
	public Texture2D AsteroidTextureNight
	{
		set
		{
			asteroidTextureNight = value;
		}
		
		get
		{
			return asteroidTextureNight;
		}
	}
	
	public int AsteroidTextureTilesX
	{
		set
		{
			value = Mathf.Max(value, 1);
			
			if (value != asteroidTextureTilesX)
			{
				asteroidTextureTilesX = value;
				modified              = true;
			}
		}
		
		get
		{
			return asteroidTextureTilesX;
		}
	}
	
	public int AsteroidTextureTilesY
	{
		set
		{
			value = Mathf.Max(value, 1);
			
			if (value != asteroidTextureTilesY)
			{
				asteroidTextureTilesY = value;
				modified              = true;
			}
		}
		
		get
		{
			return asteroidTextureTilesY;
		}
	}
	
	public Texture2D AsteroidTextureHeight
	{
		set
		{
			asteroidTextureHeight = value;
		}
		
		get
		{
			return asteroidTextureHeight;
		}
	}
	
	public float RingRadius
	{
		set
		{
			value = Mathf.Max(value, 0.0f);
			
			if (value != ringRadius)
			{
				ringRadius = value;
				modified   = true;
			}
		}
		
		get
		{
			return ringRadius;
		}
	}
	
	public float RingWidth
	{
		set
		{
			value = Mathf.Max(value, 0.0f);
			
			if (value != ringWidth)
			{
				ringWidth = value;
				modified  = true;
			}
		}
		
		get
		{
			return ringWidth;
		}
	}
	
	public float RingRadiusInner
	{
		get
		{
			return ringRadius - ringWidth * 0.5f;
		}
	}
	
	public float RingRadiusOuter
	{
		get
		{
			return ringRadius + ringWidth * 0.5f;
		}
	}
	
	public float RingHeight
	{
		set
		{
			value = Mathf.Max(value, 0.0f);
			
			if (value != ringHeight)
			{
				ringHeight = value;
				modified   = true;
			}
		}
		
		get
		{
			return ringHeight;
		}
	}
	
	public SGT_LightSource RingLightSource
	{
		set
		{
			ringLightSource = value;
		}
		
		get
		{
			return ringLightSource;
		}
	}
	
	public bool Spin
	{
		set
		{
			spin = value;
		}
		
		get
		{
			return spin;
		}
	}
	
	public float SpinRateMax
	{
		set
		{
			spinRateMax = value;
		}
		
		get
		{
			return spinRateMax;
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
	
	public bool ShadowAutoUpdate
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
	
	public float ShadowRadius
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
	
	public float ShadowWidth
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
	
	public float ShadowInnerRadius
	{
		get
		{
			return shadowRadius - shadowWidth * 0.5f;
		}
	}
	
	public float ShadowOuterRadius
	{
		get
		{
			return shadowRadius + shadowWidth * 0.5f;
		}
	}
	
	public Color ShadowUmbraColour
	{
		set
		{
			shadowUmbraColour = value;
		}
		
		get
		{
			return shadowUmbraColour;
		}
	}
	
	public Color ShadowPenumbraColour
	{
		set
		{
			shadowPenumbraColour = value;
		}
		
		get
		{
			return shadowPenumbraColour;
		}
	}
	
	public Distribution RingDistribution
	{
		set
		{
			if (value != ringDistribution)
			{
				ringDistribution = value;
				modified         = true;
			}
		}
		
		get
		{
			return ringDistribution;
		}
	}
	
	public int RingAsteroidCount
	{
		set
		{
			value = Mathf.Max(1, value);
			
			if (value != ringAsteroidCount)
			{
				ringAsteroidCount = value;
				modified          = true;
			}
		}
		
		get
		{
			return ringAsteroidCount;
		}
	}
	
	public float AsteroidRadiusMin
	{
		set
		{
			value = Mathf.Max(value, 0.0f);
			
			if (value != asteroidRadiusMin)
			{
				asteroidRadiusMin = value;
				modified          = true;
			}
		}
		
		get
		{
			return asteroidRadiusMin;
		}
	}
	
	public float AsteroidRadiusMax
	{
		set
		{
			value = Mathf.Max(value, 0.0f);
			
			if (value != asteroidRadiusMax)
			{
				asteroidRadiusMax = value;
				modified          = true;
			}
		}
		
		get
		{
			return asteroidRadiusMax;
		}
	}
	
	public float OrbitRateInner
	{
		set
		{
			if (value != orbitRateInner)
			{
				orbitRateInner = value;
				modified       = true;
			}
		}
		
		get
		{
			return orbitRateInner;
		}
	}
	
	public float OrbitRateOuter
	{
		set
		{
			if (value != orbitRateOuter)
			{
				orbitRateOuter = value;
				modified       = true;
			}
		}
		
		get
		{
			return orbitRateOuter;
		}
	}
	
	public float OrbitRateDeviation
	{
		set
		{
			if (value != orbitRateDeviation)
			{
				orbitRateDeviation = Mathf.Max(value, 0.0f);
				modified           = true;
			}
		}
		
		get
		{
			return orbitRateDeviation;
		}
	}
	
	public int RingSeed
	{
		set
		{
			if (value != ringSeed)
			{
				ringSeed = value;
				modified = true;
			}
		}
		
		get
		{
			return ringSeed;
		}
	}
	
	public int RingRenderQueue
	{
		set
		{
			if (value != ringRenderQueue)
			{
				ringRenderQueue = value;
				
				SGT_Helper.SetRenderQueue(ringMaterial, ringRenderQueue);
			}
		}
		
		get
		{
			return ringRenderQueue;
		}
	}
	
	public override void BuildUndoTargets(ObjectList list)
	{
		base.BuildUndoTargets(list);
		
		if (ringMultiMesh != null) ringMultiMesh.BuildUndoTargets(list);
	}
}