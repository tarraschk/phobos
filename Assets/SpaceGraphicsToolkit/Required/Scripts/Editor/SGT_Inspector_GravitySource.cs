using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(SGT_GravitySource))]
public class SGT_Inspector_GravitySource : SGT_Inspector<SGT_GravitySource>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Gravity Source");
		{
			Target.GravitySourceType   = (SGT_GravitySource.GravityType)SGT_EditorGUI.EnumField("Type", "The gravitational strength falloff model.", Target.GravitySourceType); SetAll("GravitySourceType");
			Target.GravitySourceForce  = SGT_EditorGUI.FloatField("Force", "The amount of force applied to objects caught in the gravity well.", Target.GravitySourceForce); SetAll("GravitySourceForce");
			Target.GravitySourceRadius = SGT_EditorGUI.FloatField("Radius", "The surface radius of the gravity source.", Target.GravitySourceRadius); SetAll("GravitySourceRadius");
			Target.GravitySourceHeight = SGT_EditorGUI.FloatField("Height", "The height the gravity well extends above the surface radius.", Target.GravitySourceHeight); SetAll("GravitySourceHeight");
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
	}
}