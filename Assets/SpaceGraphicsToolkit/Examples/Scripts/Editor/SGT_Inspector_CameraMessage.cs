using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_CameraMessage))]
public class SGT_Inspector_CameraMessage : SGT_Inspector<SGT_CameraMessage>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		Target.Font    = SGT_EditorGUI.ObjectField("Font", "The font used when displaying camera messages.", Target.Font, false);
		Target.Message = SGT_EditorGUI.StringField("Message", "The text that will be displayed as the camera message.", Target.Message);
		
		SGT_EditorGUI.Separator();
	}
}