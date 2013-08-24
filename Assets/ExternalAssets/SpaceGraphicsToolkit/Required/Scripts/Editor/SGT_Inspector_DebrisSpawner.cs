using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_DebrisSpawner))]
public class SGT_Inspector_DebrisSpawner : SGT_Inspector<SGT_DebrisSpawner>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Debris");
		{
			Target.DebrisCentre             = SGT_EditorGUI.ObjectField("Centre", null, Target.DebrisCentre, true);
			Target.DebrisCountMax           = SGT_EditorGUI.IntField("Count Max", null, Target.DebrisCountMax);
			Target.DebrisContainerRadius    = SGT_EditorGUI.FloatField("Container Radius", null, Target.DebrisContainerRadius);
			Target.DebrisContainerThickness = SGT_EditorGUI.FloatField("Container Thickness", null, Target.DebrisContainerThickness);
			Target.Debris2D                 = SGT_EditorGUI.BoolField("2D", null, Target.Debris2D);
			
			SGT_EditorGUI.Separator();
			
			if (SGT_EditorGUI.Button("Regenerate") == true)
			{
				Target.Regenerate();
			}
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Variants");
		{
			for (var i = 0; i < Target.VariantCount; i++)
			{
				var variant = Target.GetDebrisVariant(i);
				
				if (variant != null)
				{
					SGT_EditorGUI.BeginIndent(true, 1);
					{
						variant.GameObject       = SGT_EditorGUI.ObjectField("GameObject", null, variant.GameObject);
						variant.SpawnProbability = SGT_EditorGUI.FloatField("Spawn Probability", null, variant.SpawnProbability, 0.0f, 1.0f);
						
						if (SGT_EditorGUI.Button("Remove") == true)
						{
							Target.RemoveDebrisVariant(i);
						}
					}
					SGT_EditorGUI.EndIndent();
					
					SGT_EditorGUI.Separator();
				}
			}
			
			SGT_EditorGUI.BeginIndent(true, 1);
			{
				var addVariant = SGT_EditorGUI.ObjectField<GameObject>("Add Variant", null, null, Target.VariantCount == 0);
				
				if (addVariant != null)
				{
					var variant = Target.AddDebrisVariant(addVariant);
					
					variant.GameObject = addVariant;
				}
			}
			SGT_EditorGUI.EndIndent();
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
	}
}