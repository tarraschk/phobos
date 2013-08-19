using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Example/Camera Move")]
public class SGT_CameraMove : SGT_MonoBehaviour
{
	public enum MovementPlane
	{
		LocalXZ,
		LocalXY
	}
	
	[SerializeField]
	private MovementPlane movePlane = MovementPlane.LocalXZ;
	
	[SerializeField]
	private float moveSpeed = 100.0f;
	
	[SerializeField]
	private float moveSmooth = 0.25f;
	
	[SerializeField]
	private float shiftSpeedMul = 0.1f;
	
	[SerializeField]
	private bool repelBodies = true;
	
	[SerializeField]
	private float repelDistance = 0.5f;
	
	/*[SerializeField]*/
	private Vector3 translationTgt;
	
	/*[SerializeField]*/
	private Vector3 translationCur;
	
	/*[SerializeField]*/
	private Vector3 translationVel;
	
	public MovementPlane MovePlane
	{
		set
		{
			movePlane = value;
		}
		
		get
		{
			return movePlane;
		}
	}
	
	public float MoveSpeed
	{
		set
		{
			moveSpeed = value;
		}
		
		get
		{
			return moveSpeed;
		}
	}
	
	public float MoveSmooth
	{
		set
		{
			moveSmooth = value;
		}
		
		get
		{
			return moveSmooth;
		}
	}
	
	public float MoveShiftSpeedMul
	{
		set
		{
			shiftSpeedMul = value;
		}
		
		get
		{
			return shiftSpeedMul;
		}
	}
	
	public bool RepelBodies
	{
		set
		{
			repelBodies = value;
		}
		
		get
		{
			return repelBodies;
		}
	}
	
	public float RepelDistance
	{
		set
		{
			repelDistance = value;
		}
		
		get
		{
			return repelDistance;
		}
	}
	
	public void FixedUpdate()
	{
		if (rigidbody != null)
		{
			translationVel += rigidbody.velocity;
			rigidbody.velocity        = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
		}
	}
	
	public void Update()
	{
		if (rigidbody != null)
		{
			translationVel += rigidbody.velocity;
			rigidbody.velocity        = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
		}
		
		// Translation
		var moveMul = Input.GetKey(KeyCode.LeftShift) == true ? shiftSpeedMul : 1.0f;
		
		translationTgt.x = SGT_Input.MoveX * moveSpeed * moveMul;
		translationTgt.z = SGT_Input.MoveY * moveSpeed * moveMul;
		
		moveSpeed = Mathf.Max(0.0f, moveSpeed + SGT_Input.Zoom * moveSpeed * 2.0f);
		
		if (translationCur != translationTgt)
		{
			translationCur.x = Mathf.SmoothDamp(translationCur.x, translationTgt.x, ref translationVel.x, moveSmooth, float.PositiveInfinity, SGT_Helper.DeltaTime);
			translationCur.z = Mathf.SmoothDamp(translationCur.z, translationTgt.z, ref translationVel.z, moveSmooth, float.PositiveInfinity, SGT_Helper.DeltaTime);
		}
		
		var translation = translationCur * SGT_Helper.DeltaTime;
		
		if (translation != Vector3.zero)
		{
			switch (movePlane)
			{
				case MovementPlane.LocalXZ: transform.Translate(translation.x, 0.0f, translation.z); break;
				case MovementPlane.LocalXY: transform.Translate(translation.x, translation.z, 0.0f); break;
			}
		}
		
		// Prevent the camera from entering planets/stars
		if (repelBodies == true)
		{
			var currentPosition = transform.position;
			
			var planets = SGT_CachedFind<SGT_Planet>.All(1.0f);
			
			foreach (var planet in planets)
			{
				if (planet != null)
				{
					var radius = planet.SurfaceRadiusAtPoint(currentPosition) * SGT_SurfaceHelper.FindDisplacement(planet.gameObject, currentPosition);
					
					RepelByRadius(planet.transform.position, radius * planet.UniformScale);
				}
			}
			
			var stars = SGT_CachedFind<SGT_Star>.All(1.0f);
			
			foreach (var star in stars)
			{
				if (star != null)
				{
					var radius = star.SurfaceRadiusAtPoint(currentPosition) * SGT_SurfaceHelper.FindDisplacement(star.gameObject, currentPosition);
					
					RepelByRadius(star.transform.position, radius * star.UniformScale);
				}
			}
		}
	}
	
	private void RepelByRadius(Vector3 centre, float radius)
	{
		radius += repelDistance;
		
		var rel      = transform.position - centre;
		var distance = rel.magnitude;
		
		if (distance < radius && radius > 0.0f)
		{
			transform.position = centre + rel * (radius / distance);
		}
	}
	
	private void RepelByDistance(Vector3 centre, float distance)
	{
		distance -= repelDistance;
		
		var relDir = (transform.position - centre).normalized;
		
		if (distance < 0.0f)
		{
			transform.position -= relDir * distance;
		}
	}
}