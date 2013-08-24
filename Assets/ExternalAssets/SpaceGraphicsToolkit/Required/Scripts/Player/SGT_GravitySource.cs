using GravitySourceSet = System.Collections.Generic.HashSet<SGT_GravitySource>;

using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Gravity Source")]
public class SGT_GravitySource : SGT_MonoBehaviour
{
	private static GravitySourceSet gravitySources = new GravitySourceSet();
	
	public enum GravityType
	{
		Linear,
		Exponential
	}
	
	[SerializeField]
	private GravityType gravitySourceType = GravityType.Exponential;
	
	[SerializeField]
	private float gravitySourceForce = 100.0f;
	
	[SerializeField]
	private float gravitySourceRadius = 0.0f;
	
	[SerializeField]
	private float gravitySourceHeight = 1000.0f;
	
	public GravityType GravitySourceType
	{
		set
		{
			gravitySourceType = value;
		}
		
		get
		{
			return gravitySourceType;
		}
	}
	
	public float GravitySourceForce
	{
		set
		{
			gravitySourceForce = value;
		}
		
		get
		{
			return gravitySourceForce;
		}
	}
	
	public float GravitySourceRadius
	{
		set
		{
			gravitySourceRadius = value;
		}
		
		get
		{
			return gravitySourceRadius;
		}
	}
	
	public float GravitySourceHeight
	{
		set
		{
			gravitySourceHeight = value;
		}
		
		get
		{
			return gravitySourceHeight;
		}
	}
	
	public void OnEnable()
	{
		gravitySources.Add(this);
	}
	
	public void OnDisable()
	{
		gravitySources.Remove(this);
	}
	
	public Vector3 ForceAtPoint(Vector3 xyz)
	{
		var newForce = transform.position - xyz;
		
		if (newForce.sqrMagnitude > 0.0f)
		{
			switch (gravitySourceType)
			{
				case GravityType.Linear:
				{
					newForce = newForce.normalized * SGT_Helper.RemapClamped(gravitySourceRadius, gravitySourceRadius + gravitySourceHeight, newForce.magnitude, 0.0f, gravitySourceForce);
				}
				break;
				case GravityType.Exponential:
				{
					newForce = (newForce / newForce.sqrMagnitude) * gravitySourceForce;
				}
				break;
			}
		}
		
		return newForce;
	}
	
#if UNITY_EDITOR == true
	protected virtual void OnDrawGizmosSelected()
	{
		SGT_Handles.Colour = new Color(1.0f, 1.0f, 1.0f, 0.5f);  SGT_Handles.DrawSphere(transform.position, gravitySourceRadius);
		SGT_Handles.Colour = new Color(1.0f, 1.0f, 1.0f, 0.25f); SGT_Handles.DrawSphere(transform.position, gravitySourceRadius + gravitySourceHeight);
	}
#endif
}