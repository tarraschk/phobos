using UnityEngine;
using UnityEditor;

public static partial class SGT_EditorGUI
{
	public static string AssetField<T>(string handle, string tooltip, string field, bool required = false, bool isField = true)
		where T : Object
	{
		if (CanDraw == true)
		{
			var asset = AssetDatabase.LoadAssetAtPath(field == null ? string.Empty : field, typeof(T)) as T;
			
			MarkNextFieldAsError(required == true && asset == null);
			var fieldRect = ReserveField(handle, tooltip);
			
			asset = DrawEditableObject<T>(fieldRect, asset, isField);
			field = AssetDatabase.GetAssetPath(asset);
		}
		
		return field;
	}
	
	public static T ObjectField<T>(string handle, string tooltip, T field, bool required = false, bool isField = true)
		where T : Object
	{
		if (CanDraw == true)
		{
			MarkNextFieldAsError(required == true && field == null);
			var fieldRect = ReserveField(handle, tooltip);
			
			field = DrawEditableObject<T>(fieldRect, field, isField);
		}
		
		return field;
	}
	
	public static T ObjectFieldWithButton<T>(string handle, string tooltip, T field, string buttonText, out bool pressed, float buttonWidth = 25.0f, bool required = false)
		where T : Object
	{
		pressed = false;
		
		if (CanDraw == true)
		{
			MarkNextFieldAsError(required == true && field == null);
			var fieldRect = ReserveField(handle, tooltip);
			
			field = DrawEditableObjectWithButton(fieldRect, field, buttonText, out pressed, buttonWidth, true);
		}
		
		return field;
	}
	
	public static bool BoolField(string handle, string tooltip, bool field, bool isField = true)
	{
		if (CanDraw == true)
		{
			var fieldRect = ReserveField(handle, tooltip);
			
			field = DrawEditableBool(fieldRect, field, isField);
		}
		
		return field;
	}
	
	public static int IntField(string handle, string tooltip, int field, bool isField = true)
	{
		if (CanDraw == true)
		{
			var fieldRect = ReserveField(handle, tooltip);
			
			field = DrawEditableInt(fieldRect, field, isField);
		}
		
		return field;
	}
	
	public static int IntField(string handle, string tooltip, int field, int min, int max, bool isField = true)
	{
		if (CanDraw == true)
		{
			var fieldRect = ReserveField(handle, tooltip);
			
			field = DrawEditableInt(fieldRect, field, min, max, isField);
		}
		
		return field;
	}
	
	public static int IntField(string handle, string tooltip, int field, string[] options, bool required = false, bool addEmptyOption = false, bool isField = true)
	{
		if (CanDraw == true)
		{
			MarkNextFieldAsError(required == true && field == -1);
			var fieldRect = ReserveField(handle, tooltip);
			
			field = DrawEditableInt(fieldRect, field, options, addEmptyOption, isField);
		}
		
		return field;
	}
	
	public static int IntFieldWithButton(string handle, string tooltip, int field, string buttonText, out bool pressed, float buttonWidth = 25.0f)
	{
		pressed = false;
		
		if (CanDraw == true)
		{
			var fieldRect = ReserveField(handle, tooltip);
			
			field = DrawEditableIntWithButton(fieldRect, field, buttonText, out pressed, buttonWidth, true);
		}
		
		return field;
	}
	
	public static string StringField(string handle, string tooltip, string field, bool isField = true)
	{
		if (CanDraw == true)
		{
			var fieldRect = ReserveField(handle, tooltip);
			
			field = DrawEditableString(fieldRect, field, false, isField);
		}
		
		return field;
	}
	
	public static string StringField(string handle, string tooltip, string field, string[] options, bool required = false, bool isField = true)
	{
		if (CanDraw == true)
		{
			MarkNextFieldAsError(required == true && field == null);
			var fieldRect = ReserveField(handle, tooltip);
			
			field = DrawEditableString(fieldRect, field, options, isField);
		}
		
		return field;
	}
	
	public static string StringFieldWithButton(string handle, string tooltip, string field, string buttonText, out bool pressed, float buttonWidth = 25.0f)
	{
		pressed = false;
		
		if (CanDraw == true)
		{
			var fieldRect = ReserveField(handle, tooltip);
			
			field = DrawEditableStringWithButton(fieldRect, field, buttonText, out pressed, buttonWidth, true);
		}
		
		return field;
	}
	
	public static float FloatField(string handle, string tooltip, float field, bool isField = true)
	{
		if (CanDraw == true)
		{
			var fieldRect = ReserveField(handle, tooltip);
			
			field = DrawEditableFloat(fieldRect, field, isField);
		}
		
		return field;
	}
	
	public static float FloatField(string handle, string tooltip, float field, float min, float max, bool isField = true)
	{
		if (CanDraw == true)
		{
			var fieldRect = ReserveField(handle, tooltip);
			
			field = DrawEditableFloat(fieldRect, field, min, max, isField);
		}
		
		return field;
	}
	
	public static int LayerField(string handle, string tooltip, int field, bool isField = true)
	{
		if (CanDraw == true)
		{
			var fieldRect = ReserveField(handle, tooltip);
			
			field = DrawEditableLayer(fieldRect, field, isField);
		}
		
		return field;
	}
	
	public static Color ColourField(string handle, string tooltip, Color field, bool isField = true)
	{
		if (CanDraw == true)
		{
			var fieldRect = ReserveField(handle, tooltip);
			
			field = DrawEditableColour(fieldRect, field, isField);
		}
		
		return field;
	}
	
	public static Quaternion QuaternionField(string handle, string tooltip, Quaternion field, bool isField = true)
	{
		if (CanDraw == true)
		{
			var fieldRect = ReserveField(handle, tooltip);
			
			field = DrawEditableQuaternion(fieldRect, field, isField);
		}
		
		return field;
	}
	
	public static Vector2 Vector2Field(string handle, string tooltip, Vector2 field, bool isField = true)
	{
		if (CanDraw == true)
		{
			var fieldRect = ReserveField(handle, tooltip);
			
			field = DrawEditableVector2(fieldRect, field, isField);
		}
		
		return field;
	}
	
	public static Vector3 Vector3Field(string handle, string tooltip, Vector3 field, bool isField = true)
	{
		if (CanDraw == true)
		{
			var fieldRect = ReserveField(handle, tooltip);
			
			field = DrawEditableVector3(fieldRect, field, isField);
		}
		
		return field;
	}
	
	public static Vector4 Vector4Field(string handle, string tooltip, Vector4 field, bool isField = true)
	{
		if (CanDraw == true)
		{
			var fieldRect = ReserveField(handle, tooltip);
			
			field = DrawEditableVector4(fieldRect, field, isField);
		}
		
		return field;
	}
	
	public static LayerMask LayerMaskField(string handle, string tooltip, LayerMask field, bool isField = true)
	{
		if (CanDraw == true)
		{
			var fieldRect = ReserveField(handle, tooltip);
			
			field = DrawEditableLayerMask(fieldRect, field, isField);
		}
		
		return field;
	}
	
	public static System.Enum EnumField(string handle, string tooltip, System.Enum field, bool isField = true)
	{
		if (CanDraw == true)
		{
			var fieldRect = ReserveField(handle, tooltip);
			
			field = DrawEditableEnum(fieldRect, field, isField);
		}
		
		return field;
	}
	
	public static int SeedField(string handle, string tooltip, int field, bool isField = true)
	{
		bool pressed;
		
		field = IntFieldWithButton(handle, tooltip, field, "R", out pressed);
		
		if (pressed == true)
		{
			field         = Random.Range(0, 100000);
			FieldModified = true;
		}
		
		return field;
	}
	
	public static void Text(string text, Color colour, TextAnchor anchor = TextAnchor.UpperLeft, FontStyle style = FontStyle.Normal)
	{
		if (CanDraw == true)
		{
			var rect = Reserve(fieldHeight);
			
			DrawText(rect, text, colour, anchor, style);
		}
	}
	
	public static bool TextWithButton(string text, string buttonText, float buttonWidth = 25.0f)
	{
		var pressed = false;
		
		if (CanDraw == true)
		{
			var rect = Reserve(fieldHeight);
			
			pressed = DrawTextWithButton(rect, text, buttonText, buttonWidth, true);
		}
		
		return pressed;
	}
	
	public static void HelpBox(string text, MessageType mt, float height = 28.0f)
	{
		if (CanDraw == true)
		{
			var rect = Reserve(height);
			
			EditorGUI.HelpBox(rect, text, mt);
		}
	}
	
	public static void Separator(bool draw = true)
	{
		if (CanDraw == true && draw == true)
		{
			Reserve(separatorHeight);
		}
	}
	
	public static bool Button(string text)
	{
		if (CanDraw == true)
		{
			var rect = Reserve(fieldHeight);
			
			return DrawButton(rect, text);
		}
		
		return false;
	}
}