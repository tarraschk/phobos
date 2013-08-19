using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_CameraMove))]
public class SGT_Inspector_CameraMove : SGT_Inspector<SGT_CameraMove>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Move");
		{
			Target.MovePlane         = (SGT_CameraMove.MovementPlane)SGT_EditorGUI.EnumField("Plane", "The axes the camera moves on.", Target.MovePlane);
			Target.MoveSpeed         = SGT_EditorGUI.FloatField("Speed", "The maximum speed the camera can move.", Target.MoveSpeed);
			Target.MoveSmooth        = SGT_EditorGUI.FloatField("Smooth", "The amount the movement speed is smoothed.", Target.MoveSmooth);
			Target.MoveShiftSpeedMul = SGT_EditorGUI.FloatField("Shift Speed Mul", "The amount the speed is multiplied by when holding shift.", Target.MoveShiftSpeedMul);
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
		
		Target.RepelBodies = SGT_EditorGUI.BeginToggleGroup("Repel Bodies", "Makes it so the camera cannot enter planets or stars.", Target.RepelBodies);
		{
			Target.RepelDistance = SGT_EditorGUI.FloatField("Distance", "Specifies the minimum distance the camera can be from a planet or star.", Target.RepelDistance);
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
	}
}