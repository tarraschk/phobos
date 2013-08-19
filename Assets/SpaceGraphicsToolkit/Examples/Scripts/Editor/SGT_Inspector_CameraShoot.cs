using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_CameraShoot))]
public class SGT_Inspector_CameraShoot : SGT_Inspector<SGT_CameraShoot>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Shoot");
		{
			Target.ShootObject   = SGT_EditorGUI.ObjectField("Object", "The game object the camera will shoot.", Target.ShootObject);
			Target.ShootSpeed    = SGT_EditorGUI.FloatField("Speed", "Initial speed of the shot object.", Target.ShootSpeed);
			Target.ShootRequires = (SGT_CameraShoot.ShootKey)SGT_EditorGUI.EnumField("Requires", "The mouse button required to shoot an object.", Target.ShootRequires);
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
	}
}