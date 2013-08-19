using UnityEngine;

// Quadrant
// 0 - 1
// |   |
// 2 - 3

public partial class SGT_SurfaceTessellator
{
	[System.Serializable]
	public class Patch
	{
		public Patch       parent;
		public int         level;
		public int         quadrant;
		public CubemapFace face;
		public Vector2     cornerLocal;
		public Vector3     corner;
		public Vector3     axis1;
		public Vector3     axis2;
		public Patch[]     children;
		public Vector3[]   positions;
		public Vector2[]   uv0s;
		public Vector2[]   uv1s;
		public Vector3[]   normals;
		public Vector4[]   tangents;
		public int         indicesIndex;
		public Bounds      bounds;
	}
}