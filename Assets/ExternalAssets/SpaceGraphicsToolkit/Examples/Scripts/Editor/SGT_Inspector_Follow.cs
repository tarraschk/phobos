using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_Follow))]
public class SGT_Inspector_Follow : SGT_Inspector<SGT_Follow>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.HelpBox("If this script appears to update one frame late, make sure you put it last in: Edit -> Project Settings -> Script Execution Order.", MessageType.Info);
		
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Follow");
		{
			Target.FollowTarget = SGT_EditorGUI.ObjectField("Target", "The game object this game object will follow.", Target.FollowTarget);
			
			SGT_EditorGUI.MarkNextFieldAsBold(Target.FollowPosition == true);
			Target.FollowPosition = SGT_EditorGUI.BoolField("Position", "Match the target game object's position.", Target.FollowPosition);
			
			SGT_EditorGUI.BeginIndent(Target.FollowPosition == true);
			{
				Target.FollowPositionScale = SGT_EditorGUI.FloatField("Scale", null, Target.FollowPositionScale);
			}
			SGT_EditorGUI.EndIndent();
			
			Target.FollowRotation = SGT_EditorGUI.BoolField("Rotation", "Match the target game object's rotation.", Target.FollowRotation);
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
	}
}