using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Example/Snap To Surface")]
public class SGT_SnapToSurface : SGT_MonoBehaviour
{
	public enum RotationSnap
	{
		None,
		AlignToCentre,
		AlignToNormal
	}
	
	[SerializeField]
	private GameObject snapSurface;
	
	[SerializeField]
	private bool snapPosition = true;
	
	[SerializeField]
	private float snapPositionHeight = 1.0f;
	
	[SerializeField]
	private RotationSnap snapRotation = RotationSnap.None;
	
	[SerializeField]
	private float snapRotationScanDistance = 1.0f;
	
	public GameObject SnapSurface
	{
		set
		{
			snapSurface = value;
		}
		
		get
		{
			return snapSurface;
		}
	}
	
	public bool SnapPosition
	{
		set
		{
			snapPosition = value;
		}
		
		get
		{
			return snapPosition;
		}
	}
	
	public float SnapPositionHeight
	{
		set
		{
			snapPositionHeight = value;
		}
		
		get
		{
			return snapPositionHeight;
		}
	}
	
	public RotationSnap SnapRotation
	{
		set
		{
			snapRotation = value;
		}
		
		get
		{
			return snapRotation;
		}
	}
	
	public float SnapRotationScanDistance
	{
		set
		{
			snapRotationScanDistance = value;
		}
		
		get
		{
			return snapRotationScanDistance;
		}
	}
	
	public void FixedUpdate()
	{
		UpdateSnap();
	}
	
	public void Update()
	{
		UpdateSnap();
	}
	
	public void LateUpdate()
	{
		UpdateSnap();
	}
	
	private void UpdateSnap()
	{
		if (snapSurface != null && (snapPosition == true || snapRotation != RotationSnap.None))
		{
			var point    = transform.position;
			var centre   = snapSurface.transform.position;
			var position = SurfacePositionAtPoint(centre, point);
			
			if (snapPosition == true)
			{
				SGT_Helper.SetPosition(transform, position);
			}
			
			if (snapRotation == RotationSnap.AlignToCentre)
			{
				var finalRot = Quaternion.FromToRotation(transform.up, (position - centre).normalized) * transform.rotation;
				
				SGT_Helper.SetRotation(transform, finalRot);
			}
			
			if (snapRotation == RotationSnap.AlignToNormal)
			{
				var position2 = SurfacePositionAtPoint(centre, point + transform.forward * snapRotationScanDistance);
				var position3 = SurfacePositionAtPoint(centre, point + transform.right   * snapRotationScanDistance);
				var finalRot  = Quaternion.FromToRotation(transform.up, Vector3.Cross(position2 - position, position3 - position)) * transform.rotation;
				
				SGT_Helper.SetRotation(transform, finalRot);
			}
		}
	}
	
	private Vector3 SurfacePositionAtPoint(Vector3 centre, Vector3 point)
	{
		var vector    = point - centre;
		var vectorN   = vector.normalized;
		var tgtRadius = SGT_SurfaceHelper.FindRadius(snapSurface, point) + snapPositionHeight;
		
		return centre + vectorN * tgtRadius;
	}
}