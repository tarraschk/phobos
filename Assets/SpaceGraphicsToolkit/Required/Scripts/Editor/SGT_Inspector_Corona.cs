using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_Corona))]
public class SGT_Inspector_Corona : SGT_Inspector<SGT_Corona>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Corona");
		{
			Target.CoronaOffset    = SGT_EditorGUI.FloatField("Offset", "This specifies how much the corona object should be moved towards the camera. You can use this to change the depth sorting order when rendered with a star's atmosphere.", Target.CoronaOffset);
			Target.CoronaTexture   = SGT_EditorGUI.ObjectField("Texture", "Each corona plane is textured with this. The texture should be a radial corona texture, where the centre of the image represents the centre of the corona.", Target.CoronaTexture, true);
			Target.CoronaColour    = SGT_EditorGUI.ColourField("Colour", "This colour is multiplied by the corona texture. Set this to white to use the corona texture only.", Target.CoronaColour);
			Target.CoronaFalloff   = SGT_EditorGUI.FloatField("Falloff", "This specifies the relationship between the corona plane viewing angle and the corona plane's opacity. A lower value means the corona plane will be visible from most angles. A higher value means the corona plane will only be visible when viewed directly.", Target.CoronaFalloff, 1.0f, 5.0f);
			Target.CoronaPerPixel  = SGT_EditorGUI.BoolField("Per Pixel", "Calculate the falloff in the pixel shader. This will produce more accurate results, at the cost of pixel shader speed. This is ideal if your camera approches the corona.", Target.CoronaPerPixel);
			Target.CoronaObserver  = SGT_EditorGUI.ObjectField("Observer", "The camera that will render this.", Target.CoronaObserver, true);
			Target.CoronaAutoRegen = SGT_EditorGUI.BoolField("Auto Regen", null, Target.CoronaAutoRegen);
			
			if (Target.CoronaAutoRegen == false)
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
		
		SGT_EditorGUI.BeginGroup("Mesh");
		{
			Target.MeshRenderQueue = SGT_EditorGUI.IntField("Render Queue", "The render queue used by the corona mesh.", Target.MeshRenderQueue);
			
			SGT_EditorGUI.MarkNextFieldAsBold();
			Target.MeshType = (SGT_Corona.Type)SGT_EditorGUI.EnumField("Type", "This specifies the corona mesh type.", Target.MeshType);
			
			SGT_EditorGUI.BeginIndent();
			{
				Target.MeshRadius = SGT_EditorGUI.FloatField("Radius", "This specifies the size of each corona plane.", Target.MeshRadius);
				
				if (Target.MeshType == SGT_Corona.Type.Ring)
				{
					Target.MeshHeight   = SGT_EditorGUI.FloatField("Height", "The height/thickness of the coronal ring.", Target.MeshHeight);
					Target.MeshSegments = SGT_EditorGUI.IntField("Segments", "The amount of sides each coronal ring has.", Target.MeshSegments);
				}
			}
			SGT_EditorGUI.EndIndent();
			
			SGT_EditorGUI.Separator();
			
			Target.MeshAlignment = (SGT_Corona.Alignment)SGT_EditorGUI.EnumField("Alignment", "This allows you to change the alignment of all the corona parts.", Target.MeshAlignment);
			
			if (Target.MeshAlignment == SGT_Corona.Alignment.Random)
			{
				Target.MeshPlaneCount = SGT_EditorGUI.IntField("Plane Count", "The amount of planes used to represent the corona.", Target.MeshPlaneCount);
				Target.MeshSeed       = SGT_EditorGUI.SeedField("Seed", "The random seed used when generating the plane angles.", Target.MeshSeed);
			}
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
		
		Target.CullNear = SGT_EditorGUI.BeginToggleGroup("Cull Near", "This allows you to fade away pixels based on their distance to the camera. This is useful if you don't want your corona visible on the near-side of your star.", Target.CullNear);
		{
			Target.CullNearOffset = SGT_EditorGUI.FloatField("Offset", "This specifies the distance at which the fading begins. A value of 0 means any pixels closer to the camera than the corona's centre will be faded.", Target.CullNearOffset);
			Target.CullNearLength = SGT_EditorGUI.FloatField("Length", "This specifies the length of the faded region. A value of 0 will make the transition very abrupt. This value should probably be half the star's radius.", Target.CullNearLength);
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
	}
}