using UnityEngine;

[AddComponentMenu("Space Graphics Toolkit/Example/Camera Drag")]
public class SGT_CameraDrag : SGT_MonoBehaviour
{
	public enum DragKey
	{
		LeftMouseDown  = KeyCode.Mouse0,
		RightMouseDown = KeyCode.Mouse1
	}
	
	[SerializeField]
	private GameObject dragObject;
	
	[SerializeField]
	private DragKey dragRequires = DragKey.LeftMouseDown;
	
	/*[SerializeField]*/
	private bool tracking;
	
	/*[SerializeField]*/
	private Quaternion lastRotation;
	
	public GameObject DragObject
	{
		set
		{
			dragObject = value;
		}
		
		get
		{
			return dragObject;
		}
	}
	
	public DragKey DragRequires
	{
		set
		{
			dragRequires = value;
		}
		
		get
		{
			return dragRequires;
		}
	}
	
	public void LateUpdate()
	{
		if (Input.GetKey((KeyCode)dragRequires) == true)
		{
			if (tracking == true)
			{
				var change = CameraRotation * Quaternion.Inverse(lastRotation);
				
				if (dragObject != null)
				{
					dragObject.transform.position = change * dragObject.transform.position;
				}
			}
			else
			{
				tracking = true;
			}
			
			lastRotation = CameraRotation;
		}
		else
		{
			tracking = false;
		}
	}
	
	private Quaternion CameraRotation
	{
		get
		{
			if (Camera.main != null)
			{
				return Camera.main.transform.rotation;
			}
			
			return Quaternion.identity;
		}
	}
}
