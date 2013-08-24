using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Example/Follow")]
public class SGT_Follow : SGT_MonoBehaviour
{
	[SerializeField]
	private Transform followTarget;
	
	[SerializeField]
	private bool followPosition;
	
	[SerializeField]
	private float followPositionScale = 1.0f;
	
	[SerializeField]
	private bool followRotation;
	
	public Transform FollowTarget
	{
		set
		{
			followTarget = value;
		}
		
		get
		{
			return followTarget;
		}
	}
	
	public bool FollowPosition
	{
		set
		{
			followPosition = value;
		}
		
		get
		{
			return followPosition;
		}
	}
	
	public float FollowPositionScale
	{
		set
		{
			followPositionScale = value;
		}
		
		get
		{
			return followPositionScale;
		}
	}
	
	public bool FollowRotation
	{
		set
		{
			followRotation = value;
		}
		
		get
		{
			return followRotation;
		}
	}
	
	public void Awake()
	{
		UpdateFollow();
	}
	
	public void Start()
	{
		UpdateFollow();
	}
	
	public void Update()
	{
		UpdateFollow();
	}
	
	public void LateUpdate()
	{
		UpdateFollow();
	}
	
	public void UpdateFollow()
	{
		if (followTarget != null)
		{
			if (followPosition == true)
			{
				SGT_Helper.SetPosition(transform, followTarget.position * followPositionScale);
			}
			
			if (followRotation == true)
			{
				SGT_Helper.SetRotation(transform, followTarget.rotation);
			}
		}
	}
}