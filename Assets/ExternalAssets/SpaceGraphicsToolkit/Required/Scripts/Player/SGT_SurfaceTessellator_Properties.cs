using UnityEngine;

public partial class SGT_SurfaceTessellator
{
	[SerializeField]
	private float[] levelSize;
	
	[SerializeField]
	private float[] levelStep;
	
	[SerializeField]
	private float[] levelDistance;
	
	/*[SerializeField]*/
	private Patch[] sides;
	
	/*[SerializeField]*/
	private bool rebuild;
	
	/*[SerializeField]*/
	private Mesh[][] sideCombinedMeshes = new Mesh[6][];
	
	/*[SerializeField]*/
	private int[][] patchIndices; // Cannot be serialized
	
	/*[SerializeField]*/
	private System.Diagnostics.Stopwatch budgetUsage = new System.Diagnostics.Stopwatch();
	
	/*[SerializeField]*/
	private Vector3 surfaceObserverPosition;
	
	/*[SerializeField]*/
	private bool running;
	
	[SerializeField]
	private SGT_SurfaceConfiguration surfaceConfiguration = SGT_SurfaceConfiguration.Sphere;
	
	[SerializeField]
	private int patchResolution = 4;
	
	[SerializeField]
	private int maxLevels = 8;
	
	[SerializeField]
	private int verticesPerMesh = 5000;
	
	[SerializeField]
	private float minUpdateInterval = 0.0f;
	
	[SerializeField]
	private SGT_SurfaceTexture displacementTexture = new SGT_SurfaceTexture();
	
	[SerializeField]
	private float scaleMin = 1.0f;
	
	[SerializeField]
	private float scaleMax = 1.1f;
	
	[SerializeField]
	private float taskBudget = 0.001f;
	
	[SerializeField]
	private int maxSplitsPerFrame = 10;
	
	[SerializeField]
	private int maxStitchesPerFrame = 100;
	
	[SerializeField]
	private bool settleOnAwake;
	
	[SerializeField]
	private bool reportBudget;
	
	[SerializeField]
	private GameObject surface;
	
	[SerializeField]
	private Mesh lazyDupeCheck;
	
	public int LevelDistanceCount
	{
		get
		{
			return levelDistance.Length;
		}
	}
	
	public int PatchResolution
	{
		set
		{
			if (value != patchResolution && value % 2 == 0 && value >= 2)
			{
				patchResolution = value;
				
				RebuildLUT();
				RebuildPatches();
			}
		}
		
		get
		{
			return patchResolution;
		}
	}
	
	public int PatchMaxLevels
	{
		set
		{
			if (value != maxLevels)
			{
				maxLevels = value;
				
				RebuildLUT();
				RebuildPatches();
			}
		}
		
		get
		{
			return maxLevels;
		}
	}
	
	public int VerticesPerMesh
	{
		set
		{
			verticesPerMesh = value;
		}
		
		get
		{
			return verticesPerMesh;
		}
	}
	
	public float MinUpdateInterval
	{
		set
		{
			minUpdateInterval = value;
		}
		
		get
		{
			return minUpdateInterval;
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
				
				RebuildPatches();
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
			return displacementTexture;
		}
	}
	
	public float DisplacementScaleMin
	{
		set
		{
			if (value != scaleMin)
			{
				scaleMin = value;
				
				RebuildPatches();
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
				
				RebuildPatches();
			}
		}
		
		get
		{
			return scaleMax;
		}
	}
	
	public float TaskBudget
	{
		set
		{
			taskBudget = value;
		}
		
		get
		{
			return taskBudget;
		}
	}
	
	public int MaxSplitsPerFrame
	{
		set
		{
			maxSplitsPerFrame = value;
		}
		
		get
		{
			return maxSplitsPerFrame;
		}
	}
	
	public int MaxStitchesPerFrame
	{
		set
		{
			maxStitchesPerFrame = value;
		}
		
		get
		{
			return maxStitchesPerFrame;
		}
	}
	
	public bool SettleOnAwake
	{
		set
		{
			settleOnAwake = value;
		}
		
		get
		{
			return settleOnAwake;
		}
	}
	
	public bool ReportBudget
	{
		set
		{
			reportBudget = value;
		}
		
		get
		{
			return reportBudget;
		}
	}
}