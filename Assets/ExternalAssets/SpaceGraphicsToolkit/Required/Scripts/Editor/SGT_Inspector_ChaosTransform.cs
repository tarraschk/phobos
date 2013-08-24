using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(SGT_ChaosTransform))]
public class SGT_Inspector_ChaosTransform : SGT_Inspector<SGT_ChaosTransform>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		Target.Seed = SGT_EditorGUI.SeedField("Seed", null, Target.Seed); SetAll("Seed");
		
		SGT_EditorGUI.Separator();
		
		Target.Rotation = SGT_EditorGUI.BeginToggleGroup("Rotation", null, Target.Rotation); SetAll("Rotation");
		{
			Target.RotationPeriod      = SGT_EditorGUI.FloatField("Period", null, Target.RotationPeriod); SetAll("RotationPeriod");
			Target.RotationChangeDelay = SGT_EditorGUI.FloatField("Change Delay", null, Target.RotationChangeDelay); SetAll("RotationChangeDelay");
			Target.RotationDampening   = SGT_EditorGUI.FloatField("Dampening", null, Target.RotationDampening); SetAll("RotationDampening");
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
		
		Target.Scale = SGT_EditorGUI.BeginToggleGroup("Scale", null, Target.Scale); SetAll("Scale");
		{
			Target.ScaleMin         = SGT_EditorGUI.FloatField("Min", null, Target.ScaleMin); SetAll("ScaleMin");
			Target.ScaleMax         = SGT_EditorGUI.FloatField("Max", null, Target.ScaleMax); SetAll("ScaleMax");
			Target.ScaleChangeDelay = SGT_EditorGUI.FloatField("Change Delay", null, Target.ScaleChangeDelay); SetAll("ScaleChangeDelay");
			Target.ScaleDampening   = SGT_EditorGUI.FloatField("Dampening", null, Target.ScaleDampening); SetAll("ScaleDampening");
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
	}
}