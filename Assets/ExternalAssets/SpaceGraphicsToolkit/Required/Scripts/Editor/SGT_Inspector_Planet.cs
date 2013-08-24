using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_Planet))]
public class SGT_Inspector_Planet : SGT_Inspector<SGT_Planet>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Planet");
		{
			Target.PlanetLighting    = SGT_EditorGUI.Field("Lighting", "The brightness across the planet's surface. Left = Dark side. Right = Day side.", Target.PlanetLighting);
			Target.PlanetLightSource = SGT_EditorGUI.ObjectField("Light Source", "The light source that is illuminating this planet.", Target.PlanetLightSource, true);
			Target.PlanetObserver    = SGT_EditorGUI.ObjectField("Observer", "The camera rendering this planet.", Target.PlanetObserver, true);
			Target.PlanetLutSize     = (SGT_SquareSize)SGT_EditorGUI.EnumField("LUT Size", "The texture look up table resolution. Try to keep this below 256, otherwise you will use up a lot of GPU memory.", Target.PlanetLutSize);
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Surface");
		{
			Target.SurfaceRadius = SGT_EditorGUI.FloatField("Radius", "The radius of the planet surface.", Target.SurfaceRadius);
			
			SGT_EditorGUI.BeginFrozen(Target.SurfaceTextureSpecular.ContainsSomething == true);
			{
				Target.SurfaceSpecularPower = SGT_EditorGUI.FloatField("Specular Power", "The surface specular power. A higher value means the specular point will be sharper.", Target.SurfaceSpecularPower, 1.0f, 10.0f);
			}
			SGT_EditorGUI.EndFrozen();
			
			SGT_EditorGUI.BeginFrozen(Target.SurfaceTextureDetail != null);
			{
				Target.SurfaceDetailRepeat = SGT_EditorGUI.FloatField("Detail Repeat", "The amount of times the detail texture will be repeated across the planet surface.", Target.SurfaceDetailRepeat);
			}
			SGT_EditorGUI.EndFrozen();
			
			Target.SurfaceConfiguration = (SGT_SurfaceConfiguration)SGT_EditorGUI.EnumField("Configuration", "Allows you to change between using a single sphere mesh, or a cubed sphere. A cubed sphere will be free from polar distortion.", Target.SurfaceConfiguration);
			Target.SurfaceMesh          = SGT_EditorGUI.SurfaceMultiMeshField("Mesh", "This should be a sphere with a radius of 1. Search for 'surface' to find suitable meshes.", Target.SurfaceMesh, true);
			Target.SurfaceRenderQueue   = SGT_EditorGUI.IntField("Render Queue", "The render queue used by the surface mesh.", Target.SurfaceRenderQueue);
			
			SGT_EditorGUI.MarkNextFieldAsBold(Target.SurfaceCollider == true);
			Target.SurfaceCollider = SGT_EditorGUI.BoolField("Collider", "Create a MeshCollider from the surface mesh? Note: Don't use this in combination with the Surface Tessellator.", Target.SurfaceCollider);
			
			SGT_EditorGUI.BeginIndent(Target.SurfaceCollider == true);
			{
				Target.SurfaceColliderMaterial = SGT_EditorGUI.ObjectField("Material", null, Target.SurfaceColliderMaterial);
			}
			SGT_EditorGUI.EndIndent();
			
			SGT_EditorGUI.Separator();
			
			SGT_EditorGUI.BeginGroup("Texture");
			{
				Target.SurfaceTextureDay      = SGT_EditorGUI.Field("Day", "The daytime planet texture.", Target.SurfaceTextureDay, true);
				Target.SurfaceTextureNight    = SGT_EditorGUI.Field("Night", "[optional] The nighttime planet texture.", Target.SurfaceTextureNight, false);
				Target.SurfaceTextureNormal   = SGT_EditorGUI.Field("Normal", "[optional] The surface normal map.", Target.SurfaceTextureNormal, false);
				Target.SurfaceTextureSpecular = SGT_EditorGUI.Field("Specular", "[optional] The surface specular texture. White represents specular. Black represents no specular.", Target.SurfaceTextureSpecular, false);
				Target.SurfaceTextureDetail   = SGT_EditorGUI.ObjectField("Detail", "[optional] The surface detail map.", Target.SurfaceTextureDetail);
			}
			SGT_EditorGUI.EndGroup();
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
		
		Target.Atmosphere = SGT_EditorGUI.BeginToggleGroup("Atmosphere", "Does the planet have an atmosphere?", Target.Atmosphere);
		{
			Target.AtmosphereMesh           = SGT_EditorGUI.ObjectField("Mesh", "This should be an inside-out sphere with a radius of 1.", Target.AtmosphereMesh, true);
			Target.AtmosphereRenderQueue    = SGT_EditorGUI.IntField("Render Queue", "The render queue used by the atmosphere and cloud meshes.", Target.AtmosphereRenderQueue);
			Target.AtmosphereHeight         = SGT_EditorGUI.FloatField("Height", "Distance between the surface of the planet and the top of the atmosphere.", Target.AtmosphereHeight);
			Target.AtmosphereDensityColour  = SGT_EditorGUI.Field("Density Colour", "The colour of the atmosphere based on the optical thickness. Left = Planet's centre. Centre = Horizon. Right = Sky's zenith.", Target.AtmosphereDensityColour);
			Target.AtmosphereTwilightColour = SGT_EditorGUI.Field("Twilight Colour", "A colour that will overlay the atmosphere based on the angle to the light, useful for sunset/sunrise colours.", Target.AtmosphereTwilightColour);
			Target.AtmosphereNightOpacity   = SGT_EditorGUI.FloatField("Night Opacity", "The opacity of the dark side of the atmosphere.", Target.AtmosphereNightOpacity, 0.0f, 1.0f);
			Target.AtmosphereSkyAltitude    = SGT_EditorGUI.FloatField("Sky Altitude", "The altitude at which atmospheric density reaches maximum. This value is used to blend between the Atmosphere and Sky falloff values. A value of 0.25 means the observer must be 3/4 the way through the atmosphere for the atmosphere's falloff to reach the Sky falloff value.", Target.AtmosphereSkyAltitude, 0.0f, 1.0f);
			Target.AtmosphereFog            = SGT_EditorGUI.FloatField("Fog", "Specifies how much fog is present in the atmosphere.", Target.AtmosphereFog, 0.0f, 1.0f);
			
			SGT_EditorGUI.Separator();
			
			SGT_EditorGUI.BeginGroup("Falloff");
			{
				Target.AtmosphereFalloffSurface = SGT_EditorGUI.FloatField("Surface", "The atmospheric falloff on the planet's surface.", Target.AtmosphereFalloffSurface, 0.0f, 10.0f);
				Target.AtmosphereFalloffOutside = SGT_EditorGUI.FloatField("Outside", "The atmospheric falloff when viewed from space.", Target.AtmosphereFalloffOutside, 0.0f, 10.0f);
				Target.AtmosphereFalloffInside  = SGT_EditorGUI.FloatField("Inside", "The atmospheric falloff when viewed from inside the atmosphere (i.e. the sky falloff).", Target.AtmosphereFalloffInside, 0.0f, 10.0f);
			}
			SGT_EditorGUI.EndGroup();
			
			SGT_EditorGUI.Separator();
			
			Target.AtmosphereScattering = SGT_EditorGUI.BeginToggleGroup("Scattering", "Use atmospheric scattering?", Target.AtmosphereScattering);
			{
				Target.AtmosphereScatteringMie      = SGT_EditorGUI.FloatField("Mie", "The Mie asymetry. This will brighten the atmosphere based on the angle to the light source. A higher value means the light source will appear very small.", Target.AtmosphereScatteringMie, 0.0f, 1.0f);
				Target.AtmosphereScatteringRayleigh = SGT_EditorGUI.FloatField("Rayleigh", "The Rayleigh asymetry. This will brighten the parts of the atmosphere that point at and point away from the light source. A higher value means there will be a greater disparity between the brighter and darker parts.", Target.AtmosphereScatteringRayleigh, 0.0f, 5.0f);
			}
			SGT_EditorGUI.EndToggleGroup();
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
		
		Target.Clouds = SGT_EditorGUI.BeginToggleGroup("Clouds", "Does this planet have a cloud layer?", Target.Clouds);
		{
			Target.CloudsConfiguration  = (SGT_SurfaceConfiguration)SGT_EditorGUI.EnumField("Configuration", "Allows you to swap between using a sphere mesh with a cylindrical texture and using a cube mesh with a cube map.", Target.CloudsConfiguration);
			Target.CloudsMesh           = SGT_EditorGUI.SurfaceMultiMeshField("Mesh", "This should be a sphere with a radius of 1.", Target.CloudsMesh, true);
			Target.CloudsRenderQueue    = SGT_EditorGUI.IntField("Render Queue", "The render queue used by the atmosphere and cloud meshes.", Target.CloudsRenderQueue);
			Target.CloudsHeight         = SGT_EditorGUI.FloatField("Height", "The distance between the surface of the planet and the surface of the clouds.", Target.CloudsHeight);
			Target.CloudsTexture        = SGT_EditorGUI.Field("Texture", "The cloud texture applied to the cloud mesh.", Target.CloudsTexture, true);
			Target.CloudsLimbColour     = SGT_EditorGUI.Field("Limb Colour", "The cloud colour based on the surface angle. This allows you to give the edges (the limb) a different colour from the centre.", Target.CloudsLimbColour);
			Target.CloudsFalloff        = SGT_EditorGUI.FloatField("Falloff", "The opacity of the clouds at the edges. A higher value means the clouds will be more visible near the edges of the surface.", Target.CloudsFalloff, 0.0f, 10.0f);
			Target.CloudsRotationPeriod = SGT_EditorGUI.FloatField("Rotation Period", "The time in seconds it takes for the clouds to rotate about their axis.", Target.CloudsRotationPeriod);
			Target.CloudsOffset         = SGT_EditorGUI.FloatField("Offset", "The amount the clouds are moved toward the camera. This can be used to avoid depth fighting with the atmosphere.", Target.CloudsOffset);
			
			SGT_EditorGUI.BeginFrozen(Target.Atmosphere == true);
			{
				Target.CloudsTwilightOffset = SGT_EditorGUI.FloatField("Twilight Offset", "The ammount the twilight texture is shifted on the cloud surface.", Target.CloudsTwilightOffset, -0.5f, 0.5f);
			}
			SGT_EditorGUI.EndFrozen();
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
		
		Target.Shadow = SGT_EditorGUI.BeginToggleGroup("Shadow", "Does this planet recieve shadow?", Target.Shadow);
		{
			SGT_EditorGUI.BeginGroup("Caster");
			{
				Target.ShadowCasterType = (SGT_ShadowOccluder)SGT_EditorGUI.EnumField("Type", "Specify the type of object that is casting a shadow on this gas giant.", Target.ShadowCasterType);
				
				switch (Target.ShadowCasterType)
				{
					case SGT_ShadowOccluder.Ring:
					{
						Target.ShadowCasterAutoUpdate = SGT_EditorGUI.BoolField("Auto Update", "Automatically updates the Radius and Width based on the settings of any Ring componet attached to this GameObject.", Target.ShadowCasterAutoUpdate);
						
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
						Target.ShadowCasterGameObject = SGT_EditorGUI.ObjectField("GameObject", "The GameObject representing the planet that is casting a shadow on this gas giant.", Target.ShadowCasterGameObject, true);
						Target.ShadowCasterAutoUpdate = SGT_EditorGUI.BoolField("Auto Update", "Automatically updates the Radius field based on the radius of the Caster GameObject.", Target.ShadowCasterAutoUpdate);
						
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
			
			SGT_EditorGUI.BeginGroup("Texture");
			{
				Target.ShadowTextureSurface = SGT_EditorGUI.ObjectField("Surface", "The shadow texture applied to the surface mesh.", Target.ShadowTextureSurface, true);
				
				SGT_EditorGUI.BeginFrozen(Target.Atmosphere);
				{
					Target.ShadowTextureAtmosphere = SGT_EditorGUI.ObjectField("Atmosphere", "The shadow texture applied to the atmosphere mesh.", Target.ShadowTextureAtmosphere, true);
				}
				SGT_EditorGUI.EndFrozen();
				
				SGT_EditorGUI.BeginFrozen(Target.Clouds);
				{
					Target.ShadowTextureClouds = SGT_EditorGUI.ObjectField("Clouds", "The shadow texture applied to the clouds mesh.", Target.ShadowTextureClouds, true);
				}
				SGT_EditorGUI.EndFrozen();
			}
			SGT_EditorGUI.EndGroup();
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
	}
}