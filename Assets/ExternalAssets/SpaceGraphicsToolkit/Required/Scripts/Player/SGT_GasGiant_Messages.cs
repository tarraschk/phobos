using UnityEngine;
using SGT_Internal;

public partial class SGT_GasGiant
{
	public void Awake()
	{
		if (ThisHasBeenDuplicated("atmosphereMaterial", "lightingTexture") == true)
		{
			atmosphereMaterial = SGT_Helper.CloneObject(atmosphereMaterial);
			lightingTexture    = SGT_Helper.CloneObject(lightingTexture);
		}
	}
	
	public void OnEnable()
	{
		if (atmosphereMesh != null) atmosphereMesh.OnEnable();
	}
	
	public void OnDisable()
	{
		if (atmosphereMesh != null) atmosphereMesh.OnDisable();
	}
	
	public void LateUpdate()
	{
		if (oblatenessGameObject == null) oblatenessGameObject = SGT_Helper.CreateGameObject("Oblateness", gameObject);
		if (atmosphereGameObject == null) atmosphereGameObject = SGT_Helper.CreateGameObject("Atmosphere", oblatenessGameObject);
		if (atmosphereMesh       == null) atmosphereMesh      = new SGT_Mesh();
		if (gasGiantLightSource  == null) gasGiantLightSource = SGT_LightSource.Find();
		if (gasGiantObserver     == null) gasGiantObserver    = SGT_Helper.FindCamera();
		
		SGT_Helper.SetParent(oblatenessGameObject, gameObject);
		SGT_Helper.SetLayer(oblatenessGameObject, gameObject.layer);
		SGT_Helper.SetTag(oblatenessGameObject, gameObject.tag);
		
		SGT_Helper.SetParent(atmosphereGameObject, oblatenessGameObject);
		SGT_Helper.SetLayer(atmosphereGameObject, oblatenessGameObject.layer);
		SGT_Helper.SetTag(atmosphereGameObject, oblatenessGameObject.tag);
		
		if (atmosphereLightingColour == null)
		{
			atmosphereLightingColour = new SGT_ColourGradient(false, false);
			atmosphereLightingColour.AddColourNode(Color.black, 0.4f);
			atmosphereLightingColour.AddColourNode(Color.white, 0.7f);
		}
		
		if (atmosphereTwilightColour == null)
		{
			atmosphereTwilightColour = new SGT_ColourGradient(false, true);
			atmosphereTwilightColour.AddAlphaNode(1.0f, 0.4f);
			atmosphereTwilightColour.AddAlphaNode(0.0f, 0.5f);
			atmosphereTwilightColour.AddColourNode(Color.red, 0.5f);
		}
		
		if (atmosphereLimbColour == null)
		{
			atmosphereLimbColour = new SGT_ColourGradient(false, true);
			atmosphereLimbColour.AddAlphaNode(0.0f, 0.75f);
			atmosphereLimbColour.AddAlphaNode(1.0f, 1.0f);
			atmosphereLimbColour.AddColourNode(Color.blue, 0.4f);
		}
		
		if (shadowAutoUpdate == true)
		{
			switch (shadowType)
			{
				case SGT_ShadowOccluder.Ring:
				{
					var fill = new SGT_FillRingDimensions();
					
					SendMessage("FillRingDimensions", fill, SendMessageOptions.DontRequireReceiver);
					
					shadowRadius = fill.Radius;
					shadowWidth  = fill.Width;
				}
				break;
				
				case SGT_ShadowOccluder.Planet:
				{
					if (shadowGameObject != null)
					{
						var fill = new SGT_FillFloat();
						
						shadowGameObject.SendMessage("FillShadowRadius", fill, SendMessageOptions.DontRequireReceiver);
						
						shadowRadius = fill.Float;
					}
				}
				break;
			}
		}
		
		UpdateTransform();
		UpdateGradient();
		UpdateMaterial();
		UpdateShader();
		
		// Update mesh
		atmosphereMesh.GameObject          = atmosphereGameObject;
		atmosphereMesh.HasMeshRenderer     = true;
		atmosphereMesh.MeshRendererEnabled = true;
		atmosphereMesh.SharedMaterial      = atmosphereMaterial;
		atmosphereMesh.Update();
		
#if UNITY_EDITOR == true
		SGT_Helper.HideGameObject(oblatenessGameObject);
		
		atmosphereMesh.HideInEditor();
#endif
	}
	
	public new void OnDestroy()
	{
		base.OnDestroy();
		
		SGT_Helper.DestroyGameObject(atmosphereGameObject);
		SGT_Helper.DestroyGameObject(oblatenessGameObject);
		SGT_Helper.DestroyObject(lightingTexture);
		SGT_Helper.DestroyObject(atmosphereMaterial);
	}
	
	public void FillShadowRadius(SGT_FillFloat fill)
	{
		fill.Float = gasGiantEquatorialRadius;
	}
}