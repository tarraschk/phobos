using UnityEngine;
using UnityEditor;

public static partial class SGT_EditorGUI
{
	public static SGT_ColourGradient Field(string handle, string tooltip, SGT_ColourGradient field, bool isField = true)
	{
		if (CanDraw == true && field != null)
		{
			var borderRect   = ReserveField(handle, tooltip);
			var gradientRect = SGT_RectHelper.RemovePx(borderRect, 3.0f);
			var borderStyle  = EditorStyles.objectFieldThumb;
			var overlayStyle = (GUIStyle)(EditorStyles.objectFieldThumb.name + "Overlay2");
			
			gradientRect.xMax -= 4.0f;
			
			var colours = field.CalculateColours(0.0f, 1.0f, 256);
			var texture = SGT_ColourGradient.AllocateTexture(256);
			
			for (var x = 0; x < 256; x++)
			{
				texture.SetPixel(x, 0, colours[x]);
			}
			
			texture.Apply();
			
			GUI.Box(borderRect, string.Empty, borderStyle);
			DrawTiledTexture(gradientRect, SGT_Helper.CheckerTexture);
			GUI.DrawTexture(gradientRect, texture);
			
			SGT_Helper.DestroyObject(texture);
			
			if (GUI.Button(borderRect, "Edit", overlayStyle) == true)
			{
				SGT_AuxWindow_ColourGradient.Create(field, isField);
			}
		}
		
		return field;
	}
}