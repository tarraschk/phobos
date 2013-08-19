using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Example/Randomise Debris")]
public class SGT_RandomiseDebris : SGT_MonoBehaviour
{
	/*[SerializeField]*/
	private SGT_DebrisSpawner parent;
	
	[SerializeField]
	private float minScale = 1.0f;
	
	[SerializeField]
	private float maxScale = 1.0f;
	
	[SerializeField]
	private float maxSpeed = 1.0f;
	
	[SerializeField]
	private float maxSpin = 1.0f;
	
	[SerializeField]
	private float scale = 1.0f;
	
	[SerializeField]
	private bool hasBeenReset;
	
	[SerializeField]
	private float massScale;
	
	public float MinScale
	{
		set
		{
			minScale = value;
		}
		
		get
		{
			return minScale;
		}
	}
	
	public float MaxScale
	{
		set
		{
			maxScale = value;
		}
		
		get
		{
			return maxScale;
		}
	}
	
	public float MaxSpeed
	{
		set
		{
			maxSpeed = value;
		}
		
		get
		{
			return maxSpeed;
		}
	}
	
	public float MaxSpin
	{
		set
		{
			maxSpin = value;
		}
		
		get
		{
			return maxSpin;
		}
	}
	
	public float MassScale
	{
		set
		{
			massScale = value;
		}
		
		get
		{
			return massScale;
		}
	}
	
	public void Start()
	{
		ResetAsteroid();
	}
	
	public void Update()
	{
		StepScale();
	}
	
	public void ResetDebrisPosition()
	{
		hasBeenReset = false;
		
		ResetAsteroid();
	}
	
	public float RSpin
	{
		get
		{
			return Random.Range(-maxSpin, maxSpin);
		}
	}
	
	private void ResetAsteroid()
	{
		if (hasBeenReset == false)
		{
			hasBeenReset = true;
			scale        = Random.Range(minScale, maxScale);
			
			StepScale();
			
			transform.localRotation = Random.rotation;
			
			if (rigidbody != null && massScale > 0.0f)
			{
				rigidbody.mass = massScale * ((4.0f / 3.0f) * Mathf.PI * (scale * scale * scale));
			}
		}
		
		// Note: These can't be set outside of play mode, so we set it here, which will be called during Start
		if (rigidbody != null)
		{
			rigidbody.velocity        = Random.insideUnitSphere * maxSpeed;
			rigidbody.angularVelocity = new Vector3(RSpin, RSpin, RSpin);
		}
	}
	
	private void StepScale()
	{
		if (parent == null) parent = SGT_Helper.GetComponentUpwards<SGT_DebrisSpawner>(gameObject);
		
		if (parent != null)
		{
			var position = SGT_Helper.GetPosition(parent.DebrisCentre);
			var distance = (position - transform.position).magnitude;
			var scaleMul = 1.0f - SGT_Helper.RemapClamped(parent.DebrisContainerInnerRadius, parent.DebrisContainerRadius, distance, 0.0f, 1.0f);
			
			SGT_Helper.SetLocalScale(transform, SGT_Helper.NewVector3(scale * scaleMul));
		}
	}
}