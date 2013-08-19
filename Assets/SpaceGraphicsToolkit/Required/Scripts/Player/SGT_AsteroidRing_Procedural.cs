using MeshList = System.Collections.Generic.List<UnityEngine.Mesh>;

using UnityEngine;

public partial class SGT_AsteroidRing
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
			
			SGT_Helper.DestroyObjects(generatedMeshes);
			
			SGT_Helper.BeginRandomSeed(ringSeed);
			{
				var remainingAsteroids = ringAsteroidCount;
				var asteroidsPerMesh   = SGT_Helper.MeshVertexLimit / 4;
				var newMeshes          = new MeshList();
				
				while (remainingAsteroids > 0)
				{
					var asteroidsInMesh = Mathf.Min(remainingAsteroids, asteroidsPerMesh);
					
					newMeshes.Add(GenerateAsteroidMesh(asteroidsInMesh));
					
					remainingAsteroids -= asteroidsInMesh;
				}
				
				generatedMeshes = newMeshes.ToArray();
				
				if (ringMultiMesh != null)
				{
					ringMultiMesh.ReplaceAll(generatedMeshes);
					ringMultiMesh.Update();
				}
			}
			SGT_Helper.EndRandomSeed();
		}
	}
	
	private Mesh GenerateAsteroidMesh(int asteroidCount)
	{
		var indices         = new int[asteroidCount * 6];
		var positions       = new Vector3[asteroidCount * 4];
		var normals         = new Vector3[asteroidCount * 4];
		var colours         = new Color[asteroidCount * 4];
		var uv0s            = new Vector2[asteroidCount * 4];
		var uv1s            = new Vector2[asteroidCount * 4];
		var size            = (ringRadius + ringWidth + asteroidRadiusMax) * 2.0f;
		var bounds          = new Bounds(Vector3.zero, new Vector3(size, asteroidRadiusMax * 2.0f, size));
		var uvStep          = new Vector2(1.0f / (float)asteroidTextureTilesX, 1.0f / (float)asteroidTextureTilesY);
		var ringRadiusInner = RingRadiusInner;
		var ringRadiusOuter = RingRadiusOuter;
		
		for (var i = 0; i < asteroidCount; i++)
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
			
			// Calculate asteroid values
			var radius       = Random.Range(asteroidRadiusMin, asteroidRadiusMax);
			var radius2      = radius / SGT_Helper.InscribedBox;
			var textureCellX = (float)Random.Range(0, asteroidTextureTilesX);
			var textureCellY = (float)Random.Range(0, asteroidTextureTilesY);
			var uvMin        = new Vector2(uvStep.x * textureCellX, uvStep.y * textureCellY);
			var uvMax        = uvStep + uvMin;
			var roll         = Random.Range(-Mathf.PI, Mathf.PI);
			var right        = SGT_Helper.Rotate(Vector2.right * SGT_Helper.InscribedBox, roll);
			var up           = SGT_Helper.Rotate(Vector2.up    * SGT_Helper.InscribedBox, roll);
			var angle        = Random.Range(-Mathf.PI, Mathf.PI);
			var distance01   = GenerateDistance01();
			var distance     = SGT_Helper.Remap(0.0f, 1.0f, distance01, ringRadiusInner, ringRadiusOuter);
			var rps          = SGT_Helper.Clamp(Mathf.Lerp(orbitRateInner, orbitRateOuter, distance01) + Random.Range(-orbitRateDeviation, orbitRateDeviation), orbitRateInner, orbitRateOuter);
			var height       = Random.value;
			var spinRate     = Random.value;
			
			var position = new Vector3(angle, distance, rps);
			var colour   = new Color(height, spinRate, 0.0f);
			var uv1      = new Vector2(radius, radius2);
			
			// Write star values into vertex data
			positions[vertexIndex + 0] = position;
			positions[vertexIndex + 1] = position;
			positions[vertexIndex + 2] = position;
			positions[vertexIndex + 3] = position;
			
			colours[vertexIndex + 0] = colour;
			colours[vertexIndex + 1] = colour;
			colours[vertexIndex + 2] = colour;
			colours[vertexIndex + 3] = colour;
			
			normals[vertexIndex + 0] = SGT_Helper.NewVector3(-right + up, 0.0f);
			normals[vertexIndex + 1] = SGT_Helper.NewVector3( right + up, 0.0f);
			normals[vertexIndex + 2] = SGT_Helper.NewVector3(-right - up, 0.0f);
			normals[vertexIndex + 3] = SGT_Helper.NewVector3( right - up, 0.0f);
			
			uv0s[vertexIndex + 0] = new Vector2(uvMin.x, uvMax.y);
			uv0s[vertexIndex + 1] = new Vector2(uvMax.x, uvMax.y);
			uv0s[vertexIndex + 2] = new Vector2(uvMin.x, uvMin.y);
			uv0s[vertexIndex + 3] = new Vector2(uvMax.x, uvMin.y);
			
			uv1s[vertexIndex + 0] = uv1;
			uv1s[vertexIndex + 1] = uv1;
			uv1s[vertexIndex + 2] = uv1;
			uv1s[vertexIndex + 3] = uv1;
		}
		
		var asteroidMesh = new Mesh();
		
		asteroidMesh.vertices  = positions;
		asteroidMesh.triangles = indices;
		asteroidMesh.normals   = normals;
		asteroidMesh.colors    = colours;
		asteroidMesh.uv        = uv0s;
		asteroidMesh.uv1       = uv1s;
		asteroidMesh.bounds    = bounds;
		
		return asteroidMesh;
	}
	
	private float GenerateDistance01()
	{
		var distance = 0.0f;
		
		switch (ringDistribution)
		{
		case Distribution.Uniform:
			distance = Random.value;
			break;
			
		case Distribution.Exponential2:
			distance = Mathf.Pow(Random.value, 2.0f);
			break;
			
		case Distribution.Bates2:
			distance = (Random.value + Random.value) * 0.5f;
			break;
		}
		
		return distance;
	}
}