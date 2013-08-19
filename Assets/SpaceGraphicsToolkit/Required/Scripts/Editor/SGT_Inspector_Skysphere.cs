using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_Skysphere))]
public class SGT_Inspector_Skysphere : SGT_Inspector<SGT_Skysphere>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Skysphere");
		{
			Target.SkysphereMesh        = SGT_EditorGUI.ObjectField("Mesh", "This should be an inside-out sphere with a radius of 1.", Target.SkysphereMesh, true);
			Target.SkysphereRenderQueue = SGT_EditorGUI.IntField("Render Queue", "The render queue used by the skysphere mesh.", Target.SkysphereRenderQueue);
			Target.SkysphereTexture     = SGT_EditorGUI.ObjectField("Texture", "The skysphere texture.", Target.SkysphereTexture, true);
			Target.SkysphereObserver    = SGT_EditorGUI.ObjectField("Observer", "The camera rendering this.", Target.SkysphereObserver, true);
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
	}
}