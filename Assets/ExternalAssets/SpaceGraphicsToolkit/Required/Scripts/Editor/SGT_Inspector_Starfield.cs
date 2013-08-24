using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_Starfield))]
public class SGT_Inspector_Starfield : SGT_Inspector<SGT_Starfield>
{
	private bool editStar;
	private bool editVariant;
	private int  editStarIndex;
	private int  editVariantIndex;
	
	private SGT_StarfieldStarData ssd;
	
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Starfield");
		{
			Target.StarfieldStarCount    = SGT_EditorGUI.IntField("Star Count", "Amount of stars to generate in the starfield.", Target.StarfieldStarCount);
			Target.StarfieldSeed         = SGT_EditorGUI.SeedField("Seed", "The random seed to use when generating the starfield.", Target.StarfieldSeed);
			Target.StarfieldRenderQueue  = SGT_EditorGUI.IntField("Render Queue", "The render queue used by the starfield mesh.", Target.StarfieldRenderQueue);
			Target.StarfieldObserver     = SGT_EditorGUI.ObjectField("Observer", "The camera rendering this.", Target.StarfieldObserver, true);
			Target.StarfieldInBackground = SGT_EditorGUI.BoolField("In Background", "Push stars into background?", Target.StarfieldInBackground);
			Target.StarfieldAutoRegen    = SGT_EditorGUI.BoolField("Auto Regen", null, Target.StarfieldAutoRegen);
			
			if (Target.StarfieldAutoRegen == false)
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
		
		SGT_EditorGUI.MarkNextFieldAsBold();
		Target.Distribution = (SGT_StarfieldDistribution)SGT_EditorGUI.EnumField("Distribution", "The star placement distribution.", Target.Distribution);
		SGT_EditorGUI.BeginIndent();
		{
			Target.DistributionRadius = SGT_EditorGUI.FloatField("Radius", "The size of the starfield across each axis.", Target.DistributionRadius);
			
			switch (Target.Distribution)
			{
				case SGT_StarfieldDistribution.InSphere:
				{
					Target.DistributionOuter = SGT_EditorGUI.FloatField("Outer", "The minimum radius for stars in the starfield.", Target.DistributionOuter, 0.0f, 1.0f);
				}
				goto case SGT_StarfieldDistribution.OnSphere;
				case SGT_StarfieldDistribution.EllipticalGalaxy:
				{
				}
				goto case SGT_StarfieldDistribution.OnSphere;
				case SGT_StarfieldDistribution.OnSphere:
				{
					Target.DistributionSymmetry = SGT_EditorGUI.FloatField("Symmetry", "A lower symmetry value will place more stars near the horizon/equator of the skysphere than at the poles.", Target.DistributionSymmetry, 0.0f, 1.0f);
				}
				break;
			}
		}
		SGT_EditorGUI.EndIndent();
		
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Packer");
		{
			for (var i = 0; i < Target.Packer.InputCount; i++)
			{
				var input = Target.Packer.GetInput(i);
				
				SGT_EditorGUI.BeginIndent(true, 1);
				{
					input.Texture = SGT_EditorGUI.ObjectField("Texture", null, input.Texture);
					
					SGT_EditorGUI.MarkNextFieldAsBold(input.Tilesheet == true);
					input.Tilesheet = SGT_EditorGUI.BoolField("Tilesheet", "Is this texture a tilesheet?", input.Tilesheet);
					
					if (input.Tilesheet == true)
					{
						SGT_EditorGUI.BeginIndent();
						{
							input.TilesheetTilesX = SGT_EditorGUI.IntField("Tiles X", "The amount of tiles in this texture on the x axis.", input.TilesheetTilesX);
							input.TilesheetTilesY = SGT_EditorGUI.IntField("Tiles Y", "The amount of tiles in this texture on the y axis.", input.TilesheetTilesY);
						}
						SGT_EditorGUI.EndIndent();
					}
					
					if (SGT_EditorGUI.Button("Remove") == true)
					{
						Target.Packer.RemoveInput(i);
					}
				}
				SGT_EditorGUI.EndIndent();
				
				SGT_EditorGUI.Separator();
			}
			
			SGT_EditorGUI.BeginIndent(true, 1);
			{
				SGT_EditorGUI.MarkNextFieldAsError(Target.Packer.InputCount == 0);
				var newTexture = SGT_EditorGUI.ObjectField<Texture2D>("Add Texture", null, null);
				
				if (newTexture != null)
				{
					var pi = new SGT_PackerInput();
					
					pi.Texture = newTexture;
					
					Target.Packer.AddInput(pi);
				}
			}
			SGT_EditorGUI.EndIndent();
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Star");
		{
			Target.StarRadiusMin      = SGT_EditorGUI.FloatField("Radius Min", "The minimum radius of a star.", Target.StarRadiusMin);
			Target.StarRadiusMax      = SGT_EditorGUI.FloatField("Radius Max", "The maximum radius of a star.", Target.StarRadiusMax);
			Target.StarPulseRadiusMax = SGT_EditorGUI.FloatField("Pulse Radius Max", "The maximum amount a star's radius can change while pulsing (note: the final radius will always fall between the Star Radius Min/Max).", Target.StarPulseRadiusMax);
			Target.StarPulseRateMax   = SGT_EditorGUI.FloatField("Pulse Rate Max", "The maximum rate (speed) at which the stars can pulse.", Target.StarPulseRateMax);
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
		
		editVariant = SGT_EditorGUI.BeginToggleGroup("Edit Star Variant", null, editVariant);
		{
			SGT_EditorGUI.MarkNextFieldAsBold();
			editVariantIndex = SGT_EditorGUI.IntField("Index", "The star variant currently being edited.", editVariantIndex, 0, Target.StarVariantCount - 1);
			
			var sv = Target.GetStarVariant(editVariantIndex);
			var po = Target.Packer.GetOutput(editVariantIndex);
			
			if (sv != null && po != null)
			{
				SGT_EditorGUI.BeginIndent();
				{
					SGT_EditorGUI.DrawFieldTexture("Preview", null, po.OutputTexture, po.Uv);
					
					sv.SpawnProbability = SGT_EditorGUI.FloatField("Spawn Probability", null, sv.SpawnProbability, 0.0f, 1.0f);
					
					if (sv.Custom == true)
					{
						SGT_EditorGUI.MarkNextFieldAsBold();
					}
					
					var oldCustom = sv.Custom;
					
					sv.Custom = SGT_EditorGUI.BoolField("Custom", null, sv.Custom);
					
					if (sv.Custom == true)
					{
						if (oldCustom == false)
						{
							sv.CustomRadiusMin      = Target.StarRadiusMin;
							sv.CustomRadiusMax      = Target.StarRadiusMax;
							sv.CustomPulseRadiusMax = Target.StarPulseRadiusMax;
							sv.CustomPulseRateMax   = Target.StarPulseRateMax;
						}
						
						SGT_EditorGUI.BeginIndent();
						{
							sv.CustomRadiusMin      = SGT_EditorGUI.FloatField("Radius Min", null, sv.CustomRadiusMin);
							sv.CustomRadiusMax      = SGT_EditorGUI.FloatField("Radius Max", null, sv.CustomRadiusMax);
							sv.CustomPulseRadiusMax = SGT_EditorGUI.FloatField("Pulse Radius Max", null, sv.CustomPulseRadiusMax);
							sv.CustomPulseRateMax   = SGT_EditorGUI.FloatField("Pulse Rate Max", null, sv.CustomPulseRateMax);
						}
						SGT_EditorGUI.EndIndent();
					}
				}
				SGT_EditorGUI.EndIndent();
			}
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
		
		editStar = SGT_EditorGUI.BeginToggleGroup("Edit Star", null, editStar);
		{
			SGT_EditorGUI.MarkNextFieldAsBold();
			editStarIndex = SGT_EditorGUI.IntField("Index", "The star currently being edited.", editStarIndex, 0, Target.StarfieldStarCount - 1);
			ssd           = Target.ReadStar(editStarIndex);
			
			if (ssd != null)
			{
				SGT_EditorGUI.BeginIndent();
				{
					ssd.Position     = SGT_EditorGUI.Vector3Field("Position", null, ssd.Position);
					ssd.TextureIndex = SGT_EditorGUI.IntField("Texture Index", null, ssd.TextureIndex, 0, Target.Packer.OutputCount - 1);
					ssd.Angle        = SGT_EditorGUI.FloatField("Angle", null, ssd.Angle, -Mathf.PI, Mathf.PI);
					
					SGT_EditorGUI.BeginGroup("Radius");
					{
						ssd.RadiusMin       = SGT_EditorGUI.FloatField("Min", null, ssd.RadiusMin);
						ssd.RadiusMax       = SGT_EditorGUI.FloatField("Max", null, ssd.RadiusMax);
						ssd.RadiusPulseRate = SGT_EditorGUI.FloatField("Pulse Rate", null, ssd.RadiusPulseRate, 0.0f, 1.0f);
					}
					SGT_EditorGUI.EndGroup();
					
					SGT_EditorGUI.Separator();
					
					if (SGT_EditorGUI.Button("Normalize Position") == true)
					{
						ssd.Position = ssd.Position.normalized * Target.DistributionRadius;
					}
					
					if (SGT_EditorGUI.Button("Clamp Radius") == true)
					{
						ssd.RadiusMin = Mathf.Clamp(ssd.RadiusMin, Target.StarRadiusMin, Target.StarRadiusMax);
						ssd.RadiusMax = Mathf.Clamp(ssd.RadiusMax, Target.StarRadiusMin, Target.StarRadiusMax);
					}
					
					if (SGT_EditorGUI.Button("Duplicate") == true)
					{
						editStarIndex = Target.AddStar(ssd);
					}
					
					Target.WriteStar(ssd, editStarIndex);
					Target.ApplyStarChanges();
				}
				SGT_EditorGUI.EndIndent();
			}
			else
			{
				SGT_EditorGUI.HelpBox("Failed to read star data.", MessageType.Error);
			}
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
	}
	
	public void OnSceneGUI()
	{
		if (editStar == true && ssd != null && ssd.Transform != null)
		{
			var t = ssd.Transform;
			var p = t.TransformPoint(ssd.Position);
			var n = Handles.DoPositionHandle(p, Target.transform.rotation);
			
			if (n != p)
			{
				ssd.Position = t.InverseTransformPoint(n);
				
				Target.WriteStar(ssd, editStarIndex);
				Target.ApplyStarChanges();
			}
		}
	}
}