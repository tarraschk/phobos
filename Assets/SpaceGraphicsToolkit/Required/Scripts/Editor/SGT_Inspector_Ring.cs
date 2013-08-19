using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_Ring))]
public class SGT_Inspector_Ring : SGT_Inspector<SGT_Ring>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Ring");
		{
			Target.RingRenderQueue = SGT_EditorGUI.IntField("Render Queue", "The render queue used by the ring mesh.", Target.RingRenderQueue);
			Target.RingRadius      = SGT_EditorGUI.FloatField("Radius", "The radius at which the ring starts.", Target.RingRadius);
			Target.RingWidth       = SGT_EditorGUI.FloatField("Width", "The distance between the inner and outer radius.", Target.RingWidth);
			
			SGT_EditorGUI.BeginFrozen(Target.Lit == true || Target.Scattering == true || Target.Shadow == true);
			{
				Target.RingLightSource = SGT_EditorGUI.ObjectField("Light Source", "The light source used in lighting and shadow calculations.", Target.RingLightSource, true);
			}
			SGT_EditorGUI.EndFrozen();
			
			Target.RingTexture   = SGT_EditorGUI.ObjectField("Texture", "The texture the ring uses.", Target.RingTexture, true);
			Target.RingAutoRegen = SGT_EditorGUI.BoolField("Auto Regen", null, Target.RingAutoRegen);
			
			if (Target.RingAutoRegen == false)
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
		
		Target.RingSliced = SGT_EditorGUI.BeginToggleGroup("Sliced", "This will radially slice the ring mesh, allowing for improved depth sorting of the ring. Without this, the ring will be a single plane, which is incapable of properly being depth sorted with a gas giant, or a planet's atmosphere.", Target.RingSliced);
		{
			Target.RingSlicedSlices           = SGT_EditorGUI.IntField("Slices", "The amount of meshes used to represent the ring.", Target.RingSlicedSlices);
			Target.RingSlicedSegmentsPerSlice = SGT_EditorGUI.IntField("Segments Per Slice", "The amount of segments/sides each slice has.", Target.RingSlicedSegmentsPerSlice);
			
			Target.Tiled = SGT_EditorGUI.BeginToggleGroup("Tiled", "This allows you to tile a texture around the ring's surface. This is useful if your ring contains asteroids or something.", Target.Tiled);
			{
				Target.RingSlicedTextureRepeat  = SGT_EditorGUI.IntField("Texture Repeat", "The amount of times the texture is repeated on each slice.", Target.RingSlicedTextureRepeat);
			}
			SGT_EditorGUI.EndToggleGroup();
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
		
		Target.Lit = SGT_EditorGUI.BeginToggleGroup("Lit", "Allow the ring's brightness vary based on viewing angle.", Target.Lit);
		{
			Target.LitBrightnessMin = SGT_EditorGUI.FloatField("Brightness Min", "The brightness of the ring when viewed from behind.", Target.LitBrightnessMin, 0.0f, 1.0f);
			Target.LitBrightnessMax = SGT_EditorGUI.FloatField("Brightness Max", "The brightness of the ring when viewed from the front.", Target.LitBrightnessMax, 0.0f, 1.0f);
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
		
		Target.Scattering = SGT_EditorGUI.BeginToggleGroup("Scattering", "Enable light scattering through the ring. This means you will see the sun's glow from the dark side of the ring.", Target.Scattering);
		{
			Target.ScatteringMie       = SGT_EditorGUI.FloatField("Mie", "The Mie asymetry. This will brighten the ring based on the angle to the light source. A higher value means the light source will appear very small.", Target.ScatteringMie, 0.0f, 1.0f);
			Target.ScatteringOcclusion = SGT_EditorGUI.FloatField("Occlusion", "The amount the light can penetrate through the ring. Light will only be able to penetrate through transparent sections of the ring's texture.", Target.ScatteringOcclusion, 0.0f, 1.0f);
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
		
		Target.Shadow = SGT_EditorGUI.BeginToggleGroup("Shadow", "Allow the Planet or Gas Giant to cast a shadow on the ring.", Target.Shadow);
		{
			Target.ShadowAutoUpdate = SGT_EditorGUI.BoolField("Auto Update", null, Target.ShadowAutoUpdate);
			
			SGT_EditorGUI.BeginFrozen(Target.ShadowAutoUpdate == false);
			{
				Target.ShadowRadius = SGT_EditorGUI.FloatField("Radius", "The solid shadow radius.", Target.ShadowRadius);
			}
			SGT_EditorGUI.EndFrozen();
			
			Target.ShadowWidth          = SGT_EditorGUI.FloatField("Width", "The soft shadow width.", Target.ShadowWidth);
			Target.ShadowUmbraColour    = SGT_EditorGUI.ColourField("Umbra Colour", "The colour of the solid shadow.", Target.ShadowUmbraColour);
			Target.ShadowPenumbraColour = SGT_EditorGUI.ColourField("Penumbra Colour", "The colour of the soft shadow.", Target.ShadowPenumbraColour);
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
	}
}