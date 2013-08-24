using CombineInstanceList = System.Collections.Generic.List<UnityEngine.CombineInstance>;
using PatchList           = System.Collections.Generic.List<SGT_SurfaceTessellator.Patch>;

using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Surface Tessellator")]
public partial class SGT_SurfaceTessellator : SGT_MonoBehaviourUnique<SGT_SurfaceTessellator>
{
	public float RadiusScaleAtPoint(Vector3 xyz)
	{
		if (surface == null)
		{
			var fill = new SGT_Internal.SGT_FillGameObject();
			
			SendMessage("FillSurfaceGameObject", fill, SendMessageOptions.DontRequireReceiver);
			
			surface = fill.GameObject;
		}
		
		if (surface != null)
		{
			switch (DisplacementConfiguration)
			{
				case SGT_SurfaceConfiguration.Sphere:
				{
					var texture = displacementTexture.GetTexture2D(0);
					
					if (texture != null)
					{
						xyz = surface.transform.InverseTransformPoint(xyz);
						
						var pixelUV      = SGT_Helper.PixelUV(texture);
						var uv           = SGT_Helper.CartesianToPolarUV(xyz); uv.y = SGT_Helper.ClampUV(uv.y, pixelUV.y);
						var displacement = texture.GetPixelBilinear(uv.x, uv.y).r;
						var scale        = Mathf.Lerp(scaleMin, scaleMax, displacement);
						
						return scale;
					}
				}
				break;
				case SGT_SurfaceConfiguration.Cube:
				{
					xyz = surface.transform.InverseTransformPoint(xyz);
					
					var face    = SGT_Helper.CubeFace(xyz);
					var texture = displacementTexture.GetTexture2D(face);
					
					if (texture != null)
					{
						var uv           = SGT_Helper.CubeUV(face, xyz, true);
						var displacement = texture.GetPixelBilinear(uv.x, uv.y).r;
						var scale        = Mathf.Lerp(scaleMin, scaleMax, displacement);
						
						return scale;
					}
				}
				break;
			}
		}
		
		return 1.0f;
	}
	
	public Vector3[] BuildVertices(int maxLevel)
	{
		var buildList = new PatchList();
		
		for (var i = 0; i < sides.Length; i++)
		{
			BuildPatch(sides[i], buildList, maxLevel);
		}
		
		return CombineVertices(buildList);
	}
	
	private void BeginAgain()
	{
		StopAllCoroutines();
		
		running = false;
	}
	
	private Vector3[] CombineVertices(PatchList patchList)
	{
		var indicesSize = patchResolution * patchResolution * 6;
		var positions   = new Vector3[patchList.Count * indicesSize];
		
		for (var i = 0; i < patchList.Count; i++)
		{
			var patch        = patchList[i];
			var indicesIndex = i * indicesSize;
			
			var srcIndices   = patchIndices[patch.indicesIndex];
			var srcPositions = patch.positions;
			
			for (var j = 0; j < indicesSize; j++)
			{
				positions[indicesIndex + j] = srcPositions[srcIndices[j]];
			}
		}
		
		return positions;
	}
	
	private Mesh CombinePatches(PatchList patchList, int patchFrom, int patchTo)
	{
		var patchCount   = patchTo - patchFrom;
		var verticesSize = (patchResolution + 1) * (patchResolution + 1);
		var indicesSize  = patchResolution * patchResolution * 6;
		
		var positions = new Vector3[patchCount * verticesSize];
		var uv0s      = new Vector2[patchCount * verticesSize];
		var uv1s      = new Vector2[patchCount * verticesSize];
		var normals   = new Vector3[patchCount * verticesSize];
		var tangents  = new Vector4[patchCount * verticesSize];
		var indices   = new int[patchCount * indicesSize];
		
		for (var i = 0; i < patchCount; i++)
		{
			var patch        = patchList[patchFrom + i];
			var vertexIndex  = i * verticesSize;
			var indicesIndex = i * indicesSize;
			
			patch.positions.CopyTo(positions, vertexIndex);
			patch.uv0s.CopyTo(uv0s, vertexIndex);
			patch.uv1s.CopyTo(uv1s, vertexIndex);
			patch.normals.CopyTo(normals, vertexIndex);
			patch.tangents.CopyTo(tangents, vertexIndex);
			
			var srcIndices = patchIndices[patch.indicesIndex];
			
			for (var j = 0; j < indicesSize; j++)
			{
				var index = indicesIndex + j;
				
				indices[index] = srcIndices[j] + vertexIndex;
			}
		}
		
		var bounds = patchList[patchFrom].bounds;
		
		for (var i = patchFrom + 1; i < patchTo; i++)
		{
			bounds.Encapsulate(patchList[i].bounds);
		}
		
		var combinedMesh = new Mesh();
		
		combinedMesh.name      = "Tessellator Generated";
		combinedMesh.vertices  = positions;
		combinedMesh.uv        = uv0s;
		combinedMesh.uv1       = uv1s;
		combinedMesh.normals   = normals;
		combinedMesh.tangents  = tangents;
		combinedMesh.triangles = indices;
		combinedMesh.bounds    = bounds;
		
		return combinedMesh;
	}
	
	private bool OverBudgetCheck(string operation)
	{
		if (taskBudget > 0.0f)
		{
			var usage = (float)budgetUsage.Elapsed.TotalSeconds;
			
			if (usage <= taskBudget)
			{
				return false;
			}
			
			// Way over budget?
			if (reportBudget == true)
			{
				if (usage > taskBudget * 2.0f)
				{
					Debug.Log("The " + operation + " task is over budget by " + (usage / taskBudget).ToString("0.00") + "x");
				}
			}
			
			budgetUsage.Reset();
			budgetUsage.Start();
		}
		
		return true;
	}
	
	private void RebuildPatchIndices()
	{
		patchIndices = new int[16][];
		
		var patchResolution2 = patchResolution + 1;
		
		var baseIndices = new int[patchResolution * patchResolution * 6];
		
		for (var z = 0; z < patchResolution; z++)
		{
			for (var x = 0; x < patchResolution; x++)
			{
				var indicesIndex    = (z * patchResolution + x) * 6;
				var vertexIndex     = z * patchResolution2 + x;
				var vertexIndexNext = vertexIndex + patchResolution2;
				
				// Index data
				baseIndices[indicesIndex + 0] = vertexIndex     + 0;
				baseIndices[indicesIndex + 1] = vertexIndexNext + 0;
				baseIndices[indicesIndex + 2] = vertexIndexNext + 1;
				baseIndices[indicesIndex + 3] = vertexIndex     + 1;
				baseIndices[indicesIndex + 4] = vertexIndex     + 0;
				baseIndices[indicesIndex + 5] = vertexIndexNext + 1;
			}
		}
		
		for (var u = 0; u < 2; u++)
		{
			for (var d = 0; d < 2; d++)
			{
				for (var r = 0; r < 2; r++)
				{
					for (var l = 0; l < 2; l++)
					{
						var indices = new int[patchResolution * patchResolution * 6];
						
						System.Array.Copy(baseIndices, indices, indices.Length);
						
						if (d == 1)
						{
							for (var i = 0; i < patchResolution * 6; i++)
							{
								var mod = i % 12;
								if (mod == 3 || mod == 6 || mod == 10)
								{
									indices[i] -= 1;
								}
							}
						}
						
						if (u == 1)
						{
							for (var i = 0; i < patchResolution * 6; i++)
							{
								var mod = i % 12;
								if (mod == 2 || mod == 5 || mod == 7)
								{
									indices[i + patchResolution * (patchResolution - 1) * 6] -= 1;
								}
							}
						}
						
						if (l == 1)
						{
							for (var i = 0; i < patchResolution * patchResolution * 6; i++)
							{
								var mod = i % (patchResolution * 12);
								if (mod == patchResolution * 6 + 0 || mod == patchResolution * 6 + 4 || mod == 1)
								{
									indices[i] += patchResolution2;
								}
							}
						}
						
						if (r == 1)
						{
							for (var i = 0; i < patchResolution * patchResolution * 6; i++)
							{
								var mod = i % (patchResolution * 12);
								if (mod == patchResolution * 6 - 1 || mod == patchResolution * 6 - 4 || mod == patchResolution * 12 - 3)
								{
									indices[i] -= patchResolution2;
								}
							}
						}
						
						patchIndices[u + d * 2 + r * 4 + l * 8] = indices;
					}
				}
			}
		}
	}
	
	public float GetPatchLodDistance(int index)
	{
		return levelDistance[index];
	}
	
	public void SetPatchLodDistance(int index, float distance)
	{
		if (distance != levelDistance[index])
		{
			levelDistance[index] = distance;
		}
	}
	
	private void RebuildLUT()
	{
		levelSize     = new float[maxLevels];
		levelStep     = new float[maxLevels];
		levelDistance = new float[maxLevels];
		
		levelSize[0]     = 2.0f;
		levelStep[0]     = 2.0f / patchResolution;
		levelDistance[0] = 10.0f;
		
		for (var i = 1; i < maxLevels; i++)
		{
			levelSize[i]     = levelSize[i - 1] * 0.5f;
			levelStep[i]     = levelStep[i - 1] * 0.5f;
			levelDistance[i] = levelDistance[i - 1] * 0.5f;
		}
		
		RebuildPatchIndices();
		
		rebuild = true;
	}
	
	private void DestroyPatches()
	{
		sides   = null;
		rebuild = true;
	}
	
	private void RebuildPatches()
	{
		DestroyPatches();
		
		sides = new Patch[6];
		sides[0] = GeneratePatch(null, CubemapFace.PositiveX, 0, 0, new Vector2(-1.0f, -1.0f), new Vector3( 1.0f,  1.0f, -1.0f), new Vector3( 0.0f, -2.0f,  0.0f), new Vector3( 0.0f,  0.0f,  2.0f)); // X+
		sides[1] = GeneratePatch(null, CubemapFace.NegativeX, 0, 0, new Vector2(-1.0f, -1.0f), new Vector3(-1.0f, -1.0f, -1.0f), new Vector3( 0.0f,  2.0f,  0.0f), new Vector3( 0.0f,  0.0f,  2.0f)); // X-
		sides[2] = GeneratePatch(null, CubemapFace.PositiveY, 0, 0, new Vector2(-1.0f, -1.0f), new Vector3(-1.0f,  1.0f, -1.0f), new Vector3( 2.0f,  0.0f,  0.0f), new Vector3( 0.0f,  0.0f,  2.0f)); // Y+
		sides[3] = GeneratePatch(null, CubemapFace.NegativeY, 0, 0, new Vector2(-1.0f, -1.0f), new Vector3(-1.0f, -1.0f,  1.0f), new Vector3( 2.0f,  0.0f,  0.0f), new Vector3( 0.0f,  0.0f, -2.0f)); // Y-
		sides[4] = GeneratePatch(null, CubemapFace.PositiveZ, 0, 0, new Vector2(-1.0f, -1.0f), new Vector3(-1.0f,  1.0f,  1.0f), new Vector3( 2.0f,  0.0f,  0.0f), new Vector3( 0.0f, -2.0f,  0.0f)); // Z+
		sides[5] = GeneratePatch(null, CubemapFace.NegativeZ, 0, 0, new Vector2(-1.0f, -1.0f), new Vector3(-1.0f, -1.0f, -1.0f), new Vector3( 2.0f,  0.0f,  0.0f), new Vector3( 0.0f,  2.0f,  0.0f)); // Z-
		
		rebuild = true;
		
		BeginAgain();
	}
	
	private Patch InitialFindPatch(Patch patch, Vector2 local)
	{
		var newLocal = local;
		
		if (patch != null)
		{
			if (local.x >= 1.0f)
			{
				switch (patch.face)
				{
					case CubemapFace.PositiveX: patch = sides[(int)CubemapFace.NegativeY]; newLocal.x = -(local.x - 2.0f); newLocal.y = -local.y; break; // X+ to Y-
					case CubemapFace.NegativeX: patch = sides[(int)CubemapFace.PositiveY]; newLocal.x = local.x - 2.0f; break; // X- to Y+
					case CubemapFace.PositiveY: patch = sides[(int)CubemapFace.PositiveX]; newLocal.x = local.x - 2.0f; break; // Y+ to X+
					case CubemapFace.NegativeY: patch = sides[(int)CubemapFace.PositiveX]; newLocal.x = -(local.x - 2.0f); newLocal.y = -local.y; break; // Y- to X+
					case CubemapFace.PositiveZ: patch = sides[(int)CubemapFace.PositiveX]; newLocal.y = -(local.x - 2.0f); newLocal.x = local.y; break; // Z+ to X+
					case CubemapFace.NegativeZ: patch = sides[(int)CubemapFace.PositiveX]; newLocal.y = local.x - 2.0f; newLocal.x = -local.y; break; // Z- to X+
				}
			}
			else if (local.x < -1.0f)
			{
				switch (patch.face)
				{
					case CubemapFace.PositiveX: patch = sides[(int)CubemapFace.PositiveY]; newLocal.x = local.x + 2.0f; break; // X+ to Y+
					case CubemapFace.NegativeX: patch = sides[(int)CubemapFace.NegativeY]; newLocal.x = -(local.x + 2.0f); newLocal.y = -local.y; break; // X- to Y-
					case CubemapFace.PositiveY: patch = sides[(int)CubemapFace.NegativeX]; newLocal.x = local.x + 2.0f; break; // Y+ to X-
					case CubemapFace.NegativeY: patch = sides[(int)CubemapFace.NegativeX]; newLocal.x = -(local.x + 2.0f); newLocal.y = -local.y; break; // Y- to X+
					case CubemapFace.PositiveZ: patch = sides[(int)CubemapFace.NegativeX]; newLocal.y = local.x + 2.0f; newLocal.x = -local.y; break; // Z+ to X-
					case CubemapFace.NegativeZ: patch = sides[(int)CubemapFace.NegativeX]; newLocal.y = -(local.x + 2.0f); newLocal.x = local.y; break; // Z- to X-
				}
			}
			else if (local.y >= 1.0f)
			{
				switch (patch.face)
				{
					case CubemapFace.PositiveX: patch = sides[(int)CubemapFace.PositiveZ]; newLocal.x = -(local.y - 2.0f); newLocal.y = local.x; break; // X+ to Z+
					case CubemapFace.NegativeX: patch = sides[(int)CubemapFace.PositiveZ]; newLocal.x = local.y - 2.0f; newLocal.y = -local.x; break; // X- to Z+
					case CubemapFace.PositiveY: patch = sides[(int)CubemapFace.PositiveZ]; newLocal.y = local.y - 2.0f; break; // Y+ to Z+
					case CubemapFace.NegativeY: patch = sides[(int)CubemapFace.NegativeZ]; newLocal.y = local.y - 2.0f; break; // Y- to Z-
					case CubemapFace.PositiveZ: patch = sides[(int)CubemapFace.NegativeY]; newLocal.y = local.y - 2.0f; break; // Z+ to Y-
					case CubemapFace.NegativeZ: patch = sides[(int)CubemapFace.PositiveY]; newLocal.y = local.y - 2.0f; break; // Z- to Y+
				}
			}
			else if (local.y < -1.0f)
			{
				switch (patch.face)
				{
					case CubemapFace.PositiveX: patch = sides[(int)CubemapFace.NegativeZ]; newLocal.x = local.y + 2.0f; newLocal.y = -local.x; break; // X+ to Z-
					case CubemapFace.NegativeX: patch = sides[(int)CubemapFace.NegativeZ]; newLocal.x = -(local.y + 2.0f); newLocal.y = local.x; break; // X- to Z-
					case CubemapFace.PositiveY: patch = sides[(int)CubemapFace.NegativeZ]; newLocal.y = local.y + 2.0f; break; // Y+ to Z-
					case CubemapFace.NegativeY: patch = sides[(int)CubemapFace.PositiveZ]; newLocal.y = local.y + 2.0f; break; // Y- to Z+
					case CubemapFace.PositiveZ: patch = sides[(int)CubemapFace.PositiveY]; newLocal.y = local.y + 2.0f; break; // Z+ to Y+
					case CubemapFace.NegativeZ: patch = sides[(int)CubemapFace.NegativeY]; newLocal.y = local.y + 2.0f; break; // Z- to Y-
				}
			}
			else
			{
				patch = patch.parent;
			}
		}
		
		return FindPatch(patch, newLocal);
	}
	
	private Patch FindPatch(Patch patch, Vector2 local)
	{
		if (patch != null)
		{
			if (patch.children != null && patch.children.Length == 4)
			{
				var size     = levelSize[patch.level];
				var size2    = new Vector2(size, size);
				var localMin = patch.cornerLocal;
				var localMax = patch.cornerLocal + size2;
				
				if (local.x >= localMin.x && local.y >= localMin.y && local.x <= localMax.x && local.y <= localMax.y)
				{
					var localMid = localMin + size2 * 0.5f;
					
					return FindPatch(patch.children[(local.x >= localMid.x ? 1 : 0) + (local.y >= localMid.y ? 2 : 0)], local);
				}
				else
				{
					return FindPatch(patch.parent, local);
				}
			}
		}
		
		return patch;
	}
	
	private void SplitPatch(Patch patch)
	{
		patch.children = new Patch[4];
		
		var halfAxis1 = patch.axis1 * 0.5f;
		var halfAxis2 = patch.axis2 * 0.5f;
		var halfLocal1 = new Vector2(levelSize[patch.level] * 0.5f, 0.0f);
		var halfLocal2 = new Vector2(0.0f, levelSize[patch.level] * 0.5f);
		
		patch.children[0] = GeneratePatch(patch, patch.face, patch.level + 1, 0, patch.cornerLocal                          , patch.corner                        , halfAxis1, halfAxis2);
		patch.children[1] = GeneratePatch(patch, patch.face, patch.level + 1, 1, patch.cornerLocal + halfLocal1             , patch.corner + halfAxis1            , halfAxis1, halfAxis2);
		patch.children[2] = GeneratePatch(patch, patch.face, patch.level + 1, 2, patch.cornerLocal + halfLocal2             , patch.corner + halfAxis2            , halfAxis1, halfAxis2);
		patch.children[3] = GeneratePatch(patch, patch.face, patch.level + 1, 3, patch.cornerLocal + halfLocal1 + halfLocal2, patch.corner + halfAxis1 + halfAxis2, halfAxis1, halfAxis2);
		
		rebuild = true;
	}
	
	private void BuildPatch(Patch patch, PatchList buildList)
	{
		if (patch.children != null)
		{
			for (var i = 0; i < patch.children.Length; i++)
			{
				BuildPatch(patch.children[i], buildList);
			}
		}
		else
		{
			buildList.Add(patch);
		}
	}
	
	private void BuildPatch(Patch patch, PatchList buildList, int maxLevel)
	{
		if (patch.children != null && patch.level < maxLevel)
		{
			for (var i = 0; i < patch.children.Length; i++)
			{
				BuildPatch(patch.children[i], buildList, maxLevel);
			}
		}
		else
		{
			buildList.Add(patch);
		}
	}
	
	private void UpdatePatch(Patch patch, Vector3 observerPosition, PatchList splitList)
	{
		var distance = (patch.bounds.center - observerPosition).magnitude;
		var dot      = Vector3.Dot(patch.bounds.center.normalized, (observerPosition - patch.bounds.center).normalized);
		
		dot = Mathf.Clamp01(-dot);
		
		distance *= (1.0f + dot);
		
		var splitDistance = levelDistance[patch.level];
		var mergeDistance = splitDistance * 1.25f;
		
		// Is the observer's distance less than the split distance?
		if (distance < splitDistance)
		{
			// Is this patch less than the deepest level?
			if (patch.level < (maxLevels - 1))
			{
				// Split if it has no children
				if (patch.children == null)
				{
					splitList.Add(patch);
				}
				// Update if it already has some
				else
				{
					for (var i = 0; i < patch.children.Length; i++)
					{
						UpdatePatch(patch.children[i], observerPosition, splitList);
					}
				}
			}
		}
		// Is the observer's distance greater than the merge distance?
		else if (distance > mergeDistance)
		{
			// Merge it?
			if (patch.children != null)
			{
				patch.children = null;
				
				rebuild = true;
			}
		}
	}
	
	private void StitchPatch(Patch patch)
	{
		var indicesIndex = 0;
		
		//if (patch.level > 0)
		{
			var halfSize = levelSize[patch.level] * 0.5f;
			var local    = patch.cornerLocal + new Vector2(halfSize, halfSize);
			var offset   = halfSize * 1.05f;
			var u        = InitialFindPatch(patch, local + new Vector2(0.0f,  offset));
			var d        = InitialFindPatch(patch, local + new Vector2(0.0f, -offset));
			var r        = InitialFindPatch(patch, local + new Vector2( offset, 0.0f));
			var l        = InitialFindPatch(patch, local + new Vector2(-offset, 0.0f));
			
			indicesIndex += (u == null || u.level < patch.level) ? 1 : 0;
			indicesIndex += (d == null || d.level < patch.level) ? 2 : 0;
			indicesIndex += (r == null || r.level < patch.level) ? 4 : 0;
			indicesIndex += (l == null || l.level < patch.level) ? 8 : 0;
		}
		
		patch.indicesIndex = indicesIndex;
	}
}