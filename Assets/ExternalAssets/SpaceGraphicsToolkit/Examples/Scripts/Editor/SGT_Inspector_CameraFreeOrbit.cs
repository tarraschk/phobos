using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_CameraFreeOrbit))]
public class SGT_Inspector_CameraFreeOrbit : SGT_Inspector<SGT_CameraFreeOrbit>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.MarkNextFieldAsBold();
		Target.Rotation = SGT_EditorGUI.QuaternionField("Rotation", "The current orbit rotation.", Target.Rotation);
		
		SGT_EditorGUI.BeginIndent();
		{
			Target.RotationSpeed     = SGT_EditorGUI.FloatField("Speed", "The speed at which the camera can rotate.", Target.RotationSpeed);
			Target.RotationDampening = SGT_EditorGUI.FloatField("Dampening", "How sharp the rotation is. A higher value means the rotation will reach its destination quickly.", Target.RotationDampening);
			Target.RotationRoll      = SGT_EditorGUI.BoolField("Roll", "Allow the orbit to be rotated when right click is held and dragged?", Target.RotationRoll);
		}
		SGT_EditorGUI.EndIndent();
		
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.MarkNextFieldAsBold();
		Target.Distance = SGT_EditorGUI.FloatField("Distance", "The current orbit distance.", Target.Distance);
		
		SGT_EditorGUI.BeginIndent();
		{
			Target.DistanceMin       = SGT_EditorGUI.FloatField("Min", "The minimum orbit distance.", Target.DistanceMin);
			Target.DistanceMax       = SGT_EditorGUI.FloatField("Max", "The maximum orbit distance.", Target.DistanceMax);
			Target.DistanceSpeed     = SGT_EditorGUI.FloatField("Speed", "How fast the camera's distance can change when zooming.", Target.DistanceSpeed);
			Target.DistanceDampening = SGT_EditorGUI.FloatField("Dampening", "How sharp the distance change is is. A higher value means the rotation will reach its destination quickly.", Target.DistanceDampening);
		}
		SGT_EditorGUI.EndIndent();
		
		SGT_EditorGUI.Separator();
	}
}