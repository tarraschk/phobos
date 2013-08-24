using UnityEngine;
using UnityEditor;

public static partial class SGT_EditorGUI
{
	public static SGT_MultiMesh MultiMeshField(string handle, string tooltip, SGT_MultiMesh field, bool required = false, bool isField = true)
	{
		if (CanDraw == true && field != null)
		{
			var curMesh = field.GetSharedMesh(0);
			var newMesh = ObjectField(handle, tooltip, curMesh, required, isField);
			
			if (curMesh != newMesh)
			{
				field.ReplaceAll(newMesh);
			}
		}
		
		return field;
	}
}