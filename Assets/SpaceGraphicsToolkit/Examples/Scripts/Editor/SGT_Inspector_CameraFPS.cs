using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_CameraFPS))]
public class SGT_Inspector_CameraFPS : SGT_Inspector<SGT_CameraFPS>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		Target.Font = SGT_EditorGUI.ObjectField("Font", "The font used by the FPS counter.", Target.Font, false);
		
		SGT_EditorGUI.Separator();
	}
}