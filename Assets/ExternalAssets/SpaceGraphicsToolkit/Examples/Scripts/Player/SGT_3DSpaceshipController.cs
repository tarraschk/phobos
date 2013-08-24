using UnityEngine;

[AddComponentMenu("Space Graphics Toolkit/Example/3D Spaceship Controller")]
public class SGT_3DSpaceshipController : SGT_MonoBehaviour
{
	[SerializeField]
	private SGT_ThrusterController thrusterController;
	
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
	
	public void Update()
	{
		if (ThrusterController != null)
		{
			// You must call this before applying a new burn
			ThrusterController.ResetAllThrusters();
			
			// Move Forwards
			ThrusterController.ThrusterLinearBurn(Vector3.forward, Input.GetAxis("Jump"), Space.Self);
			
			// Yaw
			ThrusterController.ThrusterAngularBurn(Vector3.up, Input.GetAxis("Horizontal"), Space.Self);
			
			// Pitch
			ThrusterController.ThrusterAngularBurn(Vector3.right, Input.GetAxis("Vertical"), Space.Self);
			
			// Roll
			//ThrusterController.ThrusterAngularBurn(Vector3.forward, Input.GetAxis("Some Other Axis"), Space.Self);
		}
	}
}