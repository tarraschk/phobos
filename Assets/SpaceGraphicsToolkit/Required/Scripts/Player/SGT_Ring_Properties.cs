using ObjectList = System.Collections.Generic.List<UnityEngine.Object>;

using UnityEngine;

public partial class SGT_Ring
{
	[SerializeField]
	private bool ringAutoRegen = true;
	
	[SerializeField]
	private bool modified = true;
	
	[SerializeField]
	private bool sliced = true;
	
	[SerializeField]
	private int ringSlices = 4;
	
	[SerializeField]
	private int ringSegments = 5;
	
	[SerializeField]
	private bool tiled;
	
	[SerializeField]
	private int ringTextureRepeat = 1;
	
	[SerializeField]
	private string technique;
	
	[SerializeField]
	private GameObject ringGameObject;
	
	[SerializeField]
	private SGT_MultiMesh ringMesh;
	
	[SerializeField]
	private Material ringMaterial;
	
	[SerializeField]
	private Mesh generatedMesh;
	
	[SerializeField]
	private Quaternion[] generatedRotations;
	
	[SerializeField]
	private float ringRadius = 30.0f;
	
	[SerializeField]
	private float ringWidth = 30.0f;
	
	[SerializeField]
	private Texture ringTexture;
	
	[SerializeField]
	private SGT_LightSource lightSource;
	
	[SerializeField]
	private int renderQueue = 3000;
	
	[SerializeField]
	private bool lit;
	
	[SerializeField]
	private float litBrightnessMin = 0.5f;
	
	[SerializeField]
	private float litBrightnessMax = 1.0f;
	
	[SerializeField]
	private bool scattering;
	
	[SerializeField]
	private float scatteringMie = 0.5f;
	
	[SerializeField]
	private float scatteringOcclusion = 2.0f;
	
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
			if (modified == false)
			{
				CheckForModifications();
			}
			
			return modified;
		}
	}
	
	public bool Scattering
	{
		set
		{
			scattering = value;
		}
		
		get
		{
			return scattering;
		}
	}
	
	public float ScatteringMie
	{
		set
		{
			scatteringMie = value;
		}
		
		get
		{
			return scatteringMie;
		}
	}
	
	public float ScatteringOcclusion
	{
		set
		{
			scatteringOcclusion = value;
		}
		
		get
		{
			return scatteringOcclusion;
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
	
	public bool RingSliced
	{
		set
		{
			if (value != sliced)
			{
				sliced   = value;
				modified = true;
			}
		}
		
		get
		{
			return sliced;
		}
	}
	
	public int RingSlicedSlices
	{
		set
		{
			value = Mathf.Clamp(value, 1, 100);
			
			if (value != ringSlices)
			{
				ringSlices = value;
				modified   = true;
			}
		}
		
		get
		{
			return ringSlices;
		}
	}
	
	public int RingSlicedSegmentsPerSlice
	{
		set
		{
			value = Mathf.Max(value, 3);
			
			if (value != ringSegments)
			{
				ringSegments = value;
				modified     = true;
			}
		}
		
		get
		{
			return ringSegments;
		}
	}
	
	public bool Tiled
	{
		set
		{
			tiled = value;
		}
		
		get
		{
			return tiled;
		}
	}
	
	public int RingSlicedTextureRepeat
	{
		set
		{
			if (value != ringTextureRepeat)
			{
				ringTextureRepeat = value;
				modified          = true;
			}
		}
		
		get
		{
			return ringTextureRepeat;
		}
	}
	
	public bool Lit
	{
		set
		{
			lit = value;
		}
		
		get
		{
			return lit;
		}
	}
	
	public float LitBrightnessMin
	{
		set
		{
			litBrightnessMin = value;
		}
		
		get
		{
			return litBrightnessMin;
		}
	}
	
	public float LitBrightnessMax
	{
		set
		{
			litBrightnessMax = value;
		}
		
		get
		{
			return litBrightnessMax;
		}
	}
	
	public Texture RingTexture
	{
		set
		{
			ringTexture = value;
		}
		
		get
		{
			return ringTexture;
		}
	}
	
	public float RingRadius
	{
		set
		{
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
	
	public SGT_LightSource RingLightSource
	{
		set
		{
			lightSource = value;
		}
		
		get
		{
			return lightSource;
		}
	}
	
	public int RingRenderQueue
	{
		set
		{
			if (value != renderQueue)
			{
				renderQueue = value;
			}
		}
		
		get
		{
			return renderQueue;
		}
	}
	
	public Vector3 StarDirection
	{
		get
		{
			return (SGT_Helper.GetPosition(lightSource) - transform.position).normalized;
		}
	}
	
	public override void BuildUndoTargets(ObjectList list)
	{
		base.BuildUndoTargets(list);
		
		list.Add(ringGameObject);
		if (ringMesh != null) ringMesh.BuildUndoTargets(list);
	}
}