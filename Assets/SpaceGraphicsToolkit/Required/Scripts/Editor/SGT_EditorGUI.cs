using BoolStack = System.Collections.Generic.Stack<bool>;
using IntList   = System.Collections.Generic.List<int>;

using UnityEngine;
using UnityEditor;

public static partial class SGT_EditorGUI
{
	private static BoolStack canDrawStack    = new BoolStack();
	private static IntList   identStyles     = new IntList();
	private const  float     handleWidth     = 150.0f;
	private const  float     fieldHeight     = 16.0f;
	private const  float     separatorHeight = 5.0f;
	private const  float     indentWidth     = 10.0f;
	private const  float     toggleWidth     = 16.0f;
	private static bool      markError;
	private static bool      markBold;
	private static int       cantDrawLevel;
	private static bool      fieldModified;
	private static bool      inspectorModified;
	
	public static bool FieldModified
	{
		set
		{
			if (fieldModified != value)
			{
				fieldModified      = value;
				inspectorModified |= value;
				GUI.changed       |= value;
			}
		}
		
		get
		{
			return fieldModified;
		}
	}
	
	public static bool InspectorModified
	{
		set
		{
			if (inspectorModified != value)
			{
				inspectorModified = value;
			}
		}
		
		get
		{
			return inspectorModified;
		}
	}
	
	public static bool CanDraw
	{
		get
		{
			return cantDrawLevel == 0;
		}
	}
	
	public static float HandleWidth
	{
		get
		{
			return handleWidth;
		}
	}
	
	public static float FieldHeight
	{
		get
		{
			return fieldHeight;
		}
	}
	
	public static float SeparatorHeight
	{
		get
		{
			return separatorHeight;
		}
	}
	
	public static float IndentWidth
	{
		get
		{
			return indentWidth;
		}
	}
	
	public static float ToggleWidth
	{
		get
		{
			return toggleWidth;
		}
	}
	
	public static int IndentLevel
	{
		get
		{
			return identStyles.Count;
		}
	}
	
	public static void ResetAll()
	{
		GUI.changed   = false;
		fieldModified = false;
		cantDrawLevel = 0;
		canDrawStack.Clear();
		identStyles.Clear();
	}
	
	public static void MarkModified(bool mark, bool isField = true)
	{
		if (mark == true)
		{
			GUI.changed = true;
			
			if (isField == true)
			{
				fieldModified     = true;
				inspectorModified = true;
			}
		}
	}
	
	public static void MarkNextFieldAsBold(bool bold = true)
	{
		if (CanDraw == true)
		{
			markBold = bold;
		}
	}
	
	public static void MarkNextFieldAsError(bool error = true)
	{
		if (CanDraw == true)
		{
			if (error == true)
			{
				if (GUI.enabled == true)
				{
					markError = true;
				}
			}
		}
	}
	
	public static void DrawError(Rect r, bool error = true)
	{
		if (CanDraw == true && error == true)
		{
			var redRect = SGT_RectHelper.ExpandPx(r, 2.0f, 2.0f, 2.0f, 2.0f);
			GUI.DrawTexture(redRect, SGT_Helper.RedTexture);
		}
	}
	
	public static Rect Reserve(float height = fieldHeight, bool indent = true)
	{
		var rect = EditorGUILayout.BeginVertical();
		{
			EditorGUILayout.LabelField(string.Empty, GUILayout.Height(height));
		}
		EditorGUILayout.EndVertical();
		
		rect = SGT_RectHelper.ExpandPx(rect, -5, -5, 0, 0);
		
		return indent == true ? Indent(rect) : rect;
	}
	
	public static void ReserveFieldRaw(out Rect left, out Rect right, float height = fieldHeight)
	{
		var rect = Reserve(height, false);
		
		left  = Indent(rect);
		right = SGT_RectHelper.RemoveLeftPx(rect, Mathf.Min(handleWidth, rect.width / 2 - 5));
		
		if (markError == true)
		{
			markError = false;
			
			DrawError(left);
		}
	}
	
	public static Rect ReserveField(float height = fieldHeight)
	{
		Rect left, right;
		
		ReserveFieldRaw(out left, out right, height);
		
		return right;
	}
	
	public static Rect ReserveField(string handle, string tooltip, float height = fieldHeight, bool bold = false)
	{
		if (markBold == true)
		{
			markBold = false;
			bold     = true;
		}
		
		Rect left, right; ReserveFieldRaw(out left, out right, height);
		
		EditorGUI.LabelField(left, new GUIContent(handle, tooltip), bold ? EditorStyles.boldLabel : EditorStyles.label);
		
		return right;
	}
	
	public static Rect Indent(Rect rect)
	{
		if (CanDraw == true)
		{
			for (var i = 0; i < identStyles.Count; i++)
			{
				var iRect = rect; iRect.yMin -= 2; iRect.yMax += 1; iRect.xMin += indentWidth * 0.5f - 1;
				
				rect.xMin += indentWidth;
				
				switch (identStyles[i])
				{
					case 0:
					{
						iRect.width = 2.0f;
						
						GUI.DrawTexture(iRect, SGT_Helper.BlackTexture);
					}
					break;
					case 1:
					{
						iRect.xMax += 6;
						
						DrawShadowBoxCentre(iRect);
					}
					break;
				}
			}
		}
		
		return rect;
	}
	
	public static void BeginCanDraw(bool canDraw = true)
	{
		if (canDrawStack == null) canDrawStack = new BoolStack();
		
		canDrawStack.Push(canDraw);
		
		if (canDraw == false)
		{
			cantDrawLevel += 1;
		}
	}
	
	public static void EndCanDraw()
	{
		if (canDrawStack != null && canDrawStack.Pop() == false)
		{
			cantDrawLevel -= 1;
		}
	}
	
	public static void BeginIndent(bool canDraw = true, int style = 0)
	{
		BeginCanDraw(canDraw);
		
		switch (style)
		{
			case 0: break;
			case 1: var iRect = Reserve(3.0f); iRect.xMin += indentWidth * 0.5f - 1; iRect.xMax += 6; DrawShadowBoxTop(iRect); break;
		}
		
		identStyles.Add(style);
	}
	
	public static void EndIndent()
	{
		var style = identStyles[identStyles.Count - 1];
		
		identStyles.RemoveAt(identStyles.Count - 1);
		
		switch (style)
		{
			case 0: break;
			case 1:
			{
				var iRect = Reserve(3.0f);
				iRect.xMin += indentWidth * 0.5f - 1;
				iRect.xMax += 6;
				DrawShadowBottom(iRect);
				
				iRect.height = 1;
				iRect.y -= 2;
				DrawShadowBoxCentre(iRect);
			}
			break;
		}
		
		EndCanDraw();
	}
	
	public static void BeginFrozen(bool unfrozen)
	{
		EditorGUI.BeginDisabledGroup(unfrozen == false);
	}
	
	public static void EndFrozen()
	{
		EditorGUI.EndDisabledGroup();
	}
	
	public static void BeginGroup(string handle)
	{
		if (CanDraw == true)
		{
			ReserveField(handle, string.Empty, fieldHeight, true);
			
			BeginIndent();
		}
	}
	
	public static void EndGroup()
	{
		if (CanDraw == true)
		{
			EndIndent();
		}
	}
	
	public static bool BeginToggleGroup(string handle, string tooltip, bool toggle)
	{
		if (CanDraw == true)
		{
			var handleRect = Reserve(fieldHeight);
			var toggleRect = SGT_RectHelper.GetLeftPx(ref handleRect, toggleWidth);
			
			EditorGUI.LabelField(handleRect, new GUIContent(handle, tooltip), EditorStyles.boldLabel);
			
			var newToggle = EditorGUI.Toggle(toggleRect, toggle);
			
			FieldModified |= newToggle != toggle;
			toggle         = newToggle;
		}
		
		BeginIndent(toggle);
		
		return toggle;
	}
	
	public static void EndToggleGroup()
	{
		EndIndent();
	}
	
	public static bool BeginFoldout(string handle, bool toggle)
	{
		var rect = Reserve(fieldHeight);
		
		toggle = EditorGUI.Foldout(rect, toggle, handle);
		
		BeginIndent(toggle);
		
		// NOTE: No FieldModified check
		
		return toggle;
	}
	
	public static void EndFoldout()
	{
		EndIndent();
	}
	
	private static Rect FudgeRect(Rect r, float offset = 0.0f)
	{
		r.xMin -= Mathf.Min(handleWidth, r.width + offset - 6);
		
		return r;
	}
}