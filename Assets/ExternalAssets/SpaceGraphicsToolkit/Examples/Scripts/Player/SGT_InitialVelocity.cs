using UnityEngine;

[AddComponentMenu("Space Graphics Toolkit/Example/Initial Velocity")]
public class SGT_InitialVelocity : SGT_MonoBehaviour
{
	[SerializeField]
	private Vector3 initialVelocity;
	
	public Vector3 InitialVelocity
	{
		set
		{
			initialVelocity = value;
		}
		
		get
		{
			return initialVelocity;
		}
	}
	
	public void Start()
	{
		if (rigidbody != null)
		{
			rigidbody.velocity = initialVelocity;
		}
		
		SGT_Helper.DestroyObject(this);
	}
}