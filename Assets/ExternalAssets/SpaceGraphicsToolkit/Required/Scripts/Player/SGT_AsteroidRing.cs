using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Asteroid Ring")]
public partial class SGT_AsteroidRing : SGT_MonoBehaviourUnique<SGT_AsteroidRing>
{
	private void UpdateTechnique()
	{
		var targetTechnique = "Variant";
		
		if (shadow == true)
		{
			targetTechnique += "Shadow";
		}
		
		if (spin == true)
		{
			targetTechnique += "Spin";
		}
		
		if (technique != targetTechnique || ringMaterial == null)
		{
			SGT_Helper.DestroyObject(ringMaterial);
			
			technique    = targetTechnique;
			ringMaterial = SGT_Helper.CreateMaterial("Hidden/SGT/AsteroidRing/" + technique, ringRenderQueue);
		}
		else
		{
			SGT_Helper.SetRenderQueue(ringMaterial, ringRenderQueue);
		}
	}
	
	private void UpdateShader()
	{
		var position            = transform.position;
		var lightSourcePosition = SGT_Helper.GetPosition(ringLightSource);
		var uniformScale        = UniformScale;
		var starPositionRaw     = ringGameObject.transform.worldToLocalMatrix.MultiplyPoint(lightSourcePosition);
		
		ringMaterial.SetTexture("dayTexture", asteroidTextureDay);
		ringMaterial.SetTexture("nightTexture", asteroidTextureNight);
		ringMaterial.SetTexture("bumpTexture", asteroidTextureHeight);
		ringMaterial.SetVector("starPositionRaw", SGT_Helper.NewVector4(starPositionRaw, 1.0f));
		ringMaterial.SetVector("centrePosition", position);
		ringMaterial.SetFloat("ringHeight", ringHeight);
		
		if (shadow == true)
		{
			var shadowRatio = 0.0f;
			var shadowScale = 0.0f;
			
			if (shadowWidth > 0.0f)
			{
				shadowRatio = ShadowInnerRadius / ShadowOuterRadius;
				shadowScale = 1.0f / (1.0f - shadowRatio);
			}
			
			ringMaterial.SetColor("umbraColour", shadowUmbraColour);
			ringMaterial.SetColor("penumbraColour", shadowPenumbraColour);
			ringMaterial.SetFloat("shadowRatio", shadowRatio);
			ringMaterial.SetFloat("shadowScale", shadowScale);
			
			if (ShadowInnerRadius > 0.0f)
			{
				var direction = (position - lightSourcePosition).normalized;
				var r         = Quaternion.FromToRotation(direction, Vector3.forward);
				var s         = SGT_Helper.NewVector3(1.0f / (ShadowOuterRadius * uniformScale));
				
				var shadowT = SGT_MatrixHelper.Translation(-position);
				var shadowR = SGT_MatrixHelper.Rotation(r);
				var shadowS = SGT_MatrixHelper.Scaling(s);
				
				var shadowMatrix = shadowS * shadowR * shadowT;
				
				ringMaterial.SetMatrix("shadowMatrix", shadowMatrix);
			}
		}
		
		if (spin == true)
		{
			ringMaterial.SetFloat("spinRateMax", spinRateMax);
		}
	}
	
	private void CheckForModifications()
	{
		if (modified == false)
		{
			if (SGT_ArrayHelper.Filled(generatedMeshes, 1) == false)
			{
				modified = true;
			}
		}
	}
}