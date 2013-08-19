using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Example/Camera Free Orbit")]
public class SGT_CameraFreeOrbit : SGT_MonoBehaviour
{
	[SerializeField]
	private Quaternion targetRotation;
	
	[SerializeField]
	private float rotationSpeed = 2500.0f;
	
	[SerializeField]
	private float rotationDampening = 10.0f;
	
	[SerializeField]
	private float targetDistance = 120.0f;
	
	[SerializeField]
	private float distanceMin = 120.0f;
	
	[SerializeField]
	private float distanceMax = 120.0f;
	
	[SerializeField]
	private float distanceSpeed = 3.0f;
	
	[SerializeField]
	private float distanceDampening = 10.0f;
	
	[SerializeField]
	private bool rotationRoll = true;
	
	public Quaternion Rotation
	{
		set
		{
			targetRotation = value;
		}
		
		get
		{
			return targetRotation;
		}
	}
	
	public float RotationSpeed
	{
		set
		{
			rotationSpeed = value;
		}
		
		get
		{
			return rotationSpeed;
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
	
	public float Distance
	{
		set
		{
			targetDistance = value;
		}
		
		get
		{
			return targetDistance;
		}
	}
	
	public float DistanceMin
	{
		set
		{
			distanceMin = value;
		}
		
		get
		{
			return distanceMin;
		}
	}
	
	public float DistanceMax
	{
		set
		{
			distanceMax = value;
		}
		
		get
		{
			return distanceMax;
		}
	}
	
	public float DistanceSpeed
	{
		set
		{
			distanceSpeed = value;
		}
		
		get
		{
			return distanceSpeed;
		}
	}
	
	public float DistanceDampening
	{
		set
		{
			distanceDampening = value;
		}
		
		get
		{
			return distanceDampening;
		}
	}
	
	public bool RotationRoll
	{
		set
		{
			rotationRoll = value;
		}
		
		get
		{
			return rotationRoll;
		}
	}
	
	public void Update()
	{
		if (Application.isPlaying == true && GUIUtility.hotControl == 0)
		{
			if (Input.GetKey(KeyCode.Mouse0) == true)
			{
				var x = SGT_Input.DragY * -rotationSpeed;
				var y = SGT_Input.DragX *  rotationSpeed;
				
				targetRotation *= Quaternion.Euler(x, y, 0.0f);
			}
			
			if (rotationRoll == true)
			{
				if (Input.GetKey(KeyCode.Mouse1) == true)
				{
					var z = SGT_Input.DragRoll;
					
					targetRotation *= Quaternion.Euler(0.0f, 0.0f, z);
				}
			}
			
			targetDistance -= SGT_Input.Zoom * (1.0f + targetDistance - distanceMin) * distanceSpeed;
		}
		
		targetDistance = Mathf.Clamp(targetDistance, distanceMin, distanceMax);
		
		var currentDistance = transform.localPosition.magnitude;
		var currentRotation = transform.localRotation;
		
		if (Application.isPlaying == true)
		{
			var rotationDampenFactor = SGT_Helper.DampenFactor(rotationDampening, Time.deltaTime);
			var distanceDampenFactor = SGT_Helper.DampenFactor(distanceDampening, Time.deltaTime);
			
			currentRotation = Quaternion.Slerp(currentRotation, targetRotation, rotationDampenFactor);
			currentDistance = Mathf.Lerp(currentDistance, targetDistance, distanceDampenFactor);
		}
		else
		{
			currentRotation = targetRotation;
			currentDistance = targetDistance;
		}
		
		SGT_Helper.SetLocalRotation(transform, currentRotation);
		SGT_Helper.SetLocalPosition(transform, currentRotation * new Vector3(0.0f, 0.0f, -currentDistance));
	}
}