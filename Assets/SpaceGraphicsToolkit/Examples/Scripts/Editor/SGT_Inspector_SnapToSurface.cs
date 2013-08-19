using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_SnapToSurface))]
public class SGT_Inspector_SnapToSurface : SGT_Inspector<SGT_SnapToSurface>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Snap");
		{
			Target.SnapSurface = SGT_EditorGUI.ObjectField("Surface", "The surface this object will be snapped to. Note: This GameObject must contain either the Planet or Star component.", Target.SnapSurface, true);
			
			if (Target.SnapSurface != null && SGT_SurfaceHelper.ContainsSurface(Target.SnapSurface) == false)
			{
				SGT_EditorGUI.HelpBox("The GameObject you have chosen above doesn't contain a surface (Planet or Star component).", MessageType.Warning);
			}
			
			SGT_EditorGUI.Separator();
			
			Target.SnapPosition = SGT_EditorGUI.BeginToggleGroup("Position", null, Target.SnapPosition);
			{
				Target.SnapPositionHeight = SGT_EditorGUI.FloatField("Height", "The height about the surface the object will be snapped to.", Target.SnapPositionHeight);
			}
			SGT_EditorGUI.EndToggleGroup();
			
			if (Target.SnapRotation == SGT_SnapToSurface.RotationSnap.AlignToNormal)
			{
				SGT_EditorGUI.MarkNextFieldAsBold();
				SGT_EditorGUI.Separator();
			}
			
			Target.SnapRotation = (SGT_SnapToSurface.RotationSnap)SGT_EditorGUI.EnumField("Rotation", "Should the rotation be snapped to the surface?", Target.SnapRotation);
			
			SGT_EditorGUI.BeginIndent(Target.SnapRotation == SGT_SnapToSurface.RotationSnap.AlignToNormal);
			{
				Target.SnapRotationScanDistance = SGT_EditorGUI.FloatField("Scan Distance", "The amount of units ahead the samples used to find the surface normal will extend.", Target.SnapRotationScanDistance);
			}
			SGT_EditorGUI.EndIndent();
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
	}
}