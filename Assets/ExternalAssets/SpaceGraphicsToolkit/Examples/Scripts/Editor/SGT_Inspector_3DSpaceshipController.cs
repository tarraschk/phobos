using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(SGT_3DSpaceshipController))]
public class SGT_Inspector_3DSpaceshipController : SGT_Inspector<SGT_3DSpaceshipController>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		Target.ThrusterController = SGT_EditorGUI.ObjectField("ThrusterController", null, Target.ThrusterController, true); SetAll("ThrusterController");
		
		SGT_EditorGUI.Separator();
	}
}