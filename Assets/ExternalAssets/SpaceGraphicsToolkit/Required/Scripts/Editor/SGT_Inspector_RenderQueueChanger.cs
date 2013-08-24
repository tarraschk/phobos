using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_RenderQueueChanger))]
public class SGT_Inspector_RenderQueueChanger : SGT_Inspector<SGT_RenderQueueChanger>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		for (var i = 0; i < Target.Count; i++)
		{
			var material    = Target.GetMaterial(i);
			var renderQueue = Target.GetRenderQueue(i);
			
			SGT_EditorGUI.BeginIndent();
			{
				bool pressed; material = SGT_EditorGUI.ObjectFieldWithButton("Material", null, material, "X", out pressed, 25.0f, true);
				
				renderQueue = SGT_EditorGUI.IntField("Render Queue", null, renderQueue);
				
				if (pressed == true)
				{
					Target.Remove(i);
				}
				else
				{
					Target.SetMaterial(material, i);
					Target.SetRenderQueue(renderQueue, i);
				}
			}
			SGT_EditorGUI.EndIndent();
			
			SGT_EditorGUI.Separator();
		}
		
		var addMaterial = SGT_EditorGUI.ObjectField<Material>("Add Material", null, null);
		
		if (addMaterial != null)
		{
			Target.Add(addMaterial, addMaterial.renderQueue);
		}
		
		SGT_EditorGUI.Separator();
	}
}