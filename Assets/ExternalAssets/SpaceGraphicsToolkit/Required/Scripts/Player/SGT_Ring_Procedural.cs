using UnityEngine;

public partial class SGT_Ring
{
	public void Regenerate()
	{
		if (modified == false)
		{
			CheckForModifications();
		}
		
		if (modified == true)
		{
			modified           = false;
			generatedRotations = null;
			
			SGT_Helper.DestroyObject(generatedMesh);
			
			if (sliced == true)
			{
				GenerateSliced();
			}
			else
			{
				GenerateStretched();
			}
			
			if (ringMesh != null)
			{
				ringMesh.SharedMesh = generatedMesh;
				UpdateRingRotations();
			}
		}
	}
	
	private void GenerateSliced()
	{
		var positions       = new Vector3[ringSegments * 2 + 2];
		var indices         = new int[ringSegments * 6];
		var uv0             = new Vector2[ringSegments * 2 + 2];
		var normals         = new Vector3[ringSegments * 2 + 2];
		var vStep           = (float)ringTextureRepeat / ringSegments;
		var angleStep       = ((Mathf.PI * 2.0f) / ringSlices) / ringSegments;
		var ringRadiusInner = RingRadiusInner;
		var ringRadiusOuter = RingRadiusOuter * SGT_Helper.PolygonOuterBoundScale(angleStep);
		
		for (var i = 0; i < ringSegments; i++)
		{
			var indicesIndex    = i * 6;
			var vertexIndex     = i * 2;
			var vertexIndexNext = i * 2 + 2;
			
			// Index data
			indices[indicesIndex + 0] = vertexIndex     + 0;
			indices[indicesIndex + 1] = vertexIndex     + 1;
			indices[indicesIndex + 2] = vertexIndexNext + 1;
			indices[indicesIndex + 3] = vertexIndex     + 0;
			indices[indicesIndex + 4] = vertexIndexNext + 1;
			indices[indicesIndex + 5] = vertexIndexNext + 0;
		}
		
		for (var i = 0; i < ringSegments + 1; i++)
		{
			var vertexIndex = i * 2;
			
			// Calculate stuff
			var angle     = angleStep * (float)i;
			var direction = new Vector3(Mathf.Sin(angle), 0.0f, Mathf.Cos(angle));
			
			// Write vertex data
			positions[vertexIndex + 0] = direction * ringRadiusInner;
			positions[vertexIndex + 1] = direction * ringRadiusOuter;
			
			normals[vertexIndex + 0] = Vector3.up;
			normals[vertexIndex + 1] = Vector3.up;
			
			uv0[vertexIndex + 0] = new Vector2(0.0f, vStep * i);
			uv0[vertexIndex + 1] = new Vector2(1.0f, vStep * i);
		}
		
		generatedMesh = new Mesh();
		generatedMesh.name      = "Sliced Ring Mesh";
		generatedMesh.vertices  = positions;
		generatedMesh.triangles = indices;
		generatedMesh.normals   = normals;
		generatedMesh.uv        = uv0;
		generatedMesh.RecalculateBounds();
		
		if (ringSlices > 1)
		{
			generatedRotations = new Quaternion[ringSlices];
			
			var sliceAngleStep = 360.0f / (float)ringSlices;
			
			for (var i = 0; i < ringSlices; i++)
			{
				var angle = sliceAngleStep * (float)i;
				
				generatedRotations[i] = Quaternion.Euler(0.0f, angle, 0.0f);
			}
		}
	}
	
	private void GenerateStretched()
	{
		var positions       = new Vector3[4];
		var indices         = new int[6];
		var uv0             = new Vector2[4];
		var normals         = new Vector3[4];
		var ringRadiusOuter = RingRadiusOuter;
		var bounds          = new Bounds(Vector3.zero, new Vector3(ringRadiusOuter, 0.0f, ringRadiusOuter) * 2.0f);
		
		// Index data
		indices[0] = 0;
		indices[1] = 1;
		indices[2] = 2;
		indices[3] = 0;
		indices[4] = 2;
		indices[5] = 3;
		
		// Write vertex data
		positions[0] = new Vector3(-ringRadiusOuter, 0.0f, -ringRadiusOuter);
		positions[1] = new Vector3(-ringRadiusOuter, 0.0f,  ringRadiusOuter);
		positions[2] = new Vector3( ringRadiusOuter, 0.0f,  ringRadiusOuter);
		positions[3] = new Vector3( ringRadiusOuter, 0.0f, -ringRadiusOuter);
		
		normals[0] = Vector3.up;
		normals[1] = Vector3.up;
		normals[2] = Vector3.up;
		normals[3] = Vector3.up;
		
		uv0[0] = new Vector2(0.0f, 1.0f);
		uv0[1] = new Vector2(0.0f, 0.0f);
		uv0[2] = new Vector2(1.0f, 0.0f);
		uv0[3] = new Vector2(1.0f, 1.0f);
		
		generatedMesh = new Mesh();
		generatedMesh.name      = "Stretched Ring Mesh";
		generatedMesh.vertices  = positions;
		generatedMesh.triangles = indices;
		generatedMesh.normals   = normals;
		generatedMesh.uv        = uv0;
		generatedMesh.bounds    = bounds;
	}
}