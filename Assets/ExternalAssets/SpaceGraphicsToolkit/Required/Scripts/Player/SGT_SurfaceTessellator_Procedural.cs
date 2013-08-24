using MeshList  = System.Collections.Generic.List<UnityEngine.Mesh>;
using PatchList = System.Collections.Generic.List<SGT_SurfaceTessellator.Patch>;

using UnityEngine;
using SGT_Internal;

public partial class SGT_SurfaceTessellator
{
	private System.Collections.IEnumerator Update_Coroutine()
	{
		if (patchIndices == null)
		{
			RebuildPatchIndices();
		}
		
		sideCombinedMeshes = new Mesh[6][];
		
		// Main loop
		for (;;)
		{
			if (sides != null)
			{
				var sgtVector3 = new SGT_FillVector3();
				SendMessage("FillSurfaceObserverPosition", sgtVector3, SendMessageOptions.DontRequireReceiver);
				surfaceObserverPosition = sgtVector3.Vector3;
				
				var splitList = new PatchList();
				
				// Begin budgeting
				budgetUsage.Reset();
				budgetUsage.Start();
				
				// Update all patches
				for (var i = 0; i < sides.Length; i++)
				{
					UpdatePatch(sides[i], surfaceObserverPosition, splitList);
					
					/* Defer operation? */ if (OverBudgetCheck("Update Patches") == true){yield return new WaitForEndOfFrame(); budgetUsage.Reset(); budgetUsage.Start();}
				}
				
				// Split patches?
				if (splitList.Count > 0)
				{
					var splitCount = 0;
					
					for (var i = 0; i < splitList.Count; i++)
					{
						splitCount += 1;
						
						SplitPatch(splitList[i]);
						
						if (splitCount == maxSplitsPerFrame)
						{
							splitCount = 0;
							
							/* Defer operation? */ if (OverBudgetCheck("Split Patches") == true){yield return new WaitForEndOfFrame(); budgetUsage.Reset(); budgetUsage.Start();}
						}
					}
				}
					
				// Rebuild mesh?
				if (rebuild == true)
				{
					rebuild = false;
					
					var totalSurfaces         = SGT_SurfaceConfiguration_.SurfaceCount(surfaceConfiguration);
					var newSideCombinedMeshes = new Mesh[6][];
					
					for (var surfaceIndex = 0; surfaceIndex < totalSurfaces; surfaceIndex++)
					{
						var buildList = new PatchList();
						
						// Begin budgeting
						budgetUsage.Reset();
						budgetUsage.Start();
						
						switch (surfaceConfiguration)
						{
							case SGT_SurfaceConfiguration.Sphere:
							{
								for (var i = 0; i < 6; i++)
								{
									BuildPatch(sides[i], buildList);
									
									/* Defer operation? */ if (OverBudgetCheck("Build Patches") == true){yield return new WaitForEndOfFrame(); budgetUsage.Reset(); budgetUsage.Start();}
								}
							}
							break;
							case SGT_SurfaceConfiguration.Cube:
							{
								BuildPatch(sides[surfaceIndex], buildList);
								
								/* Defer operation? */ if (OverBudgetCheck("Build Patches") == true){yield return new WaitForEndOfFrame(); budgetUsage.Reset(); budgetUsage.Start();}
							}
							break;
						}
						
						// Stitch patches
						var stitchCount = 0;
						
						for (var i = 0; i < buildList.Count; i++)
						{
							stitchCount += 1;
							
							StitchPatch(buildList[i]);
							
							if (stitchCount == maxStitchesPerFrame)
							{
								stitchCount = 0;
								
								/* Defer operation? */ if (OverBudgetCheck("Sitch Patches") == true){yield return new WaitForEndOfFrame(); budgetUsage.Reset(); budgetUsage.Start();}
							}
						}
						
						var combinedMeshesList = new MeshList();
						var combineFrom        = 0;
						var patchesPerMesh     = verticesPerMesh / ((patchResolution + 1) * (patchResolution + 1));
						
						// Create sub meshes
						for (var i = 0; i < buildList.Count; i++)
						{
							var combineCount = (i + 1) - combineFrom;
							
							if (combineCount == patchesPerMesh || i == (buildList.Count - 1))
							{
								var combinedMesh = CombinePatches(buildList, combineFrom, combineFrom + combineCount);
								
								combinedMeshesList.Add(combinedMesh);
								
								combineFrom = i + 1;
								
								/* Defer operation? */ if (OverBudgetCheck("Combine Patches") == true){yield return new WaitForEndOfFrame(); budgetUsage.Reset(); budgetUsage.Start();}
							}
						}
						
						var combinedMeshes = combinedMeshesList.ToArray();
						
						// Append index to name?
						if (combinedMeshes.Length > 1)
						{
							for (var i = 0; i < combinedMeshes.Length; i++)
							{
								combinedMeshes[i].name = "(" + (i + 1) + "/" + combinedMeshes.Length + ") " + combinedMeshes[i].name;
							}
						}
						
						newSideCombinedMeshes[surfaceIndex] = combinedMeshes;
					}
					
					// Delete old meshes and swap
					for (var i = 0; i < 6; i++)
					{
						SGT_Helper.DestroyObjects(sideCombinedMeshes[i]);
					}
					
					sideCombinedMeshes = newSideCombinedMeshes;
					
					var generatedSurfaceMultiMesh = new SGT_SurfaceMultiMesh();
					
					generatedSurfaceMultiMesh.Configuration = surfaceConfiguration;
					
					for (var i = 0; i < totalSurfaces; i++)
					{
						generatedSurfaceMultiMesh.ReplaceAll(sideCombinedMeshes[i], i);
					}
					
					SendMessage("SetSurfaceMultiMesh", generatedSurfaceMultiMesh, SendMessageOptions.DontRequireReceiver);
					
					SendMessage("TessellationFinished", this, SendMessageOptions.DontRequireReceiver);
				}
			}
			
			if (minUpdateInterval > 0.0f)
			{
				yield return new WaitForSeconds(minUpdateInterval);
			}
			else
			{
				yield return new WaitForEndOfFrame();
			}
		}
	}
	
	private Patch GeneratePatch(Patch parent, CubemapFace face, int level, int quadrant, Vector2 cornerLocal, Vector3 corner, Vector3 axis1, Vector3 axis2)
	{
		var patchResolution2 = patchResolution + 1;
		var positions        = new Vector3[patchResolution2 * patchResolution2];
		var uv0s             = new Vector2[patchResolution2 * patchResolution2];
		var uv1s             = new Vector2[patchResolution2 * patchResolution2];
		var normals          = new Vector3[patchResolution2 * patchResolution2];
		var tangents         = new Vector4[patchResolution2 * patchResolution2];
		
		var axis1Step = axis1 / patchResolution;
		var axis2Step = axis2 / patchResolution;
		var uvSCorner = (cornerLocal * 0.5f + new Vector2(0.5f, 0.5f));
		var uv1Corner = uvSCorner;
		var uvSStep   = ((levelSize[level] * 0.5f) / patchResolution);
		var uv1Step   = uvSStep;
		
		for (var z = 0; z < patchResolution2; z++)
		{
			for (var x = 0; x < patchResolution2; x++)
			{
				var vertexIndex = z * patchResolution2 + x;
				
				// Calculate stuff
				var position0 = (corner + axis1Step *  x      + axis2Step *  z     ).normalized;
				var position1 = (corner + axis1Step * (x + 1) + axis2Step *  z     ).normalized;
				var position2 = (corner + axis1Step *  x      + axis2Step * (z + 1)).normalized;
				var position  = position0;
				
				var displacement0   = 0.0f;
				var displacement1   = 0.0f;
				var displacement2   = 0.0f;
				var displacementUV0 = Vector2.zero;
				
				switch (DisplacementConfiguration)
				{
					case SGT_SurfaceConfiguration.Sphere:
					{
						var texture = displacementTexture.GetTexture2D(0);
						
						displacementUV0 = SGT_Helper.CartesianToPolarUV(position0);
						
						if (texture != null)
						{
							var pixelUV         = SGT_Helper.PixelUV(texture);
							var displacementUV1 = SGT_Helper.CartesianToPolarUV(position1);
							var displacementUV2 = SGT_Helper.CartesianToPolarUV(position2);
							
							displacementUV0.y = SGT_Helper.ClampUV(displacementUV0.y, pixelUV.y);
							displacementUV1.y = SGT_Helper.ClampUV(displacementUV1.y, pixelUV.y);
							displacementUV2.y = SGT_Helper.ClampUV(displacementUV2.y, pixelUV.y);
							
							displacement0 = texture.GetPixelBilinear(displacementUV0.x, displacementUV0.y).r;
							displacement1 = texture.GetPixelBilinear(displacementUV1.x, displacementUV1.y).r;
							displacement2 = texture.GetPixelBilinear(displacementUV2.x, displacementUV2.y).r;
						}
					}
					break;
					case SGT_SurfaceConfiguration.Cube:
					{
						var face0    = SGT_Helper.CubeFace(position0);
						var face1    = SGT_Helper.CubeFace(position1);
						var face2    = SGT_Helper.CubeFace(position2);
						var texture0 = displacementTexture.GetTexture2D(face0);
						var texture1 = displacementTexture.GetTexture2D(face1);
						var texture2 = displacementTexture.GetTexture2D(face2);
						
						displacementUV0 = SGT_Helper.CubeUV(face0, position0, true);
						
						if (texture0 != null)
						{
							displacement0 = texture0.GetPixelBilinear(displacementUV0.x, displacementUV0.y).r;
						}
						
						if (texture1 != null)
						{
							var displacementUV1 = SGT_Helper.CubeUV(face1, position1, true);
							
							displacement1 = texture1.GetPixelBilinear(displacementUV1.x, displacementUV1.y).r;
						}
						
						if (texture2 != null)
						{
							var displacementUV2 = SGT_Helper.CubeUV(face2, position2, true);
							
							displacement2 = texture2.GetPixelBilinear(displacementUV2.x, displacementUV2.y).r;
						}
					}
					break;
				}
				
				position0 *= Mathf.Lerp(scaleMin, scaleMax, displacement0);
				position1 *= Mathf.Lerp(scaleMin, scaleMax, displacement1);
				position2 *= Mathf.Lerp(scaleMin, scaleMax, displacement2);
				
				var vec1 = (position1 - position0).normalized;
				var vec2 = (position2 - position0).normalized;
				
				// Write vertex data
				positions[vertexIndex] = position0;
				normals[vertexIndex]   = Vector3.Cross(vec2, vec1);
				tangents[vertexIndex]  = SGT_Helper.NewVector4(-vec2, 1.0f);
				
				if (DisplacementConfiguration == surfaceConfiguration)
				{
					switch (surfaceConfiguration)
					{
						case SGT_SurfaceConfiguration.Sphere: uv0s[vertexIndex] = displacementUV0;                         break;
						case SGT_SurfaceConfiguration.Cube:   uv0s[vertexIndex] = SGT_Helper.CubeUV(face, position, true); break;
					}
				}
				else
				{
					switch (surfaceConfiguration)
					{
						case SGT_SurfaceConfiguration.Sphere: uv0s[vertexIndex] = SGT_Helper.CartesianToPolarUV(position0); break;
						case SGT_SurfaceConfiguration.Cube:   uv0s[vertexIndex] = SGT_Helper.CubeUV(face, position0, true); break;
					}
				}
				
				uv1s[vertexIndex] = uv1Corner + new Vector2(uv1Step * x, uv1Step * z);
			}
		}
		
		// Remove wrapping seam
		if (surfaceConfiguration == SGT_SurfaceConfiguration.Sphere)
		{
			var indices = patchIndices[0];
			
			for (var i = 0; i < indices.Length; i += 3)
			{
				var index1 = indices[i + 0];
				var index2 = indices[i + 1];
				var index3 = indices[i + 2];
				var mid    = (positions[index1] + positions[index2] + positions[index3]) / 3.0f;
				
				if (mid.x < 0.0f && mid.z < 0.0f)
				{
					for (var j = 0; j < 3; j++)
					{
						var index = indices[i + j];
						var pos   = positions[index];
						
						if (pos.z < 0.0f && Mathf.Approximately(pos.x, 0.0f) == true)
						{
							uv0s[index].x = 1.0f;
						}
					}
				}
			}
		}
		
		var bounds = new Bounds(positions[0], Vector3.zero);
		
		for (var i = 1; i < patchResolution2 * patchResolution2; i++)
		{
			bounds.Encapsulate(positions[i]);
		}
		
		var patch = new Patch();
		
		patch.parent      = parent;
		patch.face        = face;
		patch.level       = level;
		patch.quadrant    = quadrant;
		patch.cornerLocal = cornerLocal;
		patch.corner      = corner;
		patch.axis1       = axis1;
		patch.axis2       = axis2;
		patch.positions   = positions;
		patch.uv0s        = uv0s;
		patch.uv1s        = uv1s;
		patch.normals     = normals;
		patch.tangents    = tangents;
		patch.bounds      = bounds;
		
		return patch;
	}
}