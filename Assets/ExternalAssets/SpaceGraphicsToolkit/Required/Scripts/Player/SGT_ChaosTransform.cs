using ObjectList = System.Collections.Generic.List<UnityEngine.Object>;

using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Chaos Transform")]
public class SGT_ChaosTransform : SGT_MonoBehaviour
{
	[SerializeField]
	private int seed;
	
	[SerializeField]
	private bool rotation;
	
	[SerializeField]
	private float rotationPeriod = 1.0f;
	
	[SerializeField]
	private float rotationChangeDelay = 1.0f;
	
	[SerializeField]
	private float rotationDampening = 5.0f;
	
	/*[SerializeField]*/
	private float rotationChangeTime;
	
	/*[SerializeField]*/
	private float currentBearing;
	
	/*[SerializeField]*/
	private float targetBearing;
	
	[SerializeField]
	private bool scale;
	
	[SerializeField]
	private float scaleMin = 0.8f;
	
	[SerializeField]
	private float scaleMax = 1.2f;
	
	[SerializeField]
	private float scaleChangeDelay = 1.0f;
	
	[SerializeField]
	private float scaleDampening = 5.0f;
	
	/*[SerializeField]*/
	private float scaleChangeTime;
	
	/*[SerializeField]*/
	private float targetScale;
	
	public int Seed
	{
		set
		{
			seed = value;
		}
		
		get
		{
			return seed;
		}
	}
	
	public bool Rotation
	{
		set
		{
			rotation = value;
		}
		
		get
		{
			return rotation;
		}
	}
	
	public float RotationPeriod
	{
		set
		{
			rotationPeriod = value;
		}
		
		get
		{
			return rotationPeriod;
		}
	}
	
	public float RotationChangeDelay
	{
		set
		{
			rotationChangeDelay = value;
		}
		
		get
		{
			return rotationChangeDelay;
		}
	}
	
	public float RotationDampening
	{
		set
		{
			rotationDampening = value;
		}
		
		get
		{
			return rotationDampening;
		}
	}
	
	public bool Scale
	{
		set
		{
			scale = value;
		}
		
		get
		{
			return scale;
		}
	}
	
	public float ScaleMin
	{
		set
		{
			scaleMin = value;
		}
		
		get
		{
			return scaleMin;
		}
	}
	
	public float ScaleMax
	{
		set
		{
			scaleMax = value;
		}
		
		get
		{
			return scaleMax;
		}
	}
	
	public float ScaleChangeDelay
	{
		set
		{
			scaleChangeDelay = value;
		}
		
		get
		{
			return scaleChangeDelay;
		}
	}
	
	public float ScaleDampening
	{
		set
		{
			scaleDampening = value;
		}
		
		get
		{
			return scaleDampening;
		}
	}
	
	public override void BuildUndoTargets(ObjectList list)
	{
		base.BuildUndoTargets(list);
	}
	
	public void Update()
	{
		if (Application.isPlaying == true)
		{
			SGT_Helper.BeginRandomSeed(seed);
			{
				if (rotation == true)
				{
					rotationChangeTime -= Time.deltaTime;
					
					if (rotationChangeTime <= 0.0f)
					{
						rotationChangeTime = rotationChangeDelay;
						
						targetBearing = Random.Range(-1.0f, 1.0f);
					}
					
					var dampenFactor = SGT_Helper.DampenFactor(rotationDampening, Time.deltaTime);
					
					currentBearing = Mathf.Lerp(currentBearing, targetBearing, dampenFactor);
					
					var dps = SGT_Helper.DegreesPerSecond(rotationPeriod);
					
					transform.localRotation *= Quaternion.Euler(dps * currentBearing * Time.deltaTime, dps * Time.deltaTime, 0.0f);
				}
				
				if (scale == true)
				{
					scaleChangeTime -= Time.deltaTime;
					
					if (scaleChangeTime <= 0.0f)
					{
						scaleChangeTime = scaleChangeDelay;
						
						targetScale = Random.Range(scaleMin, scaleMax);
					}
					
					var dampenFactor = SGT_Helper.DampenFactor(scaleDampening, Time.deltaTime);
					var newScale     = Mathf.Lerp(UniformScale, targetScale, dampenFactor);
					
					transform.localScale = new Vector3(newScale, newScale, newScale);
				}
			}
			seed = SGT_Helper.EndRandomSeed();
		}
	}
}