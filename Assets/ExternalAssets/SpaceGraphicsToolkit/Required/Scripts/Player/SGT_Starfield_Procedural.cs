using MeshList = System.Collections.Generic.List<UnityEngine.Mesh>;

using UnityEngine;
using SGT_Internal;

public partial class SGT_Starfield
{
	public void Regenerate()
	{
		if (modified == false) CheckForModifications();
		
		if (modified == true)
		{
			packer.Pack();
			
			DestroyGeneratedMeshes();
			
			if (packer.OutputCount > 0)
			{
				SGT_Helper.BeginRandomSeed(starfieldSeed);
				{
					var remainingStars = starfieldStarCount;
					var starsPerMesh   = SGT_Helper.MeshVertexLimit / 4;
					var newMeshes      = new MeshList();
					
					while (remainingStars > 0)
					{
						var starsInMesh = Mathf.Min(remainingStars, starsPerMesh);
						
						newMeshes.Add(GenerateStarMesh(starsInMesh));
						
						remainingStars -= starsInMesh;
					}
					
					meshes = newMeshes.ToArray();
					
					if (starfieldMultiMesh != null)
					{
						starfieldMultiMesh.ReplaceAll(meshes);
						starfieldMultiMesh.Update();
					}
				}
				SGT_Helper.EndRandomSeed();
			}
			
			MarkAsUnmodified();
		}
	}
	
	private void DestroyGeneratedMeshes()
	{
		meshDatas = null;
		meshes    = SGT_Helper.DestroyObjects(meshes);
	}
	
	private Mesh GenerateStarMesh(int starCount)
	{
		var positions = new Vector3[starCount * 4];
		var indices   = new int[starCount * 6];
		var uv0s      = new Vector2[starCount * 4];
		var uv1s      = new Vector2[starCount * 4];
		var normals   = new Vector3[starCount * 4];
		var colours   = new Color[starCount * 4];
		var bounds    = new Bounds();
		var weights   = new SGT_WeightedRandom(100);
		
		for (var i = 0; i < StarVariantCount; i++)
		{
			var ssv = GetStarVariant(i);
			
			weights.Add(i, ssv != null ? ssv.SpawnProbability : 1.0f);
		}
		
		for (var i = 0; i < starCount; i++)
		{
			var i0 =  i * 6;
			var i1 = i0 + 1;
			var i2 = i1 + 1;
			var i3 = i2 + 1;
			var i4 = i3 + 1;
			var i5 = i4 + 1;
			
			var v0 =  i * 4;
			var v1 = v0 + 1;
			var v2 = v1 + 1;
			var v3 = v2 + 1;
			
			// Index data
			indices[i0] = v0;
			indices[i1] = v1;
			indices[i2] = v2;
			indices[i3] = v3;
			indices[i4] = v2;
			indices[i5] = v1;
			
			// Calculate star values
			var position = GeneratePosition();
			var index    = weights.RandomIndex;
			var po       = packer.GetOutput(index);
			var ssv      = GetStarVariant(index);
			
			float baseRadius, minRadius, maxRadius;
			
			if (ssv != null && ssv.Custom == true)
			{
				baseRadius = Random.Range(ssv.CustomRadiusMin, ssv.CustomRadiusMax);
				minRadius  = Mathf.Max(baseRadius - ssv.CustomPulseRadiusMax, ssv.CustomRadiusMin);
				maxRadius  = Mathf.Min(baseRadius + ssv.CustomPulseRadiusMax, ssv.CustomRadiusMax);
			}
			else
			{
				baseRadius = Random.Range(starRadiusMin, starRadiusMax);
				minRadius  = Mathf.Max(baseRadius - starPulseRadiusMax, starRadiusMin);
				maxRadius  = Mathf.Min(baseRadius + starPulseRadiusMax, starRadiusMax);
			}
			
			var midRadius   = (minRadius + maxRadius) * 0.5f;
			var pulseRadius = (maxRadius - minRadius) * 0.5f;
			var pulseRate   = Random.Range(0.0f, 1.0f);
			var pulseOffset = Random.Range(0.0f, 1.0f);
			
			var colour    = new Color(pulseRate, pulseOffset, 0.0f);
			var uv1       = new Vector2(midRadius, pulseRadius);
			var rollAngle = Random.Range(-Mathf.PI, Mathf.PI);
			var right     = SGT_Helper.Rotate(Vector2.right * SGT_Helper.InscribedBox, rollAngle);
			var up        = SGT_Helper.Rotate(Vector2.up    * SGT_Helper.InscribedBox, rollAngle);
			
			bounds.Encapsulate(position);
			
			// Write star values into vertex data
			positions[v0] = position;
			positions[v1] = position;
			positions[v2] = position;
			positions[v3] = position;
			
			normals[v0] = SGT_Helper.NewVector3(-right + up, 0.0f);
			normals[v1] = SGT_Helper.NewVector3( right + up, 0.0f);
			normals[v2] = SGT_Helper.NewVector3(-right - up, 0.0f);
			normals[v3] = SGT_Helper.NewVector3( right - up, 0.0f);
			
			colours[v0] = colour;
			colours[v1] = colour;
			colours[v2] = colour;
			colours[v3] = colour;
			
			if (po != null)
			{
				uv0s[v0] = po.UvTopLeft;
				uv0s[v1] = po.UvTopRight;
				uv0s[v2] = po.UvBottomLeft;
				uv0s[v3] = po.UvBottomRight;
			}
			
			uv1s[v0] = uv1;
			uv1s[v1] = uv1;
			uv1s[v2] = uv1;
			uv1s[v3] = uv1;
		}
		
		bounds.Expand(starRadiusMax);
		
		var starMesh = new Mesh();
		
		starMesh.name      = "Starfield";
		starMesh.bounds    = bounds;
		starMesh.vertices  = positions;
		starMesh.normals   = normals;
		starMesh.colors    = colours;
		starMesh.uv        = uv0s;
		starMesh.uv1       = uv1s;
		starMesh.triangles = indices;
		
		return starMesh;
	}
	
	private Vector3 GeneratePosition()
	{
		var position = Vector3.zero;
		
		switch (distribution)
		{
			case SGT_StarfieldDistribution.OnSphere:
			{
				position = Random.onUnitSphere;
				position.y *= distributionConstantA;
				position.Normalize();
			}
			break;
			
			case SGT_StarfieldDistribution.OnDome:
			{
				position = Random.onUnitSphere;
				position.y = Mathf.Abs(position.y);
			}
			break;
			
			case SGT_StarfieldDistribution.OnCircle:
			{
				var ang = Random.Range(-180.0f, 180.0f);
				
				position.x = Mathf.Sin(ang);
				position.z = Mathf.Cos(ang);
			}
			break;
			
			case SGT_StarfieldDistribution.OnCube:
			{
				var x = Random.Range(-SGT_Helper.InscribedCube, SGT_Helper.InscribedCube);
				var y = Random.Range(-SGT_Helper.InscribedCube, SGT_Helper.InscribedCube);
				var z = Random.value >= 0.5f ? -SGT_Helper.InscribedCube : SGT_Helper.InscribedCube;
				
				switch (Random.Range(0, 3))
				{
					case 0: position = new Vector3(z, x, y); break;
					case 1: position = new Vector3(x, z, y); break;
					case 2: position = new Vector3(x, y, z); break;
				}
			}
			break;
			
			case SGT_StarfieldDistribution.InSphere:
			{
				position = Random.insideUnitSphere;
				var magnitude = distributionConstantB + position.magnitude * (1.0f - distributionConstantB);
				position.y *= distributionConstantA;
				position = position.normalized * magnitude;
			}
			break;
			
			case SGT_StarfieldDistribution.EllipticalGalaxy:
			{
				position = Random.insideUnitSphere;
				var magnitude = position.magnitude;
				position.y *= distributionConstantA;
				position = position.normalized * (1.0f - magnitude);
			}
			break;
			
			case SGT_StarfieldDistribution.InDome:
			{
				position = Random.insideUnitSphere;
				position.y = Mathf.Abs(position.y);
			}
			break;
			
			case SGT_StarfieldDistribution.InCircle:
			{
				var o = Random.insideUnitCircle;
				
				position.x = o.x;
				position.z = o.y;
			}
			break;
			
			case SGT_StarfieldDistribution.InCube:
			{
				position.x = Random.Range(-SGT_Helper.InscribedCube, SGT_Helper.InscribedCube);
				position.y = Random.Range(-SGT_Helper.InscribedCube, SGT_Helper.InscribedCube);
				position.z = Random.Range(-SGT_Helper.InscribedCube, SGT_Helper.InscribedCube);
			}
			break;
		}
		
		return position * distributionRadius;
	}
}