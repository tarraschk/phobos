using UnityEngine;
using SGT_Internal;

public partial class SGT_Starfield
{
	/*[SerializeField]*/
	private SGT_MeshData[] meshDatas;
	
	public SGT_StarfieldStarData ReadStar(int starIndex)
	{
		SGT_MeshData meshData;
		int          vertexIndex;
		
		if (FindStar(starIndex, out meshData, out vertexIndex) == true)
		{
			var ssd      = new SGT_StarfieldStarData();
			var position = meshData.Positions[vertexIndex];
			var uv0      = (meshData.Uv0s[vertexIndex] + meshData.Uv0s[vertexIndex + 3]) * 0.5f;
			var uv1      = meshData.Uv1s[vertexIndex];
			var colour   = meshData.Colours[vertexIndex];
			var normal   = meshData.Normals[vertexIndex] + meshData.Normals[vertexIndex + 1];
			
			ssd.Transform         = starfieldGameObject.transform;
			ssd.Position          = position;
			ssd.TextureIndex      = packer.FindOutputIndex(uv0);
			ssd.Angle             = -Mathf.Atan2(normal.x, normal.y);
			ssd.RadiusMin         = uv1.x - uv1.y;
			ssd.RadiusMax         = uv1.x + uv1.y;
			ssd.RadiusPulseRate   = colour.r;
			ssd.RadiusPulseOffset = colour.g;
			
			return ssd;
		}
		
		return null;
	}
	
	public void WriteStar(SGT_StarfieldStarData ssd, int starIndex)
	{
		if (ssd != null)
		{
			var po = packer.GetOutput(ssd.TextureIndex);
			
			if (po != null)
			{
				SGT_MeshData meshData;
				int          vertexIndex;
				
				if (FindStar(starIndex, out meshData, out vertexIndex) == true)
				{
					var v0       = vertexIndex;
					var v1       = v0 + 1;
					var v2       = v1 + 1;
					var v3       = v2 + 1;
					var position = ssd.Position;
					var uv1      = new Vector2((ssd.RadiusMin + ssd.RadiusMax) * 0.5f, (ssd.RadiusMax - ssd.RadiusMin) * 0.5f);
					var colour   = new Color(ssd.RadiusPulseRate, ssd.RadiusPulseOffset, 0.0f, 0.0f);
					var right    = SGT_Helper.Rotate(Vector2.right * SGT_Helper.InscribedBox, ssd.Angle);
					var up       = SGT_Helper.Rotate(Vector2.up    * SGT_Helper.InscribedBox, ssd.Angle);
					
					if (meshData.Positions[v0] != position)
					{
						meshData.Positions[v0] = position;
						meshData.Positions[v1] = position;
						meshData.Positions[v2] = position;
						meshData.Positions[v3] = position;
						
						meshData.Modified = true;
					}
					
					if (meshData.Normals[v0] != SGT_Helper.NewVector3(-right + up, 0.0f))
					{
						meshData.Normals[v0] = SGT_Helper.NewVector3(-right + up, 0.0f);
						meshData.Normals[v1] = SGT_Helper.NewVector3( right + up, 0.0f);
						meshData.Normals[v2] = SGT_Helper.NewVector3(-right - up, 0.0f);
						meshData.Normals[v3] = SGT_Helper.NewVector3( right - up, 0.0f);
						
						meshData.Modified = true;
					}
					
					if (meshData.Uv0s[v0] != po.UvTopLeft && meshData.Uv0s[v3] != po.UvBottomRight)
					{
						meshData.Uv0s[v0] = po.UvTopLeft;
						meshData.Uv0s[v1] = po.UvTopRight;
						meshData.Uv0s[v2] = po.UvBottomLeft;
						meshData.Uv0s[v3] = po.UvBottomRight;
						
						meshData.Modified = true;
					}
					
					if (meshData.Uv1s[v0] != uv1)
					{
						meshData.Uv1s[v0] = uv1;
						meshData.Uv1s[v1] = uv1;
						meshData.Uv1s[v2] = uv1;
						meshData.Uv1s[v3] = uv1;
						
						meshData.Modified = true;
					}
					
					if (meshData.Colours[v0] != colour)
					{
						meshData.Colours[v0] = colour;
						meshData.Colours[v1] = colour;
						meshData.Colours[v2] = colour;
						meshData.Colours[v3] = colour;
						
						meshData.Modified = true;
					}
				}
			}
		}
	}
	
	public int AddStar(SGT_StarfieldStarData ssd)
	{
		var starsPerMesh = SGT_Helper.MeshVertexLimit / 4;
		
		int rem; System.Math.DivRem(starfieldStarCount, starsPerMesh, out rem);
		
		// Add new mesh
		if (rem == 0)
		{
		}
		// Add to mesh
		else
		{
			var meshIndex = meshes.Length - 1;
			var mesh      = meshes[meshIndex];
			var meshData  = GetMeshData(mesh, meshIndex);
			var v0        = meshData.Positions.Length;
			
			meshData.AddPositions(new Vector3[4]);
			meshData.AddNormals(new Vector3[4]);
			meshData.AddColours(new Color[4]);
			meshData.AddUv0s(new Vector2[4]);
			meshData.AddUv1s(new Vector2[4]);
			meshData.AddIndices(new int[6] { v0, v0 + 1, v0 + 2, v0 + 3, v0 + 2, v0 + 1 });
			meshData.Apply();
		}
		
		starfieldStarCount += 1;
		
		return starfieldStarCount - 1;
	}
	
	public void ApplyStarChanges()
	{
		if (meshDatas != null)
		{
			foreach (var meshData in meshDatas)
			{
				if (meshData != null && meshData.Modified == true)
				{
					meshData.Apply();
				}
			}
		}
	}
	
	private bool FindStar(int starIndex, out SGT_MeshData meshData, out int vertexIndex)
	{
		if (starIndex >= 0 && starIndex < starfieldStarCount && meshes != null && meshes.Length > 0)
		{
			var starsPerMesh = SGT_Helper.MeshVertexLimit / 4;
			var meshIndex    = System.Math.DivRem(starIndex, starsPerMesh, out vertexIndex);
			var mesh         = meshes[meshIndex];
			
			if (mesh != null)
			{
				meshData    = GetMeshData(mesh, meshIndex);
				vertexIndex = vertexIndex * 4; // 4 verts per star
				
				return true;
			}
		}
		
		meshData    = null;
		vertexIndex = -1;
		
		return false;
	}
	
	private SGT_MeshData GetMeshData(Mesh mesh, int index)
	{
		if (index >= 0 && mesh != null && meshes != null && index < meshes.Length)
		{
			if (meshDatas == null || meshDatas.Length != meshes.Length)
			{
				meshDatas = new SGT_MeshData[meshes.Length];
			}
			
			var meshData = meshDatas[index];
			
			if (meshData == null || meshData.SharedMesh != mesh)
			{
				meshData = new SGT_MeshData(mesh);
				
				meshDatas[index] = meshData;
			}
			
			return meshData;
		}
		
		return null;
	}
}