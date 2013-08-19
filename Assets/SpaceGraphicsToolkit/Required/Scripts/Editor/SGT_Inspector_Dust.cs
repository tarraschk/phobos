using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_Dust))]
public class SGT_Inspector_Dust : SGT_Inspector<SGT_Dust>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Dust");
		{
			Target.DustTechnique   = (SGT_Dust.Technique)SGT_EditorGUI.EnumField("Technique", "The blending mode used by the dust particle shader.", Target.DustTechnique, true);
			Target.DustRenderQueue = SGT_EditorGUI.IntField("Render Queue", "The render queue used by the dust particle material.", Target.DustRenderQueue);
			Target.DustSeed        = SGT_EditorGUI.SeedField("Seed", "The random seed used when generating the dust particles.", Target.DustSeed);
			Target.DustCount       = SGT_EditorGUI.IntField("Count", "The amount of dust particles in the dust field.", Target.DustCount);
			Target.DustRadius      = SGT_EditorGUI.FloatField("Radius", "The maximum distance between the camera and a dust particle.", Target.DustRadius);
			Target.DustCamera      = SGT_EditorGUI.ObjectField("Camera", "The camera that's rendering this dust particles.", Target.DustCamera, true);
			Target.DustAutoRegen   = SGT_EditorGUI.BoolField("Auto Regen", "Automatically regenerate the dust particle mesh when making changes?", Target.DustAutoRegen);
			
			if (Target.DustAutoRegen == false)
			{
				SGT_EditorGUI.BeginFrozen(Target.DustModified == true);
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
		
		SGT_EditorGUI.BeginGroup("Particle");
		{
			Target.ParticleTexture         = SGT_EditorGUI.ObjectField("Texture", "The texture applied to the dust particles.", Target.ParticleTexture, true);
			Target.ParticleColour          = SGT_EditorGUI.ColourField("Colour", "The colour of the dust particles.", Target.ParticleColour);
			Target.ParticleScale           = SGT_EditorGUI.FloatField("Scale", "The size of the dust particles relative to the size of the dust field.", Target.ParticleScale, 0.0f, 1.0f);
			Target.ParticleFadeInDistance  = SGT_EditorGUI.FloatField("Fade In Distance", "Sets how near the dust particles can get before they fade away.", Target.ParticleFadeInDistance, 0.0f, Target.DustRadius);
			Target.ParticleFadeOutDistance = SGT_EditorGUI.FloatField("Fade Out Distance", "Sets how far the dust particles can get before they fade away.", Target.ParticleFadeOutDistance, 0.0f, Target.DustRadius);
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
	}
}