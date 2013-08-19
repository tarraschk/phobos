using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_ThrusterController))]
[CanEditMultipleObjects]
public class SGT_Inspector_ThrusterController : SGT_Inspector<SGT_ThrusterController>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Thrusters");
		{
			SGT_EditorGUI.BeginIndent(Target.ThrusterCount > 0, 1);
			{
				for (var i = 0; i < Target.ThrusterCount; i++)
				{
					var rect = SGT_EditorGUI.Reserve();
					bool pressed; SGT_EditorGUI.DrawEditableObjectWithButton(rect, Target.GetThruster(i), "X", out pressed, 25.0f, true);
					
					if (pressed == true)
					{
						Target.RemoveThruster(i);
					}
				}
			}
			SGT_EditorGUI.EndIndent();
			
			SGT_EditorGUI.Separator(Target.ThrusterCount > 0);
			
			var newThruster = SGT_EditorGUI.ObjectField<SGT_Thruster>("Add Thruster", null, null, false);
			
			if (newThruster != null)
			{
				Target.AddThruster(newThruster);
			}
			
			SGT_EditorGUI.Separator();
			
			if (SGT_EditorGUI.Button("Find In Children") == true)
			{
				SGT_EditorGUI.MarkModified(true, true);
				
				Target.FindAllThrustersInChildren();
			}
			
			if (SGT_EditorGUI.Button("Min Throttle") == true)
			{
				SGT_EditorGUI.MarkModified(true, true);
				
				Target.ResetAllThrusters();
			}
			
			if (SGT_EditorGUI.Button("Max Throttle") == true)
			{
				SGT_EditorGUI.MarkModified(true, true);
				
				Target.ThrusterBurn(1.0f);
			}
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
	}
}