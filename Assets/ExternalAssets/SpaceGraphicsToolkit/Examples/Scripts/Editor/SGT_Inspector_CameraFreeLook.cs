using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_CameraFreeLook))]
public class SGT_Inspector_CameraFreeLook : SGT_Inspector<SGT_CameraFreeLook>
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
			Target.RotationRequires  = (SGT_CameraFreeLook.LookKey)SGT_EditorGUI.EnumField("Requires", "Which mouse button must be held down for looking?", Target.RotationRequires);
		}
		SGT_EditorGUI.EndIndent();
		
		SGT_EditorGUI.Separator();
	}
}