using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(SGT_2DSpaceshipController))]
public class SGT_Inspector_2DSpaceshipController : SGT_Inspector<SGT_2DSpaceshipController>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		Target.ThrusterController = SGT_EditorGUI.ObjectField("ThrusterController", null, Target.ThrusterController, true); SetAll("ThrusterController");
		Target.Axis               = (SGT_2DSpaceshipController.RotationAxis)SGT_EditorGUI.EnumField("Axis", null, Target.Axis); SetAll("Axis");
		
		SGT_EditorGUI.Separator();
	}
}