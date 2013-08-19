using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(SGT_GravityReceiver))]
public class SGT_Inspector_GravityReceiver : SGT_Inspector<SGT_GravityReceiver>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Gravity Receiver");
		{
			Target.Type = (SGT_GravityReceiver.GravityType)SGT_EditorGUI.EnumField("Type", "The amount of gravity sources that can effect this.", Target.Type); SetAll("Type");
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
	}
}