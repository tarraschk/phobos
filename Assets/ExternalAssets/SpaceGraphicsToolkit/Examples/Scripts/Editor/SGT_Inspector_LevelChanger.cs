using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_LevelChanger))]
public class SGT_Inspector_LevelChanger : SGT_Inspector<SGT_LevelChanger>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		Target.IgnoreSceneZero = SGT_EditorGUI.BoolField("Ignore Scene Zero", "Skip past scene 0?", Target.IgnoreSceneZero);
		Target.AutoSwitch      = SGT_EditorGUI.BoolField("Auto Switch", "Automatically go to scene 1?", Target.AutoSwitch);
		
		SGT_EditorGUI.Separator();
	}
}