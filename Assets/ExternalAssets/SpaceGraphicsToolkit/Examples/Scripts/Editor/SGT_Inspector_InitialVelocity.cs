using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(SGT_InitialVelocity))]
public class SGT_Inspector_InitialVelocity : SGT_Inspector<SGT_InitialVelocity>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		Target.InitialVelocity = SGT_EditorGUI.Vector3Field("Initial Velocity", "Initial velocity of rigid body.", Target.InitialVelocity); SetAll("InitialVelocity");
		
		SGT_EditorGUI.Separator();
	}
	
	public void OnSceneGUI()
	{
		Handles.color = Color.red;
		Handles.DrawLine(Target.transform.position, Target.transform.position + Target.InitialVelocity);
	}
}