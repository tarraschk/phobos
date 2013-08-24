using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_VolumetricProbe))]
public class SGT_Inspector_VolumetricProbe : SGT_Inspector<SGT_VolumetricProbe>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Probe");
		{
			Target.ProbeRenderQueue = SGT_EditorGUI.IntField("Render Queue", "The render queue used by the probe material.", Target.ProbeRenderQueue);
			Target.ProbeRecursive   = SGT_EditorGUI.BoolField("Recursive", "Setting this to true means every renderer under this component will be affected.", Target.ProbeRecursive);
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
	}
}