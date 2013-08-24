using ThrusterList = System.Collections.Generic.List<SGT_Thruster>;

using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Thruster Controller")]
public partial class SGT_ThrusterController : SGT_MonoBehaviourUnique<SGT_ThrusterController>
{
	[SerializeField]
	private ThrusterList thrusters;
	
	public int ThrusterCount
	{
		get
		{
			return thrusters != null ? thrusters.Count : 0;
		}
	}
	
	public void FindAllThrustersInChildren()
	{
		thrusters = new ThrusterList(GetComponentsInChildren<SGT_Thruster>());
	}
	
	public void AddThruster(SGT_Thruster thruster)
	{
		if (thrusters == null) thrusters = new ThrusterList();
		
		thrusters.Add(thruster);
	}
	
	public SGT_Thruster GetThruster(int index)
	{
		return SGT_ArrayHelper.Index(thrusters, index);
	}
	
	public void RemoveThruster(int index)
	{
		SGT_ArrayHelper.Remove(thrusters, index);
	}
	
	public void RemoveThruster(SGT_Thruster thruster)
	{
		SGT_ArrayHelper.Remove(thrusters, thruster);
	}
	
	public void ResetAllThrusters()
	{
		if (thrusters != null)
		{
			foreach (var thruster in thrusters)
			{
				thruster.ThrusterThrottle = 0.0f;
			}
		}
	}
	
	public void ThrusterBurn(float throttle)
	{
		if (thrusters != null)
		{
			foreach (var thruster in thrusters)
			{
				thruster.ThrusterThrottle += throttle;
			}
		}
	}
	
	public void ThrusterLinearBurn(Vector3 direction, float throttle, Space space)
	{
		if (thrusters != null)
		{
			if (throttle < 0)
			{
				direction = -direction;
				throttle  = -throttle;
			}
			
			if (space == Space.Self)
			{
				direction = transform.rotation * direction;
			}
			
			foreach (var thruster in thrusters)
			{
				if (Vector3.Dot(thruster.transform.forward, direction) < -0.9f)
				{
					thruster.ThrusterThrottle += throttle;
				}
			}
		}
	}
	
	public void ThrusterAngularBurn(Vector3 axis, float throttle, Space space)
	{
		if (thrusters != null)
		{
			if (throttle < 0)
			{
				axis     = -axis;
				throttle = -throttle;
			}
			
			if (space == Space.Self)
			{
				axis = transform.rotation * axis;
			}
			
			foreach (var thruster in thrusters)
			{
				var force        = thruster.transform.forward;
				var centre       = SGT_Helper.ClosestPointToLineB(rigidbody.worldCenterOfMass, axis, thruster.transform.position);
				var displacement = thruster.transform.position - centre;
				var torque       = Vector3.Cross(displacement, force);
				
				if (Mathf.Abs(Vector3.Dot(displacement.normalized, force)) < 0.9f)
				{
					if (Vector3.Dot(torque.normalized, axis) < -0.9f)
					{
						thruster.ThrusterThrottle += throttle;
					}
				}
			}
		}
	}
}