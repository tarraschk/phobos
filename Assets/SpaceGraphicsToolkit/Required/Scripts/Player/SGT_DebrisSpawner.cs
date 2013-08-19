using GameObjectList = System.Collections.Generic.List<UnityEngine.GameObject>;
using VariantList    = System.Collections.Generic.List<SGT_DebrisVariant>;

using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Debris Spawner")]
public class SGT_DebrisSpawner : SGT_MonoBehaviourUnique<SGT_DebrisSpawner>
{
	[SerializeField]
	private GameObject debrisCentre;
	
	[SerializeField]
	private int debrisCountMax = 10;
	
	[SerializeField]
	private float debrisContainerRadius = 100.0f;
	
	[SerializeField]
	private float debrisContainerThickness = 100.0f;
	
	[SerializeField]
	private bool debris2D;
	
	[SerializeField]
	private VariantList variantList;
	
	[SerializeField]
	private SGT_WeightedRandom weightedRandom;
	
	[SerializeField]
	private GameObjectList debrisList;
	
	/*[SerializeField]*/
	private Vector3 oldCentre;
	
	/*[SerializeField]*/
	private Vector3 velocity;
	
	public GameObject DebrisCentre
	{
		set
		{
			debrisCentre = value;
		}
		
		get
		{
			return debrisCentre;
		}
	}
	
	public int DebrisCountMax
	{
		set
		{
			debrisCountMax = Mathf.Max(value, 0);
		}
		
		get
		{
			return debrisCountMax;
		}
	}
	
	public float DebrisContainerRadius
	{
		set
		{
			debrisContainerRadius = Mathf.Max(value, 0.0f);
		}
		
		get
		{
			return debrisContainerRadius;
		}
	}
	
	public float DebrisContainerThickness
	{
		set
		{
			debrisContainerThickness = Mathf.Max(value, 0.0f);
		}
		
		get
		{
			return debrisContainerThickness;
		}
	}
	
	public bool Debris2D
	{
		set
		{
			debris2D = value;
		}
		
		get
		{
			return debris2D;
		}
	}
	
	public int DebrisCount
	{
		get
		{
			return debrisList != null ? debrisList.Count : 0;
		}
	}
	
	public int VariantCount
	{
		get
		{
			return variantList != null ? variantList.Count : 0;
		}
	}
	
	public float DebrisContainerInnerRadius
	{
		get
		{
			return debrisContainerRadius - debrisContainerThickness * 0.5f;
		}
	}
	
	public float DebrisContainerOuterRadius
	{
		get
		{
			return debrisContainerRadius + debrisContainerThickness * 0.5f;
		}
	}
	
	public Vector3 RandomPosition
	{
		get
		{
			if (debrisCentre != null)
			{
				var unit = debris2D == true ? ToVector3(Random.insideUnitCircle) : Random.insideUnitSphere;
				return debrisCentre.transform.position + transform.TransformDirection(unit) * DebrisContainerOuterRadius;
			}
			
			return Vector3.zero;
		}
	}
	
	public Vector3 RandomEdgePosition
	{
		get
		{
			if (debrisCentre != null)
			{
				var unit   = debris2D == true ? ToVector3(SGT_Helper.RandomOnUnitCircle) : Random.onUnitSphere;
				var radius = Random.Range(debrisContainerRadius, DebrisContainerOuterRadius);
				
				if (velocity != Vector3.zero)
				{
					if (Vector3.Dot(unit, velocity) <= 0.0f)
					{
						unit = Vector3.Reflect(unit, velocity);
					}
				}
				
				return debrisCentre.transform.position + transform.TransformDirection(unit) * radius;
			}
			
			return Vector3.zero;
		}
	}
	
	public void LateUpdate()
	{
		if (debrisCentre == null) debrisCentre = SGT_Helper.FindCameraGameObject();
		
		var newCentre = SGT_Helper.GetPosition(debrisCentre);
		
		velocity = transform.InverseTransformDirection((newCentre - oldCentre).normalized);
		
		UpdateDebris();
		
		oldCentre = newCentre;
	}
	
	public new void OnDestroy()
	{
		base.OnDestroy();
		
		SGT_Helper.DestroyGameObjects(debrisList);
	}
	
#if UNITY_EDITOR == true
	protected virtual void OnDrawGizmosSelected()
	{
		if (debrisCentre != null)
		{
			if (debris2D == true)
			{
				SGT_Handles.Colour = new Color(1.0f, 1.0f, 1.0f, 0.5f); SGT_Handles.DrawDisc(debrisCentre.transform.position, transform.rotation, DebrisContainerInnerRadius, DebrisContainerOuterRadius);
			}
			else
			{
				SGT_Handles.Colour = new Color(1.0f, 1.0f, 1.0f, 0.5f); SGT_Handles.DrawSphere(debrisCentre.transform.position, DebrisContainerInnerRadius, DebrisContainerOuterRadius);
			}
		}
	}
#endif
	
	public SGT_DebrisVariant AddDebrisVariant(GameObject debrisGameObject)
	{
		if (debrisGameObject != null)
		{
			if (variantList == null) variantList = new VariantList();
			
			var variant = new SGT_DebrisVariant();
			
			variantList.Add(variant);
			
			return variant;
		}
		
		return null;
	}
	
	public SGT_DebrisVariant GetDebrisVariant(int index)
	{
		return SGT_ArrayHelper.Index(variantList, index);
	}
	
	public void RemoveDebrisVariant(int index)
	{
		SGT_ArrayHelper.Remove(variantList, index);
	}
	
	public void SpawnDebris(bool edgeOnly)
	{
		if (variantList != null && debrisCentre != null)
		{
			var index = weightedRandom.RandomIndex;
			
			if (index != -1)
			{
				var variant = variantList[index];
				
				if (variant != null && variant.GameObject != null)
				{
					if (debrisList == null) debrisList = new GameObjectList();
					
					var pos    = edgeOnly == true ? RandomEdgePosition : RandomPosition;
					var rot    = Quaternion.identity;
					var debris = (GameObject)GameObject.Instantiate(variant.GameObject, pos, rot);
					
					debris.transform.parent = transform;
					
					debrisList.Add(debris);
				}
			}
		}
	}
	
	public void RecalculateWeights()
	{
		weightedRandom = new SGT_WeightedRandom(10);
		
		if (variantList != null)
		{
			for (var i = 0; i < variantList.Count; i++)
			{
				var variant = variantList[i];
				
				weightedRandom.Add(i, variant.SpawnProbability);
			}
		}
	}
	
	public void Regenerate()
	{
		RecalculateWeights();
		
		if (debrisList != null)
		{
			SGT_Helper.DestroyObjects(debrisList);
			
			debrisList.Clear();
		}
		
		for (var i = 0; i < debrisCountMax; i++)
		{
			SpawnDebris(false);
		}
	}
	
	private void UpdateDebris()
	{
		if (debrisCentre != null)
		{
			if (debrisList != null)
			{
				var centre         = debrisCentre.transform.position;
				var sqrOuterRadius = DebrisContainerOuterRadius * DebrisContainerOuterRadius;
				
				for (var i = debrisList.Count - 1; i >= 0; i--)
				{
					var debris = debrisList[i];
					
					if (debris != null)
					{
						if ((debris.transform.position - centre).sqrMagnitude > sqrOuterRadius)
						{
							if (debrisList.Count > debrisCountMax)
							{
								debrisList[i] = SGT_Helper.DestroyGameObject(debris);
							}
							else
							{
								debris.transform.position = RandomEdgePosition;
								
								debris.SendMessage("ResetDebrisPosition", SendMessageOptions.DontRequireReceiver);
							}
						}
					}
					else
					{
						debrisList.RemoveAt(i);
					}
				}
			}
			
			for (var i = DebrisCount; i < debrisCountMax; i++)
			{
				SpawnDebris(true);
			}
		}
	}
	
	private Vector3 ToVector3(Vector2 i)
	{
		return new Vector3(i.x, 0.0f, i.y);
	}
}
