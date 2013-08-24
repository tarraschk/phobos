using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(SGT_SimpleOrbit))]
public class SGT_Inspector_SimpleOrbit : SGT_Inspector<SGT_SimpleOrbit>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		Target.Orbit = SGT_EditorGUI.BeginToggleGroup("Orbit", null, Target.Orbit); SetAll("Orbit");
		{
			Target.OrbitPeriod     = SGT_EditorGUI.FloatField("Period", "The time in seconds it takes to complete an orbit.", Target.OrbitPeriod); SetAll("OrbitPeriod");
			Target.OrbitDistance   = SGT_EditorGUI.FloatField("Distance", "The distance this GameObject will remain from its parent.", Target.OrbitDistance); SetAll("OrbitDistance");
			Target.OrbitOblateness = SGT_EditorGUI.FloatField("Oblateness", "How elliptical the orbit is..", Target.OrbitOblateness, 0.0f, 1.0f); SetAll("OrbitOblateness");
			Target.OrbitAngle      = SGT_EditorGUI.FloatField("Angle", "The angle of this GameObject about its orbit path.", Mathf.Repeat(Target.OrbitAngle + Mathf.PI, Mathf.PI * 2.0f) - Mathf.PI, -Mathf.PI, Mathf.PI); SetAll("OrbitAngle");
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
		
		Target.Rotation = SGT_EditorGUI.BeginToggleGroup("Rotation", null, Target.Rotation); SetAll("Rotation");
		{
			Target.RotationPeriod = SGT_EditorGUI.FloatField("Period", "The time in seconds it takes to complete a rotation.", Target.RotationPeriod); SetAll("RotationPeriod");
			Target.RotationAxis   = SGT_EditorGUI.Vector3Field("Axis", "The axis around which the rotation will occur.", Target.RotationAxis); SetAll("RotationAxis");
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
	}
}