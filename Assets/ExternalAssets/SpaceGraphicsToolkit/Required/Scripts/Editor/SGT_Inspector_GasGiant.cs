using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_GasGiant))]
public class SGT_Inspector_GasGiant : SGT_Inspector<SGT_GasGiant>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Gas Giant");
		{
			Target.GasGiantLightSource = SGT_EditorGUI.ObjectField("Light Source", "The GameObject that represents the light source for this gas giant.", Target.GasGiantLightSource, true);
			Target.GasGiantObserver    = SGT_EditorGUI.ObjectField("Observer", "The camera that is rendering this gas giant.", Target.GasGiantObserver, true);
			Target.GasGiantLutSize     = (SGT_SquareSize)SGT_EditorGUI.EnumField("LUT Size", "The texture look up table resolution. Use a higher value for smoother results, at the cost of GPU memory and texture sampling speed.", Target.GasGiantLutSize);
			Target.GasGiantMesh        = SGT_EditorGUI.ObjectField("Mesh", "This should be an inside-out sphere with a radius of 1.", Target.GasGiantMesh, true);
			Target.GasGiantRenderQueue = SGT_EditorGUI.IntField("Render Queue", "The render queue used by the gas giant mesh.", Target.GasGiantRenderQueue);
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Atmosphere");
		{
			Target.AtmosphereRadius         = SGT_EditorGUI.FloatField("Equatorial Radius", "This is the gas giant's radius around the equator.", Target.AtmosphereRadius);
			Target.AtmosphereOblateness     = SGT_EditorGUI.FloatField("Oblateness", "This specifies how oblate/flat/round the gas giant is. A higher value means the polar radius will be lower than the equatorial radius. Large gas giants that spin are often quite oblate.", Target.AtmosphereOblateness, 0.0f, 1.0f);
			Target.AtmosphereDensity        = SGT_EditorGUI.FloatField("Density", "This specifies how dense the gas giant's atmosphere is. A higher value means the atmosphere will be very hard to see though.", Target.AtmosphereDensity, 1.0f, 10.0f);
			Target.AtmosphereDensityFalloff = SGT_EditorGUI.FloatField("Density Falloff", "This specifies how much the gas giant's atmospheric density varies with altitude. A higher value means the upper atmosphere will be very thin compared to the centre, meaning the edges will be more transaprent.", Target.AtmosphereDensityFalloff, 1.0f, 10.0f);
			Target.AtmosphereLighting       = SGT_EditorGUI.Field("Lighting", "This is the night-day brightness gradient. You can also give the day side a colour, to simulate starlght.", Target.AtmosphereLighting);
			Target.AtmosphereTwilightColour = SGT_EditorGUI.Field("Twilight Colour", "This is multiplied with the Lighting gradient. It allows you to easily set twilight/sunset colours without modifiying the lighting.", Target.AtmosphereTwilightColour);
			Target.AtmosphereLimbColour     = SGT_EditorGUI.Field("Limb Colour", "This allows you to colour the edges of the gas giant, useful for simulating limb darkening.", Target.AtmosphereLimbColour);
			
			SGT_EditorGUI.Separator();
			
			SGT_EditorGUI.BeginGroup("Texture");
			{
				Target.AtmosphereTextureDay   = SGT_EditorGUI.ObjectField("Day", "This is the gas giant's daytime surface texture.", Target.AtmosphereTextureDay, true);
				Target.AtmosphereTextureNight = SGT_EditorGUI.ObjectField("Night", "[Optional] This is the gas giant's nighttime surface texture.", Target.AtmosphereTextureNight);
			}
			SGT_EditorGUI.EndGroup();
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
		
		Target.Shadow = SGT_EditorGUI.BeginToggleGroup("Shadow", "Does this gas giant recieve shadow?", Target.Shadow);
		{
			SGT_EditorGUI.BeginGroup("Caster");
			{
				Target.ShadowCasterType = (SGT_ShadowOccluder)SGT_EditorGUI.EnumField("Type", "Specify the type of object that is casting a shadow on this gas giant.", Target.ShadowCasterType);
				
				switch (Target.ShadowCasterType)
				{
					case SGT_ShadowOccluder.Ring:
					{
						Target.ShadowCasterAutoUpdate = SGT_EditorGUI.BoolField("Auto Update", null, Target.ShadowCasterAutoUpdate);
						
						SGT_EditorGUI.BeginFrozen(Target.ShadowCasterAutoUpdate == false);
						{
							Target.ShadowCasterRadius = SGT_EditorGUI.FloatField("Radius", "The inner radius of the ring.", Target.ShadowCasterRadius);
							Target.ShadowCasterWidth  = SGT_EditorGUI.FloatField("Width", "The width of the ring.", Target.ShadowCasterWidth);
						}
						SGT_EditorGUI.EndFrozen();
					}
					break;
					
					case SGT_ShadowOccluder.Planet:
					{
						Target.ShadowCasterGameObject = SGT_EditorGUI.ObjectField("Game Object", "The GameObject representing the planet that is casting a shadow on this gas giant.", Target.ShadowCasterGameObject, true);
						Target.ShadowCasterAutoUpdate = SGT_EditorGUI.BoolField("Auto Update", null, Target.ShadowCasterAutoUpdate);
						
						SGT_EditorGUI.BeginFrozen(Target.ShadowCasterAutoUpdate == false);
						{
							Target.ShadowCasterRadius = SGT_EditorGUI.FloatField("Radius", "The radius of the shadow caster.", Target.ShadowCasterRadius);
						}
						SGT_EditorGUI.EndFrozen();
						
						Target.ShadowCasterWidth = SGT_EditorGUI.FloatField("Width", "The width of penumbra (soft shadow) region.", Target.ShadowCasterWidth);
					}
					break;
				}
			}
			SGT_EditorGUI.EndGroup();
			
			SGT_EditorGUI.Separator();
			
			Target.ShadowTexture = SGT_EditorGUI.ObjectField("Texture", "The shadow texture. Left = inner shadow. Right = outer shadow.", Target.ShadowTexture, true);
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
	}
}