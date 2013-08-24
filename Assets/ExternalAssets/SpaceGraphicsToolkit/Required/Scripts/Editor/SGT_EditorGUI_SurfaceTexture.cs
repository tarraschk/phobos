using UnityEngine;
using UnityEditor;

public static partial class SGT_EditorGUI
{
	public static SGT_SurfaceTexture Field(string handle, string tooltip, SGT_SurfaceTexture field, bool required = false)
	{
		if (CanDraw == true)
		{
			switch (field.Configuration)
			{
				case SGT_SurfaceConfiguration.Sphere:
				{
					field.SetTexture(ObjectField(handle, tooltip, field.GetTexture(0), required), 0);
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
					
					field.SetTexture(DrawTextureFieldWithLabel(pX, "X+", 25, field.GetTexture(CubemapFace.PositiveX), required), CubemapFace.PositiveX);
					field.SetTexture(DrawTextureFieldWithLabel(pY, "Y+", 25, field.GetTexture(CubemapFace.PositiveY), required), CubemapFace.PositiveY);
					field.SetTexture(DrawTextureFieldWithLabel(pZ, "Z+", 25, field.GetTexture(CubemapFace.PositiveZ), required), CubemapFace.PositiveZ);
					
					field.SetTexture(DrawTextureFieldWithLabel(nX, "X-", 25, field.GetTexture(CubemapFace.NegativeX), required), CubemapFace.NegativeX);
					field.SetTexture(DrawTextureFieldWithLabel(nY, "Y-", 25, field.GetTexture(CubemapFace.NegativeY), required), CubemapFace.NegativeY);
					field.SetTexture(DrawTextureFieldWithLabel(nZ, "Z-", 25, field.GetTexture(CubemapFace.NegativeZ), required), CubemapFace.NegativeZ);
				}
				break;
			}
		}
		
		return field;
	}
}