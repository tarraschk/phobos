using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_Star))]
public class SGT_Inspector_Star : SGT_Inspector<SGT_Star>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Star");
		{
			Target.StarObserver = SGT_EditorGUI.ObjectField("Observer", "The camera rendering this.", Target.StarObserver, true);
			Target.StarLutSize  = (SGT_SquareSize)SGT_EditorGUI.EnumField("LUT Size", "The texture look up table resolution. Use a higher value for smoother results, at the cost of GPU memory and texture sampling speed.", Target.StarLutSize);
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Surface");
		{
			Target.SurfaceRadius        = SGT_EditorGUI.FloatField("Radius", "The star's equatorial surface radius.", Target.SurfaceRadius);
			Target.SurfaceOblateness    = SGT_EditorGUI.FloatField("Oblateness", "This specifies how oblate/flat/round the star is. A higher value means the polar radius will be lower than the equatorial radius. Large stars that spin are often quite oblate.", Target.SurfaceOblateness, 0.0f, 1.0f);
			Target.SurfaceConfiguration = (SGT_SurfaceConfiguration)SGT_EditorGUI.EnumField("Configuration", "Allows you to swap between using a sphere mesh with a cylindrical texture and using a cube mesh with a cube map.", Target.SurfaceConfiguration);
			Target.SurfaceRenderQueue   = SGT_EditorGUI.IntField("Render Queue", "The render queue used by the surface mesh.", Target.SurfaceRenderQueue);
			Target.SurfaceMesh          = SGT_EditorGUI.SurfaceMultiMeshField("Mesh", "This should be a sphere with a radius of 1.", Target.SurfaceMesh, true);
			Target.SurfaceTexture       = SGT_EditorGUI.Field("Texture", "This is the texture used by the star's surface.", Target.SurfaceTexture, true);
			
			SGT_EditorGUI.MarkNextFieldAsBold(Target.SurfaceCollider == true);
			Target.SurfaceCollider = SGT_EditorGUI.BoolField("Create Collider", "Create a MeshCollider from the surface mesh? Note: Don't use this in combination with the Surface Tessellator.", Target.SurfaceCollider);
			
			SGT_EditorGUI.BeginIndent(Target.SurfaceCollider == true);
			{
				Target.SurfaceColliderMaterial = SGT_EditorGUI.ObjectField("Collider Material", null, Target.SurfaceColliderMaterial);
			}
			SGT_EditorGUI.EndIndent();
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Atmosphere");
		{
			Target.AtmosphereMesh          = SGT_EditorGUI.ObjectField("Mesh", "This should be an inside-out sphere with a radius of 1.", Target.AtmosphereMesh, true);
			Target.AtmosphereRenderQueue   = SGT_EditorGUI.IntField("Render Queue", "The render queue used by the atmosphere and cloud meshes.", Target.AtmosphereRenderQueue);
			Target.AtmosphereHeight        = SGT_EditorGUI.FloatField("Height", "Distance between the surface of the star and the top of the atmosphere.", Target.AtmosphereHeight);
			Target.AtmosphereDensityColour = SGT_EditorGUI.Field("Density Colour", "The colour of the atmosphere based on the optical thickness. Left = Star's centre. Centre = Horizon. Right = Sky's zenith.", Target.AtmosphereDensityColour);
			Target.AtmosphereSkyAltitude   = SGT_EditorGUI.FloatField("Sky Altitude", "The altitude at which atmospheric density reaches maximum. This value is used to blend between the Atmosphere and Sky falloff values. A value of 0.25 means the observer must be 3/4 the way through the atmosphere for the atmosphere's falloff to reach the Sky falloff value.", Target.AtmosphereSkyAltitude, 0.0f, 1.0f);
			Target.AtmosphereFog           = SGT_EditorGUI.FloatField("Fog", "Specifies how much fog is present in the atmosphere.", Target.AtmosphereFog, 0.0f, 1.0f);
			
			SGT_EditorGUI.Separator();
			
			SGT_EditorGUI.BeginGroup("Falloff");
			{
				Target.AtmosphereFalloffSurface  = SGT_EditorGUI.FloatField("Surface", "The atmospheric falloff on the star's surface.", Target.AtmosphereFalloffSurface, 0.0f, 5.0f);
				Target.AtmosphereFalloffOutside  = SGT_EditorGUI.FloatField("Outside", "The sky's atmospheric falloff when viewed from space.", Target.AtmosphereFalloffOutside, 0.0f, 5.0f);
				Target.AtmosphereFalloffInside   = SGT_EditorGUI.FloatField("Inside", "The sky's atmospheric falloff when viewed from inside the atmosphere.", Target.AtmosphereFalloffInside, 0.0f, 5.0f);
				Target.AtmosphereFalloffPerPixel = SGT_EditorGUI.BoolField("Per Pixel", "Per-pixel optical depth checks.", Target.AtmosphereFalloffPerPixel);
			}
			SGT_EditorGUI.EndGroup();
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
	}
}