using UnityEngine;
using UnityEditor;

public static partial class SGT_EditorGUI
{
	public static SGT_SurfaceMultiMesh SurfaceMultiMeshField(string handle, string tooltip, SGT_SurfaceMultiMesh field, bool required = false)
	{
		if (CanDraw == true)
		{
			switch (field.Configuration)
			{
				case SGT_SurfaceConfiguration.Sphere:
				{
					MultiMeshField(handle, tooltip, field.GetMultiMesh(0), required);
				}
				break;
				case SGT_SurfaceConfiguration.Cube:
				{
					var rectP = ReserveField(handle, tooltip);
					var rectN = ReserveField();
					
					var pX = SGT_RectHelper.HorizontalSlice(rectP, 0.0f / 3.0f, 1.0f / 3.0f);
					var pY = SGT_RectHelper.HorizontalSlice(rectP, 1.0f / 3.0f, 2.0f / 3.0f);
					var pZ = SGT_RectHelper.HorizontalSlice(rectP, 2.0f / 3.0f, 3.0f / 3.0f);
					
					var nX = SGT_RectHelper.HorizontalSlice(rectN, 0.0f / 3.0f, 1.0f / 3.0f);
					var nY = SGT_RectHelper.HorizontalSlice(rectN, 1.0f / 3.0f, 2.0f / 3.0f);
					var nZ = SGT_RectHelper.HorizontalSlice(rectN, 2.0f / 3.0f, 3.0f / 3.0f);
					
					MultiMeshFieldWithLabel(pX, "X+", 25, field.GetMultiMesh(CubemapFace.PositiveX), required);
					MultiMeshFieldWithLabel(pY, "Y+", 25, field.GetMultiMesh(CubemapFace.PositiveY), required);
					MultiMeshFieldWithLabel(pZ, "Z+", 25, field.GetMultiMesh(CubemapFace.PositiveZ), required);
					
					MultiMeshFieldWithLabel(nX, "X-", 25, field.GetMultiMesh(CubemapFace.NegativeX), required);
					MultiMeshFieldWithLabel(nY, "Y-", 25, field.GetMultiMesh(CubemapFace.NegativeY), required);
					MultiMeshFieldWithLabel(nZ, "Z-", 25, field.GetMultiMesh(CubemapFace.NegativeZ), required);
				}
				break;
			}
		}
		
		return field;
	}
	
	private static SGT_MultiMesh MultiMeshFieldWithLabel(Rect rect, string label, int labelWidth, SGT_MultiMesh field, bool required = false)
	{
		if (CanDraw == true && field != null)
		{
			if (required == true)
			{
				if (field.ContainsSomething == false)
				{
					var redRect = new Rect(rect);
					redRect = SGT_RectHelper.ExpandPx(redRect, 1.0f, 1.0f, 1.0f, 1.0f);
					GUI.DrawTexture(redRect, SGT_Helper.RedTexture);
				}
			}
			
			var labelRect = SGT_RectHelper.GetLeftPx(ref rect, labelWidth);
			
			EditorGUI.LabelField(labelRect, new GUIContent(label, string.Empty), EditorStyles.label);
			
			var curMesh = field.GetSharedMesh(0);
			var newMesh = (Mesh)EditorGUI.ObjectField(rect, curMesh, typeof(Mesh), false);
			
			if (curMesh != newMesh)
			{
				field.ReplaceAll(newMesh);
			}
		}
		
		return field;
	}
}