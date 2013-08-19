using UnityEngine;

public partial class SGT_Corona
{
	public void Regenerate()
	{
		if (modified == false)
		{
			CheckForModifications();
		}
		
		if (modified == true)
		{
			modified = false;
			
			SGT_Helper.BeginRandomSeed(meshSeed);
			{
				Quaternion[] rotations = null;
				
				switch (meshAlignment)
				{
					case Alignment.Billboard:
					{
						rotations = new Quaternion[1];
						rotations[0] = Quaternion.identity;
					}
					break;
					case Alignment.AxisAlined:
					{
						coronaGameObject.transform.localRotation = Quaternion.identity;
						
						rotations = new Quaternion[3];
						rotations[0] = Quaternion.LookRotation(Vector3.right);
						rotations[1] = Quaternion.LookRotation(Vector3.up);
						rotations[2] = Quaternion.LookRotation(Vector3.forward);
					}
					break;
					case Alignment.Random:
					{
						coronaGameObject.transform.localRotation = Quaternion.identity;
						
						rotations = new Quaternion[meshPlaneCount];
						
						for (var i = 0; i < rotations.Length; i++)
						{
							rotations[i] = Random.rotation;
						}
					}
					break;
				}
				
				// Replace mesh
				SGT_Helper.DestroyObject(generatedMesh);
				
				switch (meshType)
				{
					case Type.Plane:
					{
						GeneratePlane(rotations);
					}
					break;
					
					case Type.Ring:
					{
						GenerateRing(rotations);
					}
					break;
				}
				
				if (coronaMesh != null)
				{
					coronaMesh.SharedMesh = generatedMesh;
				}
			}
			SGT_Helper.EndRandomSeed();
			
			UpdateCoronaOffset();
		}
	}
	
	private void GeneratePlane(Quaternion[] rotations)
	{
		var positions = new Vector3[rotations.Length * 4];
		var indices   = new int[rotations.Length * 6];
		var uv0       = new Vector2[rotations.Length * 4];
		
		for (var i = 0; i < rotations.Length; i++)
		{
			// Index data
			var indicesIndex = i * 6;
			var vertexIndex  = i * 4;
			
			indices[indicesIndex + 0] = vertexIndex + 0;
			indices[indicesIndex + 1] = vertexIndex + 1;
			indices[indicesIndex + 2] = vertexIndex + 2;
			indices[indicesIndex + 3] = vertexIndex + 3;
			indices[indicesIndex + 4] = vertexIndex + 2;
			indices[indicesIndex + 5] = vertexIndex + 1;
			
			// Calculate corona plane values
			var rot   = rotations[i];
			var right = rot * (meshRadius * Vector3.right);
			var up    = rot * (meshRadius * Vector3.up   );
			
			// Write star values into vertex data
			positions[vertexIndex + 0] = -right + up;
			positions[vertexIndex + 1] =  right + up;
			positions[vertexIndex + 2] = -right - up;
			positions[vertexIndex + 3] =  right - up;
			
			uv0[vertexIndex + 0] = new Vector2(0.0f, 1.0f);
			uv0[vertexIndex + 1] = new Vector2(1.0f, 1.0f);
			uv0[vertexIndex + 2] = new Vector2(0.0f, 0.0f);
			uv0[vertexIndex + 3] = new Vector2(1.0f, 0.0f);
		}
		
		generatedMesh = new Mesh();
		generatedMesh.vertices  = positions;
		generatedMesh.triangles = indices;
		generatedMesh.uv        = uv0;
		generatedMesh.RecalculateBounds();
		generatedMesh.RecalculateNormals();
	}
	
	private void GenerateRing(Quaternion[] rotations)
	{
		var positions       = new Vector3[rotations.Length * (meshSegments * 2 + 2)];
		var indices         = new int[rotations.Length * meshSegments * 6];
		var uv0             = new Vector2[rotations.Length * (meshSegments * 2 + 2)];
		var vStep           = 1.0f / meshSegments;
		var angleStep       = (Mathf.PI * 2.0f) / meshSegments;
		var meshRadiusOuter = (meshRadius + meshHeight) * SGT_Helper.PolygonOuterBoundScale(angleStep);
		
		// Write index data
		{
			var indicesIndex = 0;
			var vertexIndex  = 0;
			
			for (var r = 0; r < rotations.Length; r++)
			{
				for (var i = 0; i < meshSegments; i++)
				{
					var vertexIndexNext = vertexIndex + 2;
					
					// Index data
					indices[indicesIndex + 0] = vertexIndex     + 0;
					indices[indicesIndex + 1] = vertexIndex     + 1;
					indices[indicesIndex + 2] = vertexIndexNext + 1;
					indices[indicesIndex + 3] = vertexIndex     + 0;
					indices[indicesIndex + 4] = vertexIndexNext + 1;
					indices[indicesIndex + 5] = vertexIndexNext + 0;
					
					indicesIndex += 6;
					vertexIndex  += 2;
				}
				
				vertexIndex += 2;
			}
		}
		
		// Write vertex data
		{
			var vertexIndex = 0;
			
			for (var r = 0; r < rotations.Length; r++)
			{
				var rot = rotations[r];
				
				for (var i = 0; i < meshSegments + 1; i++)
				{
					// Calculate stuff
					var angle     = angleStep * i;
					var direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0.0f);
					
					// Write vertex data
					positions[vertexIndex + 0] = rot * (direction * meshRadius     );
					positions[vertexIndex + 1] = rot * (direction * meshRadiusOuter);
					
					uv0[vertexIndex + 0] = new Vector2(vStep * i, 0.0f);
					uv0[vertexIndex + 1] = new Vector2(vStep * i, 1.0f);
					
					vertexIndex += 2;
				}
			}
		}
		
		generatedMesh = new Mesh();
		generatedMesh.vertices  = positions;
		generatedMesh.triangles = indices;
		generatedMesh.uv        = uv0;
		generatedMesh.RecalculateBounds();
		generatedMesh.RecalculateNormals();
	}
}