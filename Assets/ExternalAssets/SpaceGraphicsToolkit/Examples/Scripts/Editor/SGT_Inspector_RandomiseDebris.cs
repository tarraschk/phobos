using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(SGT_RandomiseDebris))]
public class SGT_Inspector_RandomiseDebris : SGT_Inspector<SGT_RandomiseDebris>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		Target.MinScale  = SGT_EditorGUI.FloatField("Min Scale", "The minimum X/Y/Z scale of this debris.", Target.MinScale); SetAll("MinScale");
		Target.MaxScale  = SGT_EditorGUI.FloatField("Max Scale", "The maximum X/Y/Z scale of this debris.", Target.MaxScale); SetAll("MaxScale");
		Target.MaxSpeed  = SGT_EditorGUI.FloatField("Max Speed", "The maximum X/Y/Z velocity this debris has at spawn.", Target.MaxSpeed); SetAll("MaxSpeed");
		Target.MaxSpin   = SGT_EditorGUI.FloatField("Max Spin", "The maximum angular velocity this debris has at spawn.", Target.MaxSpin); SetAll("MaxSpin");
		Target.MassScale = SGT_EditorGUI.FloatField("Mass Scale", "The mass of this debris, relative to its volume.", Target.MassScale); SetAll("MassScale");
		
		SGT_EditorGUI.Separator();
	}
}