using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_Nebula))]
public class SGT_Inspector_Nebula : SGT_Inspector<SGT_Nebula>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Nebula");
		{
			Target.NebulaTexture     = SGT_EditorGUI.ObjectField("Texture", "A picture of your nebula/galaxy.", Target.NebulaTexture, true);
			Target.NebulaTechnique   = (SGT_Nebula.Technique)SGT_EditorGUI.EnumField("Technique", "The blending mode used by the nebula particle shader.", Target.NebulaTechnique, true);
			Target.NebulaRenderQueue = SGT_EditorGUI.IntField("Render Queue", "The render queue used by the nebula particle material.", Target.NebulaRenderQueue);
			Target.NebulaSeed        = SGT_EditorGUI.SeedField("Seed", "The random seed used when generating the nebula particles.", Target.NebulaSeed);
			Target.NebulaSize        = SGT_EditorGUI.FloatField("Size", "The size of the nebula.", Target.NebulaSize);
			Target.NebulaResolution  = SGT_EditorGUI.IntField("Resolution", "Sets how detailed the final nebula will be, relative to the nebula texture.", Target.NebulaResolution);
			Target.NebulaMirror      = SGT_EditorGUI.BoolField("Mirror", "Allows you to mirror the nebula particles, so its symmetrical.", Target.NebulaMirror);
			Target.NebulaAutoRegen   = SGT_EditorGUI.BoolField("Auto Regen", "Automatically regenerate the nebula particle mesh when making changes?", Target.NebulaAutoRegen);
			
			if (Target.NebulaAutoRegen == false)
			{
				SGT_EditorGUI.BeginFrozen(Target.NebulaModified == true);
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
		
		SGT_EditorGUI.BeginGroup("Height");
		{
			Target.HeightSource = (SGT_Nebula.TextureHeightSource)SGT_EditorGUI.EnumField("Source", "Allows you to choose which channel of the nebula texture will be used as the heightmap source.", Target.HeightSource);
			Target.HeightScale  = SGT_EditorGUI.FloatField("Scale", "Allows you to choose how steep/bumpy the final nebula will be.", Target.HeightScale);
			Target.HeightOffset = SGT_EditorGUI.FloatField("Offset", "Allows you to shift the nebula vertically, this is usually used in combination with the Nebula -> Mirror option.", Target.HeightOffset, -1.0f, 1.0f);
			Target.HeightInvert = SGT_EditorGUI.BoolField("Invert", "Invert the height values?", Target.HeightInvert);
			Target.HeightNoise  = SGT_EditorGUI.FloatField("Noise", "Allows you to add noise to the hightmap sample points.", Target.HeightNoise, 0.0f, 0.1f);
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Particle");
		{
			Target.ParticleTexture        = SGT_EditorGUI.ObjectField("Texture", "The texture applied to the nebula particles.", Target.ParticleTexture, true);
			Target.ParticleScale          = SGT_EditorGUI.FloatField("Size", "The size of the nebula particles relative to the size of the nebula.", Target.ParticleScale);
			Target.ParticleColour         = SGT_EditorGUI.ColourField("Colour", "The colour of the dust particles.", Target.ParticleColour);
			Target.ParticleJitter         = SGT_EditorGUI.FloatField("Jitter", "Allows you to add noise to the nebula particle positions.", Target.ParticleJitter, 0.0f, 0.1f);
			Target.ParticleFadeInDistance = SGT_EditorGUI.FloatField("Fade In Distance", "Sets how near the nebula particles can get before they fade away.", Target.ParticleFadeInDistance);
			
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
	}
}