using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Corona")]
public partial class SGT_Corona : SGT_MonoBehaviourUnique<SGT_Corona>
{
	private void UpdateMaterial()
	{
		var targetCoronaTechnique = "Variant";
		
		if (cullNear == true)
		{
			targetCoronaTechnique += "CullNear";
		}
		
		if (coronaPerPixel == true)
		{
			targetCoronaTechnique += "PerPixel";
		}
		
		if (meshType == Type.Ring)
		{
			targetCoronaTechnique += "Ring";
		}
		
		// Update surface?
		if (coronaTechnique != targetCoronaTechnique || coronaMaterial == null)
		{
			SGT_Helper.DestroyObject(coronaMaterial);
			
			coronaTechnique = targetCoronaTechnique;
			coronaMaterial  = SGT_Helper.CreateMaterial("Hidden/SGT/Corona/" + coronaTechnique, coronaRenderQueue);
		}
		else
		{
			SGT_Helper.SetRenderQueue(coronaMaterial, coronaRenderQueue);
		}
	}
	
	private void UpdateShader()
	{
		var uniformScale = UniformScale;
		
		// Update shader variables
		coronaMaterial.SetTexture("coronaTexture", coronaTexture);
		coronaMaterial.SetVector("coronaPosition", transform.position);
		coronaMaterial.SetColor("coronaColour", SGT_Helper.Premultiply(coronaColour));
		coronaMaterial.SetFloat("coronaFalloff", coronaFalloff * coronaFalloff);
		
		if (cullNear == true)
		{
			coronaMaterial.SetFloat("cullNearOffset", cullNearOffset * uniformScale);
			coronaMaterial.SetFloat("invCullNearLength", SGT_Helper.Reciprocal(cullNearLength * uniformScale));
		}
		
		if (meshType == Type.Ring)
		{
			coronaMaterial.SetFloat("coronaRadius", meshRadius * uniformScale);
			coronaMaterial.SetFloat("coronaHeight", meshHeight * uniformScale);
		}
	}
	
	private void UpdateCoronaOffset()
	{
		if (coronaObserver != null && coronaGameObject != null)
		{
			var observerPosition  = coronaObserver.transform.position;
			var observerDirection = (observerPosition - coronaGameObject.transform.position).normalized;
			
			SGT_Helper.SetLocalPosition(coronaGameObject.transform, observerDirection * coronaOffset);
			
			if (meshAlignment == Alignment.Billboard)
			{
				if (coronaGameObject.transform.forward != observerDirection)
				{
					coronaGameObject.transform.forward = observerDirection;
				}
			}
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