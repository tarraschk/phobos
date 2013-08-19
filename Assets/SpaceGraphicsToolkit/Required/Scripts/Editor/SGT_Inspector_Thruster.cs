using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_Thruster))]
[CanEditMultipleObjects]
public class SGT_Inspector_Thruster : SGT_Inspector<SGT_Thruster>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Thruster");
		{
			Target.ThrusterObserver   = SGT_EditorGUI.ObjectField("Observer", null, Target.ThrusterObserver, true); SetAll("ThrusterObserver");
			Target.ThrusterThrottle   = SGT_EditorGUI.FloatField("Throttle", null, Target.ThrusterThrottle, 0.0f, 1.0f); SetAll("ThrusterThrottle");
			Target.ThrusterTweenSpeed = SGT_EditorGUI.FloatField("Tween Speed", null, Target.ThrusterTweenSpeed); SetAll("ThrusterTweenSpeed");
			
			SGT_EditorGUI.Separator();
			
			Target.ThrusterPhysics = SGT_EditorGUI.BeginToggleGroup("Physics", null, Target.ThrusterPhysics); SetAll("ThrusterPhysics");
			{
				Target.ThrusterPhysicsRigidbody = SGT_EditorGUI.ObjectField("Rigidbody", null, Target.ThrusterPhysicsRigidbody, true); SetAll("ThrusterPhysicsRigidbody");
				
				SGT_EditorGUI.MarkNextFieldAsBold();
				Target.ThrusterPhysicsForce = SGT_EditorGUI.FloatField("Force", null, Target.ThrusterPhysicsForce); SetAll("ThrusterPhysicsForce");
				
				SGT_EditorGUI.BeginIndent();
				{
					Target.ThrusterPhysicsForceMode = (ForceMode)SGT_EditorGUI.EnumField("Mode", null, Target.ThrusterPhysicsForceMode); SetAll("ThrusterPhysicsForceMode");
					Target.ThrusterPhysicsForceType = (SGT_Thruster.ForceType)SGT_EditorGUI.EnumField("Type", null, Target.ThrusterPhysicsForceType); SetAll("ThrusterPhysicsForceType");
				}
				SGT_EditorGUI.EndIndent();
			}
			SGT_EditorGUI.EndToggleGroup();
			
			SGT_EditorGUI.Separator();
			
			Target.ThrusterFlame = SGT_EditorGUI.BeginToggleGroup("Flame", null, Target.ThrusterFlame); SetAll("ThrusterFlame");
			{
				Target.ThrusterFlameMesh     = SGT_EditorGUI.ObjectField("Mesh", null, Target.ThrusterFlameMesh, true); SetAll("ThrusterFlameMesh");
				Target.ThrusterFlameMaterial = SGT_EditorGUI.ObjectField("Material", null, Target.ThrusterFlameMaterial, true); SetAll("ThrusterFlameMaterial");
				Target.ThrusterFlameOffset   = SGT_EditorGUI.Vector3Field("Offset", null, Target.ThrusterFlameOffset); SetAll("ThrusterFlameOffset");
				
				SGT_EditorGUI.MarkNextFieldAsBold();
				Target.ThrusterFlameScale = SGT_EditorGUI.Vector3Field("Scale", null, Target.ThrusterFlameScale); SetAll("ThrusterFlameScale");
				
				SGT_EditorGUI.BeginIndent();
				{
					Target.ThrusterFlameScaleChange  = SGT_EditorGUI.Vector3Field("Change", null, Target.ThrusterFlameScaleChange); SetAll("ThrusterFlameScaleChange");
					Target.ThrusterFlameScaleFlicker = SGT_EditorGUI.FloatField("Flicker", null, Target.ThrusterFlameScaleFlicker, 0.0f, 1.0f); SetAll("ThrusterFlameScaleFlicker");
				}
				SGT_EditorGUI.EndIndent();
			}
			SGT_EditorGUI.EndToggleGroup();
			
			SGT_EditorGUI.Separator();
			
			Target.ThrusterFlare = SGT_EditorGUI.BeginToggleGroup("Flare", null, Target.ThrusterFlare); SetAll("ThrusterFlare");
			{
				Target.ThrusterFlareMesh        = SGT_EditorGUI.ObjectField("Mesh", null, Target.ThrusterFlareMesh, true); SetAll("ThrusterFlareMesh");
				Target.ThrusterFlareMaterial    = SGT_EditorGUI.ObjectField("Material", null, Target.ThrusterFlareMaterial, true); SetAll("ThrusterFlareMaterial");
				Target.ThrusterFlareRaycastMask = SGT_EditorGUI.LayerMaskField("Raycast Mask", null, Target.ThrusterFlareRaycastMask); SetAll("ThrusterFlareRaycastMask");
				Target.ThrusterFlareOffset      = SGT_EditorGUI.Vector3Field("Offset", null, Target.ThrusterFlareOffset); SetAll("ThrusterFlareOffset");
				
				SGT_EditorGUI.MarkNextFieldAsBold();
				Target.ThrusterFlareScale = SGT_EditorGUI.Vector3Field("Scale", null, Target.ThrusterFlareScale); SetAll("ThrusterFlareScale");
				
				SGT_EditorGUI.BeginIndent();
				{
					Target.ThrusterFlareScaleChange     = SGT_EditorGUI.Vector3Field("Change", null, Target.ThrusterFlareScaleChange); SetAll("ThrusterFlareScaleChange");
					Target.ThrusterFlareScaleFlicker    = SGT_EditorGUI.FloatField("Flicker", null, Target.ThrusterFlareScaleFlicker, 0.0f, 1.0f); SetAll("ThrusterFlareScaleFlicker");
					Target.ThrusterFlareScaleTweenSpeed = SGT_EditorGUI.FloatField("Tween Speed", null, Target.ThrusterFlareScaleTweenSpeed); SetAll("ThrusterFlareScaleTweenSpeed");
				}
				SGT_EditorGUI.EndIndent();
			}
			SGT_EditorGUI.EndToggleGroup();
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
	}
}