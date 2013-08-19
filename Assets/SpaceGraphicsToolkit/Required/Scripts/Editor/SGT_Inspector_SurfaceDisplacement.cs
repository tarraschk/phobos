using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_SurfaceDisplacement))]
public class SGT_Inspector_SurfaceDisplacement : SGT_Inspector<SGT_SurfaceDisplacement>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Source");
		{
			Target.SourceConfiguration = (SGT_SurfaceConfiguration)SGT_EditorGUI.EnumField("Configuration", null, Target.SourceConfiguration);
			Target.SourceSurfaceMesh   = SGT_EditorGUI.SurfaceMultiMeshField("Surface Mesh", "This should be a sphere with a radius of 1.", Target.SourceSurfaceMesh, true);
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Displacement");
		{
			Target.DisplacementConfiguration = (SGT_SurfaceConfiguration)SGT_EditorGUI.EnumField("Configuration", null, Target.DisplacementConfiguration);
			Target.DisplacementTexture       = SGT_EditorGUI.Field("Texture", "This should be a grayscale texture where black is Scale Min and white is Scale Max.", Target.DisplacementTexture, true);
			Target.DisplacementScaleMin      = SGT_EditorGUI.FloatField("Scale Min", "The final mesh scale if the displacement texture was purely black.", Target.DisplacementScaleMin);
			Target.DisplacementScaleMax      = SGT_EditorGUI.FloatField("Scale Max", "The final mesh scale if the displacement texture was purely white.", Target.DisplacementScaleMax);
			
			SGT_EditorGUI.Separator();
			
			Target.DisplacementUseUV     = SGT_EditorGUI.BoolField("Use UV", "Use the UV data when sampling the displacement map. By default, the UV data will be calculated based on polar coordinates.", Target.DisplacementUseUV);
			Target.DisplacementClamp     = SGT_EditorGUI.BoolField("Clamp", "Prevent the texture sampling from wrapping around the texture.", Target.DisplacementClamp);
			Target.DisplacementAutoRegen = SGT_EditorGUI.BoolField("Auto Regen", null, Target.DisplacementAutoRegen);
			
			if (Target.DisplacementAutoRegen == false)
			{
				SGT_EditorGUI.BeginFrozen(Target.Modified == true);
				{
					if (SGT_EditorGUI.Button("Regenerate") == true)
					{
						Target.Regenerate();
					}
				}
				SGT_EditorGUI.EndFrozen();
			}
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
	}
}