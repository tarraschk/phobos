using UnityEngine;
using UnityEditor;

public static partial class SGT_EditorGUI
{
	public static SGT_Mesh MeshField(string handle, string tooltip, SGT_Mesh field, bool required = false)
	{
		if (CanDraw == true && field != null)
		{
			var currentMesh = field.SharedMesh;
			
			if (required == true)
			{
				MarkNextFieldAsError(currentMesh == null);
			}
			
			var rect    = ReserveField(handle, tooltip);
			var newMesh = (Mesh)EditorGUI.ObjectField(rect, currentMesh, typeof(Mesh), false);
			
			if (currentMesh != newMesh)
			{
				field.SharedMesh = newMesh;
			}
		}
		
		return field;
	}
}