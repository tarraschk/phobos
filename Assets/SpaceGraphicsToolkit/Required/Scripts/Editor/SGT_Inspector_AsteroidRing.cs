using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_AsteroidRing))]
public class SGT_Inspector_AsteroidRing : SGT_Inspector<SGT_AsteroidRing>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Ring");
		{
			Target.RingAsteroidCount = SGT_EditorGUI.IntField("Asteroid Count", "The amount of asteroids in this asteroid ring.", Target.RingAsteroidCount);
			Target.RingSeed          = SGT_EditorGUI.SeedField("Seed", "The random seed to use when generating the asteroids.", Target.RingSeed);
			Target.RingDistribution  = (SGT_AsteroidRing.Distribution)SGT_EditorGUI.EnumField("Distribution", "The distribution model to use when plotting the asteroids.", Target.RingDistribution);
			Target.RingRadius        = SGT_EditorGUI.FloatField("Radius", "The radius at which asteroids begin to appear.", Target.RingRadius);
			Target.RingWidth         = SGT_EditorGUI.FloatField("Width", "The width of the asteroid ring. Asteroid orbit radiuses will be scattered between [Radius] and [Radius + Width]", Target.RingWidth);
			Target.RingHeight        = SGT_EditorGUI.FloatField("Height", "The the height of the asteroid ring. Asteroid vertical positions will be scattered vertically between [-Height / 2] and [Height / 2]", Target.RingHeight);
			Target.RingRenderQueue   = SGT_EditorGUI.IntField("Render Queue", "The render queue used by all asteroids in this asteroid ring.", Target.RingRenderQueue);
			Target.RingLightSource   = SGT_EditorGUI.ObjectField("Light Source", "The light source used for shading and shadows.", Target.RingLightSource, true);
			Target.RingAutoRegen     = SGT_EditorGUI.BoolField("Auto Regen", null, Target.RingAutoRegen);
			
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
		
		SGT_EditorGUI.BeginGroup("Orbit Rate");
		{
			Target.OrbitRateInner     = SGT_EditorGUI.FloatField("Inner", "The amount of orbital revolutions per second an asteroid makes at the inner edge of the ring.", Target.OrbitRateInner);
			Target.OrbitRateOuter     = SGT_EditorGUI.FloatField("Outer", "The amount of orbital revolutions per second an asteroid makes at the outer edge of the ring.", Target.OrbitRateOuter);
			Target.OrbitRateDeviation = SGT_EditorGUI.FloatField("Deviation", "This will offset the orbital revolutions per second of each asteroid between [-Deviation] and [Deviation] (note: the final orbital revolutions per second will always fall between the Inner and Outer values).", Target.OrbitRateDeviation);
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Asteroid");
		{
			Target.AsteroidRadiusMin = SGT_EditorGUI.FloatField("Radius Min", "The minimum radius of an asteroid.", Target.AsteroidRadiusMin);
			Target.AsteroidRadiusMax = SGT_EditorGUI.FloatField("Radius Max", "The maximum radius of an asteroid.", Target.AsteroidRadiusMax);
			
			SGT_EditorGUI.BeginGroup("Texture");
			{
				Target.AsteroidTextureDay    = SGT_EditorGUI.ObjectField("Day", "The asteroid texture when it's illuminated.", Target.AsteroidTextureDay, true);
				Target.AsteroidTextureNight  = SGT_EditorGUI.ObjectField("Night", "[optional] The asteroid texture when it's in shadow.", Target.AsteroidTextureNight);
				Target.AsteroidTextureHeight = SGT_EditorGUI.ObjectField("Height", "The asteroid height/displacement texture. White means the height of that pixel is equal to the asteroid's Radius.", Target.AsteroidTextureHeight, true);
				
				SGT_EditorGUI.BeginGroup("Tiles");
				{
					Target.AsteroidTextureTilesX = SGT_EditorGUI.IntField("X", "The amount of tiles stored in the asteroid textures horizontally.", Target.AsteroidTextureTilesX);
					Target.AsteroidTextureTilesY = SGT_EditorGUI.IntField("Y", "The amount of tiles stored in the asteroid textures vertically.", Target.AsteroidTextureTilesY);
				}
				SGT_EditorGUI.EndGroup();
			}
			SGT_EditorGUI.EndGroup();
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
		
		Target.Spin = SGT_EditorGUI.BeginToggleGroup("Spin", "Allow asteroids to spin as they orbit.", Target.Spin);
		{
			Target.SpinRateMax = SGT_EditorGUI.FloatField("Rate Max", "The maximum rate at which an asteroid can spin.", Target.SpinRateMax);
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