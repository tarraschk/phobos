using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Ring")]
public partial class SGT_Ring : SGT_MonoBehaviourUnique<SGT_Ring>
{
	private void UpdateMaterial()
	{
		var targetTechnique = "Variant";
		
		if (tiled == false)
		{
			targetTechnique += "Stretched";
		}
		
		if (shadow == true)
		{
			targetTechnique += "Shadow";
		}
		
		if (lit == true)
		{
			targetTechnique += "Lit";
		}
		
		if (scattering == true)
		{
			targetTechnique += "Scattering";
		}
		
		if (ringMaterial == null || technique != targetTechnique)
		{
			SGT_Helper.DestroyObject(ringMaterial);
			
			technique    = targetTechnique;
			ringMaterial = SGT_Helper.CreateMaterial("Hidden/SGT/Ring/" + technique, renderQueue);
			
			ringMesh.SharedMaterial = ringMaterial;
		}
		else
		{
			SGT_Helper.SetRenderQueue(ringMaterial, renderQueue);
		}
	}
	
	private void UpdateShader()
	{
		var uniformScale        = UniformScale;
		var position            = transform.position;
		var lightSourcePosition = SGT_Helper.GetPosition(lightSource);
		
		ringMaterial.SetTexture("ringTexture", ringTexture);
		ringMaterial.SetVector("position", position);
		ringMaterial.SetFloat("ringRadius", RingRadiusInner * uniformScale);
		ringMaterial.SetFloat("ringThickness", ringWidth * uniformScale);
		
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
		
		if (lit == true)
		{
			ringMaterial.SetVector("starDirection", StarDirection);
			ringMaterial.SetFloat("ringBrightness", (litBrightnessMin + litBrightnessMax) * 0.5f);
			ringMaterial.SetFloat("ringBrightnessRange", litBrightnessMax - litBrightnessMin);
		}
		
		if (scattering == true)
		{
			ringMaterial.SetVector("starPosition", lightSourcePosition);
			
			var mie  = -(1.0f - 1.0f / Mathf.Pow(10.0f, scatteringMie * 5.0f));
			var mie4 = new Vector4(mie * 2.0f, 1.0f - mie * mie, 1.0f + mie * mie, 1.5f);
			
			ringMaterial.SetVector("mieValues", mie4);
			ringMaterial.SetFloat("occlusion", scatteringOcclusion);
		}
	}
	
	private void UpdateRingRotations()
	{
		if (generatedRotations != null && generatedRotations.Length > 0)
		{
			ringMesh.Resize(generatedRotations.Length);
			ringMesh.Update();
			
			for (var i = 0; i < generatedRotations.Length; i++)
			{
				var meshRoot = ringMesh.GetMeshGameObject(i);
				
				if (meshRoot != null)
				{
					SGT_Helper.SetLocalRotation(meshRoot.transform, generatedRotations[i]);
				}
			}
		}
		else
		{
			ringMesh.Resize(1);
			ringMesh.Update();
		}
	}
	
	private void CheckForModifications()
	{
		if (modified == false)
		{
			if (generatedMesh == null)
			{
				modified = true;
			}
		}
	}
}