using UnityEngine;

[AddComponentMenu("Space Graphics Toolkit/Example/Camera Shoot")]
public class SGT_CameraShoot : SGT_MonoBehaviour
{
	public enum ShootKey
	{
		LeftMouseDown  = KeyCode.Mouse0,
		RightMouseDown = KeyCode.Mouse1
	}
	
	[SerializeField]
	private GameObject shootObject;
	
	[SerializeField]
	private float shootSpeed = 10.0f;
	
	[SerializeField]
	private ShootKey shootRequires = ShootKey.LeftMouseDown;
	
	public GameObject ShootObject
	{
		get
		{
			return shootObject;
		}
		
		set
		{
			shootObject = value;
		}
	}
	
	public float ShootSpeed
	{
		get
		{
			return shootSpeed;
		}
		
		set
		{
			shootSpeed = value;
		}
	}
	
	public ShootKey ShootRequires
	{
		set
		{
			shootRequires = value;
		}
		
		get
		{
			return shootRequires;
		}
	}
	
	public void Update()
	{
		if (shootObject != null)
		{
			if (Application.isPlaying == true && GUIUtility.hotControl == 0)
			{
				if (SGT_Input.GetKeyDown((KeyCode)shootRequires, 1) == true)
				{
					var bullet = (GameObject)Instantiate(shootObject, transform.position, transform.rotation);
					
					if (bullet.rigidbody != null)
					{
						bullet.rigidbody.velocity = transform.forward * shootSpeed;
					}
				}
			}
		}
	}
}