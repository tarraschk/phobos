using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_CameraDrag))]
public class SGT_Inspector_CameraDrag : SGT_Inspector<SGT_CameraDrag>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Drag");
		{
			Target.DragObject   = SGT_EditorGUI.ObjectField("Object", "The object that will be dragged based on the camera's change in angle.", Target.DragObject, true);
			Target.DragRequires = (SGT_CameraDrag.DragKey)SGT_EditorGUI.EnumField("Requires", "Which mouse button must be held down for dragging?", Target.DragRequires);
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
	}
}