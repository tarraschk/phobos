using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_SurfaceTessellator))]
public class SGT_Inspector_SurfaceTessellator : SGT_Inspector<SGT_SurfaceTessellator>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Displacement");
		{
			Target.DisplacementConfiguration = (SGT_SurfaceConfiguration)SGT_EditorGUI.EnumField("Configuration", "Allows you to swap between using a sphere mesh with a cylindrical texture and using a cube mesh with a cube map.", Target.DisplacementConfiguration);
			Target.DisplacementTexture       = SGT_EditorGUI.Field("Texture", "This should be a grayscale texture where black is Scale Min and white is Scale Max.", Target.DisplacementTexture, true);
			Target.DisplacementScaleMin      = SGT_EditorGUI.FloatField("Scale Min", "The final mesh scale if the displacement texture was purely black.", Target.DisplacementScaleMin);
			Target.DisplacementScaleMax      = SGT_EditorGUI.FloatField("Scale Max", "The final mesh scale if the displacement texture was purely white.", Target.DisplacementScaleMax);
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Patch");
		{
			Target.PatchResolution = SGT_EditorGUI.IntField("Resolution", "The width and height of each terrain patch.", Target.PatchResolution, 2, 8);
			Target.PatchMaxLevels  = SGT_EditorGUI.IntField("Max Levels", "The amount of times a terrain patch can be subdivided.", Target.PatchMaxLevels, 1, 15);
			
			SGT_EditorGUI.Separator();
			
			SGT_EditorGUI.BeginGroup("LOD Distances");
			{
				for (var i = 0; i < Target.LevelDistanceCount; i++)
				{
					Target.SetPatchLodDistance(i, SGT_EditorGUI.FloatField("Level " + i, "If the camera's distance to a level " + i + " terrain patch is less than this value, it will be split into four smaller patches, as long as it's less than PatchMaxLevels.", Target.GetPatchLodDistance(i)));
				}
			}
			SGT_EditorGUI.EndGroup();
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Misc");
		{
			Target.VerticesPerMesh     = SGT_EditorGUI.IntField("Vertices Per Mesh", "The maximum amount of vertices contained in each group of terrain patches. A surface can be represented by any number of meshes.", Target.VerticesPerMesh);
			Target.MinUpdateInterval   = SGT_EditorGUI.FloatField("Min Update Interval", "The time in seconds between each time the tessellator's update function is run. Set this to zero to only wait for the next frame.", Target.MinUpdateInterval);
			Target.TaskBudget          = SGT_EditorGUI.FloatField("Task Budget", "The amount of seconds allocated to the tessellator every frame. Set this to zero to spread the updating across as many frames as possible.", Target.TaskBudget);
			Target.MaxSplitsPerFrame   = SGT_EditorGUI.IntField("Max Splits Per Frame", "The maximum amount of patches that can be split/subdivided per frame.", Target.MaxSplitsPerFrame);
			Target.MaxStitchesPerFrame = SGT_EditorGUI.IntField("Max Stitches Per Frame", "The maximum amount of patches that can be stitched to neighbouring patches per frame.", Target.MaxStitchesPerFrame);
			//Target.SettleOnAwake       = SGT_EditorGUI.BoolField("Settle On Awake", null, Target.SettleOnAwake);
			Target.ReportBudget        = SGT_EditorGUI.BoolField("Report Budget", "This will notify you when the tessellator's current task exceeds your specified budget by two times.", Target.ReportBudget);
		}
		SGT_EditorGUI.EndGroup();
	}
}