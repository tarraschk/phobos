using UnityEngine;
using UnityEditor;

public static partial class SGT_EditorGUI
{
	private static Texture2D shadowCentreTexture;
	private static Texture2D shadowCornerTexture;
	private static Texture2D shadowVerticalTexture;
	private static Texture2D shadowHorizontalTexture;
	
	private static Texture2D ShadowCentreTexture
	{
		get
		{
			if (shadowCentreTexture == null)
			{
				shadowCentreTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
				shadowCentreTexture.hideFlags = HideFlags.HideAndDontSave;
				shadowCentreTexture.SetPixels(new Color[] { new Color(0.0f, 0.0f, 0.0f, 0.1f) });
				shadowCentreTexture.Apply();
			}
			
			return shadowCentreTexture;
		}
	}
	
	private static Texture2D ShadowVerticalTexture
	{
		get
		{
			if (shadowVerticalTexture == null)
			{
				var c = new Color(0.0f, 0.0f, 0.0f, 1.0f);
				
				var array = new Color[]
					{
						c * 0.23f, c * 0.68f, c * 0.38f, c * 0.19f
					};
				
				shadowVerticalTexture = new Texture2D(4, 1, TextureFormat.ARGB32, false);
				shadowVerticalTexture.hideFlags = HideFlags.HideAndDontSave;
				shadowVerticalTexture.SetPixels(array);
				shadowVerticalTexture.Apply();
			}
			
			return shadowVerticalTexture;
		}
	}
	
	private static Texture2D ShadowCornerTexture
	{
		get
		{
			if (shadowCornerTexture == null)
			{
				var c = new Color(0.0f, 0.0f, 0.0f, 1.0f);
				
				var array = new Color[]
					{
						c * 0.06f, c * 0.17f, c * 0.23f, c * 0.23f,
						c * 0.17f, c * 0.90f, c * 0.76f, c * 0.68f,
						c * 0.23f, c * 0.76f, c * 0.52f, c * 0.38f,
						c * 0.23f, c * 0.68f, c * 0.38f, c * 0.19f
					};
				
				shadowCornerTexture = new Texture2D(4, 4, TextureFormat.ARGB32, false);
				shadowCornerTexture.hideFlags = HideFlags.HideAndDontSave;
				shadowCornerTexture.SetPixels(array);
				shadowCornerTexture.Apply();
			}
			
			return shadowCornerTexture;
		}
	}
	
	private static Texture2D ShadowHorizontalTexture
	{
		get
		{
			if (shadowHorizontalTexture == null)
			{
				var c = new Color(0.0f, 0.0f, 0.0f, 1.0f);
				
				var array = new Color[]
					{
						c * 0.23f, c * 0.68f, c * 0.38f, c * 0.19f
					};
				
				shadowHorizontalTexture = new Texture2D(1, 4, TextureFormat.ARGB32, false);
				shadowHorizontalTexture.hideFlags = HideFlags.HideAndDontSave;
				shadowHorizontalTexture.SetPixels(array);
				shadowHorizontalTexture.Apply();
			}
			
			return shadowHorizontalTexture;
		}
	}
	
	private static void DrawShadowBoxTop(Rect r)
	{
		var u  = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
		var r1 = new Rect(r.xMin, r.yMin + 4, 4, -4);
		var r3 = new Rect(r.xMax, r.yMin + 4, -4, -4);
		var r7 = new Rect(r.xMin + 4, r.yMin + 4, r.width - 8, -4);
		
		GUI.DrawTextureWithTexCoords(r1, ShadowCornerTexture, u, true);
		GUI.DrawTextureWithTexCoords(r3, ShadowCornerTexture, u, true);
		GUI.DrawTextureWithTexCoords(r7, ShadowHorizontalTexture, u, true);
	}
	
	private static void DrawShadowBoxCentre(Rect r)
	{
		var u  = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
		
		var r5 = new Rect(r.xMin, r.yMin, 4, r.height);
		var r6 = new Rect(r.xMax, r.yMin, -4, r.height);
		var r9 = new Rect(r.xMin + 4, r.yMin, r.width - 8, r.height);
		
		GUI.DrawTextureWithTexCoords(r5, ShadowVerticalTexture, u, true);
		GUI.DrawTextureWithTexCoords(r6, ShadowVerticalTexture, u, true);
		GUI.DrawTextureWithTexCoords(r9, ShadowCentreTexture, u, true);
	}
	
	private static void DrawShadowBottom(Rect r)
	{
		var u  = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
		
		var r2 = new Rect(r.xMin, r.yMax - 4, 4,  4);
		var r4 = new Rect(r.xMax, r.yMax - 4, -4,  4);
		var r8 = new Rect(r.xMin + 4, r.yMax - 4, r.width - 8, 4);
		
		GUI.DrawTextureWithTexCoords(r2, ShadowCornerTexture, u, true);
		GUI.DrawTextureWithTexCoords(r4, ShadowCornerTexture, u, true);
		GUI.DrawTextureWithTexCoords(r8, ShadowHorizontalTexture, u, true);
		
	}
	
	public static T DrawEditableObject<T>(Rect r, T o, bool isField = false)
		where T : Object
	{
		var n = (T)EditorGUI.ObjectField(r, o, typeof(T), true);
		
		MarkModified(o != n, isField);
		
		return n;
	}
	
	public static T DrawEditableObjectWithButton<T>(Rect r, T o, string buttonText, out bool pressed, float buttonWidth = 25.0f, bool isField = false)
		where T : Object
	{
		var a = r; a.xMax -= buttonWidth + 2.0f;
		var b = r; b.xMin += r.width - buttonWidth;
		var n = (T)EditorGUI.ObjectField(a, o, typeof(T), true);
		
		pressed = DrawButton(b, buttonText);
		
		MarkModified(o != n, isField);
		
		return n;
	}
	
	public static bool DrawEditableBool(Rect r, bool o, bool isField = false)
	{
		var n = EditorGUI.Toggle(r, o);
		
		MarkModified(o != n, isField);
		
		return n;
	}
	
	public static int DrawEditableInt(Rect r, int o, bool isField = false)
	{
		var n = EditorGUI.IntField(FudgeRect(r), " ", o);
		
		MarkModified(o != n, isField);
		
		return n;
	}
	
	public static int DrawEditableInt(Rect r, int o, int min, int max, bool isField = false)
	{
		var n = EditorGUI.IntSlider(FudgeRect(r), " ", o, min, max);
		
		MarkModified(o != n, isField);
		
		return n;
	}
	
	public static int DrawEditableIntWithButton(Rect r, int o, string buttonText, out bool pressed, float buttonWidth = 25.0f, bool isField = false)
	{
		var a = r; a.xMax -= buttonWidth + 2.0f;
		var b = r; b.xMin += r.width - buttonWidth;
		var n = EditorGUI.IntField(a, o);
		
		pressed = DrawButton(b, buttonText);
		
		MarkModified(o != n, isField);
		
		return n;
	}
	
	public static int DrawEditableInt(Rect r, int o, string[] options, bool addEmptyOption = false, bool isField = false)
	{
		if (options == null)
		{
			options = new string[0];
		}
		
		if (addEmptyOption == true)
		{
			System.Array.Resize(ref options, options.Length + 1);
			
			options[options.Length - 1] = " ";
		}
		
		var n = EditorGUI.Popup(r, o, options);
		
		if (addEmptyOption == true)
		{
			if (n >= options.Length - 1)
			{
				n = -1;
			}
		}
		
		MarkModified(o != n, isField);
		
		return n;
	}
	
	public static int DrawEditableLayer(Rect r, int o, bool isField = false)
	{
		var n = EditorGUI.LayerField(r, o);
		
		MarkModified(o != n, isField);
		
		return n;
	}
	
	public static Quaternion DrawEditableQuaternion(Rect r, Quaternion o, bool isField = false)
	{
		return Quaternion.Euler(DrawEditableVector3(r, o.eulerAngles, isField));
	}
	
	public static Vector2 DrawEditableVector2(Rect r, Vector2 o, bool isField = false)
	{
		r.yMin -= 19.0f;
		
		var n = EditorGUI.Vector2Field(r, null, o);
		
		MarkModified(o != n, isField);
		
		return n;
	}
	
	public static Vector3 DrawEditableVector3(Rect r, Vector3 o, bool isField = false)
	{
		r.yMin -= 19.0f;
		
		var n = EditorGUI.Vector3Field(r, null, o);
		
		MarkModified(o != n, isField);
		
		return n;
	}
	
	public static Vector4 DrawEditableVector4(Rect r, Vector4 o, bool isField = false)
	{
		r.yMin -= 19.0f;
		
		var n = EditorGUI.Vector4Field(r, null, o);
		
		MarkModified(o != n, isField);
		
		return n;
	}
	
	public static float DrawEditableFloat(Rect r, float o, bool isField = false)
	{
		var n = EditorGUI.FloatField(FudgeRect(r), " ", o);
		
		MarkModified(o != n, isField);
		
		return n;
	}
	
	public static float DrawEditableFloat(Rect r, float o, float min, float max, bool isField = false)
	{
		var n = EditorGUI.Slider(FudgeRect(r), " ", o, min, max);
		
		MarkModified(o != n, isField);
		
		return n;
	}
	
	public static string DrawEditableString(Rect r, string o, bool wordWrap = false, bool isField = false)
	{
		var gs = new GUIStyle(EditorStyles.textField);
		
		gs.wordWrap = wordWrap;
		
		var n = EditorGUI.TextField(r, o != null ? o : "", gs);
		
		MarkModified(o != n, isField);
		
		return n;
	}
	
	public static string DrawEditableStringBox(Rect r, string o, bool isField = false)
	{
		var n = EditorGUI.TextArea(r, o);
		
		MarkModified(o != n, isField);
		
		return n;
	}
	
	public static string DrawEditableStringWithButton(Rect r, string o, string buttonText, out bool pressed, float buttonWidth = 25.0f, bool isField = false)
	{
		var a = r; a.xMax -= buttonWidth + 2.0f;
		var b = r; b.xMin += r.width - buttonWidth;
		var n = EditorGUI.TextField(a, o);
		
		pressed = DrawButton(b, buttonText);
		
		MarkModified(o != n, isField);
		
		return n;
	}
	
	public static string DrawEditableString(Rect r, string o, string[] options, bool isField = false)
	{
		if (options == null)
		{
			options = new string[0];
		}
		
		var i = System.Array.IndexOf(options, o);
		var j = EditorGUI.Popup(r, i, options);
		var n = j != -1 ? options[j] : string.Empty;
		
		MarkModified(n != o, isField);
		
		return n;
	}
	
	public static Color DrawEditableColour(Rect r, Color o, bool isField = false)
	{
		var n = EditorGUI.ColorField(r, o);
		
		MarkModified(o != n, isField);
		
		return n;
	}
	
	public static LayerMask DrawEditableLayerMask(Rect r, LayerMask o, bool isField = false)
	{
		var ops = new string[32];
		
		for (var i = 0; i < ops.Length; i++)
		{
			ops[i] = LayerMask.LayerToName(i);
		}
		
		var n = EditorGUI.MaskField(r, o, ops);
		
		MarkModified(o != n, isField);
		
		return n;
	}
	
	public static System.Enum DrawEditableEnum(Rect r, System.Enum o, bool isField = false)
	{
		var n = EditorGUI.EnumPopup(r, o);
		
		MarkModified(System.Enum.Equals(o, n) == false, isField);
		
		return n;
	}
	
	public static void DrawFieldTexture(string handle, string tooltip, Texture texture, Rect uv)
	{
		if (texture != null && CanDraw == true)
		{
			var w     = uv.width  * texture.width;
			var h     = uv.height * texture.height;
			var right = ReserveField(handle, tooltip, Mathf.Max(h, fieldHeight));
			var tRect = new Rect(right.x, right.y, w, h);
			
			GUI.DrawTextureWithTexCoords(tRect, texture, uv);
		}
	}
	
	public static void DrawTiledTexture(Rect rect, Texture texture)
	{
		var coords = new Rect(0.0f, 0.0f, rect.width / (float)texture.width, rect.height / (float)texture.height);
		
		GUI.DrawTextureWithTexCoords(rect, texture, coords, true);
	}
	
	public static void DrawHorizontalSlider(Rect rect)
	{
		GUI.Box(rect, string.Empty, GUI.skin.horizontalSlider);
	}
	
	public static Color DrawColourPicker(Rect rect, Color colour)
	{
		colour = EditorGUI.ColorField(rect, colour);
		
		return colour;
	}
	
	public static Rect DrawHorizontalSliderThumb(Rect rect, float position)
	{
		rect = SGT_RectHelper.RemoveRightPx(rect, 8.0f);
		rect.xMin += rect.width * position;
		rect.width  = 16.0f;
		rect.height = 16.0f;
		
		GUI.Box(rect, string.Empty, GUI.skin.horizontalSliderThumb);
		
		return rect;
	}
	
	public static float GetHorizontalSliderAcross(Rect rect, float x)
	{
		rect = SGT_RectHelper.RemoveRightPx(rect, 8.0f);
		
		return Mathf.Clamp01((x - rect.x - 4.0f) / rect.width);
	}
	
	public static Texture2D DrawTextureFieldWithLabel(Rect rect, string label, int labelWidth, Texture2D field, bool required = false)
	{
		return (Texture2D)DrawTextureFieldWithLabel(rect, label, labelWidth, (Texture)field, required);
	}
	
	public static Texture DrawTextureFieldWithLabel(Rect rect, string label, int labelWidth, Texture field, bool required = false)
	{
		if (CanDraw == true)
		{
			if (required == true)
			{
				if (field == null)
				{
					if (CanDraw == true)
					{
						var redRect = new Rect(rect);
						redRect = SGT_RectHelper.ExpandPx(redRect, 1.0f, 1.0f, 1.0f, 1.0f);
						GUI.DrawTexture(redRect, SGT_Helper.RedTexture);
					}
				}
			}
			
			var labelRect = SGT_RectHelper.GetLeftPx(ref rect, labelWidth);
			
			EditorGUI.LabelField(labelRect, new GUIContent(label, string.Empty), EditorStyles.label);
			
			var newField = (Texture)EditorGUI.ObjectField(rect, field, typeof(Texture), false);
			
			FieldModified |= newField != field;
			field          = newField;
		}
		
		return field;
	}
	
	public static void DrawShadowText(Rect r, string text, Color textC, Color shadowC, TextAnchor ta = TextAnchor.UpperLeft, FontStyle fs = FontStyle.Normal)
	{
		var gs = new GUIStyle(EditorStyles.label);
		
		gs.alignment = ta;
		gs.fontStyle = fs;
		gs.normal.textColor = shadowC;
		
		var r1 = r; r1.x += 1;
		var r2 = r; r2.x -= 1;
		var r3 = r; r3.y += 1;
		var r4 = r; r4.y -= 1;
		
		EditorGUI.LabelField(r1, text, gs);
		EditorGUI.LabelField(r2, text, gs);
		EditorGUI.LabelField(r3, text, gs);
		EditorGUI.LabelField(r4, text, gs);
		
		DrawText(r, text, textC, ta, fs);
	}
	
	public static void DrawText(Rect r, string o, Color c, TextAnchor ta = TextAnchor.UpperLeft, FontStyle fs = FontStyle.Normal)
	{
		var gs = new GUIStyle(EditorStyles.label);
		
		gs.alignment = ta;
		gs.fontStyle = fs;
		gs.normal.textColor = c;
		
		EditorGUI.LabelField(r, o, gs);
	}
	
	public static bool DrawTextWithButton(Rect r, string o, string buttonText, float buttonWidth = 25.0f, bool isField = false)
	{
		var a = r; a.xMax -= buttonWidth + 2.0f;
		var b = r; b.xMin += r.width - buttonWidth;
		EditorGUI.LabelField(a, o);
		
		var pressed = DrawButton(b, buttonText);
		
		MarkModified(pressed == true, isField);
		
		return pressed;
	}
	
	public static bool DrawButton(Rect r, string o)
	{
		var pressed = GUI.Button(r, o);
		
		MarkModified(pressed == true, false);
		
		return pressed;
	}
}