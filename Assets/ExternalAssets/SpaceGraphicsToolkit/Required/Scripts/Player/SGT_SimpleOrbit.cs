using ObjectList = System.Collections.Generic.List<UnityEngine.Object>;

using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Simple Orbit")]
public class SGT_SimpleOrbit : SGT_MonoBehaviour
{
	[SerializeField]
	private bool orbit;
	
	[SerializeField]
	private float orbitDistance;
	
	[SerializeField]
	private float orbitAngle;
	
	[SerializeField]
	private float orbitOblateness;
	
	[SerializeField]
	private float orbitPeriod;
	
	[SerializeField]
	private bool rotation;
	
	[SerializeField]
	private float rotationPeriod;
	
	[SerializeField]
	private Vector3 rotationAxis = Vector3.up;
	
	public bool Orbit
	{
		set
		{
			orbit = value;
		}
		
		get
		{
			return orbit;
		}
	}
	
	public float OrbitDistance
	{
		set
		{
			orbitDistance = value;
		}
		
		get
		{
			return orbitDistance;
		}
	}
	
	public float OrbitAngle
	{
		set
		{
			orbitAngle = value;
		}
		
		get
		{
			return orbitAngle;
		}
	}
	
	public float OrbitOblateness
	{
		set
		{
			orbitOblateness = Mathf.Clamp01(value);
		}
		
		get
		{
			return orbitOblateness;
		}
	}
	
	public float OrbitPeriod
	{
		set
		{
			orbitPeriod = value;
		}
		
		get
		{
			return orbitPeriod;
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
	
	public Vector3 RotationAxis
	{
		set
		{
			rotationAxis = value;
		}
		
		get
		{
			return rotationAxis;
		}
	}
	
	public override void BuildUndoTargets(ObjectList list)
	{
		base.BuildUndoTargets(list);
		
		list.Add(transform);
	}
	
	public void Update()
	{
		if (orbit == true)
		{
			if (Application.isPlaying == true)
			{
				orbitAngle += SGT_Helper.RadiansPerSecond(orbitPeriod) * Time.deltaTime;
			}
			
			var targetPosition = SGT_Helper.PolarToCartesian(new Vector2(orbitAngle, 0.0f)) * orbitDistance; targetPosition.x *= (1.0f - orbitOblateness);
			
			SGT_Helper.SetLocalPosition(transform, targetPosition);
		}
		
		if (rotation == true)
		{
			if (Application.isPlaying == true)
			{
				transform.Rotate(rotationAxis, SGT_Helper.DegreesPerSecond(rotationPeriod) * Time.deltaTime, Space.Self);
			}
		}
	}
	
#if UNITY_EDITOR == true
	protected virtual void OnDrawGizmosSelected()
	{
		var position = transform.parent != null ? transform.parent.transform.position : Vector3.zero;
		var rotation = transform.parent != null ? transform.parent.transform.rotation : Quaternion.identity;
		
		SGT_Handles.Colour = new Color(1.0f, 1.0f, 1.0f, 0.5f); SGT_Handles.DrawDisc(position, rotation, new Vector2((1.0f - orbitOblateness) * orbitDistance, orbitDistance));
	}
#endif
}