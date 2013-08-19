using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Gravity Receiver")]
public class SGT_GravityReceiver : SGT_MonoBehaviour
{
	public enum GravityType
	{
		Single,
		Multiple
	}
	
	[SerializeField]
	private GravityType type = GravityType.Multiple;
	
	[SerializeField]
	private SGT_GravitySource[] gravitySources;
	
	public GravityType Type
	{
		set
		{
			type = value;
		}
		
		get
		{
			return type;
		}
	}
	
	public void FixedUpdate()
	{
		if (rigidbody != null)
		{
			gravitySources = SGT_CachedFind<SGT_GravitySource>.All(1.0f);
			
			foreach (var gravitySource in gravitySources)
			{
				rigidbody.AddForce(gravitySource.ForceAtPoint(transform.position) * Time.fixedDeltaTime, ForceMode.Acceleration);
			}
		}
	}
}