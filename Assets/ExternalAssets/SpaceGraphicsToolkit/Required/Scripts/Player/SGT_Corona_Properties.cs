using ObjectList = System.Collections.Generic.List<UnityEngine.Object>;

using UnityEngine;

public partial class SGT_Corona
{
	public enum Type
	{
		Plane,
		Ring
	}
	
	public enum Alignment
	{
		Billboard,
		AxisAlined,
		Random
	}
	
	[SerializeField]
	private bool coronaAutoRegen = true;
	
	[SerializeField]
	private bool modified = true;
	
	[SerializeField]
	private GameObject coronaGameObject;
	
	[SerializeField]
	private SGT_Mesh coronaMesh;
	
	[SerializeField]
	private Mesh generatedMesh;
	
	[SerializeField]
	private Vector3 centrePosition;
	
	[SerializeField]
	private Vector3 observerPosition;
	
	[SerializeField]
	private string coronaTechnique;
	
	[SerializeField]
	private Material coronaMaterial;
	
	[SerializeField]
	private int coronaRenderQueue = 3000;
	
	[SerializeField]
	private bool coronaPerPixel;
	
	[SerializeField]
	private Alignment meshAlignment = Alignment.Billboard;
	
	[SerializeField]
	private Type meshType = Type.Plane;
	
	[SerializeField]
	private int meshPlaneCount = 10;
	
	[SerializeField]
	private Texture coronaTexture;
	
	[SerializeField]
	private float meshRadius = 100.0f;
	
	[SerializeField]
	private int meshSegments = 16;
	
	[SerializeField]
	private float meshHeight = 10.0f;
	
	[SerializeField]
	private Color coronaColour = Color.yellow;
	
	[SerializeField]
	private float coronaFalloff = 1.0f;
	
	[SerializeField]
	private float coronaOffset = -0.1f;
	
	[SerializeField]
	private Camera coronaObserver;
	
	[SerializeField]
	private bool cullNear;
	
	[SerializeField]
	private float cullNearLength = 10.0f;
	
	[SerializeField]
	private float cullNearOffset = 0.0f;
	
	[SerializeField]
	private int meshSeed;
	
	public bool CoronaAutoRegen
	{
		set
		{
			coronaAutoRegen = value;
		}
		
		get
		{
			return coronaAutoRegen;
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
	
	public bool CoronaPerPixel
	{
		set
		{
			coronaPerPixel = value;
		}
		
		get
		{
			return coronaPerPixel;
		}
	}
	
	public Texture CoronaTexture
	{
		set
		{
			coronaTexture = value;
		}
		
		get
		{
			return coronaTexture;
		}
	}
	
	public Color CoronaColour
	{
		set
		{
			coronaColour = value;
		}
		
		get
		{
			return coronaColour;
		}
	}
	
	public float CoronaFalloff
	{
		set
		{
			coronaFalloff = Mathf.Max(value, 0.0f);
		}
		
		get
		{
			return coronaFalloff;
		}
	}
	
	public float CoronaOffset
	{
		set
		{
			if (value != CoronaOffset)
			{
				coronaOffset = value;
				
				UpdateCoronaOffset();
			}
		}
		
		get
		{
			return coronaOffset;
		}
	}
	
	public Camera CoronaObserver
	{
		set
		{
			coronaObserver = value;
		}
		
		get
		{
			return coronaObserver;
		}
	}
	
	public int MeshRenderQueue
	{
		set
		{
			coronaRenderQueue = value;
			
			SGT_Helper.SetRenderQueue(coronaMaterial, coronaRenderQueue);
		}
		
		get
		{
			return coronaRenderQueue;
		}
	}
	
	public Alignment MeshAlignment
	{
		set
		{
			if (value != meshAlignment)
			{
				meshAlignment = value;
				modified      = true;
			}
		}
		
		get
		{
			return meshAlignment;
		}
	}
	
	public Type MeshType
	{
		set
		{
			if (value != meshType)
			{
				meshType = value;
				modified = true;
			}
		}
		
		get
		{
			return meshType;
		}
	}
	
	public int MeshPlaneCount
	{
		set
		{
			if (value != meshPlaneCount)
			{
				meshPlaneCount = value;
				modified       = true;
			}
		}
		
		get
		{
			return meshPlaneCount;
		}
	}
	
	public float MeshRadius
	{
		set
		{
			if (value != meshRadius)
			{
				meshRadius = value;
				modified   = true;
			}
		}
		
		get
		{
			return meshRadius;
		}
	}
	
	public float MeshHeight
	{
		set
		{
			if (value != meshHeight)
			{
				meshHeight = value;
				modified   = true;
			}
		}
		
		get
		{
			return meshHeight;
		}
	}
	
	public int MeshSegments
	{
		set
		{
			if (value != meshSegments)
			{
				meshSegments = value;
				modified     = true;
			}
		}
		
		get
		{
			return meshSegments;
		}
	}
	
	public int MeshSeed
	{
		set
		{
			if (value != meshSeed)
			{
				meshSeed = value;
				modified = true;
			}
		}
		
		get
		{
			return meshSeed;
		}
	}
	
	public bool CullNear
	{
		set
		{
			cullNear = value;
		}
		
		get
		{
			return cullNear;
		}
	}
	
	public float CullNearLength
	{
		set
		{
			if (value > 0.0f)
			{
				cullNearLength = value;
			}
		}
		
		get
		{
			return cullNearLength;
		}
	}
	
	public float CullNearOffset
	{
		set
		{
			cullNearOffset = value;
		}
		
		get
		{
			return cullNearOffset;
		}
	}
}