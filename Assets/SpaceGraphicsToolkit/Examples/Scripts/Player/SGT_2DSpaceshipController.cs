using UnityEngine;

[AddComponentMenu("Space Graphics Toolkit/Example/2D Spaceship Controller")]
public class SGT_2DSpaceshipController : SGT_MonoBehaviour
{
	public enum RotationAxis
	{
		TopDown, // Y
		Right,   // X
		Up,      // Y
		Front    // Z
	}
	
	[SerializeField]
	private SGT_ThrusterController thrusterController;
	
	[SerializeField]
	private RotationAxis axis = RotationAxis.TopDown;
	
	public SGT_ThrusterController ThrusterController
	{
		set
		{
			thrusterController = value;
		}
		
		get
		{
			return thrusterController;
		}
	}
	
	public RotationAxis Axis
	{
		set
		{
			axis = value;
		}
		
		get
		{
			return axis;
		}
	}
	
	public void Update()
	{
		if (ThrusterController != null)
		{
			var rotationAxis = Vector3.zero;
			
			switch (Axis)
			{
				case RotationAxis.TopDown: rotationAxis = Vector3.up;      break;
				case RotationAxis.Right:   rotationAxis = Vector3.right;   break;
				case RotationAxis.Up:      rotationAxis = Vector3.up;      break;
				case RotationAxis.Front:   rotationAxis = Vector3.forward; break;
			}
			
			// You must call this before applying a new burn
			ThrusterController.ResetAllThrusters();
			
			// Move Forwards/Backwards
			ThrusterController.ThrusterLinearBurn(Vector3.forward, Input.GetAxis("Vertical"), Space.Self);
			
			// Move Right/Left
			//ThrusterController.ThrusterLinearBurn(Vector3.right, Input.GetAxis("Some Other Axis"), Space.Self);
			
			// Rotate Right/Left
			ThrusterController.ThrusterAngularBurn(rotationAxis, Input.GetAxis("Horizontal"), Space.Self);
		}
	}
}