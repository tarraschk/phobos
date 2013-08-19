using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Example/Camera Free Look")]
public class SGT_CameraFreeLook : SGT_MonoBehaviour
{
	public enum LookKey
	{
		LeftMouseDown  = KeyCode.Mouse0,
		RightMouseDown = KeyCode.Mouse1
	}
	
	[SerializeField]
	private Quaternion targetRotation;
	
	[SerializeField]
	private float rotationSpeed = 2500.0f;
	
	[SerializeField]
	private float rotationDampening = 10.0f;
	
	[SerializeField]
	private LookKey rotationRequires = LookKey.LeftMouseDown;
	
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
	
	public LookKey RotationRequires
	{
		set
		{
			rotationRequires = value;
		}
		
		get
		{
			return rotationRequires;
		}
	}
	
	public void Update()
	{
		if (Application.isPlaying == true && GUIUtility.hotControl == 0)
		{
			if (SGT_Input.GetKey((KeyCode)rotationRequires, 1) == true)
			{
				var x = SGT_Input.DragY * -rotationSpeed;
				var y = SGT_Input.DragX *  rotationSpeed;
				
				targetRotation *= Quaternion.Euler(x, y, 0.0f);
			}
		}
		
		var currentRotation = transform.localRotation;
		
		if (Application.isPlaying == true)
		{
			var dampenFactor = SGT_Helper.DampenFactor(rotationDampening, Time.deltaTime);
			
			currentRotation = Quaternion.Slerp(currentRotation, targetRotation, dampenFactor);
		}
		else
		{
			currentRotation = targetRotation;
		}
		
		SGT_Helper.SetLocalRotation(transform, currentRotation);
	}
}