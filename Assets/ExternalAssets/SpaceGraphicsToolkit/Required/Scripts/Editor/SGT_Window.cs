using UnityEngine;
using UnityEditor;

public abstract class SGT_Window<T> : EditorWindow
	where T : SGT_Window<T>
{
	[SerializeField]
	private Vector2 scrollPos;
	
	[SerializeField]
	private float height = 100;
	
	public static void CreateWithTitle(string title)
	{
		var window = EditorWindow.GetWindow(typeof(T));
		
		window.title = title;
	}
	
	public void OnGUI()
	{
		scrollPos = GUI.BeginScrollView(new Rect (0, 0, position.width, position.height), scrollPos, new Rect (0, 0, 0, height));
		
		SGT_EditorGUI.ResetAll();
		
		OnInspector();
		
		var r = SGT_EditorGUI.Reserve(0.0f, false);
		
		if (Event.current.type == EventType.Repaint)
		{
			height = r.y;
		}
		
		GUI.EndScrollView();
	}
	
	public abstract void OnInspector();
}